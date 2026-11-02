using RealLifeViewerEvents.Events;
using ViewerEventsLibrary.Logging;

namespace RealLifeViewerEvents.EventsExecution.EventsImplementations
{
    public class WordBanEvent : TimedEndEvent
    {
        private int _failCount = 0;
        private string BannedWord => GetSingleParameter();

        public WordBanEvent(IViewerEventsLogger logger, QueuedEvent queuedEvent) : base(logger, queuedEvent, 60)
        {
            _failCount = 0;
        }

        public override bool ValidateParameters(out string errorMessage)
        {
            return base.ValidateParameters(out errorMessage);
        }

        public override void ReceivedInput(string? input)
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                _failCount++;
                _logger.LogInfo($"Said the word '{BannedWord}' {_failCount} times");
            }

            base.ReceivedInput(input);
        }

        public override bool UpdateAndTryFinish(int deltaMilliseconds)
        {
            var isDone = base.UpdateAndTryFinish(deltaMilliseconds);

            if (isDone)
            {
                _logger.LogInfo($"Total usages of the word '{BannedWord}': {_failCount}");
                _logger.LogInfo($"Owed refund to {QueuedEvent.username}: {BaseEvent.GetMultiplierCost(QueuedEvent.currentMultiplier) * _failCount} credits");
            }

            return isDone;
        }
    }
}
