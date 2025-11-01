using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Characters;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.CharacterEvents
{
    public class BabyBirther
    {
        public void SpawnNewBaby(string[] allValidNames)
        {
            var random = new Random();
            var babyGender = random.NextDouble() < 0.5;
            var babyColor = random.NextDouble() < 0.5;
            var babyName = ChooseBabyName(random, allValidNames);

            var baby = new Child(babyName, babyGender, babyColor, Game1.player)
            {
                Age = 0,
                Position = new Vector2(16f, 4f) * 64f + new Vector2(0.0f, -24f),
            };
            Utility.getHomeOfFarmer(Game1.player).characters.Add(baby);
            Game1.playSound("smallSelect");
            var spouse = Game1.player.getSpouse();

            if (spouse != null)
            {
                spouse.shouldSayMarriageDialogue.Value = true;
                spouse.currentMarriageDialogue.Insert(0, new MarriageDialogueReference("Data\\ExtraDialogue", "NewChild_Adoption", true, babyName));
            }
        }

        public void SpawnTemporaryBabies(string[] allValidNames, int count)
        {
            for (var i = 0; i < count; i++)
            {
                SpawnTemporaryBaby(allValidNames);
            }
        }

        public void SpawnTemporaryBaby(string[] allValidNames)
        {
            var babyGender = Game1.random.NextDouble() < 0.5;
            var babyColor = Game1.random.NextDouble() < 0.5;
            var babyName = ChooseBabyName(Game1.random, allValidNames);

            var currentMap = Game1.currentLocation;
            var tile = currentMap.getRandomTile() * 64f;
            var age = Game1.random.Next(4);
            var baby = new TemporaryBaby(babyName, babyGender, babyColor, Game1.player, age)
            {
                Position = tile,
            };
            Game1.currentLocation.characters.Add(baby);
        }

        private string ChooseBabyName(Random random, string[] allValidNames)
        {
            var npcNames = DataLoader.Characters(Game1.content).Keys.ToHashSet();
            foreach (var npc in Utility.getAllCharacters())
            {
                npcNames.Add(npc.Name);
            }
            string babyName;
            var maxAttempts = allValidNames.Length * 10;
            var attempt = 0;
            do
            {
                attempt++;
                babyName = allValidNames[random.Next(0, allValidNames.Length)];
                if (attempt >= maxAttempts)
                {
                    while (npcNames.Contains(babyName))
                    {
                        babyName += " ";
                    }
                }
            } while (npcNames.Contains(babyName));

            return babyName;
        }
    }
}
