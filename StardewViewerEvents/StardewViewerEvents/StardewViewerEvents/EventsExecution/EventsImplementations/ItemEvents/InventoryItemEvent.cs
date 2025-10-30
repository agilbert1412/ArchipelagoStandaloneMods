using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.ItemEvents
{
    public abstract class InventoryItemEvent : ExecutableEvent
    {
        protected InventoryItemEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override void Execute()
        {
            base.Execute();

            var slotToModify = GetSlotToEvent();
            ExecuteEvent(slotToModify);
        }

        private int GetSlotToEvent()
        {
            var potentialSlots = new List<int>();
            for (var i = 0; i < Game1.player.MaxItems; i++)
            {
                var item = Game1.player.Items[i];
                if (item == null)
                {
                    continue;
                }

                if (IsItemValid(item))
                {
                    potentialSlots.Add(i);
                }
            }

            return potentialSlots[Game1.random.Next(potentialSlots.Count)];
        }

        protected virtual bool IsItemValid(Item item)
        {
            return item.canBeShipped() && item.canBeTrashed() && item.canBeDropped() && item.CanBeLostOnDeath();
        }

        protected abstract void ExecuteEvent(int slotToModify);
    }
}
