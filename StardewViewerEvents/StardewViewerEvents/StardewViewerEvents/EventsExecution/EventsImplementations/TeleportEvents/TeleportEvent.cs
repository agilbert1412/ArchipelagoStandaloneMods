using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.TeleportEvents
{
    public abstract class TeleportEvent : ExecutableEvent
    {
        private GameLocation _destinationMap;
        private Vector2 _destinationTile;

        protected TeleportEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override bool CanExecuteRightNow()
        {
            if (!TryGetDestination(out _destinationMap, out _destinationTile))
            {
                return false;
            }

            return base.CanExecuteRightNow();
        }

        public override void Execute()
        {
            base.Execute();

            TeleportFarmerTo(_destinationMap.Name, _destinationTile);
        }

        public abstract bool TryGetDestination(out GameLocation map, out Vector2 tile);

        private void TeleportFarmerTo(string locationName, Vector2 tile)
        {
            var farmer = Game1.player;
            var multiplayerField = _modHelper.Reflection.GetField<Multiplayer>(typeof(Game1), "multiplayer");
            var multiplayer = multiplayerField.GetValue();
            for (var index = 0; index < 12; ++index)
            {
                multiplayer.broadcastSprites(farmer.currentLocation,
                    new TemporaryAnimatedSprite(354, Game1.random.Next(25, 75), 6, 1,
                        new Vector2(Game1.random.Next((int)farmer.position.X - 256, (int)farmer.position.X + 192),
                            Game1.random.Next((int)farmer.position.Y - 256, (int)farmer.position.Y + 192)), false, Game1.random.NextDouble() < 0.5));
            }

            Game1.currentLocation.playSound("wand");
            Game1.displayFarmer = false;
            farmer.temporarilyInvincible = true;
            farmer.temporaryInvincibilityTimer = -2000;
            farmer.Halt();
            farmer.faceDirection(2);
            farmer.CanMove = false;
            farmer.freezePause = 2000;
            Game1.flashAlpha = 1f;
            DelayedAction.fadeAfterDelay(() => AfterTeleport(farmer, locationName, tile), 1000);
            new Rectangle(farmer.GetBoundingBox().X, farmer.GetBoundingBox().Y, 64, 64).Inflate(192, 192);
            var num = 0;
            for (var x1 = farmer.Tile.X + 8; x1 >= farmer.Tile.X - 8; --x1)
            {
                multiplayer.broadcastSprites(farmer.currentLocation,
                    new TemporaryAnimatedSprite(6, new Vector2(x1, farmer.Tile.Y) * 64f, Color.White, animationInterval: 50f)
                    {
                        layerDepth = 1f,
                        delayBeforeAnimationStart = num * 25,
                        motion = new Vector2(-0.25f, 0.0f),
                    });
                ++num;
            }
        }

        private void AfterTeleport(Farmer farmer, string locationName, Vector2 tile)
        {
            var destination = Utility.Vector2ToPoint(tile);
            Game1.warpFarmer(locationName, destination.X, destination.Y, false);
            Game1.changeMusicTrack("none");
            Game1.fadeToBlackAlpha = 0.99f;
            Game1.screenGlow = false;
            farmer.temporarilyInvincible = false;
            farmer.temporaryInvincibilityTimer = 0;
            Game1.displayFarmer = true;
            farmer.CanMove = true;
        }
    }
}
