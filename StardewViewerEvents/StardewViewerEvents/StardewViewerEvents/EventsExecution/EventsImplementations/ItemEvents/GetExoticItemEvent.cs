using StardewModdingAPI;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.ItemEvents
{
    public class GetExoticItemEvent : GetItemEvent
    {
        public GetExoticItemEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override string GetItemId()
        {
            return $"(F)2514";
        }
    }
}
