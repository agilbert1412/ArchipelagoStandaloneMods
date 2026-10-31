using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;
using ViewerEventsLibrary.Logging;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.StatsChangeEvents
{
    public class RemoveMoneyEvent : ExecutableEvent
    {
        public const int AMOUNT_REMOVED = 500;

        public RemoveMoneyEvent(IViewerEventsLogger logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override void Execute()
        {
            base.Execute();

            Game1.player.Money -= (AMOUNT_REMOVED * QueuedEvent.queueCount);
        }
    }
}
