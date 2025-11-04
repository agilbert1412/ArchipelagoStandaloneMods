using StardewModdingAPI;
using StardewValley;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.CropEvents
{
    public class DroughtEvent : CropEvent
    {
        public const int NUMBER_UNWATER = 20;
        public const int AMOUNT_REMOVED_FROM_CAN = 20;

        public DroughtEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override bool ValidateParameters(out string errorMessage)
        {
            if (!base.ValidateParameters(out errorMessage))
            {
                return false;
            }


            var hoeDirts = GetAllHoeDirt();
            if (!hoeDirts.Any())
            {
                errorMessage = $"There are currently no hoed dirt to dry";
                return false;
            }

            return true;
        }

        public override void Execute()
        {
            base.Execute();

            var numberUngrowth = NUMBER_UNWATER * QueuedEvent.queueCount;
            DryCrops(numberUngrowth);
        }

        public void DryCrops(int numberToDry)
        {
            var hoeDirts = GetAllHoeDirt().ToArray();
            for (var i = 0; i < numberToDry; i++)
            {
                var hoeDirt = hoeDirts[Game1.random.Next(hoeDirts.Length)];
                DryCrop(hoeDirt);
            }

            var waterToRemove = AMOUNT_REMOVED_FROM_CAN * QueuedEvent.queueCount;
            foreach (var wateringCan in GetAllWateringCans())
            {
                wateringCan.WaterLeft = Math.Max(0, wateringCan.WaterLeft - waterToRemove);
            }
        }

        private void DryCrop(HoeDirt hoeDirt)
        {
            if (hoeDirt.state.Value == 1)
            {
                hoeDirt.state.Value = 0;
            }
        }


        private IEnumerable<WateringCan> GetAllWateringCans()
        {
            foreach (var item in Game1.player.Items)
            {
                if (item is not WateringCan wateringCan)
                {
                    continue;
                }

                yield return wateringCan;
            }


            foreach (var gameLocation in Game1.locations)
            {
                foreach (var (tile, gameObject) in gameLocation.Objects.Pairs)
                {
                    if (gameObject is not Chest chest)
                    {
                        continue;
                    }

                    foreach (var chestItem in chest.Items)
                    {
                        if (chestItem is not WateringCan wateringCan)
                        {
                            continue;
                        }

                        yield return wateringCan;
                    }
                }
            }
        }
    }
}
