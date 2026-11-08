using StardewModdingAPI;
using StardewViewerEvents.Events;
using ViewerEventsLibrary.Logging;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.CharacterEvents
{
    public class DeleteCowsEvent : ExecutableEvent
    {
        public const int NUMBER_REMOVED_COW_PER_PURCHASE = 1;
        private static readonly CowSpawner _cowSpawner = new CowSpawner();

        public DeleteCowsEvent(IViewerEventsLogger logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override bool CanExecuteRightNow()
        {
            if (!base.CanExecuteRightNow())
            {
                return false;
            }

            var amount = NUMBER_REMOVED_COW_PER_PURCHASE * QueuedEvent.queueCount;
            return _cowSpawner.CountGhostCowsOnCurrentMap() >= amount;
        }

        public override void Execute()
        {
            base.Execute();
            
            var amount = NUMBER_REMOVED_COW_PER_PURCHASE * QueuedEvent.queueCount;
            _cowSpawner.RemoveInvisibleCows(amount);
        }
    }
}
