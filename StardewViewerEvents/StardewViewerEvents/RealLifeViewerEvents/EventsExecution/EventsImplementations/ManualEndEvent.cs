using RealLifeViewerEvents.Events;
using ViewerEventsLibrary.Logging;

namespace RealLifeViewerEvents.EventsExecution.EventsImplementations
{
    public class ManualEndEvent : ExecutableEvent
    {
        private bool _isStarted;
        private bool _isDone;

        public ManualEndEvent(IViewerEventsLogger logger, QueuedEvent queuedEvent) : base(logger, queuedEvent)
        {
            _isStarted = false;
            _isDone = false;
        }

        public override bool CanExecuteRightNow()
        {
            return base.CanExecuteRightNow();
        }

        public override void Execute()
        {
            base.Execute();
            _isStarted = true;
        }

        public override void ReceivedInput(string? input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return;
            }

            if (input.ToLower().StartsWith("d"))
            {
                _isDone = true;
            }
        }

        public override void PrintNextCommandHelp()
        {
            Console.WriteLine($"Type 'done' when done");
        }

        public override bool WillFinishThisFrame()
        {
            return _isDone;
        }

        public override bool UpdateAndTryFinish(int deltaMilliseconds)
        {
            return _isDone;
        }
    }
}
