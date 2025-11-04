using StardewModdingAPI;
using StardewValley;
using StardewValley.Network.ChestHit;
using StardewValley.Objects;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.InventoryEvents
{
    public class NudgeEvent : InventoryEvent
    {
        public const int NUMBER_NUDGES = 4;

        public NudgeEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override bool ValidateParameters(out string errorMessage)
        {
            if (!base.ValidateParameters(out errorMessage))
            {
                return false;
            }

            var allChests = GetAllChests();
            errorMessage = $"You can only use the Nudge event when there are chests somewhere to nudge";
            return allChests.Any();
        }

        public override void Execute()
        {
            base.Execute();

            var numberNudges = NUMBER_NUDGES * QueuedEvent.queueCount;
            NudgeChests(numberNudges);
        }

        public void NudgeChests(int numberNudges)
        {
            var allChests = GetAllChests();

            for (var i = 0; i < numberNudges; i++)
            {
                var chest = allChests[Game1.random.Next(allChests.Count)];
                NudgeChestOnce(chest);
            }
        }

        private void NudgeChestOnce(Chest chest)
        {
            var seed = (int)Game1.stats.DaysPlayed + (int)(chest.TileLocation.X * 77) + (int)(chest.TileLocation.Y * 1933);
            var random = new Random(seed);
            var mutex = chest.GetMutex();

            mutex.RequestLock(() =>
            {
                chest.clearNulls();
                var chestTileBefore = chest.TileLocation;
                chest.TryMoveToSafePosition(random.Next(0, 4));

                // internal readonly ChestHitSynchronizer chestHit;
                var chestHitField = _modHelper.Reflection.GetField<ChestHitSynchronizer>(Game1.player.team, "chestHit");
                var chestHit = chestHitField.GetValue();

                chestHit.SignalMove(chest.Location, (int)chestTileBefore.X, (int)chestTileBefore.Y, (int)chest.TileLocation.X, (int)chest.TileLocation.Y);
                mutex.ReleaseLock();
            });
        }
    }
}
