using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;
using ViewerEventsLibrary.Logging;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.StatsChangeEvents
{
    public class RemoveStaminaEvent : ExecutableEvent
    {
        public const int AMOUNT_REMOVED = 30;

        public RemoveStaminaEvent(IViewerEventsLogger logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override void Execute()
        {
            base.Execute();

            Game1.player.Stamina -= (AMOUNT_REMOVED * QueuedEvent.queueCount);
        }
    }
}
