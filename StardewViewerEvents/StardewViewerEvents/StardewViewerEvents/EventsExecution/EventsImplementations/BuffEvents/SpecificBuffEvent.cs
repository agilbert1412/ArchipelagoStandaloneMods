using StardewModdingAPI;
using StardewValley;
using StardewValley.TokenizableStrings;
using StardewViewerEvents.Events;
using StardewViewerEvents.Extensions;
using System;
using StardewValley.GameData.Buffs;

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
                var buff = new Buff(buffKey);
                if (buff.millisecondsDuration < 1000 * 10)
                {
                    buff.millisecondsDuration *= 10;
                }
                Game1.player.applyBuff(buff);
            }
        }

        private bool TryGetDesiredBuff(string desiredBuffName, out string desiredBuff)
        {
            var sanitizedBuffName = desiredBuffName.SanitizeEntityName();
            var buffs = DataLoader.Buffs(Game1.content);
            foreach (var (buffKey, _) in buffs)
            {
                if (buffKey.SanitizeEntityName().Equals(sanitizedBuffName, StringComparison.InvariantCultureIgnoreCase))
                {
                    desiredBuff = buffKey;
                    return true;
                }
            }

            const string buffSuffix = "buff";
            var alternateBuffName = sanitizedBuffName.EndsWith(buffSuffix, StringComparison.InvariantCultureIgnoreCase)
                ? sanitizedBuffName.Substring(0, sanitizedBuffName.Length - buffSuffix.Length)
                : $"{sanitizedBuffName}{buffSuffix}";

            if (TryGetNamedBuff(buffs, sanitizedBuffName, out desiredBuff))
            {
                return true;
            }

            if (TryGetNamedBuff(buffs, alternateBuffName, out desiredBuff))
            {
                return true;
            }

            desiredBuff = "";
            return false;
        }

        private static bool TryGetNamedBuff(Dictionary<string, BuffData> buffs, string sanitizedBuffName, out string desiredBuff)
        {
            foreach (var (buffKey, buffData) in buffs)
            {
                var parsedDisplay = TokenParser.ParseText(buffData.DisplayName);

                var parsedDescription = TokenParser.ParseText(buffData.Description);
                if (buffData.DisplayName.SanitizeEntityName().Equals(sanitizedBuffName, StringComparison.InvariantCultureIgnoreCase) ||
                    buffData.Description.SanitizeEntityName().Equals(sanitizedBuffName, StringComparison.InvariantCultureIgnoreCase) ||
                    parsedDisplay.SanitizeEntityName().Equals(sanitizedBuffName, StringComparison.InvariantCultureIgnoreCase) ||
                    parsedDescription.SanitizeEntityName().Equals(sanitizedBuffName, StringComparison.InvariantCultureIgnoreCase))
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
