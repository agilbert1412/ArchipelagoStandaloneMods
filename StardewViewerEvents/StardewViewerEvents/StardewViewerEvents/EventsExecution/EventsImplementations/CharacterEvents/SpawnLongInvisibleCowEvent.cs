using StardewModdingAPI;
using StardewViewerEvents.Events;
using ViewerEventsLibrary.Logging;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.CharacterEvents
{
    public class SpawnLongInvisibleCowEvent : ExecutableEvent
    {
        public const int NUMBER_LONG_COWS_PER_PURCHASE = 4;
        private static readonly CowSpawner _cowSpawner = new CowSpawner();

        public SpawnLongInvisibleCowEvent(IViewerEventsLogger logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override void Execute()
        {
            base.Execute();

            var activeAccounts = ViewerEventsService.Instance.CreditAccounts.GetAccountsActiveInThePastMinutes(30);
            var activeNames = activeAccounts.Select(x => x.discordName).ToArray();

            var amount = NUMBER_LONG_COWS_PER_PURCHASE * QueuedEvent.queueCount;
            _cowSpawner.SpawnManyInvisibleCows(amount, activeNames, true);
        }
    }
}
