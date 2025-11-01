using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;
using StardewViewerEvents.EventsExecution.EventsImplementations.WeatherEvents;
using StardewViewerEvents.Extensions;
using static StardewValley.Farmer;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.PlayerEvents.EmoteEvents
{
    public class SpecificEmoteEvent : EmoteEvent
    {

        public SpecificEmoteEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override bool ValidateParameters()
        {
            if (!base.ValidateParameters())
            {
                return false;
            }

            return TryGetDesiredEmote(out _);
        }

        private bool TryGetDesiredEmote(out EmoteType emote)
        {
            var desiredEmote = GetSingleParameter();
            var lowerEmote = desiredEmote.SanitizeEntityName();
            foreach (var emoteType in EMOTES)
            {
                if (emoteType.emoteString.Equals(lowerEmote, StringComparison.InvariantCultureIgnoreCase))
                {
                    emote = emoteType;
                    return true;
                }

                if (emoteType.emoteIconIndex.ToString().Equals(lowerEmote, StringComparison.InvariantCultureIgnoreCase))
                {
                    emote = emoteType;
                    return true;
                }
            }

            emote = new EmoteType();
            return false;
        }

        protected override string GetEmoteName()
        {
            TryGetDesiredEmote(out var emote);
            return emote.emoteString;
        }
    }
}