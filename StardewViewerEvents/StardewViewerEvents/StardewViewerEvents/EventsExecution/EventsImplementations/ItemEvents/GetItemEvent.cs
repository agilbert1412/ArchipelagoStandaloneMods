using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.ItemEvents
{
    public abstract class GetItemEvent : ExecutableEvent
    {
        protected GetItemEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override void Execute()
        {
            base.Execute();

            var itemId = GetItemId();
            var itemAmount = GetItemAmount();
            var item = ItemRegistry.Create(itemId, itemAmount);
            GiveItemToPlayer(item);
        }

        public abstract string GetItemId();

        public virtual int GetItemAmount()
        {
            return 1;
        }

        public virtual void GiveItemToPlayer(Item item)
        {
            Game1.player.addItemByMenuIfNecessary(item);
        }
    }
}
