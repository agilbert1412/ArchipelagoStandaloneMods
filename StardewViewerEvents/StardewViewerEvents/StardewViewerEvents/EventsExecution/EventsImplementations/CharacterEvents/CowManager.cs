using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using ViewerEventsLibrary.Logging;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.CharacterEvents
{
    public class CowManager
    {
        public const string INVISIBLE_COW_KEY = "ViewerEvent_InvisibleCow";
        public const string LONG_INVISIBLE_COW_KEY = "ViewerEvent_LongInvisibleCow";
        public const double INVISIBLE_COW_DESPAWN_CHANCE_PER_DAY = 0.1;
        public const double LONG_INVISIBLE_COW_DESPAWN_CHANCE_PER_DAY = 0.0005;
        public const double INVISIBLE_COW_BONUS_DESPAWN_CHANCE = 0.1;
        public const double LONG_INVISIBLE_COW_BONUS_DESPAWN_CHANCE = 0.15;

        private static IViewerEventsLogger _logger;
        private static IModHelper _helper;

        public static void Initialize(IViewerEventsLogger logger, IModHelper helper)
        {
            _logger = logger;
            _helper = helper;
        }

        public static bool Draw_InvisibleCow_Prefix(FarmAnimal __instance, SpriteBatch b)
        {
            try
            {
                if (!__instance.modData.ContainsKey(INVISIBLE_COW_KEY))
                {
                    return true;
                }

                var isLongCow = __instance.modData.ContainsKey(LONG_INVISIBLE_COW_KEY);

                var vector2 = new Vector2(0.0f, __instance.yJumpOffset);
                var boundingBox = __instance.GetBoundingBox();
                var animalData = __instance.GetAnimalData();
                var isSwimming = __instance.IsActuallySwimming();
                var isBaby = __instance.isBaby();
                var shadow = animalData?.GetShadow(isBaby, isSwimming);
                if ((shadow != null ? (shadow.Visible ? 1 : 0) : 1) != 0)
                {
                    int? nullable1;
                    if (shadow == null)
                    {
                        nullable1 = null;
                    }
                    else
                    {
                        ref var local = ref shadow.Offset;
                        nullable1 = local.HasValue ? new int?(local.GetValueOrDefault().X) : null;
                    }
                    var valueOrDefault1 = nullable1.GetValueOrDefault();
                    int? nullable2;
                    if (shadow == null)
                    {
                        nullable2 = null;
                    }
                    else
                    {
                        ref var local = ref shadow.Offset;
                        nullable2 = local.HasValue ? new int?(local.GetValueOrDefault().Y) : null;
                    }
                    var valueOrDefault2 = nullable2.GetValueOrDefault();
                    if (isSwimming)
                    {
                        var scale = (float)(shadow?.Scale ?? (isBaby ? 2.5 : 3.5));
                        var globalPosition = new Vector2(__instance.Position.X + valueOrDefault1, __instance.Position.Y - 24f + valueOrDefault2);
                        __instance.Sprite.drawShadow(b, Game1.GlobalToLocal(Game1.viewport, globalPosition), scale, 0.5f);
                        var num = (int)((Math.Sin(Game1.currentGameTime.TotalGameTime.TotalSeconds * 4.0 + __instance.bobOffset) + 0.5) * 3.0);
                        vector2.Y += num;
                    }
                    else
                    {
                        var scale = (float)(shadow?.Scale ?? (isBaby ? 3.0 : 4.0));
                        var globalPosition = new Vector2(__instance.Position.X + valueOrDefault1, __instance.Position.Y - 24f + valueOrDefault2);
                        __instance.Sprite.drawShadow(b, Game1.GlobalToLocal(Game1.viewport, globalPosition), scale);
                    }
                }
                vector2.Y += __instance.yJumpOffset;
                var layerDepth = (float)((boundingBox.Center.Y + 4 + __instance.Position.X / 20000.0) / 10000.0);

                var minTransparency = 0;
                var maxTransparency = 128;
                var tickDelay = 4;
                var transparencyRange = maxTransparency - minTransparency;
                var transparencyThisFrame = 0;
                if (transparencyRange >= 1)
                {
                    transparencyThisFrame = (int)(((Game1.ticks / tickDelay) + (__instance.myID.Value % transparencyRange)) % (transparencyRange * 2));
                    if (transparencyThisFrame > transparencyRange)
                    {
                        transparencyThisFrame = transparencyRange - (transparencyThisFrame - transparencyRange);
                    }

                    transparencyThisFrame += minTransparency;
                }

                if (transparencyThisFrame == 0)
                {
                    SetRandomPosition(__instance, __instance.currentLocation);
                }

                transparencyThisFrame = Math.Max(0, transparencyThisFrame - 20);
                var tone = Math.Min(64, transparencyThisFrame * 2);

                var color = __instance.hitGlowTimer > 0 ? Color.Red : new Color(tone, tone, tone, transparencyThisFrame);
                var cowScale = 4f;
                var cowScaleX = cowScale;
                var cowScaleY = cowScale;
                var drawPosition = Utility.snapDrawPosition(Game1.GlobalToLocal(Game1.viewport,
                    __instance.Position - new Vector2(0.0f, 24f) + vector2));
                if (isLongCow)
                {
                    const float longCowScaleFactor = 3f;
                    if (__instance.FacingDirection == 1 || __instance.FacingDirection == 3)
                    {
                        cowScaleX *= longCowScaleFactor;
                        drawPosition.X -= (__instance.Sprite.SpriteWidth * longCowScaleFactor / 2);
                    }
                    else
                    {
                        cowScaleY *= longCowScaleFactor;
                        drawPosition.Y -= (__instance.Sprite.SpriteHeight * longCowScaleFactor / 2);
                    }
                }

                var cowScaleVec = new Vector2(cowScaleX, cowScaleY);
                var sprite = __instance.Sprite;
                var flip = __instance.FacingDirection == 3;
                if (sprite.Texture != null)
                {
                    var sourceRectangle = new Rectangle(sprite.sourceRect.X + 0, sprite.sourceRect.Y + 0,
                        sprite.sourceRect.Width, sprite.sourceRect.Height);
                    //b.Draw(sprite.Texture, drawPosition, sourceRectangle, color, 0.0f, Vector2.Zero, cowScale, flip || sprite.CurrentAnimation != null && sprite.CurrentAnimation[sprite.currentAnimationIndex].flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth);
                    b.Draw(sprite.Texture, drawPosition, sourceRectangle, color, 0.0f, Vector2.Zero, cowScaleVec, flip || sprite.CurrentAnimation != null && sprite.CurrentAnimation[sprite.currentAnimationIndex].flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth);
                }
                else
                {
                    sprite.draw(b, drawPosition, layerDepth, 0, 0, color, flip, cowScale);
                }

                if (!__instance.isEmoting)
                {
                    return false;
                }
                var num1 = __instance.Sprite.SpriteWidth / 2 * 4 - 32 + (animalData != null ? animalData.EmoteOffset.X : 0);
                var num2 = (animalData != null ? animalData.EmoteOffset.Y : 0) - 64;
                var local1 = Game1.GlobalToLocal(Game1.viewport, new Vector2(__instance.Position.X + vector2.X + num1, __instance.Position.Y + vector2.Y + num2));
                b.Draw(Game1.emoteSpriteSheet, local1, new Rectangle(__instance.CurrentEmoteIndex * 16 % Game1.emoteSpriteSheet.Width, __instance.CurrentEmoteIndex * 16 / Game1.emoteSpriteSheet.Width * 16, 16, 16), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, boundingBox.Bottom / 10000f);

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed in {nameof(Draw_InvisibleCow_Prefix)}:\n{ex}");
                return true;
            }

        }

        // public void dayUpdate(GameLocation environment)
        public static bool DayUpdate_InvisibleCow_Prefix(FarmAnimal __instance, GameLocation environment)
        {
            try
            {
                if (!__instance.modData.ContainsKey(INVISIBLE_COW_KEY))
                {
                    return true;
                }

                var isLongCow = __instance.modData.ContainsKey(LONG_INVISIBLE_COW_KEY);
                var chanceToDespawn = isLongCow
                    ? LONG_INVISIBLE_COW_DESPAWN_CHANCE_PER_DAY
                    : INVISIBLE_COW_DESPAWN_CHANCE_PER_DAY;

                if (__instance.wasPet.Value)
                {
                    chanceToDespawn += isLongCow ? LONG_INVISIBLE_COW_BONUS_DESPAWN_CHANCE : INVISIBLE_COW_BONUS_DESPAWN_CHANCE;
                }
                else if (__instance.wasAutoPet.Value)
                {
                    chanceToDespawn += (isLongCow ? LONG_INVISIBLE_COW_BONUS_DESPAWN_CHANCE : INVISIBLE_COW_BONUS_DESPAWN_CHANCE)/2;
                }

                var despawnRoll = Game1.random.NextDouble();
                if (despawnRoll < chanceToDespawn)
                {
                    var adjective = isLongCow ? "long " : "";
                    var cowsOnMap = __instance.currentLocation.Animals.Values.Where(x => x.modData.ContainsKey(INVISIBLE_COW_KEY) && x.modData.ContainsKey(LONG_INVISIBLE_COW_KEY) == isLongCow);
                    var numCowsOnMap = cowsOnMap.Count() - 1;
                    _logger.LogInfo($"Despawning one {adjective}invisible cow on {__instance.currentLocation.Name} ({numCowsOnMap} remaining)");
                    if (__instance.homeInterior is AnimalHouse animalHouse)
                    {
                        animalHouse.animalsThatLiveHere.Remove(__instance.myID.Value);
                    }

                    __instance.currentLocation.Animals.Remove(__instance.myID.Value);
                    return false;
                }

                __instance.StopAllActions();
                __instance.health.Value = 3;
                var flag1 = false;
                __instance.wasPet.Value = false;
                __instance.wasAutoPet.Value = false;
                ++__instance.daysOwned.Value;
                __instance.reload(__instance.homeInterior);

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed in {nameof(DayUpdate_InvisibleCow_Prefix)}:\n{ex}");
                return true;
            }

        }

        // farmerPushing()
        public static bool FarmerPushing_TakeLongerToReact_Prefix(FarmAnimal __instance)
        {
            try
            {
                if (!__instance.modData.ContainsKey(INVISIBLE_COW_KEY))
                {
                    return true;
                }

                ++__instance.pushAccumulator;
                if (__instance.pushAccumulator <= 180)
                {
                    return false;
                }

                // __instance.doFarmerPushEvent.Fire(Game1.player.FacingDirection);
                var cowBoundingBox = __instance.GetBoundingBox();
                var farmerOppositeDirection = Utility.GetOppositeFacingDirection(Game1.player.FacingDirection);
                var temporaryTile = Utility.ExpandRectangle(cowBoundingBox, farmerOppositeDirection, 6);
                temporaryTile.Inflate(-(temporaryTile.Width/8), -(temporaryTile.Height / 8));
                Game1.player.TemporaryPassableTiles.Add(temporaryTile);
                __instance.pushAccumulator = 0;

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed in {nameof(FarmerPushing_TakeLongerToReact_Prefix)}:\n{ex}");
                return true;
            }

        }

        // public void doFarmerPush(int direction)

        // public void setRandomPosition(GameLocation location)
        public static bool SetRandomPosition_DontLookForProduceArea_Prefix(FarmAnimal __instance, GameLocation location)
        {
            try
            {
                if (!__instance.modData.ContainsKey(INVISIBLE_COW_KEY))
                {
                    return true;
                }

                SetRandomPosition(__instance, location);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed in {nameof(SetRandomPosition_DontLookForProduceArea_Prefix)}:\n{ex}");
                return true;
            }

        }

        private static void SetRandomPosition(FarmAnimal cow, GameLocation location)
        {
            cow.StopAllActions();
            // Rectangle parsed;
            //if (!location.TryGetMapPropertyAs("ProduceArea", out parsed, true))
            //    return;
            var tile = location.getRandomTile();
            cow.Position = new Vector2(tile.X * 64f, tile.Y * 64f);
            var num = 0;
            while (cow.Position.Equals(Vector2.Zero) || location.Objects.ContainsKey(cow.Position) || location.isCollidingPosition(cow.GetBoundingBox(), Game1.viewport, false, 0, false, cow))
            {
                tile = location.getRandomTile();
                cow.Position = new Vector2(tile.X * 64f, tile.Y * 64f); ++num;
                if (num > 64)
                {
                    break;
                }
            }

            cow.SleepIfNecessary();
        }

        // public void pet(Farmer who, bool is_auto_pet = false)
        public static bool Pet_DontShowMenu_Prefix(FarmAnimal __instance, Farmer who, bool is_auto_pet)
        {
            try
            {
                if (!__instance.modData.ContainsKey(INVISIBLE_COW_KEY))
                {
                    return true;
                }

                var autoPetFriendship = 7;
                if (__instance.wasAutoPet.Value)
                {
                    __instance.friendshipTowardFarmer.Value = Math.Min(1000, __instance.friendshipTowardFarmer.Value + autoPetFriendship);
                }
                else if (is_auto_pet)
                {
                    __instance.friendshipTowardFarmer.Value = Math.Min(1000, __instance.friendshipTowardFarmer.Value + (15 - autoPetFriendship));
                }
                else
                {
                    __instance.friendshipTowardFarmer.Value = Math.Min(1000, __instance.friendshipTowardFarmer.Value + 15);
                }
                if (is_auto_pet)
                {
                    __instance.wasAutoPet.Value = true;
                }
                else
                {
                    __instance.wasPet.Value = true;
                }

                var animalData = __instance.GetAnimalData();
                var happinessDrain = animalData != null ? animalData.HappinessDrain : 0;
                if (!is_auto_pet)
                {
                    if (animalData != null && animalData.ProfessionForHappinessBoost >= 0 && who.professions.Contains(animalData.ProfessionForHappinessBoost))
                    {
                        __instance.friendshipTowardFarmer.Value = Math.Min(1000, __instance.friendshipTowardFarmer.Value + 15);
                        __instance.happiness.Value = (byte)Math.Min(byte.MaxValue, __instance.happiness.Value + Math.Max(5, 30 + happinessDrain));
                    }
                    var num2 = 20;
                    if (__instance.wasAutoPet.Value)
                    {
                        num2 = 32;
                    }
                    __instance.doEmote(__instance.moodMessage.Value == 4 ? 12 : num2);
                }
                __instance.happiness.Value = (byte)Math.Min(byte.MaxValue, __instance.happiness.Value + Math.Max(5, 30 + happinessDrain));
                if (!is_auto_pet)
                {
                    __instance.makeSound();
                    // who.gainExperience(0, 5);
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed in {nameof(Pet_DontShowMenu_Prefix)}:\n{ex}");
                return true;
            }

        }
    }
}
