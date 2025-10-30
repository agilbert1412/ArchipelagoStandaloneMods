using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Inventories;
using StardewValley.Objects;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.ItemEvents
{
    public class ItemHomeEvent : InventoryItemEvent
    {
        public ItemHomeEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        protected override void ExecuteEvent(int slotToModify)
        {
            var item = Game1.player.Items[slotToModify];
            if (!TrySendItemHome(item))
            {
                var point = Game1.getFarm().GetMainFarmHouseEntry();
                Game1.createItemDebris(item, new Vector2(point.X * 64, point.Y * 64), Game1.player.FacingDirection, Game1.getFarm());
                Game1.player.removeItemFromInventory(item);
            }

            Game1.player.removeItemFromInventory(item);
        }

        private bool TrySendItemHome(Item item)
        {
            var maxStack = item.maximumStackSize();
            var farm = Game1.getFarm();
            var locations = new List<GameLocation> { farm, Game1.getLocationFromName("FarmHouse") };
            foreach (var building in farm.buildings)
            {
                if (building?.indoors.Value == null)
                {
                    continue;
                }
                locations.Add(building.indoors.Value);
            }

            foreach (var gameLocation in locations)
            {
                if (TryStackItemInLocationInventories(gameLocation, item, maxStack))
                {
                    return true;
                }
            }

            foreach (var gameLocation in locations)
            {
                if (TryPlaceItemInLocationInventories(gameLocation, item, maxStack))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool TryStackItemInLocationInventories(GameLocation gameLocation, Item item, int maxStack)
        {
            foreach (var (tile, tileObject) in gameLocation.objects.Pairs)
            {
                if (tileObject is Chest chest)
                {
                    if (TryStackItemInInventory(chest.Items, item, maxStack))
                    {
                        return true;
                    }
                }
            }

            var fridge = gameLocation.GetFridge();
            if (fridge != null)
            {
                if (TryStackItemInInventory(fridge.Items, item, maxStack))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool TryStackItemInInventory(Inventory inventory, Item item, int maxStack)
        {
            foreach (var chestItem in inventory)
            {
                if (chestItem.canStackWith(item) && chestItem.Stack + item.Stack < maxStack)
                {
                    chestItem.Stack += item.Stack;
                    return true;
                }
            }

            return false;
        }

        private static bool TryPlaceItemInLocationInventories(GameLocation gameLocation, Item item, int maxStack)
        {
            foreach (var (tile, tileObject) in gameLocation.objects.Pairs)
            {
                if (tileObject is Chest chest)
                {
                    if (TryPlaceItemInChest(chest, item, maxStack))
                    {
                        return true;
                    }
                }
            }

            var fridge = gameLocation.GetFridge();
            if (fridge != null)
            {
                if (TryPlaceItemInChest(fridge, item, maxStack))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool TryPlaceItemInChest(Chest chest, Item item, int maxStack)
        {
            if (chest.Items.Count(x => x is { Stack: >= 1 }) < chest.GetActualCapacity())
            {
                chest.addItem(item);
                return true;
            }

            return false;
        }
    }
}
