using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.StatsChangeEvents
{
    public class AddHealthEvent : ExecutableEvent
    {
        public const int AMOUNT_ADDED = RemoveHealthEvent.AMOUNT_REMOVED * 125 / 100;

        public AddHealthEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override void Execute()
        {
            base.Execute();

            Game1.player.health += (AMOUNT_ADDED * QueuedEvent.queueCount);
        }
    }
}
