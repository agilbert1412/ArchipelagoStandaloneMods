﻿using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Characters;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.CharacterEvents
{
    public class CowSpawner
    {
        public void SpawnManyInvisibleCows(int amount, string[] allValidNames)
        {
            for (var i = 0; i < amount; i++)
            {
                SpawnInvisibleCow(allValidNames);
            }
        }

        public void SpawnInvisibleCow(string[] allValidNames)
        {
            var cowType = Game1.random.NextDouble() < 0.5 ? "White Cow" : "Brown Cow";
            var cowName = ChooseCowName(Game1.random, allValidNames);

            var currentMap = Game1.currentLocation;
            var tile = currentMap.getRandomTile() * 64f;
            var cow = new FarmAnimal(cowType, Game1.Multiplayer.getNewID(), Game1.player.UniqueMultiplayerID)
            {
                Position = tile,
                Name = cowName,
                displayName = cowName,
            };
            cow.growFully();

            cow.modData.Add(CowManager.INVISIBLE_COW_KEY, true.ToString());

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
    }
}
