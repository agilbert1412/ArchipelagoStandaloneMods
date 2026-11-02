using RealLifeViewerEvents.Events;
using ViewerEventsLibrary.Logging;

namespace RealLifeViewerEvents.EventsExecution.EventsImplementations
{
    public class PirateSpeakEvent : TimedEndEvent
    {
        public PirateSpeakEvent(IViewerEventsLogger logger, QueuedEvent queuedEvent) : base(logger, queuedEvent, 60)
        {
        }
    }
}
