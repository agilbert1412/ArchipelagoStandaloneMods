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
            "Bat", "Frost Bat", "Rock Golem", "Green Slime", "Frost Jelly", "Dust Spirit", "Bug", "Fly", "Grub", "Rock Crab",
        };

        private static readonly string[] _mediumMonsterTypes =
        {
            "Lava Bat", "Shadow Brute", "Sludge", "Purple Slime", "Ghost", "Metal Head", "Mummy", "Shadow Guy", "Shadow Shaman", "Squid Kid", "Skeleton",
        };
         
        private static readonly string[] _hardMonsterTypes =
        {
            "Iridium Bat", "Serpent", "Tiger Slime", "Pepper Rex", "Dwarvish Sentry", "Blue Squid", "Leaper", "Hot Head", "Shooter",
        };

        private static readonly string[] _secretMonsterTypes =
        {
            "Angry Roger", "Duggy", "Lava Lurk", "Dust Sprite", "Big Slime", "Shadow Girl", "Spiker",
        };

        public static readonly string[] AllMonsterTypes = _easyMonsterTypes.Union(_mediumMonsterTypes).Union(_hardMonsterTypes).ToArray();
        public static readonly string[] AllValidMonsterTypes = _easyMonsterTypes.Union(_mediumMonsterTypes).Union(_hardMonsterTypes).Union(_secretMonsterTypes).ToArray();

        public void SpawnManyRandomMonster(GameLocation map, int count)
        {
            for (var i = 0; i < count; i++)
            {
                SpawnOneRandomMonster(map);
            }
        }

        public void SpawnOneRandomMonster(GameLocation map)
        {
            var monster = CreateRandomMonster(map);
            SpawnMonster(map, monster);
        }

        public void SpawnManySpecificMonsters(GameLocation map, string monsterName, int count)
        {
            for (var i = 0; i < count; i++)
            {
                SpawnOneSpecificMonster(map, monsterName);
            }
        }

        public void SpawnOneSpecificMonster(GameLocation map, string monsterName)
        {
            var monster = CreateMonster(map, monsterName);
            SpawnMonster(map, monster);
        }

        public bool IsValidMonster(string monsterName)
        {
            return AllValidMonsterTypes.Any(x => x.SanitizeEntityName() == monsterName.SanitizeEntityName());
        }

        public void SpawnMonster(GameLocation map, Monster monster)
        {
            AddMonster(map, monster);
        }

        private static void AddMonster(GameLocation map, Monster monster)
        {
            monster.focusedOnFarmers = true;
            monster.wildernessFarmMonster = true;
            map.characters.Add(monster);
        }

        private Monster CreateRandomMonster(GameLocation map)
        {
            return CreateRandomMonsterFrom(map, AllMonsterTypes);
        }

        private Monster CreateRandomMonsterFrom(GameLocation map, IList<string> monsters)
        {
            var chosenMonsterType = monsters[Game1.random.Next(0, monsters.Count)];
            return CreateMonster(map, chosenMonsterType);
        }

        public Monster CreateMonster(GameLocation map, string monsterName)
        {
            var spawnPosition = _tileChooser.GetRandomTileInboundsOffScreen(map, true);
            return CreateMonster(monsterName, spawnPosition);
        }

        public Monster CreateMonster(string chosenMonsterType, Vector2 spawnPosition)
        {
            var monsterName = AllValidMonsterTypes.FirstOrDefault(x => x.SanitizeEntityName() == chosenMonsterType.SanitizeEntityName());
            var spawnPixel = spawnPosition * 64f;
            switch (monsterName)
            {
                case "Bat":
                    return new Bat(spawnPixel, 1);
                case "Frost Bat":
                    return new Bat(spawnPixel, 41);
                case "Lava Bat":
                    return new Bat(spawnPixel, 81);
                case "Iridium Bat":
                    return new Bat(spawnPixel, 172);
                case "Serpent":
                    return new Serpent(spawnPixel);
                case "Shadow Brute":
                    return new ShadowBrute(spawnPixel);
                case "Rock Golem":
                    return new RockGolem(spawnPixel, Game1.player.CombatLevel);
                case "Green Slime":
                    return new GreenSlime(spawnPixel, 1);
                case "Frost Jelly":
                    return new GreenSlime(spawnPixel, 41);
                case "Purple Slime":
                    return new GreenSlime(spawnPixel, 121);
                case "Sludge":
                    return new GreenSlime(spawnPixel, 77377);
                case "Tiger Slime":
                    var slime = new GreenSlime(spawnPixel, 0);
                    slime.makeTigerSlime();
                    return slime;
                case "Pepper Rex":
                    return new DinoMonster(spawnPixel);
                case "Angry Roger":
                    return new AngryRoger(spawnPixel);
                case "Big Slime":
                    return new BigSlime(spawnPixel, 121);
                case "Blue Squid":
                    return new BlueSquid(spawnPixel);
                case "Bug":
                    return new Bug(spawnPixel, 1);
                case "Dust Spirit":
                case "Dust Sprite":
                    return new DustSpirit(spawnPixel);
                case "Dwarvish Sentry":
                    return new DwarvishSentry(spawnPixel);
                case "Fly":
                    return new Fly(spawnPixel);
                case "Ghost":
                    return new Ghost(spawnPixel);
                case "Grub":
                    return new Grub(spawnPixel);
                case "Leaper":
                    return new Leaper(spawnPixel);
                case "Metal Head":
                    return new MetalHead(spawnPixel, 81);
                case "Mummy":
                    return new Mummy(spawnPixel);
                case "Rock Crab":
                    return new RockCrab(spawnPixel);
                case "Shadow Girl":
                    return new ShadowGirl(spawnPixel);
                case "Shadow Guy":
                    return new ShadowGuy(spawnPixel);
                case "Shadow Shaman":
                    return new ShadowShaman(spawnPixel);
                case "Shooter":
                    return new Shooter(spawnPixel);
                case "Skeleton":
                    return new Skeleton(spawnPixel);
                case "Spiker":
                    return new Spiker(spawnPixel, Game1.random.Next(4));
                case "Squid Kid":
                    return new SquidKid(spawnPixel);
                case "Hot Head":
                    return new HotHead(spawnPixel);
                case "Duggy":
                    return new Duggy(spawnPixel);
                case "Lava Lurk":
                    return new LavaLurk(spawnPixel);
                default:
                    throw new Exception($"Failed at spawning a monster of type {chosenMonsterType}");
            }
        }
    }
}
