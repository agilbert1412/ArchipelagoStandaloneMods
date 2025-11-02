using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.PlayerEvents.PlayerCharacterEvents
{
    public class ChangeFavoriteThingEvent : ExecutableEvent
    {

        public ChangeFavoriteThingEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override bool ValidateParameters(out string errorMessage)
        {
            if (!base.ValidateParameters(out errorMessage))
            {
                errorMessage = $"You must specify a new favorite thing for the player";
                return false;
            }

            return true;
        }

        public override void Execute()
        {
            base.Execute();
            var newFavoriteThing = GetSingleParameter();
            Game1.player.favoriteThing.Value = newFavoriteThing;
        }
    }
}