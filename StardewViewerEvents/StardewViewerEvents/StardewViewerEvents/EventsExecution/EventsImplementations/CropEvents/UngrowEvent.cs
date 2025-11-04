using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.TerrainFeatures;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.CropEvents
{
    public class UngrowEvent : CropEvent
    {
        public const int NUMBER_CROPS = 10;
        public const int NUMBER_DAYS = 2;

        public UngrowEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override bool ValidateParameters(out string errorMessage)
        {
            if (!base.ValidateParameters(out errorMessage))
            {
                return false;
            }


            var crops = GetCropsToUngrow();
            if (!crops.Any())
            {
                errorMessage = $"There are currently no crops to ungrow";
                return false;
            }

            return true;
        }

        public override void Execute()
        {
            base.Execute();

            var numberUngrowth = NUMBER_CROPS * QueuedEvent.queueCount;
            UngrowCrops(numberUngrowth);
        }

        private HoeDirt[] GetCropsToUngrow()
        {
            var hoeDirts = GetAllHoeDirt();
            var crops = hoeDirts.Where(x => x.crop != null).ToArray();
            return crops;
        }

        public void UngrowCrops(int numberUngrowths)
        {
            var crops = GetCropsToUngrow();
            for (var i = 0; i < numberUngrowths; i++)
            {
                var crop = crops[Game1.random.Next(crops.Length)];
                UngrowCrop(crop.crop, NUMBER_DAYS);
            }
        }

        private void UngrowCrop(Crop crop, int days)
        {
            if (crop == null)
            {
                return;
            }

            if (crop.fullyGrown.Value)
            {
                crop.fullyGrown.Set(false);
            }

            var dayOfCurrentPhase = crop.dayOfCurrentPhase.Value;
            var currentPhase = crop.currentPhase.Value;
            var daysPerPhase = crop.phaseDays.ToList();

            if (crop.RegrowsAfterHarvest() && currentPhase >= daysPerPhase.Count - 1)
            {
                var daysSinceLastReady = Math.Max(0, crop.GetData().RegrowDays - dayOfCurrentPhase);
                days = Math.Max(1, days - daysSinceLastReady);
                dayOfCurrentPhase = 0;
            }

            dayOfCurrentPhase -= days;

            while (dayOfCurrentPhase < 0)
            {
                if (currentPhase <= 0 || !daysPerPhase.Any())
                {
                    break;
                }

                if (currentPhase > daysPerPhase.Count)
                {
                    currentPhase = daysPerPhase.Count;
                }

                currentPhase -= 1;
                var daysInCurrentPhase = daysPerPhase[currentPhase];
                dayOfCurrentPhase += daysInCurrentPhase;
            }

            if (dayOfCurrentPhase < 0)
            {
                dayOfCurrentPhase = 0;
            }

            crop.currentPhase.Set(currentPhase);
            crop.dayOfCurrentPhase.Set(dayOfCurrentPhase);
            // private Vector2 tilePosition;
            var tilePositionField = _modHelper.Reflection.GetField<Vector2>(crop, "tilePosition");
            crop.updateDrawMath(tilePositionField.GetValue());
        }
    }
}
