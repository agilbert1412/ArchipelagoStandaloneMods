using RealLifeViewerEvents.Events;
using ViewerEventsLibrary.Logging;

namespace RealLifeViewerEvents.EventsExecution.EventsImplementations
{
    public class PessimistEvent : TimedEndEvent
    {
        public PessimistEvent(IViewerEventsLogger logger, QueuedEvent queuedEvent) : base(logger, queuedEvent, 60)
        {
        }
    }
}
