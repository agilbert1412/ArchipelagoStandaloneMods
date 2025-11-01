using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Internal;
using StardewValley.Objects;
using StardewValley.Tools;
using StardewViewerEvents.Events;
using StardewViewerEvents.Extensions;
using System;
using System.Linq;
using xTile.Dimensions;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.MenuEvents
{
    public class FishBiteEvent : ExecutableEvent
    {
        public const int CATEGORY_FISH = -4;
        private Item _currentFish;

        public FishBiteEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
            _currentFish = null;
        }

        public override bool CanExecuteRightNow()
        {
            if (!base.CanExecuteRightNow())
            {
                return false;
            }

            if (AnyMenuActive())
            {
                return false;
            }

            if (!Game1.currentLocation.canFishHere() || !AnyWaterOnMap())
            {
                return false;
            }

            _currentFish = Game1.currentLocation.getFish(1, "", 1, Game1.player, 0, Vector2.Zero);
            if (_currentFish == null || _currentFish.Category != CATEGORY_FISH || !DataLoader.Fish(Game1.content).ContainsKey(_currentFish.ItemId))
            {
                return false;
            }

            if (!TryFindFishingRod(out _))
            {
                return false;
            }

            return true;
        }

        public override void Execute()
        {
            base.Execute();

            if (!TryFindFishingRod(out var rod) || !TryFindNearestWater(out var waterTile))
            {
                return;
            }

            // rod.lastUser = Game1.player;
            Game1.chatBox.addMessage($"Fishy event trying to catch a {_currentFish.Name}", Color.Yellow);
            // rod.startMinigameEndFunction(_currentFish);
            var waterPixel = new Vector2((int)waterTile.X * 64 + 32, (int)waterTile.Y * 64 + 32);
            rod.bobber.Set(waterPixel);
            rod.DoFunction(Game1.currentLocation, (int)waterPixel.X, (int)waterPixel.Y, 0, Game1.player);
            rod.timeUntilFishingBite = 0;
            rod.isNibbling = true;
            rod.DoFunction(Game1.currentLocation, (int)waterPixel.X, (int)waterPixel.Y, 0, Game1.player);
        }

        private bool AnyWaterOnMap()
        {
            var tiles = Game1.currentLocation.map.Layers[0].Tiles.Array;
            for (var x = 0; x < tiles.GetLength(0); x += 1)
            {
                for (var y = 0; y < tiles.GetLength(1); y += 1)
                {
                    if (Game1.currentLocation.isTileFishable(x, y))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool TryFindNearestWater(out Vector2 waterTile)
        {
            var tiles = Game1.currentLocation.map.Layers[0].Tiles.Array;
            var fishableTiles = new List<Vector2>();
            for (var x = 0; x < tiles.GetLength(0); x += 1)
            {
                for (var y = 0; y < tiles.GetLength(1); y += 1)
                {
                    if (Game1.currentLocation.isTileFishable(x, y))
                    {
                        fishableTiles.Add(new Vector2(x, y));
                    }
                }
            }

            if (!fishableTiles.Any())
            {
                waterTile = Game1.player.Tile;
                return false;
            }

            var sortedTiles = fishableTiles.OrderBy(x => Vector2.Distance(x, Game1.player.Tile)).ToArray();
            waterTile = sortedTiles.First();
            return true;
        }

        private bool TryFindFishingRod(out FishingRod rod)
        {
            rod = null;
            for (var i = 0; i < Math.Min(Game1.player.MaxItems, 12); i++)
            {
                var item = Game1.player.Items[i];
                if (item is FishingRod playerRod)
                {
                    rod = playerRod;
                    Game1.player.CurrentToolIndex = i;
                    return true;
                }
            }

            if (rod == null)
            {
                Utility.ForEachItemContext(GiveFishingRodToPlayer);
                if (Game1.player.CurrentTool is FishingRod playerRod)
                {
                    rod = playerRod;
                    return true;
                }
            }

            return false;
        }

        private bool GiveFishingRodToPlayer(in ForEachItemContext context)
        {
            if (context.Item is not FishingRod fishingRod)
            {
                return true;
            }

            var slotToFill = 0;
            for (var i = 0; i < Game1.player.MaxItems; i++)
            {
                var item = Game1.player.Items[i];
                if (item is null)
                {
                    slotToFill = i;
                    break;
                }
            }

            context.ReplaceItemWith(Game1.player.Items[slotToFill]);
            Game1.player.Items[slotToFill] = fishingRod;
            Game1.player.CurrentToolIndex = slotToFill;
            return false;
        }
    }
}
