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

        public override bool ValidateParameters(out string errorMessage)
        {
            var desiredMonster = GetSingleParameter();
            errorMessage = $"";
            if (!_monsterSpawner.IsValidMonster(desiredMonster))
            {
                errorMessage = $"Unrecognized monster [{desiredMonster}]. Valid monsters: [{string.Join(',', MonsterSpawner.AllMonsterTypes)}]";
                return false;
            }

            return true;
        }

        public override void Execute()
        {
            base.Execute();

            var desiredMonster = GetSingleParameter();
            _monsterSpawner.SpawnManySpecificMonsters(Game1.currentLocation, desiredMonster, QueuedEvent.queueCount);
        }
    }
}
