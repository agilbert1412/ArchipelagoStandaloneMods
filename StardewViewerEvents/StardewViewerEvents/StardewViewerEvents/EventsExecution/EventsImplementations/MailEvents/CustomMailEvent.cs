using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Internal;
using StardewValley.Menus;
using StardewValley.Tools;
using StardewViewerEvents.Events;
using StardewViewerEvents.Extensions;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.MailEvents
{
    public class CustomMailEvent : ExecutableEvent
    {
        public CustomMailEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override bool ValidateParameters()
        {
            var content = GetSingleParameter();
            return !string.IsNullOrWhiteSpace(content);
        }

        public override bool CanExecuteRightNow()
        {
            if (!base.CanExecuteRightNow())
            {
                return false;
            }

            if (AnyMenuActive())
            {
                return false;
            }

            return true;
        }

        public override void Execute()
        {
            base.Execute();
            var content = GetSingleParameter();
            var formattedContent = content.Replace("\r\n", "^").Replace("\r", "^").Replace("\n", "^");
            var mailKey = $"CustomMailEvent_from_{QueuedEvent.username.SanitizeEntityName()}_{DateTime.Now.ToShortDateString()}_{DateTime.Now.ToLongTimeString()}";
            var mailData = DataLoader.Mail(Game1.content);
            mailData[mailKey] = formattedContent;
            Game1.player.mailReceived.Add(mailKey);
            Game1.activeClickableMenu = new LetterViewerMenu(formattedContent, mailKey);
        }
    }
}
