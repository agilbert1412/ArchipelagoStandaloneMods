using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Pathfinding;
using StardewViewerEvents.Extensions;
using xTile.Dimensions;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace StardewViewerEvents.EventsExecution.EventsImplementations
{
    public class TileChooser
    {
        private const int MAX_RETRIES = 20;

        public Vector2? GetRandomTileInbounds(GameLocation area, bool needClearTile)
        {
            return GetRandomTileInbounds(area, Point.Zero, int.MaxValue, needClearTile);
        }

        public Vector2? GetRandomTileInbounds(GameLocation area, Point origin, int maxDistance, bool needClearTile)
        {
            var triesRemaining = MAX_RETRIES;
            var tile = area.getRandomTile();
            var tilePoint = Utility.Vector2ToPoint(tile);
            var tileLocation = new Location(tilePoint.X, tilePoint.Y);
            while (IsTileInvalid(area, origin, maxDistance, tilePoint, tile, needClearTile))
            {
                tile = area.getRandomTile();
                tilePoint = Utility.Vector2ToPoint(tile);
                tileLocation = new Location(tilePoint.X, tilePoint.Y);
                triesRemaining--;
                if (triesRemaining <= 0)
                {
                    return null;
                }
            }

            return tile;
        }

        private bool IsTileInvalid(GameLocation area, Point origin, int maxDistance, Point tilePoint, Vector2 tile, bool needClearTile)
        {
            if (tilePoint.GetTotalDistance(origin) > maxDistance)
            {
                return true;
            }
            if (area.isWaterTile(tilePoint.X, tilePoint.Y))
            {
                return true;
            }
            if (needClearTile)
            {
                if (area.IsTileOccupiedBy(tile))
                {
                    return true;
                }
                if (!area.isTilePassable(tile))
                {
                    return true;
                }
                if (!area.isTilePlaceable(tile))
                {
                    return true;
                }
            }
            if (!CanPathFindToAnyWarp(area, tilePoint, needClearTile: needClearTile))
            {
                return true;
            }

            return false;
        }

        public Vector2 GetRandomTileInboundsOffScreen(GameLocation area, bool needClearTile)
        {
            var numberRetries = 5;
            var spawnPosition = GetRandomTileInbounds(area, needClearTile);
            if (spawnPosition == null)
            {
                return area.getRandomTile();
            }

            while (numberRetries > 0 && Utility.isOnScreen(Utility.Vector2ToPoint(spawnPosition.Value), 64, area))
            {
                numberRetries--;
                spawnPosition = GetRandomTileInbounds(area, needClearTile);
                if (spawnPosition == null)
                {
                    return area.getRandomTile();
                }
            }

            return spawnPosition.Value;
        }

        public bool CanPathFindToAnyWarp(GameLocation location, Point startPoint, int minimumDistance = 0, int maximumDistance = 500, bool needClearTile = true)
        {
            if (location.warps == null || location.warps.Count < 1)
            {
                return true;
            }

            if (needClearTile && location.isCollidingPosition(new Rectangle(startPoint.X * 64 + 1, startPoint.Y * 64 + 1, 62, 62),
                    Game1.viewport, true, 0, false, Game1.player, true))
            {
                return false;
            }

            foreach (var warp in location.warps)
            {
                var endPoint = new Point(warp.X, warp.Y);
                var endPointFunction = new PathFindController.isAtEnd(PathFindController.isAtEndPoint);
                var character = (Character)Game1.player;
                var path = PathFindController.findPath(startPoint, endPoint, endPointFunction, location, character, 250);
                if (path != null && path.Count >= minimumDistance && path.Count <= maximumDistance)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
