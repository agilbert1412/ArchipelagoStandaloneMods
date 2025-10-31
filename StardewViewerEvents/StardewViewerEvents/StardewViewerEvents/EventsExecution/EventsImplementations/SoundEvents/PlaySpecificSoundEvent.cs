using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;
using StardewViewerEvents.EventsExecution.EventsImplementations.CharacterEvents;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.SoundEvents
{
    public class PlaySpecificSoundEvent : PlaySoundEvent
    {

        public PlaySpecificSoundEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override bool ValidateParameters()
        {
            var desiredSoundCue = GetSingleParameter();
            return desiredSoundCue.Equals("random", StringComparison.InvariantCultureIgnoreCase) || SoundCueIsValid(desiredSoundCue);
        }

        public override string GetSoundCue()
        {
            var desiredSoundCue = GetSingleParameter();
            if (desiredSoundCue.Equals("random", StringComparison.InvariantCultureIgnoreCase))
            {
                // private SoundBank soundBank;
                var soundBankField = _modHelper.Reflection.GetField<SoundBank>(Game1.soundBank, "soundBank");
                var soundBank = soundBankField.GetValue();

                // private readonly Dictionary<string, CueDefinition> _cues = new Dictionary<string, CueDefinition>();
                var cuesField = _modHelper.Reflection.GetField<Dictionary<string, CueDefinition>>(soundBank, "_cues");
                var cues = cuesField.GetValue().Keys.ToArray();

                desiredSoundCue = cues[Game1.random.Next(cues.Length)];
            }

            return desiredSoundCue;
        }

        protected override int GetSoundCount()
        {
            return 1;
        }
    }
}
