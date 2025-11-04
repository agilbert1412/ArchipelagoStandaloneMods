using StardewModdingAPI;
using StardewValley;
using StardewValley.Inventories;
using StardewValley.Locations;
using StardewValley.Objects;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.InventoryEvents
{
    public class InventoryEvent : ExecutableEvent
    {

        public InventoryEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        protected List<Chest> GetAllChests()
        {
            var allLocations = Game1.locations.ToList();
            allLocations.AddRange(Game1.getFarm().buildings.Where(building => building?.indoors.Value != null).Select(building => building.indoors.Value));

            var allChests = new List<Chest>();
            foreach (var location in allLocations)
            {
                foreach (var gameObject in location.Objects.Values.ToArray())
                {
                    if (gameObject is not Chest chest)
                    {
                        continue;
                    }

                    allChests.Add(chest);
                }
            }

            return allChests;
        }

        protected List<Inventory> GetAllInventoriesWithItems()
        {
            var inventories = new List<Inventory>();
            inventories.Add(Game1.player.Items);
            var chests = GetAllChests();
            foreach (var chest in chests)
            {
                if (chest.Items.Any(x => x != null))
                {
                    inventories.Add(chest.Items);
                }
            }

            if (Game1.getLocationFromName("FarmHouse") is not FarmHouse farmHouse)
            {
                return null;
            }

            var farmFridge = farmHouse.fridge.Value;
            if (farmFridge != null && farmFridge.Items.Any(x => x != null))
            {
                inventories.Add(farmFridge.Items);
            }

            if (Game1.getLocationFromName("IslandFarmHouse") is not IslandFarmHouse islandHouse)
            {
                return null;
            }

            var islandFridge = islandHouse.fridge.Value;
            if (islandFridge != null && islandFridge.Items.Any(x => x != null))
            {
                inventories.Add(islandFridge.Items);
            }


            return inventories;
        }

        protected Dictionary<Item, Tuple<Inventory, int>> GenerateItemMap(List<Inventory> inventories)
        {
            var itemMap = new Dictionary<Item, Tuple<Inventory, int>>();
            foreach (var inventory in inventories)
            {
                for (var index = 0; index < inventory.Count; index++)
                {
                    var item = inventory[index];
                    if (item == null || item.Stack <= 0)
                    {
                        continue;
                    }
                    itemMap.Add(item, new Tuple<Inventory, int>(inventory, index));
                }
            }

            return itemMap;
        }

        protected void SwapTwoItems(Dictionary<Item, Tuple<Inventory, int>> itemMap)
        {
            var validItemsToSwap = itemMap.Keys.ToArray();
            var totalCount = validItemsToSwap.Length;
            var indexSwap1 = Game1.random.Next(totalCount);
            var indexSwap2 = Game1.random.Next(totalCount);
            if (indexSwap1 == indexSwap2)
            {
                return;
            }

            var item1 = validItemsToSwap[indexSwap1];
            var item2 = validItemsToSwap[indexSwap2];
            var (inventory1, index1) = itemMap[item1];
            var (inventory2, index2) = itemMap[item2];
            inventory1[index1] = item2;
            inventory2[index2] = item1;
            itemMap[item1] = new Tuple<Inventory, int>(inventory2, index2);
            itemMap[item2] = new Tuple<Inventory, int>(inventory1, index1);
        }
    }
}
