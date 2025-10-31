using StardewModdingAPI;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.SoundEvents
{
    public class PlayMeowEvent : PlaySoundEvent
    {
        public PlayMeowEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override string GetSoundCue()
        {
            return "cat";
        }

        protected override int GetSoundCount()
        {
            return 10;
        }
    }
}
