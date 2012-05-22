// ParseModXIV
// StatGroupAbility.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using ParseModXIV.Monitors;

namespace ParseModXIV.Stats
{
    public class StatGroupAbility : StatGroup
    {
        public StatGroupAbility(string name, Stat<Decimal> overall)
            : base(name)
        {
            InitStats(overall);
        }

        private void InitStats(Stat<Decimal> overall)
        {
            Stats.AddStats(TotalStatList());
        }

        #region " GETSET ABILITY "

        /// <summary>
        /// 
        /// </summary>
        /// <param name="monster"></param>
        /// <param name="ability"></param>
        /// <param name="amount"></param>
        /// <param name="didhit"></param>
        /// <param name="didcrit"></param>
        /// <param name="resisted"></param>
        public void GetSetAbility(string monster, string ability, decimal amount, bool didhit, bool didcrit, bool resisted)
        {
            var abilityGroup = GetGroup("Abilities");
            StatGroup subAGroup;
            if (!abilityGroup.TryGetGroup(ability, out subAGroup))
            {
                subAGroup = new StatGroup(ability);
                subAGroup.Stats.AddStats(AbilityStatList("a", null));
                abilityGroup.AddGroup(subAGroup);
            }
            var monsterGroup = GetGroup("Monsters");
            StatGroup subMGroup;
            if (!monsterGroup.TryGetGroup(monster, out subMGroup))
            {
                subMGroup = new StatGroup(monster);
                subMGroup.Stats.AddStats(AbilityStatList("m", null));
                monsterGroup.AddGroup(subMGroup);
            }
            var monsters = subMGroup.GetGroup("Abilities");
            StatGroup subMaGroup;
            if (!monsters.TryGetGroup(ability, out subMaGroup))
            {
                subMaGroup = new StatGroup(ability);
                subMaGroup.Stats.AddStats(AbilityStatList("ma", subMGroup));
                monsters.AddGroup(subMaGroup);
            }
            subAGroup.Stats.Clear();
            if (didhit)
            {
                Stats.GetStat("Total").Value += amount;
                subAGroup.Stats.GetStat("Total").Value += amount;
                subMGroup.Stats.GetStat("Total").Value += amount;
                subMaGroup.Stats.GetStat("Total").Value += amount;
                if (didcrit)
                {
                    Stats.GetStat("Crit").Value += amount;
                    subAGroup.Stats.GetStat("Crit").Value += amount;
                    subMGroup.Stats.GetStat("Crit").Value += amount;
                    subMaGroup.Stats.GetStat("Crit").Value += amount;
                    Stats.GetStat("C Hit").Value += 1;
                    subAGroup.Stats.GetStat("C Hit").Value += 1;
                    subMGroup.Stats.GetStat("C Hit").Value += 1;
                    subMaGroup.Stats.GetStat("C Hit").Value += 1;
                }
                else
                {
                    Stats.GetStat("Reg").Value += amount;
                    subAGroup.Stats.GetStat("Reg").Value += amount;
                    subMGroup.Stats.GetStat("Reg").Value += amount;
                    subMaGroup.Stats.GetStat("Reg").Value += amount;
                    Stats.GetStat("Hit").Value += 1;
                    subAGroup.Stats.GetStat("Hit").Value += 1;
                    subMGroup.Stats.GetStat("Hit").Value += 1;
                    subMaGroup.Stats.GetStat("Hit").Value += 1;
                }
            }
            else
            {
                Stats.GetStat("Miss").Value += 1;
                subAGroup.Stats.GetStat("Miss").Value += 1;
                subMGroup.Stats.GetStat("Miss").Value += 1;
                subMaGroup.Stats.GetStat("Miss").Value += 1;
            }
            if (!resisted)
            {
                return;
            }
            subAGroup.Stats.GetStat("Resist").Value += 1;
            subMGroup.Stats.GetStat("Resist").Value += 1;
            subMaGroup.Stats.GetStat("Resist").Value += 1;
        }

        #endregion

