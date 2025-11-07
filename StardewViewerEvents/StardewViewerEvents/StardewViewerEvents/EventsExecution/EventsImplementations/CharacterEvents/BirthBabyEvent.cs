using StardewModdingAPI;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.CharacterEvents
{
    public class BirthBabyEvent : BabyEvent
    {
        public BirthBabyEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override void Execute()
        {
            base.Execute();

            var numberBabies = QueuedEvent.queueCount;
            var activeAccounts = ViewerEventsService.Instance.CreditAccounts.GetAccountsActiveInThePastMinutes(30);
            var activeNames = activeAccounts.Select(x => x.discordName).ToArray();

            for (var i = 0; i < numberBabies; i++)
            {
                _babyBirther.SpawnNewBaby(activeNames);
            }
        }
    }
}
