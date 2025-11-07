using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.CharacterEvents
{
    public abstract class SpawnMonsterEvent : ExecutableEvent
    {
        protected MonsterSpawner _monsterSpawner;

        public SpawnMonsterEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
            _monsterSpawner = new MonsterSpawner(_tileChooser);
        }
    }
}
