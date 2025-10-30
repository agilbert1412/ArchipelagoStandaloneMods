using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;
using StardewViewerEvents.Extensions;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.ItemEvents
{
    public class GetSpecificItemEvent : GetItemEvent
    {
        public GetSpecificItemEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override bool ValidateParameters()
        {
            if (QueuedEvent.parameters == null || QueuedEvent.parameters.Length < 1)
            {
                return false;
            }

            var desiredItem = GetSingleParameter();
            return ItemUtility.ItemExists(desiredItem);
        }

        public override string GetItemId()
        {
            var desiredItem = GetSingleParameter();
            if (ItemUtility.TryFindItem(desiredItem, out var foundItem))
            {
                return foundItem.QualifiedItemId;
            }

            return $"(O)390";
        }
    }
}
