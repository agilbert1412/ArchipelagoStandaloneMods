using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;
using StardewViewerEvents.EventsExecution.EventsImplementations.ItemEvents;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.CharacterEvents
{
    public class SpawnSpecificMonsterEvent : SpawnRandomMonsterEvent
    {
        public SpawnSpecificMonsterEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override bool ValidateParameters()
        {
            var desiredMonster = GetSingleParameter();
            return _monsterSpawner.IsValidMonster(desiredMonster);
        }

        public override void Execute()
        {
            base.Execute();

            var desiredMonster = GetSingleParameter();
            _monsterSpawner.SpawnOneSpecificMonster(Game1.currentLocation, desiredMonster);
        }
    }
}
