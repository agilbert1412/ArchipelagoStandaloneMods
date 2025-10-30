using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.CharacterEvents
{
    public abstract class BabyEvent : ExecutableEvent
    {
        protected BabyBirther _babyBirther;

        public BabyEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
            _babyBirther = new BabyBirther();
        }
    }
}
