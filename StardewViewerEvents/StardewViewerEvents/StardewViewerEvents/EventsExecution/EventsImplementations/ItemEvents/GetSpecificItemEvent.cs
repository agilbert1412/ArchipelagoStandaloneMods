using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;
using StardewViewerEvents.Extensions;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.ItemEvents
{
    public class GetSpecificItemEvent : GetItemEvent
    {
        private string _itemId;

        public GetSpecificItemEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
            _itemId = null;
        }

        public override bool ValidateParameters(out string errorMessage)
        {
            if (QueuedEvent.parameters == null || QueuedEvent.parameters.Length < 1)
            {
                errorMessage = $"Must specify what item to give the player.";
                return false;
            }

            var desiredItem = GetSingleParameter();
            errorMessage = $"Unrecognized item [{desiredItem}]. You must specify an item name or item ID that exists in Stardew Valley.";
            return ItemUtility.ItemExists(desiredItem);
        }

        public override string GetItemId()
        {
            if (!string.IsNullOrWhiteSpace(_itemId))
            {
                return _itemId;
            }

            var desiredItem = GetSingleParameter();
            if (ItemUtility.TryFindItem(desiredItem, out var foundItem))
            {
                _itemId = foundItem.QualifiedItemId;
            }
            else
            {
                _itemId = $"(O)390";
            }

            return _itemId;
        }
    }
}
