using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.DurationEvents
{
    public abstract class DurationEvent : ExecutableEvent
    {
        protected int _tickStarted;
        protected TimeSpan _gametimeStarted;
        protected DateTime _realTimeStarted;

        protected abstract int TicksDuration { get; }
        protected abstract int SecondsDuration { get; }

        protected DurationEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override bool CanExecuteRightNow()
        {
            return base.CanExecuteRightNow();
        }

        public override void Execute()
        {
            base.Execute();

            _tickStarted = Game1.ticks;
            _gametimeStarted = Game1.currentGameTime.TotalGameTime;
            _realTimeStarted = DateTime.Now;
        }

        protected override string AppendQueueCount(string message)
        {
            if (QueuedEvent.queueCount > 1)
            {
                message += $" for {secondsDuration}s";
            }

            return message;
        }
    }
}
