using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.StatsChangeEvents
{
    public class RemoveStaminaEvent : ExecutableEvent
    {
        public const int AMOUNT_REMOVED = 30;

        public RemoveStaminaEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override void Execute()
        {
            base.Execute();

            Game1.player.Stamina -= (AMOUNT_REMOVED * QueuedEvent.queueCount);
        }
    }
}
