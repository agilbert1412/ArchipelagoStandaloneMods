using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;
using StardewViewerEvents.Events.Constants;
using StardewViewerEvents.EventsExecution.EventsImplementations;

namespace StardewViewerEvents.EventsExecution
{
    public abstract class ExecutableEvent
    {
        protected readonly IMonitor _logger;
        protected readonly IModHelper _modHelper;
        protected static readonly TileChooser _tileChooser = new();
        protected QueuedEvent QueuedEvent { get; }
        private ViewerEvent BaseEvent => QueuedEvent.BaseEvent;

        public virtual bool CanBeSimultaneous => false;

        protected ExecutableEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent)
        {
            _logger = logger;
            _modHelper = modHelper;
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
            if (AnyEventActive())
            {
                return false;
            }

            if (AnyFadeActive())
            {
                return false;
            }

            if (AnyMenuActive())
            {
                return false;
            }

            return true;
        }

        protected bool AnyEventActive()
        {
            return Game1.eventUp || Game1.CurrentEvent != null;
        }

        protected bool AnyFadeActive()
        {
            return Game1.fadeIn || Game1.fadeToBlack || Game1.globalFade || Game1.nonWarpFade;
        }

        protected bool AnyMenuActive()
        {
            return Game1.activeClickableMenu != null || Game1.nextClickableMenu.Any();
        }

        public virtual void Execute()
        {
            ShowEventInChat();
        }

        /// <summary>
        /// Update this active event on every frame
        /// </summary>
        /// <returns>true if the event should finish, false if the event continue being active</returns>
        public virtual bool UpdateAndTryFinish()
        {
            return true;
        }

        private void ShowEventInChat()
        {
            var message = GetChatMessage();

            var color = Color.LightBlue;
            if (BaseEvent.alignment == Alignment.POSITIVE)
            {
                color = Color.LightGreen;
            }
            else if (BaseEvent.alignment == Alignment.NEGATIVE)
            {
                color = Color.IndianRed;
            }

            Game1.chatBox.addMessage(message, color);
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
    }
}
