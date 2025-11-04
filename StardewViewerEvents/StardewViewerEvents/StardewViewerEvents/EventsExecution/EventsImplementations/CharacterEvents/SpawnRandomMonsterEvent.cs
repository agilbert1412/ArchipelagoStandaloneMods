using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.CharacterEvents
{
    public class SpawnRandomMonsterEvent : ExecutableEvent
    {
        protected MonsterSpawner _monsterSpawner;

        public SpawnRandomMonsterEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
            _monsterSpawner = new MonsterSpawner(_tileChooser);
        }

        public override void Execute()
        {
            base.Execute();

            _monsterSpawner.SpawnManyRandomMonster(Game1.currentLocation, QueuedEvent.queueCount);
        }
    }
}
