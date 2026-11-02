using RealLifeViewerEvents.Events;
using ViewerEventsLibrary.Logging;

namespace RealLifeViewerEvents.EventsExecution.EventsImplementations
{
    public class HydrateEvent : InstantEvent
    {
        public HydrateEvent(IViewerEventsLogger logger, QueuedEvent queuedEvent) : base(logger, queuedEvent)
        {
        }
    }
}
