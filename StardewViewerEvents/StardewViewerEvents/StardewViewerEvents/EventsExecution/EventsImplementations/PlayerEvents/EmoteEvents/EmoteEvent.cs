using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;
using ViewerEventsLibrary.Logging;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.PlayerEvents.EmoteEvents
{
    public abstract class EmoteEvent : ExecutableEvent
    {
        protected EmoteEvent(IViewerEventsLogger logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override bool CanExecuteRightNow()
        {
            if (!base.CanExecuteRightNow())
            {
                return false;
            }

            if (Game1.eventUp || Game1.player.isEmoting || !Game1.player.CanEmote() || Game1.player.isEmoteAnimating)
            {
                return false;
            }

            return true;
        }

        public override void Execute()
        {
            base.Execute();

            var emoteName = GetEmoteName();
            Game1.player.performPlayerEmote(emoteName);
        }

        protected abstract string GetEmoteName();
    }
}
