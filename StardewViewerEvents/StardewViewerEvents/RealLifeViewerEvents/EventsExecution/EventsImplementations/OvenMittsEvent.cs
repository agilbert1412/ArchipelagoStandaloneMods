using RealLifeViewerEvents.Events;
using ViewerEventsLibrary.Logging;

namespace RealLifeViewerEvents.EventsExecution.EventsImplementations
{
    public class OvenMittsEvent : TimedEndEvent
    {
        public OvenMittsEvent(IViewerEventsLogger logger, QueuedEvent queuedEvent) : base(logger, queuedEvent, 60)
        {
        }
    }
}
