using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;
using StardewViewerEvents.Extensions;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.SoundEvents
{
    public abstract class PlaySoundEvent : ExecutableEvent
    {
        protected PlaySoundEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
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
            return Game1.soundBank.Exists(soundCue);
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
                Game1.playSound(soundCue);
            }
        }
    }
}
