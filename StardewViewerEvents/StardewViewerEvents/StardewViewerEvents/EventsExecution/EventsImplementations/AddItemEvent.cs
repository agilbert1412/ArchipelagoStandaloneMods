using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations
{
    public class AddItemEvent : ExecutableEvent
    {
        public AddItemEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override void Execute()
        {
            base.Execute();

            var stoneId = $"(O)390";
            var item = ItemRegistry.Create(stoneId);
            Game1.player.addItemByMenuIfNecessary(item);
        }
    }
}
