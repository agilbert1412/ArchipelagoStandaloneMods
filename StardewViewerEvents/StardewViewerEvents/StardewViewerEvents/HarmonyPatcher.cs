using HarmonyLib;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Characters;
using StardewViewerEvents.EventsExecution.EventsImplementations.CharacterEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardewViewerEvents
{
    internal class HarmonyPatcher
    {

        public void Initialize(IMonitor logger, IModHelper helper, Harmony harmony)
        {
            InitializeTemporaryBaby(logger, helper, harmony);
        }

        private void InitializeTemporaryBaby(IMonitor logger, IModHelper helper, Harmony harmony)
        {
            TemporaryBaby.Initialize(logger, helper);

            harmony.Patch(
                original: AccessTools.Method(typeof(Child), nameof(Child.tenMinuteUpdate)),
                prefix: new HarmonyMethod(typeof(TemporaryBaby), nameof(TemporaryBaby.ChildTenMinuteUpdate_MoveBabiesAnywhere_Prefix))
            );

            harmony.Patch(
                original: AccessTools.Method(typeof(GameLocation), nameof(GameLocation.performTenMinuteUpdate)),
                prefix: new HarmonyMethod(typeof(TemporaryBaby), nameof(TemporaryBaby.GameLocationPerformTenMinuteUpdate_MoveBabiesAnywhere_Prefix))
            );
        }
    }
}
