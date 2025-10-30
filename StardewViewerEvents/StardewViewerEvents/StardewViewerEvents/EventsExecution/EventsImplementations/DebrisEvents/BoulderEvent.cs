using StardewModdingAPI;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.DebrisEvents
{
    public class BoulderEvent : DebrisEvent
    {
        public BoulderEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override void Execute()
        {
            base.Execute();
            _debrisSpawner.SpawnSingleBoulder();
        }
    }
}
