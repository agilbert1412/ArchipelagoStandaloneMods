using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;
using StardewViewerEvents.EventsExecution.EventsImplementations;

namespace StardewViewerEvents.EventsExecution
{
    public abstract class ExecutableEvent
    {
        protected readonly IMonitor _logger;
        protected readonly IModHelper _modHelper;
        protected static readonly TileChooser _tileChooser = new();
        private QueuedEvent QueuedEvent { get; }
        private ViewerEvent BaseEvent => QueuedEvent.BaseEvent;

        protected ExecutableEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent)
        {
            _logger = logger;
            _modHelper = modHelper;
            QueuedEvent = queuedEvent;
        }

        public virtual bool ValidateParameters()
        {
            if (BaseEvent.hasParameters)
            {
                return QueuedEvent.parameters is { Length: >= 1 };
            }

            return QueuedEvent.parameters is not { Length: > 0 };
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

            return true;
        }

        private static bool AnyEventActive()
        {
            return Game1.eventUp || Game1.CurrentEvent != null;
        }

        private static bool AnyFadeActive()
        {
            return Game1.fadeIn || Game1.fadeToBlack || Game1.globalFade || Game1.nonWarpFade;
        }

        public virtual void Execute()
        {
            ShowEventInChat();
        }

        private void ShowEventInChat()
        {
            var message = GetChatMessage();

            Game1.chatBox.addMessage(message, Color.Blue);
        }

        private string GetChatMessage()
        {
            var message = $"{QueuedEvent.BaseEvent.name}";
            if (QueuedEvent.queueCount > 1)
            {
                message += $" (x{QueuedEvent.queueCount})";
            }

            message += $" from {QueuedEvent.username}";
            return message;
        }
    }
}
