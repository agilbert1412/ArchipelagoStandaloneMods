using System.Text;
using StardewModdingAPI;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.PlayerEvents.PlayerCharacterEvents
{
    public class ThemedOutfitEvent : OutfitEvent
    {

        public ThemedOutfitEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override bool CanExecuteRightNow()
        {
            if (!base.CanExecuteRightNow())
            {
                return false;
            }

            return TryGetMakeoverOutfit(out _);
        }

        public override void Execute()
        {
            base.Execute();

            if (TryGetMakeoverOutfit(out var makeover))
            {
                EquipMakeoverOutfit(makeover);
            }
        }
    }
}