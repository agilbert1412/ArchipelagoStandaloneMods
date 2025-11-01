﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.GameData.FarmAnimals;
using StardewValley.Objects;
using StardewViewerEvents.Extensions;
using System;
using static StardewValley.Minigames.CraneGame;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.CharacterEvents
{
    public class CowManager
    {
        public const string INVISIBLE_COW_KEY = "InvisibleCow";

        private static IMonitor _logger;
        private static IModHelper _helper;

        public static void Initialize(IMonitor logger, IModHelper helper)
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

                transparencyThisFrame = Math.Max(0, transparencyThisFrame - 20);
                var tone = Math.Min(64, transparencyThisFrame * 2);
                
                var color = __instance.hitGlowTimer > 0 ? Color.Red : new Color(tone, tone, tone, transparencyThisFrame);
                __instance.Sprite.draw(b, Utility.snapDrawPosition(Game1.GlobalToLocal(Game1.viewport, __instance.Position - new Vector2(0.0f, 24f) + vector2)), layerDepth, 0, 0, color, __instance.FacingDirection == 3, 4f);

                if (!__instance.isEmoting)
                    return false;
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

                const double chanceToDespawnPerDay = 0.1;
                if (Game1.random.NextDouble() < chanceToDespawnPerDay)
                {
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
    }
}
