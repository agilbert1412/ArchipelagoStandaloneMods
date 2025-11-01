using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.CharacterEvents
{
    internal class InvisibleCow : FarmAnimal
    {
        private static IMonitor _logger;
        private static IModHelper _helper;

        public static void Initialize(IMonitor logger, IModHelper helper)
        {
            _logger = logger;
            _helper = helper;
        }

        public InvisibleCow(string type, long id, long ownerID) : base(type, id, ownerID)
        {
            growFully();
        }

        public override void draw(SpriteBatch b)
        {
            var vector2 = new Vector2(0.0f, yJumpOffset);
            var boundingBox = GetBoundingBox();
            var animalData = GetAnimalData();
            var isSwimming = IsActuallySwimming();
            var isBaby = this.isBaby();
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
                    var globalPosition = new Vector2(Position.X + valueOrDefault1, Position.Y - 24f + valueOrDefault2);
                    Sprite.drawShadow(b, Game1.GlobalToLocal(Game1.viewport, globalPosition), scale, 0.5f);
                    var num = (int)((Math.Sin(Game1.currentGameTime.TotalGameTime.TotalSeconds * 4.0 + bobOffset) + 0.5) * 3.0);
                    vector2.Y += num;
                }
                else
                {
                    var scale = (float)(shadow?.Scale ?? (isBaby ? 3.0 : 4.0));
                    var globalPosition = new Vector2(Position.X + valueOrDefault1, Position.Y - 24f + valueOrDefault2);
                    Sprite.drawShadow(b, Game1.GlobalToLocal(Game1.viewport, globalPosition), scale);
                }
            }
            vector2.Y += yJumpOffset;
            var layerDepth = (float)((boundingBox.Center.Y + 4 + Position.X / 20000.0) / 10000.0);

            var color = hitGlowTimer > 0 ? Color.Red : Color.White;
            color.A = 63;
            Sprite.draw(b, Utility.snapDrawPosition(Game1.GlobalToLocal(Game1.viewport, Position - new Vector2(0.0f, 24f) + vector2)), layerDepth, 0, 0, color, FacingDirection == 3, 4f);
            
            if (!isEmoting)
                return;
            var num1 = Sprite.SpriteWidth / 2 * 4 - 32 + (animalData != null ? animalData.EmoteOffset.X : 0);
            var num2 = (animalData != null ? animalData.EmoteOffset.Y : 0) - 64;
            var local1 = Game1.GlobalToLocal(Game1.viewport, new Vector2(Position.X + vector2.X + num1, Position.Y + vector2.Y + num2));
            b.Draw(Game1.emoteSpriteSheet, local1, new Rectangle(CurrentEmoteIndex * 16 % Game1.emoteSpriteSheet.Width, CurrentEmoteIndex * 16 / Game1.emoteSpriteSheet.Width * 16, 16, 16), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, boundingBox.Bottom / 10000f);
        }
    }
}
