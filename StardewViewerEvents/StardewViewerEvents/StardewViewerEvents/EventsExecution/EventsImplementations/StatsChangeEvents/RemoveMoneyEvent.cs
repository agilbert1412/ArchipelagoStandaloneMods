using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.StatsChangeEvents
{
    public class RemoveMoneyEvent : ExecutableEvent
    {
        public const int AMOUNT_REMOVED = 100;

        public RemoveMoneyEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override void Execute()
        {
            base.Execute();

            Game1.player.Money -= (AMOUNT_REMOVED * QueuedEvent.queueCount);
        }
    }
}
