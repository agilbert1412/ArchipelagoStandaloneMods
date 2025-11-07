using StardewModdingAPI;
using StardewValley;
using StardewValley.TokenizableStrings;
using StardewViewerEvents.Events;
using StardewViewerEvents.Extensions;
using System;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.BuffEvents
{
    public class SpecificBuffEvent : ExecutableEvent
    {

        public SpecificBuffEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override bool ValidateParameters(out string errorMessage)
        {
            var desiredBuffName = GetSingleParameter();
            if (string.IsNullOrWhiteSpace(desiredBuffName))
            {
                errorMessage = $"You must provide a buff name or ID";
                return false;
            }

            if (TryGetDesiredBuff(desiredBuffName, out _))
            {
                errorMessage = "";
                return true;
            }

            errorMessage = $"Unknown Buff `{desiredBuffName}`. You must specify either a buff name or buff ID from Stardew Valley";
            return false;
        }

        public override void Execute()
        {
            base.Execute();

            var desiredBuffName = GetSingleParameter();
            if (TryGetDesiredBuff(desiredBuffName, out var buffKey))
            {
                Game1.player.applyBuff(buffKey);
            }
        }

        private bool TryGetDesiredBuff(string desiredBuffName, out string desiredBuff)
        {
            var sanitizedBuff = desiredBuffName.SanitizeEntityName();
            var buffs = DataLoader.Buffs(Game1.content);
            foreach (var (buffKey, _) in buffs)
            {
                if (buffKey.SanitizeEntityName().Equals(sanitizedBuff, StringComparison.InvariantCultureIgnoreCase))
                {
                    desiredBuff = buffKey;
                    return true;
                }
            }

            foreach (var (buffKey, buffData) in buffs)
            {
                var parsedDisplay = TokenParser.ParseText(buffData.DisplayName);
                var parsedDescription = TokenParser.ParseText(buffData.Description);
                if (buffData.DisplayName.SanitizeEntityName().Equals(sanitizedBuff, StringComparison.InvariantCultureIgnoreCase) ||
                    buffData.Description.SanitizeEntityName().Equals(sanitizedBuff, StringComparison.InvariantCultureIgnoreCase) ||
                    parsedDisplay.SanitizeEntityName().Equals(sanitizedBuff, StringComparison.InvariantCultureIgnoreCase) ||
                    parsedDescription.SanitizeEntityName().Equals(sanitizedBuff, StringComparison.InvariantCultureIgnoreCase))
                {
                    desiredBuff = buffKey;
                    return true;
                }
            }

            desiredBuff = "";
            return false;
        }
    }
}
