using RealLifeViewerEvents.Events;
using ViewerEventsLibrary.Logging;

namespace RealLifeViewerEvents.EventsExecution.EventsImplementations
{
    public class ReverseShirtEvent : InstantEvent
    {
        public ReverseShirtEvent(IViewerEventsLogger logger, QueuedEvent queuedEvent) : base(logger, queuedEvent)
        {
        }
    }
}
