using StardewModdingAPI;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.SoundEvents
{
    public class PlaySpecificSoundEvent : PlaySoundEvent
    {

        public PlaySpecificSoundEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override bool ValidateParameters(out string errorMessage)
        {
            var desiredSoundCue = GetSingleParameter();
            errorMessage =
                $"Unrecognized sound cue [{desiredSoundCue}]. You must specify the exact cue of a sound in Stardew Valley.";
            return SoundCueIsValid(desiredSoundCue);
        }

        public override string GetSoundCue()
        {
            var desiredSoundCue = GetSingleParameter();
            return desiredSoundCue;
        }

        protected override int GetSoundCount()
        {
            return 1;
        }
    }
}
