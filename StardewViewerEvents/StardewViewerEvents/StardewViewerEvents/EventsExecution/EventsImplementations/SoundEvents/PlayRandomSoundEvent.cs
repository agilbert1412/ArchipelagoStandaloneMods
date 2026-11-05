using Microsoft.Xna.Framework.Audio;
using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;
using ViewerEventsLibrary.Logging;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.SoundEvents
{
    public class PlayRandomSoundEvent : PlaySoundEvent
    {
        private string _soundCue;

        public PlayRandomSoundEvent(IViewerEventsLogger logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
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
            var notLoopingSounds = FilterSoundsLooping(cues, false);

            _soundCue = notLoopingSounds[Game1.random.Next(notLoopingSounds.Length)];
            return _soundCue;
        }

        private string[] FilterSoundsLooping(IEnumerable<string> sounds, bool looping)
        {
            return sounds
                .Where(soundCue => Game1.soundBank.GetCueDefinition(soundCue).sounds
                    .Any(sound => sound.soundClips == null || sound.soundClips
                        .Any(clip => clip.clipEvents == null || clip.clipEvents
                            .Any(clipEvent => clipEvent == null || clipEvent is not PlayWaveEvent playWaveEvent || (playWaveEvent.Loop == looping))))).ToArray();
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
