using StardewModdingAPI;
using StardewValley;
using StardewValley.Extensions;
using StardewValley.GameData.MakeoverOutfits;
using StardewValley.Objects;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.PlayerEvents.PlayerCharacterEvents
{
    public abstract class OutfitEvent : ExecutableEvent
    {
        protected OutfitEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public bool TryGetMakeoverOutfit(out MakeoverOutfit chosenMakeoverOutfit)
        {
            chosenMakeoverOutfit = null;
            var random = Game1.random;
            var makeoverOutfits = DataLoader.MakeoverOutfits(Game1.content);
            if (makeoverOutfits == null)
            {
                return false;
            }

            for (var index = 0; index < makeoverOutfits.Count; ++index)
            {
                var makeoverOutfit = makeoverOutfits[index];
                if (makeoverOutfit.Gender.HasValue && makeoverOutfit.Gender.Value != Game1.player.Gender)
                {
                    makeoverOutfits.RemoveAt(index);
                    --index;
                    continue;
                }

                foreach (var outfitPart in makeoverOutfit.OutfitParts)
                {
                    if (!outfitPart.MatchesGender(Game1.player.Gender))
                    {
                        continue;
                    }

                    var dataOrErrorItem = ItemRegistry.GetDataOrErrorItem(outfitPart.ItemId);
                    var alreadyWearingThisPart =
                        Game1.player.hat.Value?.QualifiedItemId == dataOrErrorItem.QualifiedItemId ||
                        Game1.player.shirtItem.Value?.QualifiedItemId == dataOrErrorItem.QualifiedItemId ||
                        Game1.player.pantsItem.Value?.QualifiedItemId == dataOrErrorItem.QualifiedItemId;
                    if (alreadyWearingThisPart)
                    {
                        makeoverOutfits.RemoveAt(index);
                        --index;
                        break;
                    }
                }
            }

            chosenMakeoverOutfit = random.ChooseFrom(makeoverOutfits);
            var daySaveRandom = Utility.CreateDaySaveRandom();
            if (daySaveRandom.NextDouble() < 0.03)
            {
                chosenMakeoverOutfit = new MakeoverOutfit()
                {
                    OutfitParts = new List<MakeoverItem>()
                    {
                        new() { ItemId = "(H)LaurelWreathCrown" },
                        new()
                        {
                            ItemId = "(P)3",
                            Color = "247 245 205"
                        },
                        new() { ItemId = "(S)1199" }
                    }
                };
            }

            if (chosenMakeoverOutfit?.OutfitParts == null)
            {
                return false;
            }

            return true;
        }

        public void EquipMakeoverOutfit(MakeoverOutfit makeoverOutfit)
        {
            var player = Game1.player;
            player.Equip(null, player.shirtItem);
            player.Equip(null, player.pantsItem);
            player.Equip(null, player.hat);

            var hasEquippedHat = false;
            var hasEquippedShirt = false;
            var hasEquippedPants = false;
            foreach (var outfitPart in makeoverOutfit.OutfitParts)
            {
                if (!outfitPart.MatchesGender(Game1.player.Gender))
                {
                    continue;
                }
                var obj = ItemRegistry.Create(outfitPart.ItemId);
                if (obj is Hat newItem2)
                {
                    if (!hasEquippedHat)
                    {
                        Game1.player.Equip(newItem2, Game1.player.hat);
                        hasEquippedHat = true;
                    }

                    continue;
                }

                if (obj is Clothing newItem1)
                {
                    var color = Utility.StringToColor(outfitPart.Color);
                    if (color.HasValue)
                    {
                        newItem1.clothesColor.Value = color.Value;
                    }

                    switch (newItem1.clothesType.Value)
                    {
                        case Clothing.ClothesType.SHIRT:
                            if (!hasEquippedShirt)
                            {
                                Game1.player.Equip(newItem1, Game1.player.shirtItem);
                                hasEquippedShirt = true;
                                continue;
                            }

                            continue;
                        case Clothing.ClothesType.PANTS:
                            if (!hasEquippedPants)
                            {
                                Game1.player.Equip(newItem1, Game1.player.pantsItem);
                                hasEquippedPants = true;
                                continue;
                            }

                            continue;
                        default:
                            continue;
                    }
                }
            }
        }

        public void RandomizeFullOutfit()
        {
            RandomizeHair();
            RandomizePants();
            RandomizeShirt();
        }

        public void RandomizePants()
        {
            var pants = DataLoader.Pants(Game1.content);
            var pantKeys = pants.Keys.ToArray();
            var chosenPantKey = pantKeys[Game1.random.Next(pantKeys.Length)];
            var chosenPant = ItemRegistry.Create<Clothing>($"(P){chosenPantKey}");
            Game1.player.Equip(chosenPant, Game1.player.pantsItem);
        }

        public void RandomizeShirt()
        {
            var shirts = DataLoader.Shirts(Game1.content);
            var shirtKeys = shirts.Keys.ToArray();
            var chosenShirtKey = shirtKeys[Game1.random.Next(shirtKeys.Length)];
            var chosenShirt = ItemRegistry.Create<Clothing>($"(S){chosenShirtKey}");
            Game1.player.Equip(chosenShirt, Game1.player.shirtItem);
        }

        public void RandomizeHair()
        {
            var hairs = DataLoader.HairData(Game1.content);
            var hairKeys = hairs.Keys.ToArray();
            var chosenHairKey = hairKeys[Game1.random.Next(hairKeys.Length)];
            Game1.player.changeHairStyle(chosenHairKey);
        }

        public void RandomizeHat()
        {
            var hats = DataLoader.Hats(Game1.content);
            var hatKeys = hats.Keys.ToArray();
            var chosenHatKey = hatKeys[Game1.random.Next(hatKeys.Length)];
            var chosenHat = ItemRegistry.Create<Hat>($"(H){chosenHatKey}");
            Game1.player.Equip(chosenHat, Game1.player.hat);
        }
    }
}
