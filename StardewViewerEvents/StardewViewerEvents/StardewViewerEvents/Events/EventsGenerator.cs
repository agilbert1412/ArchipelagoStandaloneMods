using StardewViewerEvents.Events.Constants;

namespace StardewViewerEvents.Events;

public class EventsGenerator
{
    public List<ViewerEvent> GenerateDefaultEvents()
    {
        var events = new List<ViewerEvent>
        {
            CreateEvent(EventName.TELEPORT_HOME, Alignment.NEUTRAL, 10, "Teleport home", false),
            CreateEvent(EventName.TELEPORT_NEAR, Alignment.NEGATIVE, 5, "Teleport to a random tile on the current map", false),
            CreateEvent(EventName.TELEPORT_RANDOM, Alignment.NEUTRAL, 15, "Teleport to a random map", false),
            CreateEvent(EventName.TELEPORT_MAP, Alignment.NEUTRAL, 25, "Teleport to a specific map. Must use stardew internal map names", false, true),

            CreateEvent(EventName.ITEM_GET_RANDOM, Alignment.POSITIVE, 10, "Give a random item", false),
            CreateEvent(EventName.ITEM_GET_SPECIFIC, Alignment.POSITIVE, 25, "Give a specific item. Can use either an item ID, a Qualified item ID, or an item name", false, true),
            CreateEvent(EventName.ITEM_FILL_INVENTORY, Alignment.NEUTRAL, 50, "Fills inventory with random trash-tier items", false),
            CreateEvent(EventName.ITEM_GET_EXOTIC, Alignment.NEGATIVE, 10, "Give an Exotic Double Bed", false),

            CreateEvent(EventName.ITEM_HOME, Alignment.NEUTRAL, 10, "Send a random item stack home", false),
            CreateEvent(EventName.ITEM_SELL, Alignment.NEUTRAL, 30, "Instantly sell a random item stack", false),
            CreateEvent(EventName.ITEM_SHIP, Alignment.NEUTRAL, 50, "Instantly ship a random item stack", false),
            CreateEvent(EventName.ITEM_TRASH, Alignment.NEGATIVE, 100, "Instantly delete a random item stack", false),
            CreateEvent(EventName.ITEM_DROP, Alignment.NEGATIVE, 2, "Drop a random item stack to the ground", false),
            CreateEvent(EventName.ITEM_QUALITY_DOWN, Alignment.NEUTRAL, 20, "Lower the quality of a random item", false),
            CreateEvent(EventName.ITEM_QUALITY_UP, Alignment.POSITIVE, 10, "Increase the quality of a random item", false),

            CreateEvent(EventName.SPAWN_MONSTER_RANDOM, Alignment.NEGATIVE, 5, "Spawn random monster", false),
            CreateEvent(EventName.SPAWN_MONSTER_SPECIFIC, Alignment.NEGATIVE, 10, "Spawn specific monster", false, true),
            CreateEvent(EventName.TEMPORARY_BABY, Alignment.NEGATIVE, 1, "Spawn a temporary baby", false),
            CreateEvent(EventName.NEW_BABY, Alignment.NEUTRAL, 10, "Welcome a new child to the family", false),
            CreateEvent(EventName.BYE_BABY, Alignment.NEUTRAL, 10, "Choose the childfree life, retroactively", false),

            CreateEvent(EventName.DEBRIS_RANDOM, Alignment.NEGATIVE, 2, "Spawn a random piece of debris nearby", false),
            CreateEvent(EventName.DEBRIS_BOULDER, Alignment.NEGATIVE, 10, "Spawn a boulder nearby", false),
            CreateEvent(EventName.DEBRIS_TREE, Alignment.NEGATIVE, 10, "Spawn a tree nearby", false),
            CreateEvent(EventName.DEBRIS_X_SHAPE, Alignment.NEGATIVE, 30, "Spawn debris in the shape of an X, centered on the player", false),
            CreateEvent(EventName.DEBRIS_O_SHAPE, Alignment.NEGATIVE, 30, "Spawn debris in the shape of an O, centered on the player", false),

            CreateEvent(EventName.SOUND_MEOW, Alignment.NEUTRAL, 1, "Play a meow", false),
            CreateEvent(EventName.SOUND_BARK, Alignment.NEUTRAL, 1, "Play a bark", false),
            CreateEvent(EventName.SOUND_SPECIFIC, Alignment.NEUTRAL, 2, "Play a sound of your choice. Must use a stardew internal sound name", false, true),

            CreateEvent(EventName.SLEEP_ONCE, Alignment.NEUTRAL, 20, "Go to sleep", false),
            CreateEvent(EventName.SLEEP_WEEK, Alignment.NEGATIVE, 200, "Sleep a whole week", false),
            CreateEvent(EventName.SLEEP_MONTH, Alignment.NEGATIVE, 1000, "Sleep a whole month", false),

            CreateEvent(EventName.BITE_RANDOM, Alignment.NEUTRAL, 10, "Trigger an immediate fish bite, regardless of fishing or not", false),

            CreateEvent(EventName.WEATHER_RANDOM, Alignment.NEUTRAL, 5, "Change to a random weather", false),

            CreateEvent(EventName.SHOW_MAIL, Alignment.NEUTRAL, 10, "Send a letter to the player, you specify the contents", false, true),

            CreateEvent(EventName.EMOTE_RANDOM, Alignment.NEUTRAL, 10, "Trigger a random emote", false),
            CreateEvent(EventName.EMOTE_SPECIFIC, Alignment.NEUTRAL, 10, "Trigger a specific emote. Must use stardew internal emote names", false, true),

            CreateEvent(EventName.INVISIBLE_COWS, Alignment.NEGATIVE, 2, "Spawn one invisible cow on the current map", false),
        };

        return events;
    }

    private ViewerEvent CreateEvent(string name, string alignment, int cost, string description, bool stackable, bool hasParameters = false, string descriptionAnsi = null)
    {
        return new ViewerEvent
        {
            name = name,
            alignment = alignment,
            cost = cost,
            stackable = stackable,
            hasParameters = hasParameters,
            bank = 0,
            description = description,
            descriptionAnsi = descriptionAnsi ?? description,
        };
    }
}