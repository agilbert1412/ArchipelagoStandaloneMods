using StardewViewerEvents.Events.Constants;

namespace StardewViewerEvents.Events;

public class EventsGenerator
{
    public List<ViewerEvent> GenerateDefaultEvents()
    {
        var events = new List<ViewerEvent>
        {
            CreateEvent(EventName.ITEM_ADD, Alignment.POSITIVE, 500, "Give an item to the player"),
            CreateEvent(EventName.ITEM_REMOVE, Alignment.NEGATIVE, 200, "Take away an item from the player"),
            CreateEvent(EventName.TELEPORT, Alignment.NEUTRAL, 50, "Take away an item from the player"),
        };

        return events;
    }

    private ViewerEvent CreateEvent(string name, string alignment, int cost, string description, string descriptionAnsi = null)
    {
        return new ViewerEvent
        {
            name = name,
            alignment = alignment,
            cost = cost,
            bank = 0,
            description = description,
            descriptionAnsi = descriptionAnsi ?? description,
        };
    }
}