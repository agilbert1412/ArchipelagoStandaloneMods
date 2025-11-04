using StardewModdingAPI;
using StardewViewerEvents.Events;
using StardewViewerEvents.Extensions;
using static StardewValley.Farmer;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.PlayerEvents.EmoteEvents
{
    public class SpecificEmoteEvent : EmoteEvent
    {

        public SpecificEmoteEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override bool ValidateParameters(out string errorMessage)
        {
            if (!base.ValidateParameters(out errorMessage))
            {
                return false;
            }

            var desiredEmote = GetSingleParameter();
            errorMessage =
                $"Unrecognized emote [{desiredEmote}]. You must specify either the name or the ID of an emote in Stardew Valley.";
            return TryGetDesiredEmote(desiredEmote, out _);
        }

        private bool TryGetDesiredEmote(string desiredEmote, out EmoteType emote)
        {
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
            var desiredEmote = GetSingleParameter();
            TryGetDesiredEmote(desiredEmote, out var emote);
            return emote.emoteString;
        }
    }
}