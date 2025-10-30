using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.ItemEvents
{
    public class ItemTrashEvent : InventoryItemEvent
    {
        public ItemTrashEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        protected override void ExecuteEvent(int slotToModify)
        {
            var item = Game1.player.Items[slotToModify];
            if (Utility.getTrashReclamationPrice(item, Game1.player) > 0)
                Game1.player.Money += Utility.getTrashReclamationPrice(item, Game1.player);
            Game1.player.removeItemFromInventory(item);
            Game1.playSound("trashcan");
        }
    }
}
