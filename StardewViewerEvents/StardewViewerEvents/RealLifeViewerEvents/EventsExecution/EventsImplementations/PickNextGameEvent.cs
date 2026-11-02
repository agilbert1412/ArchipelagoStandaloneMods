using RealLifeViewerEvents.Events;
using ViewerEventsLibrary.Logging;

namespace RealLifeViewerEvents.EventsExecution.EventsImplementations
{
    public class PickNextGameEvent : InstantEvent
    {
        public PickNextGameEvent(IViewerEventsLogger logger, QueuedEvent queuedEvent) : base(logger, queuedEvent)
        {
        }
    }
}
