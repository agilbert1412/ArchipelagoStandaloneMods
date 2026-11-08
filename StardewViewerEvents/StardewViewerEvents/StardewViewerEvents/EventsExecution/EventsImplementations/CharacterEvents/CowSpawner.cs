using StardewValley;
using xTile;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.CharacterEvents
{
    public class CowSpawner
    {
        public void SpawnManyInvisibleCows(int amount, string[] allValidNames, bool longCows)
        {
            for (var i = 0; i < amount; i++)
            {
                SpawnInvisibleCow(allValidNames, longCows);
            }
        }

        public void SpawnInvisibleCow(string[] allValidNames, bool longCow)
        {
            var cowType = Game1.random.NextDouble() < 0.5 ? "White Cow" : "Brown Cow";
            var cowName = ChooseCowName(Game1.random, allValidNames);

            var currentMap = Game1.currentLocation;
            var tile = currentMap.getRandomTile() * 64f;
            var cow = new FarmAnimal(cowType, Game1.random.NextInt64(), Game1.player.UniqueMultiplayerID)
            {
                Position = tile,
                Name = cowName,
                displayName = cowName,
            };
            cow.growFully();

            cow.modData.Add(CowManager.INVISIBLE_COW_KEY, true.ToString());
            if (longCow)
            {
                cow.modData.Add(CowManager.LONG_INVISIBLE_COW_KEY, true.ToString());
            }

            // ((AnimalHouse)this.newAnimalHome.GetIndoors()).adoptAnimal(this.animalBeingPurchased);

            Game1.currentLocation.Animals.Add(cow.myID.Value, cow);
            cow.currentLocation = Game1.currentLocation;
            // Game1.currentLocation.Animals.Add(cow.myID.Value);
            cow.homeInterior = Game1.currentLocation;
            cow.setRandomPosition(Game1.currentLocation);
        }

        private string ChooseCowName(Random random, string[] allValidNames)
        {
            var npcNames = DataLoader.Characters(Game1.content).Keys.ToHashSet();
            foreach (var npc in Utility.getAllCharacters())
            {
                npcNames.Add(npc.Name);
            }
            string cowName;
            var maxAttempts = allValidNames.Length * 10;
            var attempt = 0;
            do
            {
                attempt++;
                cowName = allValidNames[random.Next(0, allValidNames.Length)];
                if (attempt >= maxAttempts)
                {
                    while (npcNames.Contains(cowName))
                    {
                        cowName += " ";
                    }
                }
            } while (npcNames.Contains(cowName));

            return cowName;
        }

        public int CountGhostCowsOnCurrentMap()
        {
            var map = Game1.player.currentLocation;
            var ghostCows = map.Animals.Values.Where(x => x.modData.ContainsKey(CowManager.INVISIBLE_COW_KEY));
            return ghostCows.Count();
        }

        public void RemoveInvisibleCows(int amount)
        {
            while (amount > 0)
            {
                if (TryDeleteLongCowOnCurrentMap())
                {
                    amount--;
                    continue;
                }
                if (TryDeleteSmallCowOnCurrentMap())
                {
                    amount--;
                    continue;
                }

                return;
            }
        }

        private bool TryDeleteLongCowOnCurrentMap()
        {
            var map = Game1.player.currentLocation;
            var cowsOnMap = map.Animals.Values.Where(x => x.modData.ContainsKey(CowManager.INVISIBLE_COW_KEY) && x.modData.ContainsKey(CowManager.LONG_INVISIBLE_COW_KEY)).ToArray();

            if (!cowsOnMap.Any())
            {
                return false;
            }

            DeleteRandomCow(cowsOnMap);
            return true;
        }

        private bool TryDeleteSmallCowOnCurrentMap()
        {
            var map = Game1.player.currentLocation;
            var cowsOnMap = map.Animals.Values.Where(x => x.modData.ContainsKey(CowManager.INVISIBLE_COW_KEY) && !x.modData.ContainsKey(CowManager.LONG_INVISIBLE_COW_KEY)).ToArray();

            if (!cowsOnMap.Any())
            {
                return false;
            }

            DeleteRandomCow(cowsOnMap);
            return true;
        }

        private static void DeleteRandomCow(FarmAnimal[] cowsOnMap)
        {
            var cowToDelete = cowsOnMap[Game1.random.Next(cowsOnMap.Length)];

            if (cowToDelete.homeInterior is AnimalHouse animalHouse)
            {
                animalHouse.animalsThatLiveHere.Remove(cowToDelete.myID.Value);
            }

            cowToDelete.currentLocation.Animals.Remove(cowToDelete.myID.Value);
        }
    }
}
