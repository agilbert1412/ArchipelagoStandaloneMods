using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.ItemEvents
{
    public class GetRandomItemEvent : GetItemEvent
    {
        public GetRandomItemEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override string GetItemId()
        {
            var allObjects = DataLoader.Objects(Game1.content);
            var allObjectIds = allObjects.Keys.ToArray();
            var randomIndex = Game1.random.Next(allObjectIds.Length);
            var chosenId = allObjectIds[randomIndex];
            return chosenId;
        }
    }
}
