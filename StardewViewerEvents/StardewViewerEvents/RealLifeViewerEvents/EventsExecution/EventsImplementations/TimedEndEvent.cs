using RealLifeViewerEvents.Events;
using ViewerEventsLibrary.Logging;

namespace RealLifeViewerEvents.EventsExecution.EventsImplementations
{
    public class TimedEndEvent : ExecutableEvent
    {
        private bool _isStarted;
        private DateTime _timeStarted;
        private DateTime _lastPrinted;
        private bool _isDone;
        private int _durationInSeconds;

        public TimedEndEvent(IViewerEventsLogger logger, QueuedEvent queuedEvent, int durationInSeconds) : base(logger, queuedEvent)
        {
            _isStarted = false;
            _isDone = false;
            _timeStarted = DateTime.Now;
            _lastPrinted = DateTime.Now;
            _durationInSeconds = durationInSeconds * queuedEvent.queueCount;
        }

        public override bool CanExecuteRightNow()
        {
            return base.CanExecuteRightNow();
        }

        public override void Execute()
        {
            base.Execute();
            _isStarted = true;
            _timeStarted = DateTime.Now;
        }

        public override void ReceivedInput(string? input)
        {
            if (string.IsNullOrEmpty(input) || !_isStarted || _isDone)
            {
                return;
            }

            PrintRemainingTime();
        }

        private void PrintRemainingTime()
        {
            var remainingSeconds = GetRemainingSeconds();
            var remainingMinutes = remainingSeconds / 60;
            if (remainingMinutes > 1)
            {
                var minutes = Math.Floor(remainingMinutes);
                var seconds = Math.Round(remainingSeconds - (minutes * 60));
                Console.WriteLine($"{BaseEvent.name} - {minutes}m {seconds}s remaining");
            }
            else
            {
                var seconds = Math.Round(remainingSeconds);
                Console.WriteLine($"{BaseEvent.name} - {Math.Round(seconds)}s remaining");
            }

            _lastPrinted = DateTime.Now;
        }

        private double GetRemainingSeconds()
        {
            var time = DateTime.Now;
            var elapsed = time - _timeStarted;
            var elapsedMillieconds = elapsed.TotalMilliseconds;
            var totalMilliseconds = _durationInSeconds * 1000;
            var remainingMilliseconds = totalMilliseconds - elapsedMillieconds;
            var remainingSeconds = remainingMilliseconds / 1000;
            return remainingSeconds;
        }

        public override bool WillFinishThisFrame()
        {
            return _isDone;
        }

        public override bool UpdateAndTryFinish(int deltaMilliseconds)
        {
            var remainingSeconds = GetRemainingSeconds();
            if (remainingSeconds > 0)
            {
                var secondsSinceLastUpdate = (DateTime.Now - _lastPrinted).TotalSeconds;
                if (secondsSinceLastUpdate > 15 || Math.Round(secondsSinceLastUpdate) % 10 == 0)
                {
                    PrintRemainingTime();
                }
                return false;
            }

            _isDone = true;
            return true;
        }
    }
}
