using Discord.WebSocket;
using HarmonyLib;
using StardewModdingAPI;
using StardewViewerEvents.Credits;
using StardewViewerEvents.EventsExecution;

namespace StardewViewerEvents.DiscordIntegration
{
    internal class DiscordBot
    {
        private IBotCommunicator _discord;
        private DiscordModule _discordModule;

        public DiscordBot(IMonitor logger, Harmony harmony, ViewerEventsExecutor eventsExecutor, CreditAccounts creditAccounts, string path)
        {
            _discord = new DiscordWrapper(logger);
            _discordModule = new DiscordModule(logger, harmony, _discord, eventsExecutor, creditAccounts, path);
        }

        public IBotCommunicator Communications => _discord;

        public async Task InitializeAsync(string token)
        {
            _discord.InitializeClient();
            _discord.InitializeLog();

            await _discord.Login(token);
            await _discord.Start(HandleCommandAsync);
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            // Bail out if it's a System Message.
            if (arg is not SocketUserMessage msg)
            {
                return;
            }

            // We don't want the bot to respond to itself
            if (msg.Author.Id == _discord.MyId)
            {
                return;
            }

            await _discordModule.ExecuteViewerCommand(msg);
        }
    }
}
