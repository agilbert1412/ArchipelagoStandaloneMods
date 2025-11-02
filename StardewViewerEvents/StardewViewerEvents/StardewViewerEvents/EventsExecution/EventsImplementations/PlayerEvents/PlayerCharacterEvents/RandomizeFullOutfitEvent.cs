using StardewModdingAPI;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.PlayerEvents.PlayerCharacterEvents
{
    public class RandomizeFullOutfitEvent : OutfitEvent
    {

        public RandomizeFullOutfitEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override void Execute()
        {
            base.Execute();

            RandomizeFullOutfit();
        }
    }
}