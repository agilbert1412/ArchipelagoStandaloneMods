using Newtonsoft.Json.Linq;
using StardewModdingAPI;
using StardewViewerEvents.Events.Constants;
using StardewViewerEvents.EventsExecution;
using StardewViewerEvents.EventsExecution.EventsImplementations.BombEvents;
using StardewViewerEvents.EventsExecution.EventsImplementations.BuffEvents;
using StardewViewerEvents.EventsExecution.EventsImplementations.CharacterEvents;
using StardewViewerEvents.EventsExecution.EventsImplementations.CropEvents;
using StardewViewerEvents.EventsExecution.EventsImplementations.DebrisEvents;
using StardewViewerEvents.EventsExecution.EventsImplementations.DurationEvents;
using StardewViewerEvents.EventsExecution.EventsImplementations.InventoryEvents;
using StardewViewerEvents.EventsExecution.EventsImplementations.ItemEvents;
using StardewViewerEvents.EventsExecution.EventsImplementations.MailEvents;
using StardewViewerEvents.EventsExecution.EventsImplementations.MenuEvents;
using StardewViewerEvents.EventsExecution.EventsImplementations.PlayerEvents.EmoteEvents;
using StardewViewerEvents.EventsExecution.EventsImplementations.PlayerEvents.PlayerCharacterEvents;
using StardewViewerEvents.EventsExecution.EventsImplementations.SoundEvents;
using StardewViewerEvents.EventsExecution.EventsImplementations.StatsChangeEvents;
using StardewViewerEvents.EventsExecution.EventsImplementations.TeleportEvents;
using StardewViewerEvents.EventsExecution.EventsImplementations.WeatherEvents;
using OpenMenuEvent = StardewViewerEvents.EventsExecution.EventsImplementations.MenuEvents.OpenMenuEvent;

namespace StardewViewerEvents.Events
{
    public class ViewerEvent
    {
        public string name;
        public int cost; // The current cost
        public int bank; // The currently donated credits contributing to the next activation
        public bool stackable;
        public bool queueable;
        public bool hasParameters;
        public string alignment; // "positive", "negative" or "neutral"
        public string description;
        public string descriptionAnsi; // the description with coloring!

        public ViewerEvent()
        {
        }

        public ViewerEvent(JObject data)
        {
            name = data["name"].ToString();
            cost = int.Parse(data["cost"].ToString());
            bank = int.Parse(data["bank"].ToString());
            stackable = bool.Parse(data["stackable"].ToString());
            queueable = bool.Parse(data["queueable"].ToString());
            hasParameters = bool.Parse(data["hasParameters"].ToString());
            alignment = data["alignment"].ToString();
            description = data["description"].ToString();

            descriptionAnsi = data.ContainsKey("descriptionAnsi") && !string.IsNullOrWhiteSpace(data["descriptionAnsi"].ToString()) ? data["descriptionAnsi"].ToString() : description;
        }

        public int CheckCost()
        {
            return bank / cost;
        }

        public void CallEvent(double multiplier)
        {
            bank -= GetMultiplierCost(multiplier);
        }

        public int GetCostToNextActivation(double multiplier)
        {
            return GetMultiplierCost(multiplier) - GetBank();
        }

        public int GetBank()
        {
            return bank;
        }

        public bool IsStackable()
        {
            return stackable;
        }

        public bool IsQueueable()
        {
            return queueable;
        }

        public void AddToBank(int amountToAdd)
        {
            bank += amountToAdd;
        }

        public void SetBank(int newBank)
        {
            bank = newBank;
        }

        public int GetMultiplierCost(double multiplier)
        {
            return (int)Math.Ceiling(cost * multiplier);
        }

        public void SetCost(int newCost)
        {
            cost = newCost;
        }

        public void SetCostWithMultiplier(int newCost, double multiplier)
        {
            cost = (int)Math.Round(newCost / multiplier);
        }

        
        
        
        
