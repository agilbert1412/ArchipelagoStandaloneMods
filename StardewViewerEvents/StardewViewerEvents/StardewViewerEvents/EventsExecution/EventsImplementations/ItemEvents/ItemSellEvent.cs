using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.ItemEvents
{
    public class ItemSellEvent : InventoryItemEvent
    {
        public ItemSellEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        protected override void ExecuteEvent(int slotToModify)
        {
            var item = Game1.player.Items[slotToModify];
            var price = item.sellToStorePrice() * item.Stack;
            Game1.player.Money += price;
            Game1.player.removeItemFromInventory(item);
            Game1.playSound("sell");
        }
    }
}
