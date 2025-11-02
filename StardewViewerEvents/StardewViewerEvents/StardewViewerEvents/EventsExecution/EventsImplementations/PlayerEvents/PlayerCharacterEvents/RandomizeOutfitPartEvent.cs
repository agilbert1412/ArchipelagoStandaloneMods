using StardewModdingAPI;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.PlayerEvents.PlayerCharacterEvents
{
    public class RandomizeOutfitPartEvent : OutfitEvent
    {
        public const string HAIR = "hair";
        public const string SHIRT = "shirt";
        public const string PANTS = "pants";
        public const string HAT = "hat";

        public static readonly string[] _validParts = new[] { HAIR, SHIRT, PANTS, HAT };


        public RandomizeOutfitPartEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override bool ValidateParameters(out string errorMessage)
        {
            var outfitPart = GetSingleParameter();
            if (_validParts.Any(x => x.Equals(outfitPart, StringComparison.InvariantCultureIgnoreCase)))
            {
                errorMessage = "";
                return true;
            }

            errorMessage = $"Parameter [{outfitPart}] is invalid. Allowed Outfit parts: [{string.Join(",", _validParts)}]";
            return false;
        }

        public override void Execute()
        {
            base.Execute();

            var outfitPart = GetSingleParameter();
            var desiredPart = _validParts.First(x => x.Equals(outfitPart, StringComparison.InvariantCultureIgnoreCase));
            switch (desiredPart)
            {
                case HAIR:
                    RandomizeHair();
                    break;
                case HAT:
                    RandomizeHat();
                    break;
                case SHIRT:
                    RandomizeShirt();
                    break;
                case PANTS:
                    RandomizePants();
                    break;
                default:
                    break;
            }
        }
    }
}