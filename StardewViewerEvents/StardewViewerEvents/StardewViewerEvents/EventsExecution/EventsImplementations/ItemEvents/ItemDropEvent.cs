using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.ItemEvents
{
    public class ItemDropEvent : InventoryItemEvent
    {
        public ItemDropEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        protected override void ExecuteEvent(int slotToModify)
        {
            var item = Game1.player.Items[slotToModify];
            for (var i = 0; i < item.Stack; i++)
            {
                Game1.player.dropItem(item);
            }
            Game1.player.removeItemFromInventory(item);
        }
    }
}
