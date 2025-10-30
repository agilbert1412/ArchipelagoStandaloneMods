using StardewModdingAPI;
using StardewViewerEvents.Credits;
using StardewViewerEvents.DiscordIntegration;
using StardewViewerEvents.EventsExecution;
using StardewViewerEvents.Extensions;
using System.IO;

namespace StardewViewerEvents
{
    public class ViewerEventsService
    {
        public static ViewerEventsService Instance;

        private IMonitor _logger;
        private ModConfig _config;
        private ViewerEventsExecutor _eventsExecutor;
        private DiscordBot _discordBot;
        private CreditAccounts _creditAccounts;

        public CreditAccounts CreditAccounts => _creditAccounts;

        public IBotCommunicator DiscordCommunications => _discordBot.Communications;
        public bool IsInitialized { get; set; }

        public ViewerEventsService(IMonitor logger, ModConfig config, ViewerEventsExecutor eventsExecutor)
        {
            if (Instance != null)
            {
                throw new Exception($"Cannot initialize the service more than once");
            }

            _logger = logger;
            _config = config;
            _eventsExecutor = eventsExecutor;
            IsInitialized = false;
            Instance = this;
        }

        public async Task Initialize(string path)
        {
            if (IsInitialized)
            {
                return;
            }

            _creditAccounts = new CreditAccounts(path);
            await InitializeDiscordIntegration(path);
            await InitializeTwitchIntegration(path);
            _creditAccounts.SetCommunicator(_discordBot.Communications);
            IsInitialized = true;
        }

        private async Task InitializeDiscordIntegration(string path)
        {
            if (string.IsNullOrWhiteSpace(_config.DiscordToken))
            {
                return;
            }

            try
            {
                _logger.LogInfo($"Initializing Discord Integration...");
                _discordBot = new DiscordBot(_logger, _eventsExecutor, _creditAccounts, path);
                await _discordBot.InitializeAsync(_config.DiscordToken);

                _logger.LogInfo($"Discord Integration Initialized!");
            }
            catch (Exception e)
            {
                _logger.LogError($"Could not initialize Discord Integration.", e);
            }
        }

        private async Task InitializeTwitchIntegration(string path)
        {
            if (string.IsNullOrWhiteSpace(_config.TwitchToken))
            {
                return;
            }


            try
            {
                throw new NotImplementedException($"Twitch Integration is not implemented at the moment");

                //_logger.LogInfo($"Initializing Twitch Integration...");
                //var bot = new TwitchBot();
                //await bot.InitializeAsync();

                //_logger.LogInfo($"Twitch Integration Initialized!");
                //await Task.Delay(-1);
            }
            catch (Exception e)
            {
                _logger.LogError($"Could not initialize Twitch Integration.", e);
            }
        }
    }
}
