using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.PlayerEvents.PlayerCharacterEvents
{
    public class RandomizeProfessionsEvent : ExecutableEvent
    {

        public RandomizeProfessionsEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override bool ValidateParameters(out string errorMessage)
        {
            errorMessage =
                $"Cannot change professions until at least one skill is level 5 and the player knows at least one profession.";
            return Game1.player.professions.Any() &&
                   (Game1.player.FarmingLevel >= 5 || 
                   Game1.player.FishingLevel >= 5 || 
                   Game1.player.ForagingLevel >= 5 || 
                   Game1.player.MiningLevel >= 5 || 
                   Game1.player.CombatLevel >= 5);
        }

        public override void Execute()
        {
            base.Execute();
            RandomizeFarmingProfessions();
            RandomizeForagingProfessions();
            RandomizeFishingProfessions();
            RandomizeMiningProfessions();
            RandomizeCombatProfessions();
        }

        private void RandomizeFarmingProfessions()
        {
            //SkillType.Farming = 0;
            var level = Game1.player.FarmingLevel;
            RandomizeSkillProfessions(level, new[] { 0, 1 }, new[] { 2, 3 }, new[] { 4, 5 });
        }

        private void RandomizeForagingProfessions()
        {
            //SkillType.Foraging = 2;
            var level = Game1.player.ForagingLevel;
            RandomizeSkillProfessions(level, new[] { 12, 13 }, new[] { 14, 15 }, new[] { 16, 17 });
        }

        private void RandomizeFishingProfessions()
        {
            //SkillType.Fishing = 1;
            var level = Game1.player.FishingLevel;
            RandomizeSkillProfessions(level, new[] { 6, 7 }, new[] { 8, 9 }, new[] { 10, 11 });
        }

        private void RandomizeMiningProfessions()
        {
            //SkillType.Mining = 3;
            var level = Game1.player.MiningLevel;
            RandomizeSkillProfessions(level, new[] { 18, 19 }, new[] { 20, 21 }, new[] { 22, 23 });
        }

        private void RandomizeCombatProfessions()
        {
            //SkillType.Combat = 4;
            var level = Game1.player.CombatLevel;
            RandomizeSkillProfessions(level, new[] { 24, 25 }, new[] { 26, 27 }, new[] { 28, 29 });
        }

        private static void RandomizeSkillProfessions(int level, int[] level5Professions, int[] level10Professions1, int[] level10Professions2)
        {
            if (level < 5)
            {
                return;
            }

            foreach (var profession in level5Professions)
            {
                Game1.player.professions.Remove(profession);
            }

            var level5Profession = level5Professions[Game1.random.Next(level5Professions.Length)];
            Game1.player.professions.Add(level5Profession);

            if (level < 10)
            {
                return;
            }

            var level10Professions = level5Profession == level5Professions.First() ? level10Professions1 : level10Professions2;

            foreach (var profession in level10Professions)
            {
                Game1.player.professions.Remove(profession);
            }

            var level10Profession = level10Professions[Game1.random.Next(level10Professions.Length)];
            Game1.player.professions.Add(level10Profession);
        }

    }
}