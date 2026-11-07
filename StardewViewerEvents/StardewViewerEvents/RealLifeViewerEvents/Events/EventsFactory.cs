using RealLifeViewerEvents.Events.Constants;
using RealLifeViewerEvents.EventsExecution;
using RealLifeViewerEvents.EventsExecution.EventsImplementations;
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
                case EventName.MOVIE_RECAP:
                    return new MovieRecapEvent(logger, queuedEvent);
                case EventName.OVEN_MITTS:
                    return new OvenMittsEvent(logger, queuedEvent);
                case EventName.DOGGO:
                    return new DoggoEvent(logger, queuedEvent);
                case EventName.PUSH_UPS:
                    return new PushUpEvent(logger, queuedEvent);
                case EventName.HORNET_MASK:
                    return new MaskEvent(logger, queuedEvent);
                case EventName.BLINDFOLD:
                    return new BlindfoldEvent(logger, queuedEvent);
                case EventName.SILENCE:
                    return new SilenceEvent(logger, queuedEvent);
                case EventName.MUTE:
                    return new MuteEvent(logger, queuedEvent);
                case EventName.NO_KEYBOARD:
                    return new NoKeyboardEvent(logger, queuedEvent);
                case EventName.NO_MOUSE:
                    return new NoMouseEvent(logger, queuedEvent);
                case EventName.INVERT_HANDS:
                    return new InvertHandsEvent(logger, queuedEvent);
                case EventName.CALL_A_FRIEND:
                    return new CallAFriendEvent(logger, queuedEvent);
                case EventName.HYDRATE:
                    return new HydrateEvent(logger, queuedEvent);
                case EventName.SPLASH:
                    return new SplashEvent(logger, queuedEvent);
                case EventName.WORD_BAN:
                    return new WordBanEvent(logger, queuedEvent);
                case EventName.PIRATE_SPEAK:
                    return new PirateSpeakEvent(logger, queuedEvent);
                case EventName.NYAN_ACCENT:
                    return new NyanAccentEvent(logger, queuedEvent);
                case EventName.OPTIMIST:
                    return new OptimistEvent(logger, queuedEvent);
                case EventName.PESSIMIST:
                    return new PessimistEvent(logger, queuedEvent);
                case EventName.HYPE:
                    return new HypeEvent(logger, queuedEvent);
                case EventName.REVERSE_SHIRT:
                    return new ReverseShirtEvent(logger, queuedEvent);
                case EventName.PICK_NEXT_GAME:
                    return new PickNextGameEvent(logger, queuedEvent);
            }

            throw new NotImplementedException($"No Executable event found for event '{queuedEvent.BaseEvent.name}'");
        }
    }
}
