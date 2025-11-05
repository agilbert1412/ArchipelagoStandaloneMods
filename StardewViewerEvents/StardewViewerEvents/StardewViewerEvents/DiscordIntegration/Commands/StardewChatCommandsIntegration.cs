using HarmonyLib;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using Microsoft.Xna.Framework;
using StardewViewerEvents.Credits;
using StardewViewerEvents.EventsExecution;
using StardewViewerEvents.Extensions;

namespace StardewViewerEvents.DiscordIntegration.Commands
{
    public class StardewChatCommandsIntegration
    {
        public const string COMMAND_PREFIX = "|";

        private static IMonitor _logger;
        private static Harmony _harmony;
        private static HelpProvider _helpProvider;
        private static CreditsCommandsHandler _creditsCommandsHandler;
        private static EventsCommandsHandler _eventsCommandsHandler;
        private static CreditAccounts _accounts;
        private static ViewerEventsExecutor _eventsExecutor;

        private static string _lastCommand;

        public StardewChatCommandsIntegration(IMonitor logger, Harmony harmony, HelpProvider helpProvider,
            CreditsCommandsHandler creditsCommandsHandler, EventsCommandsHandler eventsCommandsHandler,
            CreditAccounts accounts, ViewerEventsExecutor eventsExecutor)
        {
            _logger = logger;
            _harmony = harmony;
            _helpProvider = helpProvider;
            _creditsCommandsHandler = creditsCommandsHandler;
            _eventsCommandsHandler = eventsCommandsHandler;
            _accounts = accounts;
            _eventsExecutor = eventsExecutor;
            _lastCommand = null;
            ListenToChatMessages();
        }

        public void ListenToChatMessages()
        {
            _harmony.Patch(
                original: AccessTools.Method(typeof(ChatBox), nameof(ChatBox.receiveChatMessage)),
                postfix: new HarmonyMethod(typeof(StardewChatCommandsIntegration), nameof(ReceiveChatMessage_ForwardToEventsIntegration_PostFix))
            );
        }

        // public virtual void receiveChatMessage(long sourceFarmer, int chatKind, LocalizedContentManager.LanguageCode language, string message)
        public static void ReceiveChatMessage_ForwardToEventsIntegration_PostFix(ChatBox __instance, long sourceFarmer, int chatKind, LocalizedContentManager.LanguageCode language, string message)
        {
            try
            {
                if (sourceFarmer == 0 || chatKind != 0)
                {
                    return;
                }

                if (TryHandleCommand(message))
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed in {nameof(ReceiveChatMessage_ForwardToEventsIntegration_PostFix)}:\n{ex}");
            }
        }

        private static bool TryHandleCommand(string message)
        {
            if (string.IsNullOrWhiteSpace(message) || !message.StartsWith(COMMAND_PREFIX))
            {
                return false;
            }

            var messageLower = message.ToLower();
            if (HandleReCommand(messageLower))
            {
                return true;
            }

            _lastCommand = message;
            var discordMessage = $"!{message.Substring(COMMAND_PREFIX.Length)}";
            _creditsCommandsHandler.HandleCreditsAdminCommands((ulong.Parse(ModEntry.Instance.Config.DiscordId), discordMessage, _accounts);
            _eventsCommandsHandler.HandleEventsAdminCommands(null, discordMessage, _eventsExecutor);
            _creditsCommandsHandler.HandleCreditsUserCommands(null, discordMessage, _accounts);
            _eventsCommandsHandler.HandleEventsUserCommands(null, discordMessage, _accounts, _eventsExecutor);
            _helpProvider.HandleHelpCommand(discordMessage, _eventsExecutor.Events);
            if (discordMessage.Equals("test", StringComparison.InvariantCultureIgnoreCase))
            {
                Game1.chatBox?.addMessage("Toast", Color.Gold);
            }

            return true;
        }

        private static bool HandleReCommand(string message)
        {
            if (message != $"{COMMAND_PREFIX}re" && message != $"{COMMAND_PREFIX}{COMMAND_PREFIX.First()}" && message != $"{COMMAND_PREFIX}redo")
            {
                return false;
            }


            return TryHandleCommand(_lastCommand);
        }
    }
}
