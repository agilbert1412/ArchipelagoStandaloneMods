using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.TerrainFeatures;
using Object = StardewValley.Object;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.DebrisEvents
{
    public class DebrisSpawner
    {
        private const string TWIG_1 = "294";
        private const string TWIG_2 = "295";
        private const string STONE_1 = "343";
        private const string STONE_2 = "450";
        private const string WEEDS = "750";

        public DebrisSpawner()
        {
        }

        public void SpawnManyDebris(int amount)
        {
            var currentLocation = Game1.player.currentLocation;
            SpawnManyDebris(currentLocation, amount);
        }

        public void SpawnManyDebris(GameLocation location, int amount)
        {
            for (var i = 0; i < amount; ++i)
            {
                SpawnSingleDebris(location);
            }
        }

        public void SpawnSingleDebris()
        {
            SpawnSingleDebris(Game1.currentLocation);
        }

        public void SpawnSingleDebris(GameLocation location)
        {
            if (!TryFindTileForDebris(location, false, out var tile))
            {
                return;
            }

            SpawnSingleDebris(location, tile);
        }

        public void SpawnManyTrees(int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                SpawnSingleTree();
            }
        }

        public void SpawnSingleTree()
        {
            SpawnSingleTree(Game1.currentLocation);
        }

        private void SpawnSingleTree(GameLocation location)
        {
            if (!TryFindTileForDebris(location, false, out var tile))
            {
                return;
            }

            SpawnSingleTree(location, tile);
        }

        public void SpawnManyBoulders(int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                SpawnSingleBoulder();
            }
        }

        public void SpawnSingleBoulder()
        {
            SpawnSingleBoulder(Game1.currentLocation);
        }

        public static void SpawnSingleBoulder(GameLocation location)
        {
            if (!TryFindTileForDebris(location, true, out var tile))
            {
                return;
            }

            SpawnSingleBoulder(location, tile);
        }

        public void SpawnSingleDebris(GameLocation location, Vector2 tile)
        {
            var itemIdToSpawn = ChooseRandomDebris(location);
            SpawnSingleDebris(location, tile, itemIdToSpawn);
        }

        public void SpawnSingleDebris(GameLocation location, Vector2 tile, string itemIdToSpawn)
        {
            var itemToSpawn = ItemRegistry.Create<Object>(itemIdToSpawn);
            location.objects.Add(tile, itemToSpawn);
        }

        private static bool TryFindTileForDebris(GameLocation location, bool allowOverlap, out Vector2 chosenTile)
        {
            var mapSize = location.map.Layers[0].LayerWidth * location.map.Layers[0].LayerHeight;
            var numRolls = (int)Math.Ceiling(allowOverlap ? Math.Sqrt(mapSize) : Math.Cbrt(mapSize));
            var validTiles = new List<Vector2>();
            for (var i = 0; i < numRolls; i++)
            {
                var tile = new Vector2(Game1.random.Next(location.map.Layers[0].LayerWidth), Game1.random.Next(location.map.Layers[0].LayerHeight));
                var noSpawn = location.doesTileHaveProperty((int)tile.X, (int)tile.Y, "NoSpawn", "Back") != null;
                var wood = location.doesTileHaveProperty((int)tile.X, (int)tile.Y, "Type", "Back") == "Wood";
                var tileIsClear = allowOverlap || (location.CanItemBePlacedHere(tile) && !location.objects.ContainsKey(tile) && !location.terrainFeatures.ContainsKey(tile));
                
                if (!noSpawn && !wood && tileIsClear)
                {
                    validTiles.Add(tile);
                }
            }

            if (!validTiles.Any())
            {
                chosenTile = Vector2.Zero;
                return false;
            }

            chosenTile = validTiles.OrderBy(x => Vector2.Distance(Game1.player.Tile, x)).First();
            return true;
        }

        private static void SpawnSingleTree(GameLocation location, Vector2 tile)
        {
            var treeId = (Game1.random.Next(3) + 1);
            var growthStage = Game1.random.Next(8);
            SpawnSingleTree(location, tile, treeId, growthStage);
        }

        private static void SpawnSingleTree(GameLocation location, Vector2 tile, int treeId, int growthStage)
        {
            location.terrainFeatures.Add(tile, new Tree(treeId.ToString(), growthStage));
        }

        private static void SpawnSingleBoulder(GameLocation location, Vector2 tile)
        {
            // location.addResourceClumpAndRemoveUnderlyingTerrain(672, 2, 2, tile);
            location.resourceClumps.Add(new ResourceClump(672, 2, 2, tile));
        }

        public string ChooseRandomDebris()
        {
            return ChooseRandomDebris(Game1.currentLocation);
        }

        public string ChooseRandomDebris(GameLocation location)
        {
            var typeRoll = Game1.random.NextDouble();
            if (typeRoll < 0.33)
            {
                return Game1.random.NextDouble() < 0.5 ? TWIG_1 : TWIG_2;
            }
            if (typeRoll < 0.67)
            {
                return Game1.random.NextDouble() < 0.5 ? STONE_1 : STONE_2;
            }

            return GameLocation.getWeedForSeason(Game1.random, location.GetSeason());
        }
    }
}
