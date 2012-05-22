// ParseModXIV
// StatGroupDamage.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using ParseModXIV.Monitors;

namespace ParseModXIV.Stats
{
    public class StatGroupDamage : StatGroup
    {
        public StatGroupDamage(string name, Stat<Decimal> overall)
            : base(name)
        {
            InitStats(overall);
        }

        private void InitStats(Stat<Decimal> overall)
        {
            Stats.AddStats(TotalStatList());
        }

        #region " GETSET DAMAGE "

        /// <summary>
        /// 
        /// </summary>
        /// <param name="monster"></param>
        /// <param name="ability"></param>
        /// <param name="amount"></param>
        /// <param name="didhit"></param>
        /// <param name="didcrit"></param>
        /// <param name="blocked"></param>
        public void GetSetDamage(string monster, string ability, decimal amount, bool didhit, bool didcrit, bool blocked)
        {
            var damageGroup = GetGroup("Damage");
            StatGroup subMGroup;
            if (!damageGroup.TryGetGroup(monster, out subMGroup))
            {
                subMGroup = new StatGroup(monster);
                subMGroup.Stats.AddStats(DamageStatList("m", null));
                damageGroup.AddGroup(subMGroup);
            }
            var abilities = subMGroup.GetGroup("Abilities");
            StatGroup subMaGroup;
            if (!abilities.TryGetGroup(ability, out subMaGroup))
            {
                subMaGroup = new StatGroup(ability);
                subMaGroup.Stats.AddStats(DamageStatList("a", subMGroup));
                abilities.AddGroup(subMaGroup);
            }

            if (didhit)
            {
                subMGroup.Stats.GetStat("Total").Value += amount;
                subMaGroup.Stats.GetStat("Total").Value += amount;
                Stats.GetStat("DT Total").Value += amount;
                if (didcrit)
                {
                    subMGroup.Stats.GetStat("Crit").Value += amount;
                    subMGroup.Stats.GetStat("C Hit").Value += 1;
                    subMaGroup.Stats.GetStat("Crit").Value += amount;
                    subMaGroup.Stats.GetStat("C Hit").Value += 1;
                    Stats.GetStat("DT Crit").Value += amount;
                    Stats.GetStat("DT C Hit").Value += 1;
                }
                else
                {
                    subMGroup.Stats.GetStat("Reg").Value += amount;
                    subMGroup.Stats.GetStat("Hit").Value += 1;
                    subMaGroup.Stats.GetStat("Reg").Value += amount;
                    subMaGroup.Stats.GetStat("Hit").Value += 1;
                    Stats.GetStat("DT Reg").Value += amount;
                    Stats.GetStat("DT Hit").Value += 1;
                }
            }
            if (!blocked)
            {
                return;
            }
            subMGroup.Stats.GetStat("Block").Value += 1;
            subMaGroup.Stats.GetStat("Block").Value += 1;
        }

        #endregion

        #region " STAT LISTS "

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static Stat<Decimal>[] TotalStatList()
        {
            var totalDtStat = new TotalStat("DT Total");
            var totalDtrStat = new TotalStat("DT Reg");
            var totalDtcStat = new TotalStat("DT Crit");
            StatMonitor.PartyTotalTaken.AddDependency(totalDtStat);
            StatMonitor.PartyTotalRTaken.AddDependency(totalDtrStat);
            StatMonitor.PartyTotalCTaken.AddDependency(totalDtcStat);
            var dtHitStat = new TotalStat("DT Hit");
            var dtcHitStat = new TotalStat("DT C Hit");
            var damageDtPctStat = new PercentStat("% of DT Dmg", totalDtStat, StatMonitor.PartyTotalTaken);
            var cDamageDtPctStat = new PercentStat("% of DT C Dmg", totalDtcStat, StatMonitor.PartyTotalCTaken);
            var minDtStat = new MinStat("DT Low", totalDtStat);
            var maxDtStat = new MaxStat("DT High", totalDtStat);
            var minDtcStat = new MinStat("DT C Low", totalDtcStat);
            var maxDtcStat = new MaxStat("DT C High", totalDtcStat);
            var avgDtDamageStat = new AverageStat("DT Avg", totalDtStat);
            var avgDtcDamageStat = new AverageStat("DT C Avg", totalDtcStat);
            var dtpsStat = new PerSecondAverageStat("DTPS", totalDtStat);
            return new Stat<decimal>[] { totalDtStat, totalDtrStat, totalDtcStat, dtHitStat, dtcHitStat, damageDtPctStat, cDamageDtPctStat, minDtStat, maxDtStat, minDtcStat, maxDtcStat, avgDtDamageStat, avgDtcDamageStat, dtpsStat };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sub"></param>
        /// <returns></returns>
        private Stat<Decimal>[] DamageStatList(string type, StatGroup sub)
        {
            var totalStat = new CounterStat("Total");
            var dpsStat = new PerSecondAverageStat("DTPS", totalStat);
            var regularTotalStat = new CounterStat("Reg");
            var critTotalStat = new CounterStat("Crit");
            Stat<Decimal> abilityPctStat = null;
            Stat<Decimal> abilityCPctStat = null;
            switch (type)
            {
                case "m":
                    abilityPctStat = new PercentStat("% of Reg", regularTotalStat, Stats.GetStat("DT Reg"));
                    abilityCPctStat = new PercentStat("% of Crit", critTotalStat, Stats.GetStat("DT Crit"));
                    break;
                case "a":
                    abilityPctStat = new PercentStat("% of Reg", regularTotalStat, sub.Stats.GetStat("Reg"));
                    abilityCPctStat = new PercentStat("% of Crit", critTotalStat, sub.Stats.GetStat("Crit"));
                    break;
            }
            var hitStat = new CounterStat("Hit");
            var attackStat = new TotalStat("A Hit");
            var chitStat = new CounterStat("C Hit");
            attackStat.AddDependency(hitStat);
            attackStat.AddDependency(chitStat);
            var blockStat = new CounterStat("Block");
            var blockPctStat = new PercentStat("Block %", blockStat, attackStat);
            var minStat = new MinStat("Low", regularTotalStat);
            var maxStat = new MaxStat("High", regularTotalStat);
            var cminStat = new MinStat("C Low", critTotalStat);
            var cmaxStat = new MaxStat("C High", critTotalStat);
            var avgDamageStat = new AverageStat("Avg", regularTotalStat);
            var avgCDamageStat = new AverageStat("C Avg", critTotalStat);
            return new[] { totalStat, dpsStat, regularTotalStat, critTotalStat, abilityPctStat, abilityCPctStat, hitStat, blockStat, blockPctStat, chitStat, minStat, cminStat, maxStat, cmaxStat, avgDamageStat, avgCDamageStat };
        }

        #endregion
    }
}