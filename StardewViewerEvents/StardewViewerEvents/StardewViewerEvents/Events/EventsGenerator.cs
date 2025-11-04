using StardewViewerEvents.Events.Constants;
using StardewViewerEvents.EventsExecution.EventsImplementations.CharacterEvents;
using StardewViewerEvents.EventsExecution.EventsImplementations.CropEvents;
using StardewViewerEvents.EventsExecution.EventsImplementations.DebrisEvents;
using StardewViewerEvents.EventsExecution.EventsImplementations.InventoryEvents;
using StardewViewerEvents.EventsExecution.EventsImplementations.StatsChangeEvents;

namespace StardewViewerEvents.Events;

public class EventsGenerator
{
    public List<ViewerEvent> GenerateDefaultEvents()
    {
        var events = new List<ViewerEvent>
        {
            CreateUnstackableEvent(EventName.TELEPORT_HOME, Alignment.NEUTRAL, 10, "Teleport home"),
            CreateUnstackableEvent(EventName.TELEPORT_NEAR, Alignment.NEGATIVE, 5, "Teleport to a random tile on the current map"),
            CreateUnstackableEvent(EventName.TELEPORT_RANDOM, Alignment.NEUTRAL, 15, "Teleport to a random map"),
            CreateParameterUnstackableEvent(EventName.TELEPORT_MAP, Alignment.NEUTRAL, 25, "Teleport to a specific map. Must use stardew internal map names"),

            CreateStackableEvent(EventName.ITEM_GET_RANDOM, Alignment.POSITIVE, 10, "Give a random item"),
            CreateParameterStackableEvent(EventName.ITEM_GET_SPECIFIC, Alignment.POSITIVE, 25, "Give a specific item. Can use either an item ID, a Qualified item ID, or an item name"),
            CreateUnqueueableEvent(EventName.ITEM_FILL_INVENTORY, Alignment.NEUTRAL, 50, "Fills inventory with random trash-tier items"),
            CreateStackableEvent(EventName.ITEM_GET_EXOTIC, Alignment.NEGATIVE, 10, "Give an Exotic Double Bed"),

            CreateUnstackableEvent(EventName.ITEM_DROP, Alignment.NEGATIVE, 2, "Drop a random item stack to the ground"),
            CreateUnstackableEvent(EventName.ITEM_HOME, Alignment.NEUTRAL, 10, "Send a random item stack home"),
            CreateUnstackableEvent(EventName.ITEM_SELL, Alignment.NEUTRAL, 30, "Instantly sell a random item stack"),
            CreateUnstackableEvent(EventName.ITEM_SHIP, Alignment.NEUTRAL, 50, "Instantly ship a random item stack"),
            CreateUnstackableEvent(EventName.ITEM_TRASH, Alignment.NEGATIVE, 100, "Instantly delete a random item stack"),
            CreateUnstackableEvent(EventName.ITEM_QUALITY_DOWN, Alignment.NEUTRAL, 20, "Lower the quality of a random item"),
            CreateUnstackableEvent(EventName.ITEM_QUALITY_UP, Alignment.POSITIVE, 10, "Increase the quality of a random item"),

            CreateStackableEvent(EventName.SPAWN_MONSTER_RANDOM, Alignment.NEGATIVE, 5, "Spawn random monster"),
            CreateParameterStackableEvent(EventName.SPAWN_MONSTER_SPECIFIC, Alignment.NEGATIVE, 10, "Spawn specific monster"),
            CreateStackableEvent(EventName.TEMPORARY_BABY, Alignment.NEGATIVE, 4, $"Spawn {SpawnTemporaryBabyEvent.NUMBER_TEMPORARY_BABIES} temporary babies"),
            CreateUnstackableEvent(EventName.NEW_BABY, Alignment.NEUTRAL, 10, "Welcome a new child to the family"),
            CreateUnstackableEvent(EventName.BYE_BABY, Alignment.NEUTRAL, 10, "Choose the childfree life, retroactively"),

            CreateStackableEvent(EventName.DEBRIS_RANDOM, Alignment.NEGATIVE, 5, $"Spawn {RandomDebrisEvent.AMOUNT} random pieces of debris nearby"),
            CreateStackableEvent(EventName.DEBRIS_BOULDER, Alignment.NEGATIVE, 10, "Spawn a boulder nearby"),
            CreateStackableEvent(EventName.DEBRIS_TREE, Alignment.NEGATIVE, 10, "Spawn a tree nearby"),
            CreateUnstackableEvent(EventName.DEBRIS_X_SHAPE, Alignment.NEGATIVE, 10, "Spawn debris in the shape of an X, centered on the player"),
            CreateUnstackableEvent(EventName.DEBRIS_O_SHAPE, Alignment.NEGATIVE, 10, "Spawn debris in the shape of an O, centered on the player"),

            CreateUnstackableEvent(EventName.SMALL_BOMB, Alignment.NEGATIVE, 10, "Spawn a small lit bomb"),
            CreateUnstackableEvent(EventName.BOMB, Alignment.NEGATIVE, 50, "Spawn a medium lit bomb"),
            CreateUnstackableEvent(EventName.BIG_BOMB, Alignment.NEGATIVE, 80, "Spawn a large lit bomb"),

            CreateStackableEvent(EventName.SOUND_MEOW, Alignment.NEUTRAL, 1, "Play a series of meows"),
            CreateStackableEvent(EventName.SOUND_BARK, Alignment.NEUTRAL, 1, "Play a series of barks"),
            CreateParameterStackableEvent(EventName.SOUND_SPECIFIC, Alignment.NEUTRAL, 1, "Play a sound of your choice"),
            CreateStackableEvent(EventName.SOUND_RANDOM, Alignment.NEUTRAL, 1, "Play a random stardew sound"),

            CreateUnstackableEvent(EventName.FISH_BITE, Alignment.NEUTRAL, 10, "Trigger an immediate fish bite, regardless of fishing or not"),

            CreateUnqueueableEvent(EventName.WEATHER_RANDOM, Alignment.NEUTRAL, 3, "Change to a random weather"),
            CreateParameterUnqueueableEvent(EventName.WEATHER_SPECIFIC, Alignment.NEUTRAL, 5, "Change to a specific weather"),

            CreateParameterUnstackableEvent(EventName.SEND_CUSTOM_MAIL, Alignment.NEUTRAL, 10, "Send a letter to the player, you specify the contents"),

            CreateUnstackableEvent(EventName.EMOTE_RANDOM, Alignment.NEUTRAL, 1, "Trigger a random emote"),
            CreateParameterUnstackableEvent(EventName.EMOTE_SPECIFIC, Alignment.NEUTRAL, 2, "Trigger a specific emote. Must use stardew internal emote names"),

            CreateStackableEvent(EventName.INVISIBLE_COWS, Alignment.NEGATIVE, 2, $"Spawn {SpawnInvisibleCowEvent.NUMBER_COWS_PER_PURCHASE} invisible cows on the current map. They despawn slowly over days"),

            CreateUnqueueableEvent(EventName.THEMATIC_FULL_OUTFIT, Alignment.NEUTRAL, 8, "Change the player's entire outfit, based on an Emily theme"),
            CreateUnqueueableEvent(EventName.RANDOMIZE_FULL_OUTFIT, Alignment.NEUTRAL, 6, "Randomize the player's entire appearance"),
            CreateParameterUnqueueableEvent(EventName.RANDOMIZE_ONE_OUTFIT_PART, Alignment.NEUTRAL, 2, "Randomize either pants, shirt, hair"),
            CreateUnqueueableEvent(EventName.CHANGE_GENDER, Alignment.NEUTRAL, 4, "Switch the player's gender"),
            CreateUnqueueableEvent(EventName.RANDOMIZE_PROFESSIONS, Alignment.NEUTRAL, 20, "Randomize all professions"),
            CreateParameterUnqueueableEvent(EventName.CHANGE_FAVORITE_THING, Alignment.NEUTRAL, 8, "Change the player's favorite thing to a specific value"),
            // CreateUnqueueableEvent(EventName.RANDOM_NAME, Alignment.NEUTRAL, 5, ""),

            CreateStackableEvent(EventName.MONEY_ADD, Alignment.POSITIVE, 1, $"Gain {AddMoneyEvent.AMOUNT_ADDED}g"),
            CreateStackableEvent(EventName.MONEY_REMOVE, Alignment.NEGATIVE, 1, $"Lose {RemoveMoneyEvent.AMOUNT_REMOVED}g"),
            CreateUnstackableEvent(EventName.HEALTH_ADD, Alignment.POSITIVE, 1, $"Gain {AddHealthEvent.AMOUNT_ADDED} health"),
            CreateUnstackableEvent(EventName.HEALTH_REMOVE, Alignment.NEGATIVE, 1, $"Lose {RemoveHealthEvent.AMOUNT_REMOVED} health"),
            CreateUnstackableEvent(EventName.STAMINA_ADD, Alignment.POSITIVE, 1, $"Gain {AddStaminaEvent.AMOUNT_ADDED} stamina"),
            CreateUnstackableEvent(EventName.STAMINA_REMOVE, Alignment.NEGATIVE, 1, $"Lose {RemoveStaminaEvent.AMOUNT_REMOVED} stamina"),
            CreateStackableEvent(EventName.TIME_FORWARD, Alignment.POSITIVE, 1, $"Advance time by {TimeForwardsEvent.MINUTES_FORWARD} minutes"),
            CreateStackableEvent(EventName.TIME_BACKWARDS, Alignment.NEGATIVE, 1, $"Move time backwards by {TimeBackwardsEvent.MINUTES_BACKWARDS} minutes"),

            CreateUnstackableEvent(EventName.OPEN_MENU, Alignment.NEGATIVE, 1, $"Open a random menu"),

            CreateStackableEvent(EventName.CROW, Alignment.NEGATIVE, 10, $"Instantly send {CrowEvent.NUMBER_CROWS} to try to eat crops. Each crow has a {(int)(CrowEvent.SCARECROW_EFFICIENCY * 100)}% chance of getting stopped by each scarecrow in range."),
            CreateStackableEvent(EventName.BENJAMIN_BUDTON, Alignment.NEGATIVE, 10, $"Instantly degrow {UngrowEvent.NUMBER_CROPS} by {UngrowEvent.NUMBER_DAYS}"),
            CreateStackableEvent(EventName.DROUGHT, Alignment.NEGATIVE, 5, $"Instantly unwater {DroughtEvent.NUMBER_UNWATER} crops and remove {DroughtEvent.AMOUNT_REMOVED_FROM_CAN} water from the watering can"),

            CreateStackableEvent(EventName.NUDGE, Alignment.NEGATIVE, 5, $"Perform {NudgeEvent.NUMBER_NUDGES} nudges to random chests"),
            CreateStackableEvent(EventName.SHUFFLE_INVENTORY, Alignment.NEGATIVE, 4, $"Perform {ShuffleInventoryEvent.NUMBER_SWAPS} swaps between two random inventory items"),
            CreateStackableEvent(EventName.SHUFFLE_EVERYWHERE, Alignment.NEGATIVE, 30, $"Perform {ShuffleEverywhereEvent.NUMBER_SWAPS} swaps between two random items anywhere in the world"),

            //CreateEvent(EventName.SLEEP_ONCE, Alignment.NEUTRAL, 20, "Go to sleep", false),
            //CreateEvent(EventName.SLEEP_WEEK, Alignment.NEGATIVE, 200, "Sleep a whole week", false),
            //CreateEvent(EventName.SLEEP_MONTH, Alignment.NEGATIVE, 1000, "Sleep a whole month", false),
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