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

            //if (AnyFadeActive())
            //{
            //    return false;
            //}

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

        private string GetChatMessage()
        {
            var message = $"{QueuedEvent.username} sent {QueuedEvent.BaseEvent.name}";
            if (QueuedEvent.queueCount > 1)
            {
                message += $" (x{QueuedEvent.queueCount})";
            }

            return message;
        }
    }
}
