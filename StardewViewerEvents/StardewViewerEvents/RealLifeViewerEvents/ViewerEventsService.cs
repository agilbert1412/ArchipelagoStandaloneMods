using RealLifeViewerEvents.DiscordIntegration;
using RealLifeViewerEvents.DiscordIntegration.Commands;
using RealLifeViewerEvents.EventsExecution;
using ViewerEventsLibrary;
using ViewerEventsLibrary.Credits;
using ViewerEventsLibrary.DiscordIntegration.Commands;

namespace RealLifeViewerEvents
{
    public class ViewerEventsService
    {
        public static ViewerEventsService Instance;

        private readonly RealLifeLogger _logger;
        private Config _config;
        private ViewerEventsExecutor _eventsExecutor;
        private DiscordBot _discordBot;
        private CreditAccounts _creditAccounts;
        private readonly CommandReader _commandReader;

        public CreditAccounts CreditAccounts => _creditAccounts;

        public IBotCommunicator DiscordCommunications => _discordBot.Communications;
        public bool IsInitialized { get; set; }

        public ViewerEventsService(RealLifeLogger logger, Config config, ViewerEventsExecutor eventsExecutor)
        {
            if (Instance != null)
            {
                throw new Exception($"Cannot initialize the service more than once");
            }

            _logger = logger;
            _config = config;
            _eventsExecutor = eventsExecutor;
            _commandReader = new CommandReader();
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
            InitializeTwitchIntegration(path);
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
                _discordBot = new DiscordBot(_logger, _eventsExecutor, _creditAccounts, _commandReader, path);
                await _discordBot.InitializeAsync(_config.DiscordToken);

                _logger.LogInfo($"Discord Integration Initialized!");
            }
            catch (Exception e)
            {
                _logger.LogError($"Could not initialize Discord Integration.", e);
            }
        }

        private void InitializeTwitchIntegration(string path)
        {
            if (string.IsNullOrWhiteSpace(_config.TwitchBotUsername) || string.IsNullOrWhiteSpace(_config.TwitchBotToken) || string.IsNullOrWhiteSpace(_config.TwitchChannel))
            {
                return;
            }
            
            try
            {
                _logger.LogInfo($"Initializing Twitch Integration...");
                var bot = new TwitchBot(_logger, _config, _creditAccounts, _eventsExecutor, _commandReader);

                _logger.LogInfo($"Twitch Integration Initialized!");
            }
            catch (Exception e)
            {
                _logger.LogError($"Could not initialize Twitch Integration.", e);
            }
        }
    }
}
