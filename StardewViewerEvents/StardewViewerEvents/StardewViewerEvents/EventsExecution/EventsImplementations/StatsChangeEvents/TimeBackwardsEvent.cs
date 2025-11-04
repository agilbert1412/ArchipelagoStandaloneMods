using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.StatsChangeEvents
{
    public class TimeBackwardsEvent : ExecutableEvent
    {
        public const int MINUTES_BACKWARDS = 10;

        public TimeBackwardsEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override void Execute()
        {
            base.Execute();

            var timeBackwards = MINUTES_BACKWARDS * QueuedEvent.queueCount;
            Game1.timeOfDay -= timeBackwards;
            Game1.timeOfDay -= 10;
            Game1.performTenMinuteClockUpdate();
        }
    }
}
