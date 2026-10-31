using StardewModdingAPI;
using StardewValley;
using StardewValley.Inventories;
using StardewViewerEvents.Events;
using ViewerEventsLibrary.Logging;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.InventoryEvents
{
    public class ShuffleInventoryEvent : InventoryEvent
    {
        public const int NUMBER_SWAPS = 10;

        public ShuffleInventoryEvent(IViewerEventsLogger logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
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
            var inventories = new List<Inventory>()
            {
                Game1.player.Items,
            };

            var itemMap = GenerateItemMap(inventories);

            for (var i = 0; i < numberSwaps; i++)
            {
                SwapTwoItems(itemMap);
            }
        }
    }
}
