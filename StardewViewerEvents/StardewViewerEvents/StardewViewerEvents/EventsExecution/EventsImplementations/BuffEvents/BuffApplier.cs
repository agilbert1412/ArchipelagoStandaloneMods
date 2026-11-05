//using StardewValley;

//namespace StardewViewerEvents.EventsExecution.EventsImplementations.BuffEvents
//{
//    public class BuffApplier
//    {
//        private const string BuffStateFile = "BuffsState.json";
//        private BuffStateDto _buffState;

//        public BuffApplier()
//        {
//            _buffState = ImportBuffs();
//        }

//        public BuffStateDto ImportBuffs()
//        {
//            return null;
//        }

//        public void ExportBuffs()
//        {

//        }

//        public void AddBuff(Buffs whichBuff, int duration, bool realTime)
//        {
//            if (duration <= 0)
//            {
//                return;
//            }

//            var buff = new Buff(whichBuff.ToString());
//            buff.millisecondsDuration = duration;
//            buff.totalMillisecondsDuration = duration;

//            switch (whichBuff)
//            {
//                case Buffs.Farming:
//                    buff.effects.FarmingLevel.Value += 1;
//                    break;
//                case Buffs.Foraging:
//                    buff.effects.ForagingLevel.Value += 1;
//                    break;
//                case Buffs.Fishing:
//                    buff.effects.FishingLevel.Value += 1;
//                    break;
//                case Buffs.Mining:
//                    buff.effects.MiningLevel.Value += 1;
//                    break;
//                case Buffs.Attack:
//                    buff.effects.Attack.Value += 1;
//                    break;
//                case Buffs.Defense:
//                    buff.effects.Defense.Value += 1;
//                    break;
//                case Buffs.Speed:
//                    buff.effects.Speed.Value += 1;
//                    break;
//                case Buffs.Luck:
//                    buff.effects.LuckLevel.Value += 1;
//                    break;
//                case Buffs.MagneticRadius:
//                    buff.effects.MagneticRadius.Value += 1;
//                    break;
//                case Buffs.MaxStamina:
//                    buff.effects.MaxStamina.Value += 1;
//                    break;
//            }

//            Game1.player.applyBuff(buff);
//            if (!_buffState.CurrentBuffs.ContainsKey(whichBuff))
//            {
//                _buffState.CurrentBuffs.Add(whichBuff, 0);
//            }

//            _buffState.CurrentBuffs[whichBuff] = Math.Max(_buffState.CurrentBuffs[whichBuff], duration);
//        }

//        public void LoadBuffs()
//        {
//            foreach (var realTimeBuff in _buffState.CurrentBuffs.Keys.ToArray())
//            {
//                LoadBuff(realTimeBuff);
//            }
//        }

//        public void SaveBuffs()
//        {
//            foreach (var realTimeBuff in _buffState.CurrentBuffs.Keys.ToArray())
//            {
//                SaveBuff(realTimeBuff);
//            }
//        }

//        private void SaveBuff(Buffs realTimeBuff)
//        {
//            foreach (var (appliedBuffName, appliedBuff) in Game1.player.buffs.AppliedBuffs)
//            {
//                if (appliedBuff.id.Equals(((int)realTimeBuff).ToString(), StringComparison.InvariantCultureIgnoreCase))
//                {
//                    var timeRemaining = appliedBuff.millisecondsDuration;
//                    _buffState.CurrentBuffs[realTimeBuff] = timeRemaining;
//                    return;
//                }
//            }

//            _buffState.CurrentBuffs.Remove(realTimeBuff);
//        }

//        private void LoadBuff(Buffs realTimeBuff)
//        {
//            foreach (var (appliedBuffName, appliedBuff) in Game1.player.buffs.AppliedBuffs)
//            {
//                if (appliedBuff.id.Equals(((int)realTimeBuff).ToString(), StringComparison.InvariantCultureIgnoreCase))
//                {
//                    appliedBuff.millisecondsDuration = Math.Max(appliedBuff.millisecondsDuration, _buffState.CurrentBuffs[realTimeBuff]);
//                    return;
//                }
//            }

//            AddBuff(realTimeBuff, _buffState.CurrentBuffs[realTimeBuff], true);
//        }
//    }

//    public class BuffStateDto
//    {

//    }

//    public enum Buffs
//    {
//        Farming = 0,
//        Fishing = 1,
//        Mining = 2,
//        Luck = 4,
//        Foraging = 5,
//        Crafting = 6,
//        MaxStamina = 7,
//        MagneticRadius = 8,
//        Speed = 9,
//        Defense = 10,
//        Attack = 11,
//        GoblinsCurse = 12,
//        Slimed = 13,
//        EvilEye = 14,
//        ChickenedOut = 15,
//        Tipsy = 17,
//        Fear = 18,
//        Frozen = 19,
//        WarriorEnergy = 20,
//        YobaBlessing = 21,
//        AdrenalineRush = 22,
//        AvoidMonsters = 23,
//        SpawnMonsters = 24,
//        Nauseous = 25,
//        Darkness = 26,
//        Weakness = 27,
//        SquidInkRavioli = 28,
//        Full = 6,
//        Quenched = 7,
//        TotalNumberOfBuffableAttriutes = 12,
//    }
//}
