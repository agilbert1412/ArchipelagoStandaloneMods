using StardewModdingAPI;
using StardewViewerEvents.DiscordIntegration;
using StardewViewerEvents.EventsExecution;
using StardewViewerEvents.Extensions;

namespace StardewViewerEvents
{
    public class ViewerEventsService
    {
        private IMonitor _logger;
        private ModConfig _config;
        private ViewerEventsExecutor _eventsExecutor;

        public ViewerEventsService(IMonitor logger, ModConfig config, ViewerEventsExecutor eventsExecutor)
        {
            _logger = logger;
            _config = config;
            _eventsExecutor = eventsExecutor;
        }

        public async Task Initialize(string path)
        {
            await InitializeDiscordIntegration(path);
            await InitializeTwitchIntegration(path);
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
                var bot = new DiscordBot(_logger, _eventsExecutor, path);
                await bot.InitializeAsync(_config.DiscordToken);

                _logger.LogInfo($"Discord Integration Initialized!");
                await Task.Delay(-1);
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
