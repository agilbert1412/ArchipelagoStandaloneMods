using RealLifeViewerEvents.Events;
using ViewerEventsLibrary.Logging;

namespace RealLifeViewerEvents.EventsExecution.EventsImplementations
{
    public class NoKeyboardEvent : TimedEndEvent
    {
        public NoKeyboardEvent(IViewerEventsLogger logger, QueuedEvent queuedEvent) : base(logger, queuedEvent, 60)
        {
        }
    }
}
