using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Characters;
using StardewValley.Objects;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.CharacterEvents
{
    public class DoveBabyEvent : BabyEvent
    {
        public DoveBabyEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override void Execute()
        {
            base.Execute();

            var activeAccounts = ViewerEventsService.Instance.CreditAccounts.GetAccountsActiveInThePastMinutes(30);
            var activeNames = activeAccounts.Select(x => x.discordName).ToArray();
            var homeOfFarmer = Utility.getHomeOfFarmer(Game1.player);
            for (var index = homeOfFarmer.characters.Count - 1; index >= 0; --index)
            {
                if (homeOfFarmer.characters[index] is Child character)
                {
                    homeOfFarmer.GetChildBed((int)character.Gender)?.mutex.ReleaseLock();
                    if (character.hat.Value != null)
                    {
                        var hat = character.hat.Value;
                        character.hat.Value = null;
                        Game1.player.addItemToInventory(hat);
                    }
                    homeOfFarmer.characters.RemoveAt(index);
                    var num = (int)Game1.stats.Increment("childrenTurnedToDoves");
                    break;
                }
            }
            BroadcastSacrificeSprites();
        }

        private void BroadcastSacrificeSprites()
        {
            Game1.Multiplayer.broadcastSprites(Game1.currentLocation, new TemporaryAnimatedSprite("LooseSprites\\Cursors", new Rectangle(536, 1945, 8, 8), new Vector2(156f, 388f), false, 0.0f, Color.White)
            {
                interval = 50f,
                totalNumberOfLoops = 99999,
                animationLength = 7,
                layerDepth = 0.0385000035f,
                scale = 4f,
            });
            for (var index = 0; index < 20; ++index)
            {
                Game1.Multiplayer.broadcastSprites(Game1.currentLocation,
                    new TemporaryAnimatedSprite("LooseSprites\\Cursors", new Rectangle(372, 1956, 10, 10), new Vector2(2f, 6f) * 64f + new Vector2((float)Game1.random.Next(-32, 64), (float)Game1.random.Next(16)),
                        false, 1f / 500f, Color.LightGray)
                    {
                        alpha = 0.75f,
                        motion = new Vector2(1f, -0.5f),
                        acceleration = new Vector2(-1f / 500f, 0.0f),
                        interval = 99999f,
                        layerDepth = (float)(0.03840000182390213 + (double)Game1.random.Next(100) / 10000.0),
                        scale = 3f,
                        scaleChange = 0.01f,
                        rotationChange = (float)((double)Game1.random.Next(-5, 6) * 3.1415927410125732 / 256.0),
                        delayBeforeAnimationStart = index * 25,
                    });
            }
            Game1.currentLocation.playSound("fireball");
            Game1.Multiplayer.broadcastSprites(Game1.currentLocation,
                new TemporaryAnimatedSprite("LooseSprites\\Cursors", new Rectangle(388, 1894, 24, 22), 100f, 6, 9999, new Vector2(2f, 5f) * 64f, false, true, 1f, 0.0f, Color.White, 4f, 0.0f, 0.0f, 0.0f)
                {
                    motion = new Vector2(4f, -2f),
                });
            if (Game1.player.getChildrenCount() > 1)
            {
                Game1.Multiplayer.broadcastSprites(Game1.currentLocation,
                    new TemporaryAnimatedSprite("LooseSprites\\Cursors", new Rectangle(388, 1894, 24, 22), 100f, 6, 9999, new Vector2(2f, 5f) * 64f, false, true, 1f, 0.0f, Color.White, 4f, 0.0f, 0.0f, 0.0f)
                    {
                        motion = new Vector2(4f, -1.5f),
                        delayBeforeAnimationStart = 50,
                    });
            }
        }
    }
}
