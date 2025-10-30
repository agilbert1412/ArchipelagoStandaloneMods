using StardewModdingAPI;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.DebrisEvents
{
    public class RandomDebrisEvent : DebrisEvent
    {
        public const int AMOUNT = 10;

        public RandomDebrisEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override void Execute()
        {
            base.Execute();
            _debrisSpawner.SpawnManyDebris(AMOUNT);
        }
    }
}
