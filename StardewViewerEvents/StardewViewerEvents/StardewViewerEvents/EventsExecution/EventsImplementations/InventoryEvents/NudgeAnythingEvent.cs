using StardewModdingAPI;
using StardewViewerEvents.Events;
using ViewerEventsLibrary.Logging;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.InventoryEvents
{
    public class NudgeAnythingEvent : InventoryEvent
    {
        public const int NUMBER_NUDGES = 4;
        protected ObjectNudger _objectNudger; 

        public NudgeAnythingEvent(IViewerEventsLogger logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
            _objectNudger = new ObjectNudger(logger, modHelper);
        }

        public override bool ValidateParameters(out string errorMessage)
        {
            if (!base.ValidateParameters(out errorMessage))
            {
                return false;
            }

            var allChests = GetAllChests();
            errorMessage = $"You can only use the Nudge event when there are chests somewhere to nudge";
            return allChests.Any();
        }

        public override void Execute()
        {
            base.Execute();

            var numberNudges = NUMBER_NUDGES * QueuedEvent.queueCount;
            _objectNudger.NudgeObjectsEverywhere(numberNudges, false);
        }
    }
}
