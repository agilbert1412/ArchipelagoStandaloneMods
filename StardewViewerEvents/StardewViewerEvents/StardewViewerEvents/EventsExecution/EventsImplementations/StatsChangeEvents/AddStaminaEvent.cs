using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.StatsChangeEvents
{
    public class AddStaminaEvent : ExecutableEvent
    {
        public const int AMOUNT_ADDED = RemoveStaminaEvent.AMOUNT_REMOVED * 125 / 100;

        public AddStaminaEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override void Execute()
        {
            base.Execute();

            Game1.player.Stamina += (AMOUNT_ADDED * QueuedEvent.queueCount);
        }
    }
}
