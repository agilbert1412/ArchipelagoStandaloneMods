using RealLifeViewerEvents.Events;
using ViewerEventsLibrary.Events;
using ViewerEventsLibrary.Logging;

namespace RealLifeViewerEvents.EventsExecution
{
    public abstract class ExecutableEvent
    {
        protected readonly IViewerEventsLogger _logger;
        protected QueuedEvent QueuedEvent { get; }
        private ViewerEvent BaseEvent => QueuedEvent.BaseEvent;

        public virtual bool CanBeSimultaneous => false;

        protected ExecutableEvent(IViewerEventsLogger logger, QueuedEvent queuedEvent)
        {
            QueuedEvent = queuedEvent;
        }

        public virtual bool ValidateParameters(out string errorMessage)
        {
            if (BaseEvent.hasParameters)
            {
                errorMessage = $"Event '{QueuedEvent.baseEventName}' requires parameters to be triggered.";
                return QueuedEvent.parameters is { Length: >= 1 };
            }

            errorMessage = $"Event '{QueuedEvent.baseEventName}' should not have parameters. [{string.Join(", ", QueuedEvent.parameters)}].";
            return QueuedEvent.parameters is not { Length: > 0 };
        }

        protected virtual string GetSingleParameter()
        {
            return string.Join(" ", QueuedEvent.parameters);
        }

        public virtual bool CanExecuteRightNow()
        {
            return true;
        }

        public virtual void Execute()
        {
            ShowEventInChat();
        }

        /// <summary>
        /// Update this active event on every frame
        /// </summary>
        /// <returns>true if the event should finish, false if the event continue being active</returns>
        public virtual bool UpdateAndTryFinish(int deltaMilliseconds)
        {
            return true;
        }

        private void ShowEventInChat()
        {
            var message = GetChatMessage();

            Console.WriteLine(message);
        }

        protected virtual string GetChatMessage()
        {
            var message = $"{QueuedEvent.username} sent {QueuedEvent.BaseEvent.name}";

            message = AppendParameters(message);

            message = AppendQueueCount(message);

            return message;
        }

        protected virtual string AppendParameters(string message)
        {
            if (QueuedEvent.parameters.Any())
            {
                message += $" {GetSingleParameter()}";
            }

            return message;
        }

        protected virtual string AppendQueueCount(string message)
        {
            if (QueuedEvent.queueCount > 1)
            {
                message += $" (x{QueuedEvent.queueCount})";
            }

            return message;
        }

        public virtual void ReceivedInput(string? input)
        {
        }
    }
}
