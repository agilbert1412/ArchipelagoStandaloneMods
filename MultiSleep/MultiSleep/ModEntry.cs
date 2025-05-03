using HarmonyLib;
using MultiSleep.Integrations.GenericModConfigMenu;
using MultiSleep.MultiSleep;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace MultiSleep
{
    internal sealed class ModEntry : Mod
    {
        public static ModEntry Instance;
        public ModConfig Config;
        private IModHelper _helper;
        private Harmony _harmony;
        private MultiSleepManager _multiSleepManager;

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
            if (ArchipelagoIsLoaded(helper))
            {
                return;
            }

            _helper = helper;
            _harmony = new Harmony(ModManifest.UniqueID);
            _multiSleepManager = new MultiSleepManager(Monitor, _helper, _harmony);

            Config = Helper.ReadConfig<ModConfig>();
            _helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            _helper.Events.GameLoop.SaveCreating += OnSaveCreating;
            _helper.Events.GameLoop.DayStarted += OnDayStarted;

            _multiSleepManager.InjectMultiSleepOption(Config.MultiSleepCostPerDay);
        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            ResetModIntegrations();
        }

        private void OnSaveCreating(object sender, SaveCreatingEventArgs e)
        {
            _helper.WriteConfig(Config);
        }

        private void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            if (MultiSleepManager.TryDoMultiSleepOnDayStarted())
            {
                return;
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
