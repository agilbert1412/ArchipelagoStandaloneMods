using StardewModdingAPI;
using StardewViewerEvents.Events;
using ViewerEventsLibrary.Logging;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.DebrisEvents
{
    public class RandomDebrisEvent : DebrisEvent
    {
        public const int AMOUNT = 10;

        public RandomDebrisEvent(IViewerEventsLogger logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override void Execute()
        {
            base.Execute();
            _debrisSpawner.SpawnManyDebris(AMOUNT * QueuedEvent.queueCount);
        }
    }
}
