using Microsoft.Xna.Framework.Audio;
using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.SoundEvents
{
    public class PlayRandomSoundEvent : PlaySoundEvent
    {
        private string _soundCue;

        public PlayRandomSoundEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
            _soundCue = null;
        }

        public override string GetSoundCue()
        {
            if (!string.IsNullOrWhiteSpace(_soundCue) && QueuedEvent.queueCount == 1)
            {
                return _soundCue;
            }

            // private SoundBank soundBank;
            var soundBankField = _modHelper.Reflection.GetField<SoundBank>(Game1.soundBank, "soundBank");
            var soundBank = soundBankField.GetValue();

            // private readonly Dictionary<string, CueDefinition> _cues = new Dictionary<string, CueDefinition>();
            var cuesField = _modHelper.Reflection.GetField<Dictionary<string, CueDefinition>>(soundBank, "_cues");
            var cues = cuesField.GetValue().Keys.ToArray();

            _soundCue = cues[Game1.random.Next(cues.Length)];
            return _soundCue;
        }

        protected override int GetSoundCount()
        {
            return 1;
        }

        protected override string AppendParameters(string message)
        {
            if (QueuedEvent.queueCount == 1)
            {
                message += $" [{GetSoundCue()}]";
            }

            return message;
        }
    }
}
