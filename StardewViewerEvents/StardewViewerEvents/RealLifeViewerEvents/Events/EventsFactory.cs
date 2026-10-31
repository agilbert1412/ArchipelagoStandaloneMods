using RealLifeViewerEvents.Events.Constants;
using RealLifeViewerEvents.EventsExecution;
using ViewerEventsLibrary.Logging;

namespace RealLifeViewerEvents.Events
{
    public class EventsFactory
    {
        public ExecutableEvent GetExecutableEvent(IViewerEventsLogger logger, QueuedEvent queuedEvent)
        {
            switch (queuedEvent.BaseEvent.name)
            {
                case EventName.KARAOKE_SONG:
                    return new KaraokeSongEvent(logger, queuedEvent);
            }

            throw new NotImplementedException($"No Executable event found for event '{queuedEvent.BaseEvent.name}'");
        }
    }
}
