using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.ItemEvents
{
    public class ItemQualityDownEvent : InventoryItemEvent
    {
        public ItemQualityDownEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        protected override void ExecuteEvent(int slotToModify)
        {
            var item = Game1.player.Items[slotToModify];
            item.Quality--;
            if (item.Quality == 3)
            {
                item.Quality--;
            }
            item.FixQuality();
        }
    }
}
