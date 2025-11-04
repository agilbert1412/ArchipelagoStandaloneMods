using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.CropEvents
{
    public class CrowEvent : CropEvent
    {
        public const int NUMBER_CROWS = 10;
        public const double SCARECROW_EFFICIENCY = 0.4;

        public CrowEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override void Execute()
        {
            base.Execute();

            var numberCrows = NUMBER_CROWS * QueuedEvent.queueCount;
            SendCrows(numberCrows);
        }

        public void SendCrows(int numberCrows)
        {
            var allTargets = GetAllCrops();
            var allTargetsByCrop = new Dictionary<HoeDirt, Tuple<GameLocation, Vector2>>();
            foreach (var (gameLocation, target) in allTargets)
            {
                foreach (var (tile, crop) in target)
                {
                    allTargetsByCrop.Add(crop, new Tuple<GameLocation, Vector2>(gameLocation, tile));
                }
            }

            for (var i = 0; i < numberCrows; i++)
            {
                if (!allTargetsByCrop.Any())
                {
                    return;
                }

                var keys = allTargetsByCrop.Keys.ToArray();
                var chosenTarget = keys[Game1.random.Next(keys.Length)];
                var chosenTargetDetails = allTargetsByCrop[chosenTarget];
                if (CrowTryEatCrop(chosenTargetDetails.Item1, chosenTarget, chosenTargetDetails.Item2))
                {
                    allTargetsByCrop.Remove(chosenTarget);
                }
            }
        }

        private static bool CrowTryEatCrop(GameLocation map, HoeDirt crop, Vector2 cropPosition)
        {
            var scarecrowPositions = GetScarecrowPositions(map);
            map.critters ??= new List<Critter>();

            if (CrowShouldEatCrop(map, cropPosition, scarecrowPositions))
            {
                crop.destroyCrop(true);
                map.critters.Add(new Crow((int)crop.Tile.X, (int)crop.Tile.Y));
                return true;
            }

            return false;
        }

        private static List<Vector2> GetScarecrowPositions(GameLocation map)
        {
            var scarecrowPositions = new List<Vector2>();
            foreach (var (position, placedObject) in map.objects.Pairs)
            {
                if (placedObject.bigCraftable.Value && placedObject.IsScarecrow())
                {
                    scarecrowPositions.Add(position);
                }
            }

            return scarecrowPositions;
        }

        private static bool CrowShouldEatCrop(GameLocation map, Vector2 cropPosition, List<Vector2> scarecrowPositions)
        {
            var vulnerability = GetNumberOfDefendingScarecrows(map, scarecrowPositions, cropPosition);
            for (var i = 0; i < vulnerability; i++)
            {
                if (Game1.random.NextDouble() < SCARECROW_EFFICIENCY)
                {
                    return false;
                }
            }

            return true;
        }

        private static int GetNumberOfDefendingScarecrows(GameLocation map, List<Vector2> scarecrowPositions, Vector2 cropPosition)
        {
            var numberOfDefendingScarecrows = 0;
            foreach (var scarecrowPosition in scarecrowPositions)
            {
                var radiusForScarecrow = map.objects[scarecrowPosition].GetRadiusForScarecrow();
                if (Vector2.Distance(scarecrowPosition, cropPosition) < radiusForScarecrow)
                {
                    numberOfDefendingScarecrows++;
                }
            }

            return numberOfDefendingScarecrows;
        }

        private static Dictionary<GameLocation, Dictionary<Vector2, HoeDirt>> GetAllCrops()
        {
            var allCrops = new Dictionary<GameLocation, Dictionary<Vector2, HoeDirt>>();
            foreach (var gameLocation in Game1.locations)
            {
                var allCropsInLocation = GetAllCrops(gameLocation);
                foreach (var (tile, crop) in allCropsInLocation)
                {
                    if (!allCrops.ContainsKey(gameLocation))
                    {
                        allCrops.Add(gameLocation, new Dictionary<Vector2, HoeDirt>());
                    }
                    allCrops[gameLocation].Add(tile, crop);
                }
            }

            return allCrops;
        }

        private static Dictionary<Vector2, HoeDirt> GetAllCrops(GameLocation location)
        {
            var allCrops = new Dictionary<Vector2, HoeDirt>();
            foreach (var (cropPosition, cropTile) in location.terrainFeatures.Pairs)
            {
                if (cropTile is not HoeDirt dirt || dirt.crop == null || dirt.crop.currentPhase.Value <= 1)
                {
                    continue;
                }

                allCrops.Add(cropPosition, dirt);
            }

            foreach (var (cropPosition, gameObject) in location.Objects.Pairs)
            {
                if (gameObject is not IndoorPot gardenPot || gardenPot.hoeDirt.Value.crop == null || gardenPot.hoeDirt.Value.crop.currentPhase.Value <= 1)
                {
                    continue;
                }

                allCrops.Add(cropPosition, gardenPot.hoeDirt.Value);
            }

            return allCrops;
        }
    }
}
