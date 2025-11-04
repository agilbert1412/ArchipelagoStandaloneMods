using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.StatsChangeEvents
{
    public class RemoveHealthEvent : ExecutableEvent
    {
        public const int AMOUNT_REMOVED = 1;

        public RemoveHealthEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override void Execute()
        {
            base.Execute();

            Game1.player.health -= (AMOUNT_REMOVED * QueuedEvent.queueCount);
        }
    }
}
