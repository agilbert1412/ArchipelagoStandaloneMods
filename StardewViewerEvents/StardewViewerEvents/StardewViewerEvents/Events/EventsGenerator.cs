﻿using StardewViewerEvents.Events.Constants;
using StardewViewerEvents.EventsExecution.EventsImplementations.CharacterEvents;
using StardewViewerEvents.EventsExecution.EventsImplementations.DebrisEvents;

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
            CreateUnqueueableEvent(EventName.WEATHER_SPECIFIC, Alignment.NEUTRAL, 5, "Change to a specific weather"),

            CreateParameterUnstackableEvent(EventName.SEND_CUSTOM_MAIL, Alignment.NEUTRAL, 10, "Send a letter to the player, you specify the contents"),

            CreateUnstackableEvent(EventName.EMOTE_RANDOM, Alignment.NEUTRAL, 1, "Trigger a random emote"),
            CreateParameterUnstackableEvent(EventName.EMOTE_SPECIFIC, Alignment.NEUTRAL, 2, "Trigger a specific emote. Must use stardew internal emote names"),

            CreateStackableEvent(EventName.INVISIBLE_COWS, Alignment.NEGATIVE, 2, $"Spawn {SpawnInvisibleCowEvent.NUMBER_COWS_PER_PURCHASE} invisible cows on the current map. They despawn slowly over days"),

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