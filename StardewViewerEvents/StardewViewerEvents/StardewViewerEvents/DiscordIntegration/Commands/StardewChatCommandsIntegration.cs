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
        private static SimplifiedCommandsHandler _simplifiedCommandsHandler;
        private static CreditAccounts _accounts;
        private static ViewerEventsExecutor _eventsExecutor;

        private static string _lastCommand;

        public StardewChatCommandsIntegration(IMonitor logger, Harmony harmony, HelpProvider helpProvider,
            CreditAccounts accounts, ViewerEventsExecutor eventsExecutor, CommandReader commandReader)
        {
            _logger = logger;
            _harmony = harmony;
            _helpProvider = helpProvider;
            _simplifiedCommandsHandler = new SimplifiedCommandsHandler(commandReader, COMMAND_PREFIX);
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

            if (message.Equals("test", StringComparison.InvariantCultureIgnoreCase))
            {
                Game1.chatBox?.addMessage("Toast", Color.Gold);
                return true;
            }

            if (string.IsNullOrWhiteSpace(ModEntry.Instance.Config.DiscordId))
            {
                Game1.chatBox?.addMessage("You must first set up the Discord link in your mod config", Color.Gold);
                return true;
            }

            _lastCommand = message;
            var author = new Author(ulong.Parse(ModEntry.Instance.Config.DiscordId), Game1.player.Name);

            if (_simplifiedCommandsHandler.HandleCreditsAdminCommands(message, _accounts, out var response) ||
                _simplifiedCommandsHandler.HandleEventsAdminCommands(message, _eventsExecutor, out response) ||
                _simplifiedCommandsHandler.HandleCreditsUserCommands(message, _accounts, author, out response) ||
                _simplifiedCommandsHandler.HandleEventsUserCommands(message, _accounts, _eventsExecutor, author, out response))
            {
                Game1.chatBox?.addMessage(response, Color.Gold);
                return true;
            }

            _helpProvider.HandleHelpCommand(response, _eventsExecutor.Events, COMMAND_PREFIX);
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
