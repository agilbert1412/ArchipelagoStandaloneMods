using Discord.WebSocket;
using RealLifeViewerEvents.DiscordIntegration;
using RealLifeViewerEvents.Events;
using RealLifeViewerEvents.EventsExecution.EventsImplementations;
using RealLifeViewerEvents.Extensions;
using TwitchLib.Api.Helix;
using ViewerEventsLibrary;
using ViewerEventsLibrary.Credits;
using ViewerEventsLibrary.DiscordIntegration;
using ViewerEventsLibrary.Events;
using ViewerEventsLibrary.Extensions;
using ViewerEventsLibrary.Logging;

namespace RealLifeViewerEvents.EventsExecution
{
    public class ViewerEventsExecutor : IEventsExecutor<QueuedEvent>
    {

        private readonly IViewerEventsLogger _logger;
        private readonly SoundPlayer _soundPlayer;
        public EventCollection Events { get; }
        public IEventQueue<QueuedEvent> Queue { get; }
        private ExecutableEvent? _activeEvent;

        public ViewerEventsExecutor(IViewerEventsLogger logger)
        {
            _logger = logger;
            _soundPlayer = new SoundPlayer(_logger);
            Events = new EventCollection(new EventsGenerator());
            Queue = new EventQueue(logger, _soundPlayer);
            _activeEvent = null;

            _logger.LogInfo(Events.Count + " is the total events count.");
        }

        public async Task DequeueEvent(IViewerEventsLogger logger, IBotCommunicator communications,
            CreditAccounts accounts, ChannelSet channels)
        {
            if (Queue.IsEmpty || Queue.IsPaused() || _activeEvent != null)
            {
                return;
            }

            var eventToSend = Queue.First;
            var baseEvent = eventToSend.BaseEvent;
            var baseEventName = baseEvent.name;
            _logger.LogInfo(
                $"Executing {eventToSend.queueCount} instances of {baseEventName} triggered by {eventToSend.username}.");
            Queue.RemoveFirst();

            SendSameTypeEventsToBackOfQueue(eventToSend);

            try
            {
                var executableEvent = eventToSend.GetExecutableEvent(logger);

                if (!executableEvent.ValidateParameters(out var errorMessage))
                {
                    var accountToRefund = accounts[eventToSend.userId];
                    var refundAmount = baseEvent.GetMultiplierCost(eventToSend.currentMultiplier) * eventToSend.queueCount;
                    accountToRefund.RefundCredits(refundAmount);
                    await communications.SendMessageAsync(channels.EventsChannel,
                        $"{errorMessage} You have been refunded {refundAmount} credits. Current Balance: {accountToRefund.GetCredits()}");
                    return;
                }

                try
                {
                    executableEvent.Execute();
                    _activeEvent = executableEvent;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error while executing event '{baseEventName}'", ex);
                }
            }
            catch (NotImplementedException ex)
            {
                _logger.LogError(ex.Message);
                await communications.SendMessageAsync(channels.AdminChannel, ex.Message);
            }
        }

        private void SendSameTypeEventsToBackOfQueue(QueuedEvent eventToSend)
        {
            if (Queue.IsEmpty || Queue.Count < 2)
            {
                return;
            }

            if (Queue.All(x => x.baseEventName != eventToSend.baseEventName) ||
                Queue.All(x => x.baseEventName == eventToSend.baseEventName))
            {
                return;
            }

            var nextEvent = Queue.First;
            var counter = 0;
            while (counter < Queue.Count && nextEvent.baseEventName == eventToSend.baseEventName)
            {
                counter++;
                Queue.QueueEvent(nextEvent);
                Queue.RemoveFirst();
                nextEvent = Queue.First;
            }
        }

        public void AddOrIncrementEventInQueue(SocketUser sender, ViewerEvent chosenEvent, string[] args)
        {
            if (TryIncrementEventInQueue(chosenEvent))
            {
                return;
            }

            AddEventToQueue(sender, chosenEvent, args);
        }

        public void AddOrIncrementEventInQueue(Author sender, ViewerEvent chosenEvent, string[] args)
        {
            if (TryIncrementEventInQueue(chosenEvent))
            {
                return;
            }

            AddEventToQueue(sender, chosenEvent, args);
        }

        private bool TryIncrementEventInQueue(ViewerEvent chosenEvent)
        {
            if (chosenEvent.IsStackable())
            {
                foreach (var qe in Queue)
                {
                    if (qe.baseEventName == chosenEvent.name)
                    {
                        qe.queueCount += 1;
                        _logger.LogDebug($"Increased queue count of {chosenEvent.name} to {qe.queueCount}.");
                        return true;
                    }
                }
            }

            return false;
        }

