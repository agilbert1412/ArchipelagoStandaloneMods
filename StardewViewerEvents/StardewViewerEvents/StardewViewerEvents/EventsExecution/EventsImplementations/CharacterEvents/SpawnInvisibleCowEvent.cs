using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.CharacterEvents
{
    public class SpawnInvisibleCowEvent : ExecutableEvent
    {
        public const int NUMBER_COWS_PER_PURCHASE = 10;
        private static readonly CowSpawner _cowSpawner = new CowSpawner();

        public SpawnInvisibleCowEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override void Execute()
        {
            base.Execute();

            var activeAccounts = ViewerEventsService.Instance.CreditAccounts.GetAccountsActiveInThePastMinutes(30);
            var activeNames = activeAccounts.Select(x => x.discordName).ToArray();

            var amount = NUMBER_COWS_PER_PURCHASE * QueuedEvent.queueCount;
            _cowSpawner.SpawnManyInvisibleCows(amount, activeNames);
        }
    }
}
