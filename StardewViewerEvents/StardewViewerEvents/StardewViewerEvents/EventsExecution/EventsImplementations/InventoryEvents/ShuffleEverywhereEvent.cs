using StardewModdingAPI;
using StardewValley;
using StardewValley.Inventories;
using StardewValley.Network.ChestHit;
using StardewValley.Objects;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.InventoryEvents
{
    public class ShuffleEverywhereEvent : InventoryEvent
    {
        public const int NUMBER_SWAPS = 10;

        public ShuffleEverywhereEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override void Execute()
        {
            base.Execute();

            var numberSwaps = NUMBER_SWAPS * QueuedEvent.queueCount;
            SwapItems(numberSwaps);
        }

        public void SwapItems(int numberSwaps)
        {
            var inventories = GetAllInventoriesWithItems();

            var itemMap = GenerateItemMap(inventories);

            for (var i = 0; i < numberSwaps; i++)
            {
                SwapTwoItems(itemMap);
            }
        }
    }
}
