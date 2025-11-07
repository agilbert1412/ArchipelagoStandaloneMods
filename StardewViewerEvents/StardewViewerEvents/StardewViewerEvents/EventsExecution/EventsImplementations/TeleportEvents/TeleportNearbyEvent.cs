using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.TeleportEvents
{
    public class TeleportNearbyEvent : TeleportEvent
    {
        public TeleportNearbyEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override bool TryGetDestination(out GameLocation map, out Vector2 tile)
        {
            map = Game1.player.currentLocation;
            tile = Game1.player.Tile;
            if (map == null)
            {
                return false;
            }

            var destinationTile = _tileChooser.GetRandomTileInbounds(map, Game1.player.TilePoint, 20, true);
            if (destinationTile == null)
            {
                return false;
            }

            destinationTile = _tileChooser.GetRandomTileInbounds(map, Game1.player.TilePoint, 20, false);
            if (destinationTile == null)
            {
                return false;
            }

            tile = destinationTile.Value;
            return true;
        }
    }
}
