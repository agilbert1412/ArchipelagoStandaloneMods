using StardewModdingAPI;
using StardewViewerEvents.Events;
using ViewerEventsLibrary.Logging;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.ItemEvents
{
    public class GetExoticItemEvent : GetItemEvent
    {
        public GetExoticItemEvent(IViewerEventsLogger logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override string GetItemId()
        {
            return $"(F)2514";
        }
    }
}
