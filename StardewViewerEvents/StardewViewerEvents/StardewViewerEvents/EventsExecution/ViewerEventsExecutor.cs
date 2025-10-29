using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;
using StardewViewerEvents.Extensions;

namespace StardewViewerEvents.EventsExecution
{
    public class ViewerEventsExecutor
    {
        private readonly IMonitor _logger;
        public EventCollection Events { get; }
        public EventQueue Queue { get; }

        public ViewerEventsExecutor(IMonitor logger)
        {
            _logger = logger;
            Events = new EventCollection();
            Queue = new EventQueue(logger);

            _logger.LogInfo(Events.Count + " is the total events count.");
        }

        public void DequeueEvent(IMonitor logger, IModHelper modHelper)
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

            var executableEvent = eventToSend.GetExecutableEvent(logger, modHelper);

            if (!executableEvent.CanExecuteRightNow())
            {
                Queue.QueueEvent(eventToSend);
                return;
            }

            try
            {
                executableEvent.Execute();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while executing event '{baseEventName}'", ex);
            }
        }

        public void AddOrIncrementEventInQueue(string senderName, ViewerEvent chosenEvent, string[] args)
        {
            if (chosenEvent.IsStackable() && (args == null || args.Length == 0))
            {
                foreach (var qe in Queue)
                {
                    if (qe.baseEventName == chosenEvent.name)
                    {
                        qe.queueCount += 1;
                        Console.WriteLine($"Increased queue count of {chosenEvent.name} to {qe.queueCount}.");
                        return;
                    }
                }
            }

            AddEventToQueueIfNeeded(senderName, chosenEvent, args);
        }

        private void AddEventToQueueIfNeeded(string senderName, ViewerEvent chosenEvent, string[] args)
        {
            var invokedEvent = new QueuedEvent(chosenEvent, args);
            invokedEvent.username = senderName;

            Queue.QueueEvent(invokedEvent);
            invokedEvent.queueCount = 1;

            Console.WriteLine($"Added {invokedEvent.baseEventName} to queue.");
        }
    }
}
