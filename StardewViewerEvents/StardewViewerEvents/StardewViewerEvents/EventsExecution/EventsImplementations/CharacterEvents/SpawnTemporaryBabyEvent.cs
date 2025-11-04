using StardewModdingAPI;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.CharacterEvents
{
    public class SpawnTemporaryBabyEvent : BabyEvent
    {
        public const int NUMBER_TEMPORARY_BABIES = 25;

        public SpawnTemporaryBabyEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override void Execute()
        {
            base.Execute();

            var activeAccounts = ViewerEventsService.Instance.CreditAccounts.GetAccountsActiveInThePastMinutes(30);
            var activeNames = activeAccounts.Select(x => x.discordName).ToArray();
            var amount = NUMBER_TEMPORARY_BABIES * QueuedEvent.queueCount;
            _babyBirther.SpawnTemporaryBabies(activeNames, amount);
        }
    }
}
