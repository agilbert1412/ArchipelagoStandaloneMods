using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;
using ViewerEventsLibrary.Logging;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.StatsChangeEvents
{
    public class RemoveHealthEvent : ExecutableEvent
    {
        public const int AMOUNT_REMOVED = 25;

        public RemoveHealthEvent(IViewerEventsLogger logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override void Execute()
        {
            base.Execute();

            Game1.player.health -= (AMOUNT_REMOVED * QueuedEvent.queueCount);
        }
    }
}
