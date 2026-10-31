using StardewModdingAPI;
using StardewViewerEvents.Events;
using ViewerEventsLibrary.Logging;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.CharacterEvents
{
    public abstract class BabyEvent : ExecutableEvent
    {
        protected BabyBirther _babyBirther;

        public BabyEvent(IViewerEventsLogger logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
            _babyBirther = new BabyBirther();
        }
    }
}
