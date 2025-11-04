using HarmonyLib;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewViewerEvents.DiscordIntegration;
using StardewViewerEvents.EventsExecution;
using StardewViewerEvents.Integrations.GenericModConfigMenu;

namespace StardewViewerEvents
{
    internal sealed class ModEntry : Mod
    {
        public static ModEntry Instance;
        public ModConfig Config;
        private IModHelper _helper;
        private Harmony _harmony;
        private ViewerEventsService _viewerEventsService;
        private ViewerEventsExecutor _viewerEventsExecutor;
        private HarmonyPatcher _harmonyPatcher;

        public ModEntry() : base()
        {
            Instance = this;
        }

        /*********
         ** Public methods
         *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            _helper = helper;
            _harmony = new Harmony(ModManifest.UniqueID);

            Config = Helper.ReadConfig<ModConfig>();

            _viewerEventsExecutor = new ViewerEventsExecutor(Monitor);
            _viewerEventsService = new ViewerEventsService(Monitor, Config, _viewerEventsExecutor);

            _helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            _helper.Events.GameLoop.SaveCreating += OnSaveCreating;
            _helper.Events.GameLoop.SaveLoaded += OnSaveLoading;
            _helper.Events.GameLoop.DayStarted += OnDayStarted;
            _helper.Events.GameLoop.DayEnding += OnDayEnding;
            _helper.Events.GameLoop.UpdateTicked += OnUpdateTicked;

            _harmonyPatcher = new HarmonyPatcher();
            _harmonyPatcher.Initialize(Monitor, _helper, _harmony);
        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            ResetModIntegrations();
        }

        private void OnSaveCreating(object sender, SaveCreatingEventArgs e)
        {
            _helper.WriteConfig(Config);
        }

        private void OnSaveLoading(object? sender, SaveLoadedEventArgs e)
        {
            var dataPath = Path.Combine(_helper.DirectoryPath, "PersistentData");
            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }
            _viewerEventsService.Initialize(dataPath);
        }

        private void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            _viewerEventsExecutor.Queue.Unpause();
        }

        private void OnDayEnding(object? sender, DayEndingEventArgs e)
        {
            _viewerEventsExecutor.Queue.Pause();
            //_viewerEventsService.DiscordCommunications
        }

        private void OnUpdateTicked(object? sender, UpdateTickedEventArgs e)
        {
            if (!_viewerEventsService.IsInitialized)
            {
                return;
            }

            _viewerEventsExecutor.Update();

            var framesBetweenEvents = Config.EventsDelay * 60;
            if (e.IsMultipleOf(framesBetweenEvents))
            {
                _viewerEventsExecutor.DequeueEvent(Monitor, Helper, _viewerEventsService.DiscordCommunications, _viewerEventsService.CreditAccounts, DiscordModule.ActiveChannels);
            }
        }

        private static bool ArchipelagoIsLoaded(IModHelper helper)
        {
            return helper.ModRegistry.IsLoaded("KaitoKid.StardewArchipelago");
        }

        private void ResetModIntegrations()
        {
            var GenericModConfigMenu = new GenericModConfig(this);
            GenericModConfigMenu.RegisterConfig();
        }
    }
}
