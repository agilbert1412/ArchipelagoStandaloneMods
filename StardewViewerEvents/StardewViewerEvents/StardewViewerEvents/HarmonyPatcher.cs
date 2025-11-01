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
using Microsoft.Xna.Framework.Graphics;

namespace StardewViewerEvents
{
    internal class HarmonyPatcher
    {

        public void Initialize(IMonitor logger, IModHelper helper, Harmony harmony)
        {
            InitializeTemporaryBaby(logger, helper, harmony);
            InitializeInvisibleCow(logger, helper, harmony);
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

        private void InitializeInvisibleCow(IMonitor logger, IModHelper helper, Harmony harmony)
        {
            CowManager.Initialize(logger, helper);

            harmony.Patch(
                original: AccessTools.Method(typeof(FarmAnimal), nameof(FarmAnimal.draw), new []{typeof(SpriteBatch)}),
                prefix: new HarmonyMethod(typeof(CowManager), nameof(CowManager.Draw_InvisibleCow_Prefix))
            );

            harmony.Patch(
                original: AccessTools.Method(typeof(FarmAnimal), nameof(FarmAnimal.dayUpdate)),
                prefix: new HarmonyMethod(typeof(CowManager), nameof(CowManager.DayUpdate_InvisibleCow_Prefix))
            );
        }
    }
}
