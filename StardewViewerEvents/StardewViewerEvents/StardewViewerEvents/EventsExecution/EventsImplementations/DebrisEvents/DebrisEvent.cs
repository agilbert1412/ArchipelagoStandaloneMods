using StardewModdingAPI;
using StardewViewerEvents.Events;
using StardewViewerEvents.EventsExecution.EventsImplementations.DurationEvents;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.DebrisEvents
{
    public abstract class DebrisEvent : ExecutableEvent
    {
        protected DebrisSpawner _debrisSpawner;

        public DebrisEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
            _debrisSpawner = new DebrisSpawner();
        }
    }
}
