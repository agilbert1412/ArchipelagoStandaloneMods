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
        protected readonly TileChooser _tileChooser;
        private QueuedEvent QueuedEvent { get; }

        protected ExecutableEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent)
        {
            _logger = logger;
            _modHelper = modHelper;
            _tileChooser = new TileChooser();
            QueuedEvent = queuedEvent;
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
