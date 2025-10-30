using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.TeleportEvents
{
    public class TeleportHomeEvent : TeleportEvent
    {
        public TeleportHomeEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override bool TryGetDestination(out GameLocation map, out Vector2 tile)
        {
            map = Game1.getFarm();
            tile = Game1.player.Tile;

            var homeOfFarmer = Utility.getHomeOfFarmer(Game1.player);
            if (homeOfFarmer == null)
            {
                return false;
            }

            var frontDoorSpot = homeOfFarmer.getFrontDoorSpot();
            tile = new Vector2(frontDoorSpot.X, frontDoorSpot.Y);
            return true;
        }
    }
}