        #region " STAT LISTS "

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static Stat<Decimal>[] TotalStatList()
        {
            var totalStat = new TotalStat("Total");
            var regularTotalStat = new TotalStat("Reg");
            var critTotalStat = new TotalStat("Crit");
            StatMonitor.TotalDamage.AddDependency(totalStat);
            StatMonitor.PartyDamage.AddDependency(regularTotalStat);
            StatMonitor.PartyCritDamage.AddDependency(critTotalStat);
            var hitStat = new TotalStat("Hit");
            var cHitStat = new TotalStat("C Hit");
            var missStat = new TotalStat("Miss");
            var accuracyStat = new AccuracyStat("Acc", hitStat, missStat);
            var evadeStat = new TotalStat("Evade");
            var damagePctStat = new PercentStat("% of Dmg", totalStat, StatMonitor.TotalDamage);
            var cDamagePctStat = new PercentStat("% of Crit", critTotalStat, StatMonitor.PartyCritDamage);
            var critPctStat = new PercentStat("Crit %", cHitStat, hitStat);
            var minStat = new MinStat("Low", regularTotalStat);
            var maxStat = new MaxStat("High", regularTotalStat);
            var minCStat = new MinStat("C Low", critTotalStat);
            var maxCStat = new MaxStat("C High", critTotalStat);
            var avgDamageStat = new AverageStat("Avg", totalStat);
            var avgCDamageStat = new AverageStat("C Avg", critTotalStat);
            var dpsStat = new PerSecondAverageStat("DPS", totalStat);
            return new Stat<decimal>[] { totalStat, regularTotalStat, critTotalStat,  hitStat, cHitStat, missStat, accuracyStat, evadeStat, damagePctStat, cDamagePctStat, critPctStat,  minStat, maxStat, minCStat, maxCStat, avgDamageStat, avgCDamageStat,  dpsStat };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sub"></param>
        /// <returns></returns>
        private Stat<Decimal>[] AbilityStatList(string type, StatGroup sub)
        {
            var totalStat = new CounterStat("Total");
            var dpsStat = new PerSecondAverageStat("DPS", totalStat);
            var regularTotalStat = new CounterStat("Reg");
            var critTotalStat = new CounterStat("Crit");
            Stat<Decimal> abilityPctStat = null;
            Stat<Decimal> abilityCPctStat = null;
            switch (type)
            {
                case "a":
                    abilityPctStat = new PercentStat("% of Reg", regularTotalStat, Stats.GetStat("Reg"));
                    abilityCPctStat = new PercentStat("% of Crit", critTotalStat, Stats.GetStat("Crit"));
                    break;
                case "m":
                    abilityPctStat = new PercentStat("% of Reg", regularTotalStat, Stats.GetStat("Reg"));
                    abilityCPctStat = new PercentStat("% of Crit", critTotalStat, Stats.GetStat("Crit"));
                    break;
                case "ma":
                    abilityPctStat = new PercentStat("% of Reg", regularTotalStat, sub.Stats.GetStat("Reg"));
                    abilityCPctStat = new PercentStat("% of Crit", critTotalStat, sub.Stats.GetStat("Crit"));
                    break;
            }
            var hitStat = new CounterStat("Hit");
            var missStat = new CounterStat("Miss");
            var resistStat = new CounterStat("Resist");
            var resistPctStat = new PercentStat("Resist %", resistStat, hitStat);
            var chitStat = new CounterStat("C Hit");
            var accuracyStat = new AccuracyStat("Acc", hitStat, missStat);
            var minStat = new MinStat("Low", regularTotalStat);
            var maxStat = new MaxStat("High", regularTotalStat);
            var cminStat = new MinStat("C Low", critTotalStat);
            var cmaxStat = new MaxStat("C High", critTotalStat);
            var avgDamageStat = new AverageStat("Avg", regularTotalStat);
            var avgCDamageStat = new AverageStat("C Avg", critTotalStat);
            return new[] { totalStat, dpsStat, regularTotalStat, critTotalStat, abilityPctStat, abilityCPctStat, hitStat, chitStat, missStat, resistStat, resistPctStat, accuracyStat, minStat, cminStat, maxStat, cmaxStat, avgDamageStat, avgCDamageStat };
        }

        #endregion
    }
}