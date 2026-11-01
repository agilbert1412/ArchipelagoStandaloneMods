using Discord.WebSocket;
using RealLifeViewerEvents.Events;
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
        public EventCollection Events { get; }
        public IEventQueue<QueuedEvent> Queue { get; }
        private ExecutableEvent? _activeEvent;

        public ViewerEventsExecutor(IViewerEventsLogger logger)
        {
            _logger = logger;
            Events = new EventCollection(new EventsGenerator());
            Queue = new EventQueue(logger);
            _activeEvent = null;

            _logger.LogInfo(Events.Count + " is the total events count.");
        }

        public async Task DequeueEvent(IViewerEventsLogger logger, IBotCommunicator communications, CreditAccounts accounts, ChannelSet channels)
        {
            if (Queue.IsEmpty || Queue.IsPaused() || _activeEvent != null)
            {
                return;
            }

            var eventToSend = Queue.First;
            var baseEvent = eventToSend.BaseEvent;
            var baseEventName = baseEvent.name;
            _logger.LogInfo($"Attempting to dequeue {eventToSend.queueCount} instances of {baseEventName} triggered by {eventToSend.username}.");
            Queue.RemoveFirst();
            Queue.PrintToConsole();

            SendSameTypeEventsToBackOfQueue(eventToSend);

            try
            {
                var executableEvent = eventToSend.GetExecutableEvent(logger);

                if (!executableEvent.ValidateParameters(out var errorMessage))
                {
                    var accountToRefund = accounts[eventToSend.userId];
                    var refundAmount = baseEvent.GetMultiplierCost(Events.CurrentMultiplier) * eventToSend.queueCount;
                    accountToRefund.AddCredits(refundAmount);
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
                        Console.WriteLine($"Increased queue count of {chosenEvent.name} to {qe.queueCount}.");
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
            Console.WriteLine($"Added {invokedEvent.baseEventName} to queue.");
        }

        public QueuedEvent CreateQueuedEvent(SocketUser sender, ViewerEvent chosenEvent, string[] args)
        {
            var invokedEvent = new QueuedEvent(chosenEvent, args);
            invokedEvent.username = sender.GetDisplayName();
            invokedEvent.userId = sender.Id;
            invokedEvent.queueCount = 1;
            return invokedEvent;
        }

        public QueuedEvent CreateQueuedEvent(Author sender, ViewerEvent chosenEvent, string[] args)
        {
            var invokedEvent = new QueuedEvent(chosenEvent, args);
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
                _activeEvent = null;
            }
        }

        public void ReceivedInput(string? input)
        {
            _activeEvent?.ReceivedInput(input);
        }
    }
}
