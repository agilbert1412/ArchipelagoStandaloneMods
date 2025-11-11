using StardewValley;
using StardewValley.GameData.Pants;

namespace StardewViewerEvents.Extensions
{
    public static class ItemUtility
    {
        public static bool ItemExists(string desiredItem)
        {
            return TryFindItem(desiredItem, out _);
        }

        public static bool TryFindItem(string desiredItem, out Item item)
        {
            item = ItemRegistry.Create(desiredItem, allowNull: true);
            if (item != null)
            {
                return true;
            }

            desiredItem = desiredItem.SanitizeEntityName();
            item = ItemRegistry.Create(desiredItem, allowNull: true);
            if (item != null)
            {
                return true;
            }

            var objects = DataLoader.Objects(Game1.content);
            foreach (var (id, data) in objects)
            {
                if (data.Name.SanitizeEntityName() == desiredItem)
                {
                    item = ItemRegistry.Create($"(O){id}");
                    return true;
                }
            }

            var bigCraftables = DataLoader.BigCraftables(Game1.content);
            foreach (var (id, data) in bigCraftables)
            {
                if (data.Name.SanitizeEntityName() == desiredItem)
                {
                    item = ItemRegistry.Create($"(BC){id}");
                    return true;
                }
            }

            var furnitures = DataLoader.Furniture(Game1.content);
            foreach (var (id, data) in furnitures)
            {
                if (data.Split("/").First().SanitizeEntityName() == desiredItem)
                {
                    item = ItemRegistry.Create($"(F){id}");
                    return true;
                }
            }

            var hats = DataLoader.Hats(Game1.content);
            foreach (var (id, data) in hats)
            {
                if (data.Split("/").First().SanitizeEntityName() == desiredItem)
                {
                    item = ItemRegistry.Create($"(H){id}");
                    return true;
                }
            }

            var pants = DataLoader.Pants(Game1.content);
            foreach (var (id, data) in pants)
            {
                if (data.Name.SanitizeEntityName() == desiredItem)
                {
                    item = ItemRegistry.Create($"(P){id}");
                    return true;
                }
            }

            var shirts = DataLoader.Shirts(Game1.content);
            foreach (var (id, data) in shirts)
            {
                if (data.Name.SanitizeEntityName() == desiredItem)
                {
                    item = ItemRegistry.Create($"(S){id}");
                    return true;
                }
            }

            item = null;
            return false;
        }
    }
}
