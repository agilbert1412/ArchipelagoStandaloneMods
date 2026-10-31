using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;
using ViewerEventsLibrary.Logging;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.StatsChangeEvents
{
    public class TimeForwardsEvent : ExecutableEvent
    {
        public const int MINUTES_FORWARD = 10;

        public TimeForwardsEvent(IViewerEventsLogger logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override void Execute()
        {
            base.Execute();

            var numberIncrements = MINUTES_FORWARD * QueuedEvent.queueCount / 10;
            for (var i = 0; i < numberIncrements; i++)
            {
                Game1.performTenMinuteClockUpdate();
            }
        }
    }
}
