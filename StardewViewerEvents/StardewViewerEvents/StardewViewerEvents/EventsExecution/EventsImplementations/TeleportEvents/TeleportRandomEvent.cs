using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.TeleportEvents
{
    public class TeleportRandomEvent : TeleportEvent
    {
        public TeleportRandomEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override bool TryGetDestination(out GameLocation map, out Vector2 tile)
        {
            var validMaps = Game1.locations;
            GameLocation chosenLocation = null;
            Vector2? chosenTile = null;
            while (chosenLocation == null || chosenTile == null)
            {
                chosenLocation = validMaps[Game1.random.Next(validMaps.Count)];
                chosenTile = _tileChooser.GetRandomTileInbounds(chosenLocation, true);
            }

            map = chosenLocation;
            tile = chosenTile.Value;
            return true;
        }
    }
}
