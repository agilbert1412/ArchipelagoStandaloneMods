using StardewModdingAPI;
using StardewViewerEvents.Events;
using ViewerEventsLibrary.Logging;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.CharacterEvents
{
    public class SpawnTemporaryBabyEvent : BabyEvent
    {
        public const int NUMBER_TEMPORARY_BABIES = 50;

        public SpawnTemporaryBabyEvent(IViewerEventsLogger logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override void Execute()
        {
            base.Execute();

            var activeAccounts = ViewerEventsService.Instance.CreditAccounts.GetAccountsActiveInThePastMinutes(30);
            var activeNames = activeAccounts.Select(x => x.discordName).ToList();

            if (!activeNames.Any())
            {
                activeNames = ViewerEventsService.Instance.CreditAccounts.GetAllAccounts().Select(x => x.discordName).ToList();
            }

            var i = 0;
            while (activeNames.Count < 30)
            {
                activeNames.Add($"Baby{i}");
                i++;
            }

            var amount = NUMBER_TEMPORARY_BABIES * QueuedEvent.queueCount;
            _babyBirther.SpawnTemporaryBabies(activeNames.ToArray(), amount);
        }
    }
}
