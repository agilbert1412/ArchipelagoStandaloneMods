using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;
using StardewViewerEvents.Extensions;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.TeleportEvents
{
    public class TeleportMapEvent : TeleportEvent
    {
        public TeleportMapEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override bool ValidateParameters()
        {
            if (QueuedEvent.parameters == null || QueuedEvent.parameters.Length < 1)
            {
                return false;
            }

            var desiredMapName = string.Join(" ", QueuedEvent.parameters);
            return TryGetDesiredMap(desiredMapName, out _);
        }

        public override bool TryGetDestination(out GameLocation map, out Vector2 tile)
        {
            map = Game1.player.currentLocation;
            tile = Game1.player.Tile;
            var desiredMapName = string.Join(" ", QueuedEvent.parameters);
            if (!TryGetDesiredMap(desiredMapName, out map))
            {
                return false;
            }

            var chosenTile = _tileChooser.GetRandomTileInbounds(map);
            if (chosenTile == null)
            {
                return false;
            }

            tile = chosenTile.Value;
            return true;
        }

        private bool TryGetDesiredMap(string desiredMapName, out GameLocation desiredMap)
        {
            var sanitizedDesiredMapName = desiredMapName.SanitizeEntityName();
            foreach (var gameLocation in Game1.locations)
            {
                if (gameLocation.Name.SanitizeEntityName() == sanitizedDesiredMapName)
                {

                    desiredMap = gameLocation;
                    return true;
                }
            }
            foreach (var gameLocation in Game1.locations)
            {
                if (gameLocation.DisplayName.SanitizeEntityName() == sanitizedDesiredMapName)
                {

                    desiredMap = gameLocation;
                    return true;
                }
            }
            foreach (var gameLocation in Game1.locations)
            {
                if (gameLocation.NameOrUniqueName.SanitizeEntityName() == sanitizedDesiredMapName)
                {

                    desiredMap = gameLocation;
                    return true;
                }
            }

            desiredMap = Game1.currentLocation;
            return false;
        }
    }
}
