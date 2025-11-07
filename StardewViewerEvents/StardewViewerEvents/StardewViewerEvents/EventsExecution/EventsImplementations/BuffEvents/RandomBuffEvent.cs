using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.BuffEvents
{
    public class RandomBuffEvent : ExecutableEvent
    {

        public RandomBuffEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override void Execute()
        {
            base.Execute();

            var buffs = DataLoader.Buffs(Game1.content);
            var buffKeys = buffs.Keys.ToArray();
            var randomBuffId = buffKeys[Game1.random.Next(buffKeys.Length)];
            var buffData = buffs[randomBuffId];
            Game1.player.applyBuff(randomBuffId);
        }
    }
}
