using Newtonsoft.Json.Linq;
using StardewModdingAPI;
using StardewViewerEvents.Events.Constants;
using StardewViewerEvents.EventsExecution;
using StardewViewerEvents.EventsExecution.EventsImplementations;
using StardewViewerEvents.EventsExecution.EventsImplementations.ItemEvents;
using StardewViewerEvents.EventsExecution.EventsImplementations.Teleport;

namespace StardewViewerEvents.Events
{
    public class ViewerEvent
    {
        public string name;
        public int cost; // The current cost
        public int bank; // The currently donated credits contributing to the next activation
        public bool stackable;
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
            }

            throw new NotImplementedException($"No Executable event found for event '{queuedEvent.BaseEvent.name}'");
        }
    }
}