        public ExecutableEvent GetExecutableEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent)
        {
            switch (queuedEvent.BaseEvent.name)
            {
                case EventName.TELEPORT_HOME:
                    return new TeleportHomeEvent(logger, modHelper, queuedEvent);
                case EventName.TELEPORT_MAP:
                    return new TeleportMapEvent(logger, modHelper, queuedEvent);
                case EventName.TELEPORT_NEAR:
                    return new TeleportNearbyEvent(logger, modHelper, queuedEvent);
                case EventName.TELEPORT_RANDOM:
                    return new TeleportRandomEvent(logger, modHelper, queuedEvent);
                case EventName.ITEM_GET_RANDOM:
                    return new GetRandomItemEvent(logger, modHelper, queuedEvent);
                case EventName.ITEM_GET_SPECIFIC:
                    return new GetSpecificItemEvent(logger, modHelper, queuedEvent);
                case EventName.ITEM_GET_EXOTIC:
                    return new GetExoticItemEvent(logger, modHelper, queuedEvent);
                case EventName.ITEM_FILL_INVENTORY:
                    return new FillInventoryEvent(logger, modHelper, queuedEvent);
                case EventName.ITEM_DROP:
                    return new ItemDropEvent(logger, modHelper, queuedEvent);
                case EventName.ITEM_HOME:
                    return new ItemHomeEvent(logger, modHelper, queuedEvent);
                case EventName.ITEM_SELL:
                    return new ItemSellEvent(logger, modHelper, queuedEvent);
                case EventName.ITEM_SHIP:
                    return new ItemShipEvent(logger, modHelper, queuedEvent);
                case EventName.ITEM_TRASH:
                    return new ItemTrashEvent(logger, modHelper, queuedEvent);
                case EventName.ITEM_QUALITY_DOWN:
                    return new ItemQualityDownEvent(logger, modHelper, queuedEvent);
                case EventName.ITEM_QUALITY_UP:
                    return new ItemQualityUpEvent(logger, modHelper, queuedEvent);
                case EventName.SPAWN_MONSTER_RANDOM:
                    return new SpawnRandomMonsterEvent(logger, modHelper, queuedEvent);
                case EventName.SPAWN_MONSTER_SPECIFIC:
                    return new SpawnSpecificMonsterEvent(logger, modHelper, queuedEvent);
                case EventName.TEMPORARY_BABY:
                    return new SpawnTemporaryBabyEvent(logger, modHelper, queuedEvent);
                case EventName.NEW_BABY:
                    return new BirthBabyEvent(logger, modHelper, queuedEvent);
                case EventName.BYE_BABY:
                    return new DoveBabyEvent(logger, modHelper, queuedEvent);
                case EventName.DEBRIS_RANDOM:
                    return new RandomDebrisEvent(logger, modHelper, queuedEvent);
                case EventName.DEBRIS_BOULDER:
                    return new BoulderEvent(logger, modHelper, queuedEvent);
                case EventName.DEBRIS_TREE:
                    return new TreeEvent(logger, modHelper, queuedEvent);
                case EventName.DEBRIS_O_SHAPE:
                    return new OShapeDebrisEvent(logger, modHelper, queuedEvent);
                case EventName.DEBRIS_X_SHAPE:
                    return new XShapeDebrisEvent(logger, modHelper, queuedEvent);
                case EventName.SMALL_BOMB:
                    return new SmallBombEvent(logger, modHelper, queuedEvent);
                case EventName.BOMB:
                    return new MediumBombEvent(logger, modHelper, queuedEvent);
                case EventName.BIG_BOMB:
                    return new BigBombEvent(logger, modHelper, queuedEvent);
                case EventName.SOUND_MEOW:
                    return new PlayMeowEvent(logger, modHelper, queuedEvent);
                case EventName.SOUND_BARK:
                    return new PlayBarkEvent(logger, modHelper, queuedEvent);
                case EventName.SOUND_SPECIFIC:
                    return new PlaySpecificSoundEvent(logger, modHelper, queuedEvent);
                case EventName.SOUND_RANDOM:
                    return new PlayRandomSoundEvent(logger, modHelper, queuedEvent);
                case EventName.FISH_BITE:
                    return new FishBiteEvent(logger, modHelper, queuedEvent);
                case EventName.WEATHER_RANDOM:
                    return new RandomWeatherEvent(logger, modHelper, queuedEvent);
                case EventName.WEATHER_SPECIFIC:
                    return new SpecificWeatherEvent(logger, modHelper, queuedEvent);
                case EventName.SEND_CUSTOM_MAIL:
                    return new CustomMailEvent(logger, modHelper, queuedEvent);
                case EventName.EMOTE_RANDOM:
                    return new RandomEmoteEvent(logger, modHelper, queuedEvent);
                case EventName.EMOTE_SPECIFIC:
                    return new SpecificEmoteEvent(logger, modHelper, queuedEvent);
                case EventName.INVISIBLE_COWS:
                    return new SpawnInvisibleCowEvent(logger, modHelper, queuedEvent);
                case EventName.THEMATIC_FULL_OUTFIT:
                    return new ThemedOutfitEvent(logger, modHelper, queuedEvent);
                case EventName.RANDOMIZE_FULL_OUTFIT:
                    return new RandomizeFullOutfitEvent(logger, modHelper, queuedEvent);
                case EventName.RANDOMIZE_ONE_OUTFIT_PART:
                    return new RandomizeOutfitPartEvent(logger, modHelper, queuedEvent);
                case EventName.CHANGE_GENDER:
                    return new ChangeGenderEvent(logger, modHelper, queuedEvent);
                case EventName.RANDOMIZE_PROFESSIONS:
                    return new RandomizeProfessionsEvent(logger, modHelper, queuedEvent);
                case EventName.CHANGE_FAVORITE_THING:
                    return new ChangeFavoriteThingEvent(logger, modHelper, queuedEvent);
                case EventName.MONEY_ADD:
                    return new AddMoneyEvent(logger, modHelper, queuedEvent);
                case EventName.MONEY_REMOVE:
                    return new RemoveMoneyEvent(logger, modHelper, queuedEvent);
                case EventName.HEALTH_ADD:
                    return new AddHealthEvent(logger, modHelper, queuedEvent);
                case EventName.HEALTH_REMOVE:
                    return new RemoveHealthEvent(logger, modHelper, queuedEvent);
                case EventName.STAMINA_ADD:
                    return new AddStaminaEvent(logger, modHelper, queuedEvent);
                case EventName.STAMINA_REMOVE:
                    return new RemoveStaminaEvent(logger, modHelper, queuedEvent);
                case EventName.TIME_FORWARD:
                    return new TimeForwardsEvent(logger, modHelper, queuedEvent);
                case EventName.TIME_BACKWARDS:
                    return new TimeBackwardsEvent(logger, modHelper, queuedEvent);
                case EventName.OPEN_MENU:
                    return new OpenMenuEvent(logger, modHelper, queuedEvent);
                case EventName.CROW:
                    return new CrowEvent(logger, modHelper, queuedEvent);
                case EventName.BENJAMIN_BUDTON:
                    return new UngrowEvent(logger, modHelper, queuedEvent);
                case EventName.DROUGHT:
                    return new DroughtEvent(logger, modHelper, queuedEvent);
                case EventName.NUDGE:
                    return new NudgeEvent(logger, modHelper, queuedEvent);
                case EventName.SHUFFLE_INVENTORY:
                    return new ShuffleInventoryEvent(logger, modHelper, queuedEvent);
                case EventName.SHUFFLE_EVERYWHERE:
                    return new ShuffleEverywhereEvent(logger, modHelper, queuedEvent);
                case EventName.REVERSE_CONTROLS:
                    return new ReverseControlsEvent(logger, modHelper, queuedEvent);
                case EventName.RANDOM_BUFF:
                    return new RandomBuffEvent(logger, modHelper, queuedEvent);
                case EventName.SPECIFIC_BUFF:
                    return new SpecificBuffEvent(logger, modHelper, queuedEvent);
            }

            throw new NotImplementedException($"No Executable event found for event '{queuedEvent.BaseEvent.name}'");
        }
    }
}
