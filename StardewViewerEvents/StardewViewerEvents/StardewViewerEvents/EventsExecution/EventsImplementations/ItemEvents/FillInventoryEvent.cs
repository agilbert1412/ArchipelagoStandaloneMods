using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.ItemEvents
{
    public class FillInventoryEvent : GetItemEvent
    {
        public FillInventoryEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override void Execute()
        {
            base.Execute();

            for (var i = 0; i < Game1.player.MaxItems; i++)
            {
                if (Game1.player.Items[i] != null)
                {
                    continue;
                }

                var itemId = GetItemId();
                var item = ItemRegistry.Create(itemId);
                Game1.player.Items[i] = item;
            }
        }

        public override string GetItemId()
        {
            var trashItems = new[] { "92", "388", "390", "167", "168", "169", "170", "171", "172", "747" };
            var chosenItemId = trashItems[Game1.random.Next(trashItems.Length)];
            return $"(O){chosenItemId}";
        }
    }
}
