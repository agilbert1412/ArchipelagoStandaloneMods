using StardewModdingAPI;
using StardewViewerEvents.Events;
using ViewerEventsLibrary.Logging;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.SoundEvents
{
    public class PlayBarkEvent : PlaySoundEvent
    {
        public PlayBarkEvent(IViewerEventsLogger logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override string GetSoundCue()
        {
            return "dog_bark";
        }

        protected override int GetSoundCount()
        {
            return 10;
        }
    }
}
