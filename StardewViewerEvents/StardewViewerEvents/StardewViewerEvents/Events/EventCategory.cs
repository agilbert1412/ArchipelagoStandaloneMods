using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewViewerEvents.Events.Constants;

namespace StardewViewerEvents.Events
{
    public static class EventCategory
    {
        public const string OTHER = "Other Events";
        public const string TELEPORT = "Teleport Events";
        public const string ITEM = "Item Events";
        public const string SPAWN = "Spawning Events";
        public const string DEBRIS = "Debris Events";
        public const string DESTRUCTION = "Destruction Events";
        public const string SOUND = "Sound Events";
        public const string MENU = "Menu Events";
        public const string EMOTE = "Emote Events";
        public const string CUSTOMIZATION = "Customization Events";
        public const string STATE = "State Events";
        public const string TIME = "Time Events";
        public const string CROPS = "Crop Events";
        public const string INVENTORY = "Inventory Events";
        public const string CONTROLS = "Controls Events";
        public const string WEATHER = "Weather Events";
        public const string BUFF = "Buff Events";

        public static readonly Dictionary<string, string[]> CATEGORY_MAP = new()
        {
            {
                TELEPORT,
                new[]
                {
                    EventName.TELEPORT_HOME, EventName.TELEPORT_MAP, EventName.TELEPORT_NEAR, EventName.TELEPORT_RANDOM,
                }
            },
            {
                ITEM,
                new[]
                {
                    EventName.ITEM_DROP, EventName.ITEM_SELL, EventName.ITEM_SHIP, EventName.ITEM_TRASH,
                    EventName.ITEM_HOME, EventName.ITEM_GET_EXOTIC, EventName.ITEM_GET_RANDOM,
                    EventName.ITEM_GET_SPECIFIC, EventName.ITEM_FILL_INVENTORY, EventName.ITEM_QUALITY_DOWN,
                    EventName.ITEM_QUALITY_UP,
                }
            },
            {
                SPAWN,
                new[]
                {
                    EventName.SPAWN_MONSTER_RANDOM, EventName.SPAWN_MONSTER_SPECIFIC, EventName.TEMPORARY_BABY,
                    EventName.NEW_BABY, EventName.BYE_BABY, EventName.INVISIBLE_COWS,
                }
            },
            {
                DEBRIS,
                new[]
                {
                    EventName.DEBRIS_RANDOM, EventName.DEBRIS_O_SHAPE, EventName.DEBRIS_X_SHAPE, EventName.DEBRIS_TREE,
                    EventName.DEBRIS_BOULDER,
                }
            },
            {
                DESTRUCTION,
                new[]
                {
                    EventName.SMALL_BOMB, EventName.BOMB, EventName.BIG_BOMB,
                }
            },
            {
                WEATHER,
                new[]
                {
                    EventName.WEATHER_RANDOM, EventName.WEATHER_SPECIFIC,
                }
            },
            {
                SOUND,
                new[]
                {
                    EventName.SOUND_MEOW, EventName.SOUND_BARK, EventName.SOUND_RANDOM, EventName.SOUND_SPECIFIC,
                }
            },
            {
                MENU,
                new[]
                {
                    EventName.FISH_BITE, EventName.OPEN_MENU, EventName.SEND_CUSTOM_MAIL,
                }
            },
            {
                EMOTE,
                new[]
                {
                    EventName.EMOTE_RANDOM, EventName.EMOTE_SPECIFIC,
                }
            },
            {
                CUSTOMIZATION,
                new[]
                {
                    EventName.THEMATIC_FULL_OUTFIT, EventName.RANDOMIZE_FULL_OUTFIT,
                    EventName.RANDOMIZE_ONE_OUTFIT_PART, EventName.CHANGE_GENDER, EventName.RANDOMIZE_PROFESSIONS,
                    EventName.CHANGE_FAVORITE_THING,
                }
            },
            {
                STATE,
                new[]
                {
                    EventName.MONEY_ADD, EventName.MONEY_REMOVE, EventName.HEALTH_ADD, EventName.HEALTH_REMOVE,
                    EventName.STAMINA_ADD, EventName.STAMINA_REMOVE,
                }
            },
            {
                TIME,
                new[]
                {
                    EventName.TIME_BACKWARDS, EventName.TIME_FORWARD,
                }
            },
            {
                CROPS,
                new[]
                {
                    EventName.CROW, EventName.BENJAMIN_BUDTON, EventName.DROUGHT,
                }
            },
            {
                INVENTORY,
                new[]
                {
                    EventName.NUDGE, EventName.SHUFFLE_INVENTORY, EventName.SHUFFLE_EVERYWHERE,
                }
            },
            {
                CONTROLS,
                new[]
                {
                    EventName.REVERSE_CONTROLS, 
                }
            },
            {
                BUFF,
                new[]
                {
                    EventName.RANDOM_BUFF, EventName.SPECIFIC_BUFF,
                }
            },
        };
    }
}
