using StardewModdingAPI;
using StardewViewerEvents.Events;
using ViewerEventsLibrary.Logging;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.CharacterEvents
{
    public abstract class SpawnMonsterEvent : ExecutableEvent
    {
        protected MonsterSpawner _monsterSpawner;

        public SpawnMonsterEvent(IViewerEventsLogger logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
            _monsterSpawner = new MonsterSpawner(_tileChooser);
        }
    }
}
