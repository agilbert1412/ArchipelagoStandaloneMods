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
            CreateParameterUnstackableEvent(EventName.KARAOKE_SONG, Alignment.NEUTRAL, 10, "Make me sing a song"),
            //CreateUnstackableEvent(EventName.TELEPORT_HOME, Alignment.NEUTRAL, 10, "Teleport home"),
            //CreateParameterUnstackableEvent(EventName.TELEPORT_MAP, Alignment.NEUTRAL, 50, "Teleport to a specific map. Must use stardew internal map names"),
            //CreateStackableEvent(EventName.ITEM_GET_RANDOM, Alignment.POSITIVE, 10, "Give a random item"),
            //CreateParameterStackableEvent(EventName.ITEM_GET_SPECIFIC, Alignment.POSITIVE, 25, "Give a specific item. Can use either an item ID, a Qualified item ID, or an item name"),
            //CreateUnqueueableEvent(EventName.ITEM_FILL_INVENTORY, Alignment.NEUTRAL, 50, "Fills inventory with random trash-tier items"),
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