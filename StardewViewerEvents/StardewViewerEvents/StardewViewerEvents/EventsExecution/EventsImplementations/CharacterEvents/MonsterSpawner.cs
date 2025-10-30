using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Monsters;
using StardewViewerEvents.EventsExecution.EventsImplementations;

namespace StardewArchipelago.Items.Traps
{
    public class MonsterSpawner
    {
        private readonly TileChooser _tileChooser;

        public MonsterSpawner(TileChooser tileChooser)
        {
            _tileChooser = tileChooser;
        }

        private static readonly string[] _easyMonsterTypes =
        {
            "Bat", "Frost Bat", "Rock Golem", "Green Slime", "Frost Jelly",
        };

        private static readonly string[] _mediumMonsterTypes =
        {
            "Lava Bat", "Shadow Brute", "Sludge", "Purple Slime",
        };
         
        private static readonly string[] _hardMonsterTypes =
        {
            "Iridium Bat", "Serpent", "Tiger Slime",
        };

        public static readonly string[] AllMonsterTypes = _easyMonsterTypes.Union(_mediumMonsterTypes).Union(_hardMonsterTypes).ToArray();

        public void SpawnOneMonster(GameLocation map)
        {
            var monster = ChooseRandomMonster(map);
            AddMonster(map, monster);
        }

        private static void AddMonster(GameLocation map, Monster monster)
        {
            monster.focusedOnFarmers = true;
            monster.wildernessFarmMonster = true;
            map.characters.Add(monster);
        }

        private Monster ChooseRandomMonster(GameLocation map)
        {
            var monsters = new List<string>();
            monsters.AddRange(_easyMonsterTypes);
            monsters.AddRange(_mediumMonsterTypes);
            monsters.AddRange(_hardMonsterTypes);
            return ChooseRandomMonsterFrom(map, monsters);
        }

        private Monster ChooseRandomMonsterFrom(GameLocation map, IList<string> monsters)
        {
            var spawnPosition = _tileChooser.GetRandomTileInboundsOffScreen(map);
            var chosenMonsterType = monsters[Game1.random.Next(0, monsters.Count)];
            return GetMonster(chosenMonsterType, spawnPosition);
        }

        public Monster GetMonster(string chosenMonsterType, Vector2 spawnPosition)
        {
            switch (chosenMonsterType)
            {
                case "Bat":
                    return new Bat(spawnPosition * 64f, 1);
                case "Frost Bat":
                    return new Bat(spawnPosition * 64f, 41);
                case "Lava Bat":
                    return new Bat(spawnPosition * 64f, 81);
                case "Iridium Bat":
                    return new Bat(spawnPosition * 64f, 172);
                case "Serpent":
                    return new Serpent(spawnPosition * 64f);
                case "Shadow Brute":
                    return new ShadowBrute(spawnPosition * 64f);
                case "Rock Golem":
                    return new RockGolem(spawnPosition * 64f, Game1.player.CombatLevel);
                case "Green Slime":
                    return new GreenSlime(spawnPosition * 64f, 1);
                case "Frost Jelly":
                    return new GreenSlime(spawnPosition * 64f, 41);
                case "Purple Slime":
                    return new GreenSlime(spawnPosition * 64f, 121);
                case "Sludge":
                    return new GreenSlime(spawnPosition * 64f, 77377);
                case "Tiger Slime":
                    var slime = new GreenSlime(spawnPosition * 64f, 0);
                    slime.makeTigerSlime();
                    return slime;
                default:
                    throw new Exception($"Failed at spawning a monster of type {chosenMonsterType}");
            }
        }
    }
}
