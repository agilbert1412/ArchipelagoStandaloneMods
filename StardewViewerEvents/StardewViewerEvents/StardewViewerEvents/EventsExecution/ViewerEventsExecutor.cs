using Discord.WebSocket;
using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Credits;
using StardewViewerEvents.DiscordIntegration;
using StardewViewerEvents.Events;
using StardewViewerEvents.Extensions;

namespace StardewViewerEvents.EventsExecution
{
    public class ViewerEventsExecutor
    {
        private readonly IMonitor _logger;

        public EventCollection Events { get; }
        public EventQueue Queue { get; }
        private List<ExecutableEvent> _activeEvents;

        public ViewerEventsExecutor(IMonitor logger)
        {
            _logger = logger;
            Events = new EventCollection();
            Queue = new EventQueue(logger);
            _activeEvents = new List<ExecutableEvent>();

            _logger.LogInfo(Events.Count + " is the total events count.");
        }

        public async Task DequeueEvent(IMonitor logger, IModHelper modHelper, IBotCommunicator communications, CreditAccounts accounts, ChannelSet channels)
        {
            if (Queue.IsEmpty || Queue.IsPaused())
            {
                return;
            }

            if (Game1.eventUp || Game1.CurrentEvent != null)
            {
                return;
            }

            var eventToSend = Queue.First;
            var baseEvent = eventToSend.BaseEvent;
            var baseEventName = baseEvent.name;
            _logger.LogInfo(
                $"Attempting to dequeue {eventToSend.queueCount} instances of {baseEventName} triggered by {eventToSend.username}.");
            Queue.RemoveFirst();
            Queue.PrintToConsole();

            try
            {
                var executableEvent = eventToSend.GetExecutableEvent(logger, modHelper);

                if (!executableEvent.ValidateParameters(out var errorMessage))
                {
                    var accountToRefund = accounts[eventToSend.userId];
                    var refundAmount = baseEvent.GetMultiplierCost(Events.CurrentMultiplier) * eventToSend.queueCount;
                    accountToRefund.AddCredits(refundAmount);
                    await communications.SendMessageAsync(channels.EventsChannel,
                        $"{errorMessage} You have been refunded {refundAmount} credits. Current Balance: {accountToRefund.GetCredits()}");
                    return;
                }

                if (_activeEvents.Any(x => x.GetType() == executableEvent.GetType()) || !executableEvent.CanExecuteRightNow())
                {
                    Queue.QueueEvent(eventToSend);
                    return;
                }

                try
                {
                    executableEvent.Execute();
                    _activeEvents.Add(executableEvent);
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

        public void Update()
        {
            for (var i = _activeEvents.Count - 1; i >= 0; i--)
            {
                var activeEvent = _activeEvents[i];
                if (activeEvent.UpdateAndTryFinish())
                {
                    _activeEvents.RemoveAt(i);
                }
            }
        }
    }
}
