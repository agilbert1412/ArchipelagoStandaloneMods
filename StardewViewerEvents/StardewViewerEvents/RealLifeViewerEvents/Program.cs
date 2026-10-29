using System.Reflection;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using RealLifeViewerEvents.Extensions;

namespace RealLifeViewerEvents
{
    public class Program
    {
        private Config _config;
        private ViewerEventsService _viewerEventsService;
        private ViewerEventsExecutor _viewerEventsExecutor;

        public async Task Main(string[] args)
        {
            try
            {
                _config = ReadConfig();
                _viewerEventsExecutor = new ViewerEventsExecutor();
                _viewerEventsService = new ViewerEventsService(_config, _viewerEventsExecutor);

                InitializePersistentData();
                StartQueueProcessAsync().FireAndForget();
                _viewerEventsExecutor.Queue.Unpause();

                await Task.Delay(-1);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine();
                Console.WriteLine("Stack Trace: " + Environment.NewLine + "\t");
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                Console.ReadKey();
            }
        }

        private string ConfigPath => Path.Combine(Directory.GetCurrentDirectory(), "Config.json");

        private Config ReadConfig()
        {
            var path = ConfigPath;
            if (!File.Exists(path))
            {
                var newConfig = new Config();
                WriteConfig(newConfig);
                return newConfig;
            }

            var configContent = File.ReadAllText(path);
            var config = JsonConvert.DeserializeObject<Config>(configContent);
            return config;
        }

        private void WriteConfig(Config config)
        {
            var path = ConfigPath;
            var serializedConfig = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(path, serializedConfig);
        }

        private void InitializePersistentData()
        {
            var dataPath = Path.Combine(Directory.GetCurrentDirectory(), "PersistentData");
            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }
            _viewerEventsService.Initialize(dataPath);
        }

        private async Task StartQueueProcessAsync()
        {
            const int tickDelay = 1000;
            while (true)
            {
                try
                {
                    OnTick(tickDelay);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine();
                    Console.WriteLine("Stack Trace: " + Environment.NewLine + "\t");
                    Console.WriteLine(e.StackTrace);
                }

                await Task.Run(() => Thread.Sleep(tickDelay));
            }
        }

        private int tickCount = 0;
        private void OnTick(int deltaMilliseconds)
        {
            if (!_viewerEventsService.IsInitialized)
            {
                return;
            }

            _viewerEventsExecutor.Update(deltaMilliseconds);
            _viewerEventsExecutor.DequeueEvent(_viewerEventsService.DiscordCommunications, _viewerEventsService.CreditAccounts, DiscordModule.ActiveChannels);
        }
    }
}