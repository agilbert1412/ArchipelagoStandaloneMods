using StardewModdingAPI;
using StardewViewerEvents.Events;
using ViewerEventsLibrary.Logging;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.DebrisEvents
{
    public abstract class DebrisEvent : ExecutableEvent
    {
        protected DebrisSpawner _debrisSpawner;

        public DebrisEvent(IViewerEventsLogger logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
            _debrisSpawner = new DebrisSpawner();
        }
    }
}
