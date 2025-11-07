using Force.DeepCloner;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.TerrainFeatures;
using StardewViewerEvents.Events;
using xTile.Dimensions;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.DebrisEvents
{
    public class BoulderEvent : DebrisEvent
    {
        public const int DURATION_SECONDS = 60;
        public const int DURATION_TICKS = DURATION_SECONDS * 60;

        protected override int TicksDuration => DURATION_TICKS;
        protected override int SecondsDuration => TicksDuration * 60;

        private List<ResourceClump> _boulders;

        public BoulderEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
            _boulders = new List<ResourceClump>();
        }

        public override void Execute()
        {
            base.Execute();
            _boulders = _debrisSpawner.SpawnManyBoulders(QueuedEvent.queueCount);
        }

        public override bool UpdateAndTryFinish()
        {
            var elapsedTicks = Game1.ticks - _tickStarted;
            var shouldEnd = elapsedTicks > TicksDuration;
            if (shouldEnd)
            {
                foreach (var boulder in _boulders)
                {
                    var location = boulder.Location;
                    if (location != null)
                    {
                        location.playSound("boulderBreak", boulder.Tile);
                        location.resourceClumps.Remove(boulder);
                    }
                    boulder.health.Value = 0;
                }
                return true;
            }

            return false;
        }
    }
}
