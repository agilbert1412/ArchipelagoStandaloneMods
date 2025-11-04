using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.StatsChangeEvents
{
    public class AddMoneyEvent : ExecutableEvent
    {
        public const int AMOUNT_ADDED = RemoveMoneyEvent.AMOUNT_REMOVED * 125 / 100;

        public AddMoneyEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override void Execute()
        {
            base.Execute();

            Game1.player.Money += (AMOUNT_ADDED * QueuedEvent.queueCount);
        }
    }
}
