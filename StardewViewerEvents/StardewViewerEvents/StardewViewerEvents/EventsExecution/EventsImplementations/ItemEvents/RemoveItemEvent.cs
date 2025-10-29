using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.ItemEvents
{
    public class RemoveItemEvent : ExecutableEvent
    {
        public RemoveItemEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override void Execute()
        {
            base.Execute();

            var candidates =
                Game1.player.Items.Where(x =>x != null && (x.CanBeLostOnDeath() || x.canBeShipped() || x.canBeTrashed())).ToArray();
            var random = new Random();
            var chosenItem = candidates[random.Next(candidates.Length)];
            Game1.player.removeItemFromInventory(chosenItem);
        }
    }
}
