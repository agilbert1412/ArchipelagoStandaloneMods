using RealLifeViewerEvents.Events.Constants;
using ViewerEventsLibrary;
using ViewerEventsLibrary.Events;
using ViewerEventsLibrary.Events.Constants;

namespace RealLifeViewerEvents.Events;

public class EventsGenerator : IEventsGenerator
{
    public List<ViewerEvent> GenerateDefaultEvents()
    {
        var events = new List<ViewerEvent>
        {
            CreateParameterUnstackableEvent(EventName.KARAOKE_SONG, Alignment.NEUTRAL, 500, "Make me sing a song. Warning: I sing VERY badly"),
            CreateStackableEvent(EventName.OVEN_MITTS, Alignment.NEGATIVE, 100, "Wear Oven Mitts for 1 minute"),
            CreateUnstackableEvent(EventName.DOGGO, Alignment.POSITIVE, 50, "Show the stream a good boy"),
            CreateStackableEvent(EventName.PUSH_UPS, Alignment.NEGATIVE, 50, "Do one push-up"),
            CreateStackableEvent(EventName.HORNET_MASK, Alignment.NEGATIVE, 50, "Wear a Hornet Mask for 1 minute"),
            CreateStackableEvent(EventName.BLINDFOLD, Alignment.NEGATIVE, 200, "Wear a Blindfold for 30 seconds"),
            CreateStackableEvent(EventName.SILENCE, Alignment.NEGATIVE, 50, "Lose all audio for 1 minute"),
            CreateStackableEvent(EventName.MUTE, Alignment.NEGATIVE, 20, "Mute your Discord microphone for 1 minute (stream mic will still work)"),
            CreateStackableEvent(EventName.NO_KEYBOARD, Alignment.NEGATIVE, 100, "Can't use keyboard for 1 minute"),
            CreateStackableEvent(EventName.NO_MOUSE, Alignment.NEGATIVE, 50, "Can't use mouse for 1 minute"),
            CreateStackableEvent(EventName.INVERT_HANDS, Alignment.NEGATIVE, 50, "Swap hands for mouse and keyboard for 1 minute"),
            CreateStackableEvent(EventName.GAMING_RELAY, Alignment.NEUTRAL, 100, "Give control of the game to someone else for 1 minute"),
            CreateUnstackableEvent(EventName.HYDRATE, Alignment.POSITIVE, 50, "Drink something"),
            // CreateUnstackableEvent(EventName.SPLASH, Alignment.NEGATIVE, 1000, "Splash something on your head"),
            CreateParameterStackableEvent(EventName.WORD_BAN, Alignment.NEUTRAL, 50, "Ban a word for 1 minute. Every time I say the word, you are refunded 1x the base cost of the event"),
            // CreateStackableEvent(EventName.PIRATE_SPEAK, Alignment.NEUTRAL, 100, "Speak like a pirate for 1 minute"),
            // CreateStackableEvent(EventName.NYAN_ACCENT, Alignment.NEUTRAL, 200, "Speak like a catgirl for 1 minute"),
            CreateStackableEvent(EventName.OPTIMIST, Alignment.NEUTRAL, 50, "Be extremely positive and happy for 1 minute"),
            CreateStackableEvent(EventName.PESSIMIST, Alignment.NEUTRAL, 50, "Be extremely negative and grumpy for 1 minute"),
            CreateStackableEvent(EventName.HYPE, Alignment.NEUTRAL, 50, "Be extremely hype and energetic for 1 minute"),
            CreateUnstackableEvent(EventName.REVERSE_SHIRT, Alignment.NEUTRAL, 100, "Reverse your shirt"),
            CreateParameterUnstackableEvent(EventName.PICK_NEXT_GAME, Alignment.NEUTRAL, 1000, "Choose the next game I'll play. I'll try to switch relatively soon. Has to be a game I already own."),
            /*
             *
             */
        };

        return events;
    }

    private ViewerEvent CreateUnstackableEvent(string name, string alignment, int cost, string description)
    {
        return CreateEvent(name, alignment, cost, description, false, true, false);
    }

    private ViewerEvent CreateUnqueueableEvent(string name, string alignment, int cost, string description)
    {
        return CreateEvent(name, alignment, cost, description, false, false, false);
    }

    private ViewerEvent CreateStackableEvent(string name, string alignment, int cost, string description)
    {
        return CreateEvent(name, alignment, cost, description, true, true, false);
    }

    private ViewerEvent CreateParameterUnstackableEvent(string name, string alignment, int cost, string description)
    {
        return CreateEvent(name, alignment, cost, description, false, true, true);
    }

    private ViewerEvent CreateParameterUnqueueableEvent(string name, string alignment, int cost, string description)
    {
        return CreateEvent(name, alignment, cost, description, false, false, true);
    }

    private ViewerEvent CreateParameterStackableEvent(string name, string alignment, int cost, string description)
    {
        return CreateEvent(name, alignment, cost, description, true, true, true);
    }

    private ViewerEvent CreateEvent(string name, string alignment, int cost, string description, bool stackable, bool queueable, bool hasParameters, string descriptionAnsi = null)
    {
        return new ViewerEvent
        {
            name = name,
            alignment = alignment,
            cost = cost,
            stackable = stackable,
            queueable = queueable,
            hasParameters = hasParameters,
            bank = 0,
            description = description,
            descriptionAnsi = descriptionAnsi ?? description,
        };
    }
}