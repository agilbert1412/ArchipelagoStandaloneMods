using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Monsters;
using StardewViewerEvents.Extensions;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.CharacterEvents
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

        public void SpawnOneRandomMonster(GameLocation map)
        {
            var monster = ChooseRandomMonster(map);
            SpawnOneMonster(map, monster);
        }

        public void SpawnOneSpecificMonster(GameLocation map, string monsterName)
        {
            var monster = GetSpecificMonster(map, monsterName);
            SpawnOneMonster(map, monster);
        }

        public bool IsValidMonster(string monsterName)
        {
            return AllMonsterTypes.Any(x => x.SanitizeEntityName() == monsterName.SanitizeEntityName());
        }

        public void SpawnOneMonster(GameLocation map, Monster monster)
        {
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
            return GetRandomMonsterFrom(map, monsters);
        }

        private Monster GetRandomMonsterFrom(GameLocation map, IList<string> monsters)
        {
            var chosenMonsterType = monsters[Game1.random.Next(0, monsters.Count)];
            return GetSpecificMonster(map, chosenMonsterType);
        }

        public Monster GetSpecificMonster(GameLocation map, string monsterName)
        {
            var spawnPosition = _tileChooser.GetRandomTileInboundsOffScreen(map);
            return GetMonster(monsterName, spawnPosition);
        }

        public Monster GetMonster(string chosenMonsterType, Vector2 spawnPosition)
        {
            var monsterName = AllMonsterTypes.FirstOrDefault(x => x.SanitizeEntityName() == chosenMonsterType.SanitizeEntityName());
            switch (monsterName)
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
