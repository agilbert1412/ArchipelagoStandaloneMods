using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;
using static StardewValley.Farmer;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.PlayerEvents.EmoteEvents
{
    public class RandomEmoteEvent : EmoteEvent
    {

        public RandomEmoteEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        protected override string GetEmoteName()
        {
            var chosenEmote = EMOTES[Game1.random.Next(EMOTES.Length)];
            return chosenEmote.emoteString;
        }
    }
}