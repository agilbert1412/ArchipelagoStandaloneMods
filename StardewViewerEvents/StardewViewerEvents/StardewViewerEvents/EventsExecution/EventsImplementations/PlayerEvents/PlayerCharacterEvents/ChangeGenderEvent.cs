using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.PlayerEvents.PlayerCharacterEvents
{
    public class ChangeGenderEvent : ExecutableEvent
    {

        public ChangeGenderEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override void Execute()
        {
            base.Execute();
            Game1.player.changeGender(Game1.player.Gender == Gender.Female);
        }
    }
}