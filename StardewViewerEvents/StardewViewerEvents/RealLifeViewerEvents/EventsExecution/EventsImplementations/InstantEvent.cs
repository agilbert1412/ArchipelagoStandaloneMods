using RealLifeViewerEvents.Events;
using ViewerEventsLibrary.Logging;

namespace RealLifeViewerEvents.EventsExecution.EventsImplementations
{
    public class InstantEvent : ExecutableEvent
    {
        public InstantEvent(IViewerEventsLogger logger, QueuedEvent queuedEvent) : base(logger, queuedEvent)
        {
        }

        public override bool CanExecuteRightNow()
        {
            return base.CanExecuteRightNow();
        }

        public override void Execute()
        {
            base.Execute();
        }

        public override void PrintNextCommandHelp()
        {
        }

        public override bool WillFinishThisFrame()
        {
            return true;
        }

        public override bool UpdateAndTryFinish(int deltaMilliseconds)
        {
            return true;
        }
    }
}
