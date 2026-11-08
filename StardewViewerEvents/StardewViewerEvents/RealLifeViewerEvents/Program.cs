using Newtonsoft.Json;
using RealLifeViewerEvents.DiscordIntegration;
using RealLifeViewerEvents.EventsExecution;
using RealLifeViewerEvents.Extensions;
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
                _viewerEventsExecutor.Queue.PrintToConsole();
                StartQueueProcessAsync().FireAndForget();
                StartInputProcessAsync().FireAndForget();
                _viewerEventsExecutor.Queue.Unpause();

                await Task.Delay(-1);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
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
                    _logger.LogError(e);
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
        }

        private static async Task StartInputProcessAsync()
        {
            const int tickDelay = 100;
            while (true)
            {
                try
                {
                    var input = Console.ReadLine();
                    if (!HandleInput(input))
                    {
                        _viewerEventsExecutor.ReceivedInput(input, _viewerEventsService);
                        _viewerEventsExecutor.PrintNextCommandHelp();
                    }

                    _viewerEventsService.ExportData();
                }
                catch (Exception e)
                {
                    _logger.LogError(e);
                }

                await Task.Run(() => Thread.Sleep(tickDelay));
            }
        }

        public static readonly string[] COMMANDS_START_EVENT = new[] { "go", "start", "trigger" };
        public const string COMMAND_SKIP_EVENT = "skip";
        public const string COMMAND_SEE_QUEUE = "queue";
        public const string COMMAND_SEE_NEXT_EVENT = "next";
        public const string COMMAND_HELP = "help";
        public const string COMMAND_REFUND = "refund";
        public const string COMMAND_TRANSFER_FROM_OTHER_ACCOUNT = "transfer_from_other_accounts";

        private static bool HandleInput(string input)
        {
            var sanitizedInput = input.SanitizeName();
            if (COMMANDS_START_EVENT.Contains(sanitizedInput))
            {
                _viewerEventsExecutor.DequeueEvent(_logger, _viewerEventsService.DiscordCommunications,
                    _viewerEventsService.CreditAccounts, DiscordModule.ActiveChannels).FireAndForget();
                return true;
            }

            if (sanitizedInput == COMMAND_SKIP_EVENT)
            {
                _viewerEventsExecutor.SkipEvent();
                _viewerEventsExecutor.Queue.PrintToConsole();
                return true;
            }

            if (sanitizedInput == COMMAND_SEE_QUEUE)
            {
                _viewerEventsExecutor.Queue.PrintToConsole();
                return true;
            }

            if (sanitizedInput == COMMAND_SEE_NEXT_EVENT)
            {
                _viewerEventsExecutor.Queue.PrintNextEventAndCommand();
                return true;
            }

            if (sanitizedInput == COMMAND_REFUND)
            {
                _viewerEventsExecutor.RefundNextEvent(_logger, _viewerEventsService.DiscordCommunications,
                    _viewerEventsService.CreditAccounts, DiscordModule.ActiveChannels);
                return true;
            }

            if (sanitizedInput.StartsWith(COMMAND_TRANSFER_FROM_OTHER_ACCOUNT.SanitizeName()))
            {
                DoTransferFromOtherAccounts(input);
                return true;
            }

            if (sanitizedInput == COMMAND_HELP)
            {
                _logger.LogInfo($"{COMMANDS_START_EVENT.First()}: Start the next event");
                _logger.LogInfo($"{COMMAND_SKIP_EVENT}: Skip the next event and send it to the back of the queue");
                _logger.LogInfo($"{COMMAND_SEE_QUEUE}: Show the full queue");
                _logger.LogInfo($"{COMMAND_SEE_NEXT_EVENT}: Show next event");
                _logger.LogInfo(
                    $"{COMMAND_REFUND}: Refund the next event to whoever paid for it and remove it from the queue");
                _logger.LogInfo($"{COMMAND_TRANSFER_FROM_OTHER_ACCOUNT}: Transfer credits remaining and spent, at custom rates (default 100% and 20%), from another credits file from a previous game.");
                return true;
            }

            return false;
        }

        private static void DoTransferFromOtherAccounts(string command)
        {
            var inputWords = command.Split(" ");

            var path = Path.Combine(Directory.GetCurrentDirectory(), "PersistentData",
                $"Transfert_{DiscordModule.CREDITS_FILE}");

            var refundRateForRemainingCredits = 1.0;
            var refundRateForSpentCredits = 0.2;

            if (inputWords.Length > 1)
            {
                refundRateForRemainingCredits = double.Parse(inputWords[1]);
            }

            if (inputWords.Length > 2)
            {
                refundRateForSpentCredits = double.Parse(inputWords[2]);
            }

            var changes = _viewerEventsService.CreditAccounts.DoPartialRefundFrom(path, refundRateForRemainingCredits,
                refundRateForSpentCredits);

            if (changes == null)
            {
                _logger.LogError($"Could not find a credits file to transfer from. Place a file at `{path}`");
                return;
            }

            _logger.LogInfo($"Successfully added credits to {changes.Count} viewers:");

            foreach (var (discordId, amount) in changes)
            {
                var account = _viewerEventsService.CreditAccounts[discordId];
                var name = account.discordName;
                _logger.LogInfo($"\t{name}: {amount} credits added (Current: {account.GetCredits()})");
            }
        }
    }
}