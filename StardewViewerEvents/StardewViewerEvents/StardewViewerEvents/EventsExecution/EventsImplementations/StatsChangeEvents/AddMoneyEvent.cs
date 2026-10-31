using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;
using ViewerEventsLibrary.Logging;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.StatsChangeEvents
{
    public class AddMoneyEvent : ExecutableEvent
    {
        public const int AMOUNT_ADDED = RemoveMoneyEvent.AMOUNT_REMOVED;

        public AddMoneyEvent(IViewerEventsLogger logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override void Execute()
        {
            base.Execute();

            Game1.player.Money += (AMOUNT_ADDED * QueuedEvent.queueCount);
        }
    }
}
