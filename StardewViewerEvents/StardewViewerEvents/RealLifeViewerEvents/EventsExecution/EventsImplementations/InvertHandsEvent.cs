using RealLifeViewerEvents.Events;
using ViewerEventsLibrary.Logging;

namespace RealLifeViewerEvents.EventsExecution.EventsImplementations
{
    public class InvertHandsEvent : TimedEndEvent
    {
        public InvertHandsEvent(IViewerEventsLogger logger, QueuedEvent queuedEvent) : base(logger, queuedEvent, 60)
        {
        }
    }
}
