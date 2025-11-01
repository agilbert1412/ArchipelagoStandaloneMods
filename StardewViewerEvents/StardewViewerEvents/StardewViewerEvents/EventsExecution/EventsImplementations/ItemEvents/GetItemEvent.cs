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

        public override bool CanExecuteRightNow()
        {
            if (!base.CanExecuteRightNow())
            {
                return false;
            }

            if (AnyMenuActive())
            {
                return false;
            }

            return true;
        }

        public override void Execute()
        {
            base.Execute();

            var itemAmount = GetItemAmount();
            var items = new List<Item>();
            for (var i = 0; i < itemAmount; i++)
            {
                var itemId = GetItemId();
                var item = ItemRegistry.Create(itemId);
                items.Add(item);
            }
            GiveItemsToPlayer(items);
        }

        public abstract string GetItemId();

        public virtual int GetItemAmount()
        {
            return QueuedEvent.queueCount;
        }

        public virtual void GiveItemsToPlayer(List<Item> items)
        {
            Game1.player.addItemsByMenuIfNecessary(items);
        }
    }
}
