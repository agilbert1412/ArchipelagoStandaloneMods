using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations
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
            var destinationTile = _tileChooser.GetRandomTileInbounds(map, Game1.player.TilePoint, 20);
            if (map == null || destinationTile == null)
            {
                return false;
            }

            tile = destinationTile.Value;
            return true;
        }
    }
}
