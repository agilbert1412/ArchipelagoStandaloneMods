using StardewModdingAPI;
using StardewViewerEvents.Events;
using StardewViewerEvents.EventsExecution.EventsImplementations.DurationEvents;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.DebrisEvents
{
    public abstract class DebrisEvent : DurationEvent
    {
        protected DebrisSpawner _debrisSpawner;

        protected override int TicksDuration => 0;
        protected override int SecondsDuration => TicksDuration * 60;

        public DebrisEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
            _debrisSpawner = new DebrisSpawner();
        }
    }
}
