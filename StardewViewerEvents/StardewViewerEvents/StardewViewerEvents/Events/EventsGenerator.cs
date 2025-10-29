using StardewViewerEvents.Events.Constants;

namespace StardewViewerEvents.Events;

public class EventsGenerator
{
    public List<ViewerEvent> GenerateDefaultEvents()
    {
        var events = new List<ViewerEvent>
        {
            new()
            {
                name = EventName.ITEM_ADD,
                alignment = Alignment.POSITIVE,
                cost = 500,
                description = "Give an item to the player",
            },
            new()
            {
                name = EventName.ITEM_REMOVE,
                alignment = Alignment.NEGATIVE,
                cost = 200,
                description = "Take away an item from the player",
            },
            new()
            {
                name = EventName.TELEPORT,
                alignment = Alignment.NEUTRAL,
                cost = 50,
                description = "Take away an item from the player",
            },
        };

        return events;
    }
}