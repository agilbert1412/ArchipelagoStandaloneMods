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
            while (timeBackwards >= 0)
            {
                Game1.timeOfDay -= 10;
                timeBackwards -= 10;
                var minutes = Game1.timeOfDay % 100;
                if (minutes > 60)
                {
                    Game1.timeOfDay = Game1.timeOfDay - minutes + 50;
                }
            }
            Game1.performTenMinuteClockUpdate();
        }
    }
}
