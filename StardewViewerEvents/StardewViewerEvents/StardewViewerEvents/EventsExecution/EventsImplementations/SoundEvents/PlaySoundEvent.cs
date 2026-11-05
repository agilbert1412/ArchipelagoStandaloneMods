using Microsoft.Xna.Framework.Audio;
using StardewModdingAPI;
using StardewValley;
using StardewValley.TerrainFeatures;
using StardewViewerEvents.Events;
using StardewViewerEvents.Extensions;
using ViewerEventsLibrary.Extensions;
using ViewerEventsLibrary.Logging;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.SoundEvents
{
    public abstract class PlaySoundEvent : ExecutableEvent
    {
        protected PlaySoundEvent(IViewerEventsLogger logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override void Execute()
        {
            base.Execute();

            var count = GetSoundCount() * QueuedEvent.queueCount;
            PlaySoundsAsync(count).FireAndForget();
        }

        public abstract string GetSoundCue();

        protected virtual bool SoundCueIsValid(string soundCue)
        {
            if (Game1.soundBank.Exists(soundCue))
            {
                return false;
            }

            return !IsSoundLooping(soundCue);
        }

        protected bool IsSoundLooping(string soundCue)
        {
            return Game1.soundBank.GetCueDefinition(soundCue).sounds
                    .Any(sound => sound.soundClips == null || sound.soundClips
                        .Any(clip => clip.clipEvents == null || clip.clipEvents
                            .Any(clipEvent => clipEvent == null || clipEvent is not PlayWaveEvent playWaveEvent || playWaveEvent.Loop)));
        }

        protected virtual int GetSoundCount()
        {
            return 1;
        }

        private async Task PlaySoundsAsync(int numberOfSounds)
        {
            for (var i = 0; i < numberOfSounds; i++)
            {
                await Task.Run(() => Thread.Sleep(2000));
                var soundCue = GetSoundCue();
                Game1.playSound(soundCue, out var cue);
                StopSoundAsync(cue).FireAndForget();
            }
        }

        private async Task StopSoundAsync(ICue soundCue)
        {
            await Task.Run(() => Thread.Sleep(60 * 1000));
            soundCue.Stop(AudioStopOptions.Immediate);
        }
    }
}