        public void AddEventToQueue(SocketUser sender, ViewerEvent chosenEvent, string[] args)
        {
            var invokedEvent = CreateQueuedEvent(sender, chosenEvent, args);
            AddEventToQueue(invokedEvent);
        }

        public void AddEventToQueue(Author sender, ViewerEvent chosenEvent, string[] args)
        {
            var invokedEvent = CreateQueuedEvent(sender, chosenEvent, args);
            AddEventToQueue(invokedEvent);
        }

        private void AddEventToQueue(QueuedEvent invokedEvent)
        {
            Queue.QueueEvent(invokedEvent);
            _logger.LogInfo($"Added {invokedEvent.baseEventName} to queue.");
        }

        public QueuedEvent CreateQueuedEvent(SocketUser sender, ViewerEvent chosenEvent, string[] args)
        {
            var invokedEvent = new QueuedEvent(chosenEvent, args, Events.CurrentMultiplier);
            invokedEvent.username = sender.GetDisplayName();
            invokedEvent.userId = sender.Id;
            invokedEvent.queueCount = 1;
            return invokedEvent;
        }

        public QueuedEvent CreateQueuedEvent(Author sender, ViewerEvent chosenEvent, string[] args)
        {
            var invokedEvent = new QueuedEvent(chosenEvent, args, Events.CurrentMultiplier);
            invokedEvent.username = sender.DisplayName;
            invokedEvent.userId = sender.Id;
            invokedEvent.queueCount = 1;
            return invokedEvent;
        }

        public void Update(int deltaMilliseconds)
        {
            if (_activeEvent == null)
            {
                return;
            }

            if (_activeEvent.UpdateAndTryFinish(deltaMilliseconds))
            {
                _logger.LogInfo($"{_activeEvent.BaseEvent.name} is done!");
                _activeEvent = null;
                Queue.PrintToConsole();
                Queue.PrintNextEventAndCommand();
                _soundPlayer.PlayEventDoneSound();
            }
        }

        public void ReceivedInput(string? input, ViewerEventsService viewerEventsService)
        {
            if (_activeEvent == null)
            {
                return;
            }

            _activeEvent.ReceivedInput(input);
        }

        public void PrintNextCommandHelp()
        {
            if (_activeEvent == null)
            {
                Queue.PrintToConsole();
                Queue.PrintNextEventCommand();
                return;
            }

            if (!_activeEvent.WillFinishThisFrame())
            {
                _activeEvent.PrintNextCommandHelp();
                return;
            }
        }

        public void SkipEvent()
        {
            if (Queue.IsEmpty || Queue.Count < 2)
            {
                return;
            }

            var nextEvent = Queue.First;

            if (Queue.All(x => x.baseEventName == nextEvent.baseEventName))
            {
                return;
            }

            var eventsToMove = Queue.Where(x => x.baseEventName == nextEvent.baseEventName).ToList();
            var eventsToKeep = Queue.Where(x => x.baseEventName != nextEvent.baseEventName).ToList();

            Queue.Clear();
            foreach (var eventToAdd in eventsToKeep)
            {
                Queue.QueueEvent(eventToAdd);
            }

            foreach (var eventToAdd in eventsToMove)
            {
                Queue.QueueEvent(eventToAdd);
            }
        }

        public void RefundNextEvent(IViewerEventsLogger logger, IBotCommunicator communications, CreditAccounts accounts, ChannelSet channels)
        {
            if (Queue.IsEmpty)
            {
                _logger.LogInfo($"No event to refund");
                return;
            }

            var eventToRefund = Queue.First;
            Queue.RemoveFirst();

            var baseEvent = eventToRefund.BaseEvent;
            var accountToRefund = accounts[eventToRefund.userId];
            var refundAmount = baseEvent.GetMultiplierCost(eventToRefund.currentMultiplier) * eventToRefund.queueCount;
            accountToRefund.RefundCredits(refundAmount);
            communications.SendMessageAsync(channels.EventsChannel, $"Event '{baseEvent.name}' was cancelled. You have been refunded {refundAmount} credits. Current Balance: {accountToRefund.GetCredits()}").FireAndForget();
            _logger.LogInfo($"Event '{baseEvent.name}' cancelled. {refundAmount} credits were refunded to {accountToRefund.discordName}");
            return;

        }
    }
}
