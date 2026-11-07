using StardewModdingAPI;
using StardewViewerEvents.Events;
using ViewerEventsLibrary.Logging;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.CharacterEvents
{
    public class SpawnInvisibleCowEvent : ExecutableEvent
    {
        public const int NUMBER_COWS_PER_PURCHASE = 10;
        private static readonly CowSpawner _cowSpawner = new CowSpawner();

        public SpawnInvisibleCowEvent(IViewerEventsLogger logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override void Execute()
        {
            base.Execute();

            var activeAccounts = ViewerEventsService.Instance.CreditAccounts.GetAccountsActiveInThePastMinutes(30);
            var activeNames = activeAccounts.Select(x => x.discordName).ToArray();
            if (!activeNames.Any())
            {
                activeNames = ViewerEventsService.Instance.CreditAccounts.GetAllAccounts().Select(x => x.discordName).ToArray();
            }
            if (!activeNames.Any())
            {
                activeNames = new[] { "Cow1", "Cow2", "Cow3" };
            }

            var amount = NUMBER_COWS_PER_PURCHASE * QueuedEvent.queueCount;
            _cowSpawner.SpawnManyInvisibleCows(amount, activeNames, false);
        }
    }
}
