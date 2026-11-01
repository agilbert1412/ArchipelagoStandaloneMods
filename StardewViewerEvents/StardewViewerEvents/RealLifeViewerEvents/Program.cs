using System.Reflection;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using RealLifeViewerEvents.DiscordIntegration;
using RealLifeViewerEvents.EventsExecution;
using ViewerEventsLibrary.Extensions;

namespace RealLifeViewerEvents
{
    public class Program
    {
        private static RealLifeLogger _logger;
        private static Config _config;
        private static ViewerEventsService _viewerEventsService;
        private static ViewerEventsExecutor _viewerEventsExecutor;

        public static async Task Main(string[] args)
        {
            try
            {
                _logger = new RealLifeLogger();
                _config = ReadConfig();
                _viewerEventsExecutor = new ViewerEventsExecutor(_logger);
                _viewerEventsService = new ViewerEventsService(_logger, _config, _viewerEventsExecutor);

                InitializePersistentData();
                StartQueueProcessAsync().FireAndForget();
                StartInputProcessAsync().FireAndForget();
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

        private static string ConfigPath => Path.Combine(Directory.GetCurrentDirectory(), "Config.json");

        private static Config ReadConfig()
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

        private static void WriteConfig(Config config)
        {
            var path = ConfigPath;
            var serializedConfig = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(path, serializedConfig);
        }

        private static void InitializePersistentData()
        {
            var dataPath = Path.Combine(Directory.GetCurrentDirectory(), "PersistentData");
            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }
            _viewerEventsService.Initialize(dataPath);
        }

        private static async Task StartQueueProcessAsync()
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

        private static int tickCount = 0;
        private static void OnTick(int deltaMilliseconds)
        {
            if (!_viewerEventsService.IsInitialized)
            {
                return;
            }

            _viewerEventsExecutor.Update(deltaMilliseconds);
            _viewerEventsExecutor.DequeueEvent(_logger, _viewerEventsService.DiscordCommunications, _viewerEventsService.CreditAccounts, DiscordModule.ActiveChannels);
        }

        private static async Task StartInputProcessAsync()
        {
            const int tickDelay = 100;
            while (true)
            {
                try
                {
                    var input = Console.ReadLine();
                    _viewerEventsExecutor.ReceivedInput(input);
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
    }
}