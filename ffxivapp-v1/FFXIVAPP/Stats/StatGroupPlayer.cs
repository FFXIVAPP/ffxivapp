// FFXIVAPP
// StatGroupPlayer.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FFXIVAPP.Classes.Helpers;
using FFXIVAPP.Monitors;

namespace FFXIVAPP.Stats
{
    public class StatGroupPlayer : StatGroup
    {
        private static readonly IList<string> LD = new[] {"Counter", "Block", "Parry", "Resist", "Evade"};

        public StatGroupPlayer(string name) : base(name)
        {
            InitStats();
        }

        private void InitStats()
        {
            Stats.AddStats(TotalStatList());
        }

        /// <summary>
        /// </summary>
        /// <param name="d"> </param>
        public void AddAbilityStats(ParseHelper.LineData d)
        {
            var fields = d.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
            var abilityGroup = GetGroup("Abilities");
            StatGroup subAGroup;
            if (!abilityGroup.TryGetGroup(d.Action, out subAGroup))
            {
                subAGroup = new StatGroup(d.Action);
                subAGroup.Stats.AddStats(AbilityStatList("a", null));
                abilityGroup.AddGroup(subAGroup);
            }
            var monsterGroup = GetGroup("Monsters");
            StatGroup subMGroup;
            if (!monsterGroup.TryGetGroup(d.Target, out subMGroup))
            {
                subMGroup = new StatGroup(d.Target);
                subMGroup.Stats.AddStats(AbilityStatList("m", null));
                monsterGroup.AddGroup(subMGroup);
            }
            var monsters = subMGroup.GetGroup("Abilities");
            StatGroup subMaGroup;
            if (!monsters.TryGetGroup(d.Action, out subMaGroup))
            {
                subMaGroup = new StatGroup(d.Action);
                subMaGroup.Stats.AddStats(AbilityStatList("ma", subMGroup));
                monsters.AddGroup(subMaGroup);
            }
            subAGroup.Stats.GetStat("Used").Value += 1;
            subMGroup.Stats.GetStat("Used").Value += 1;
            subMaGroup.Stats.GetStat("Used").Value += 1;
            if (d.Hit)
            {
                Stats.GetStat("Total").Value += d.Amount;
                subAGroup.Stats.GetStat("Total").Value += d.Amount;
                subMGroup.Stats.GetStat("Total").Value += d.Amount;
                subMaGroup.Stats.GetStat("Total").Value += d.Amount;
                if (d.Crit)
                {
                    Stats.GetStat("Crit").Value += d.Amount;
                    subAGroup.Stats.GetStat("Crit").Value += d.Amount;
                    subMGroup.Stats.GetStat("Crit").Value += d.Amount;
                    subMaGroup.Stats.GetStat("Crit").Value += d.Amount;
                    Stats.GetStat("C Hit").Value += 1;
                    subAGroup.Stats.GetStat("C Hit").Value += 1;
                    subMGroup.Stats.GetStat("C Hit").Value += 1;
                    subMaGroup.Stats.GetStat("C Hit").Value += 1;
                }
                else
                {
                    Stats.GetStat("Reg").Value += d.Amount;
                    subAGroup.Stats.GetStat("Reg").Value += d.Amount;
                    subMGroup.Stats.GetStat("Reg").Value += d.Amount;
                    subMaGroup.Stats.GetStat("Reg").Value += d.Amount;
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
            foreach (var s in fields.SelectMany(f => LD.Where(t => t == f.Name && Equals(f.GetValue(d), true))))
            {
                subAGroup.Stats.GetStat(s).Value += 1;
                subMGroup.Stats.GetStat(s).Value += 1;
                subMaGroup.Stats.GetStat(s).Value += 1;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="d"> </param>
        public void AddHealingStats(ParseHelper.LineData d)
        {
            var abilityGroup = GetGroup("Healing");
            StatGroup subAGroup;
            if (!abilityGroup.TryGetGroup(d.Action, out subAGroup))
            {
                subAGroup = new StatGroup(d.Action);
                subAGroup.Stats.AddStats(HealingStatList("a", null));
                abilityGroup.AddGroup(subAGroup);
            }
            var playerGroup = GetGroup("Players");
            StatGroup subPGroup;
            if (!playerGroup.TryGetGroup(d.Target, out subPGroup))
            {
                subPGroup = new StatGroup(d.Target);
                subPGroup.Stats.AddStats(HealingStatList("p", null));
                playerGroup.AddGroup(subPGroup);
            }
            var abilities = subPGroup.GetGroup("Abilities");
            StatGroup subPaGroup;
            if (!abilities.TryGetGroup(d.Action, out subPaGroup))
            {
                subPaGroup = new StatGroup(d.Action);
                subPaGroup.Stats.AddStats(HealingStatList("pa", subPGroup));
                abilities.AddGroup(subPaGroup);
            }
            Stats.GetStat("H Total").Value += d.Amount;
            subAGroup.Stats.GetStat("Total").Value += d.Amount;
            subPGroup.Stats.GetStat("Total").Value += d.Amount;
            subPaGroup.Stats.GetStat("Total").Value += d.Amount;
        }

        /// <summary>
        /// </summary>
        /// <param name="d"> </param>
        public void AddDamageStats(ParseHelper.LineData d)
        {
            var fields = d.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
            var damageGroup = GetGroup("Damage");
            StatGroup subMGroup;
            if (!damageGroup.TryGetGroup(d.Source, out subMGroup))
            {
                subMGroup = new StatGroup(d.Source);
                subMGroup.Stats.AddStats(DamageStatList("m", null));
                damageGroup.AddGroup(subMGroup);
            }
            var abilities = subMGroup.GetGroup("Abilities");
            StatGroup subMaGroup;
            if (!abilities.TryGetGroup(d.Action, out subMaGroup))
            {
                subMaGroup = new StatGroup(d.Action);
                subMaGroup.Stats.AddStats(DamageStatList("a", subMGroup));
                abilities.AddGroup(subMaGroup);
            }
            subMGroup.Stats.GetStat("Used").Value += 1;
            subMaGroup.Stats.GetStat("Used").Value += 1;
            if (d.Hit)
            {
                subMGroup.Stats.GetStat("Total").Value += d.Amount;
                subMaGroup.Stats.GetStat("Total").Value += d.Amount;
                Stats.GetStat("DT Total").Value += d.Amount;
                if (d.Crit)
                {
                    subMGroup.Stats.GetStat("Crit").Value += d.Amount;
                    subMGroup.Stats.GetStat("C Hit").Value += 1;
                    subMaGroup.Stats.GetStat("Crit").Value += d.Amount;
                    subMaGroup.Stats.GetStat("C Hit").Value += 1;
                    Stats.GetStat("DT Crit").Value += d.Amount;
                    Stats.GetStat("DT C Hit").Value += 1;
                }
                else
                {
                    subMGroup.Stats.GetStat("Reg").Value += d.Amount;
                    subMGroup.Stats.GetStat("Hit").Value += 1;
                    subMaGroup.Stats.GetStat("Reg").Value += d.Amount;
                    subMaGroup.Stats.GetStat("Hit").Value += 1;
                    Stats.GetStat("DT Reg").Value += d.Amount;
                    Stats.GetStat("DT Hit").Value += 1;
                }
            }
            foreach (var s in fields.SelectMany(f => LD.Where(t => t == f.Name && Equals(f.GetValue(d), true))))
            {
                subMGroup.Stats.GetStat(s).Value += 1;
                subMaGroup.Stats.GetStat(s).Value += 1;
            }
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        private static Stat<Decimal>[] TotalStatList()
        {
            var totalStat = new TotalStat("Total");
            var regularTotalStat = new TotalStat("Reg");
            var critTotalStat = new TotalStat("Crit");
            var healTotalStat = new TotalStat("H Total");
            var totalDtStat = new TotalStat("DT Total");
            var totalDtrStat = new TotalStat("DT Reg");
            var totalDtcStat = new TotalStat("DT Crit");
            StatMonitor.TotalDamage.AddDependency(totalStat);
            StatMonitor.PartyDamage.AddDependency(regularTotalStat);
            StatMonitor.PartyCritDamage.AddDependency(critTotalStat);
            StatMonitor.PartyHealing.AddDependency(healTotalStat);
            StatMonitor.PartyTotalTaken.AddDependency(totalDtStat);
            StatMonitor.PartyTotalRTaken.AddDependency(totalDtrStat);
            StatMonitor.PartyTotalCTaken.AddDependency(totalDtcStat);
            var hitStat = new TotalStat("Hit");
            var cHitStat = new TotalStat("C Hit");
            var dtHitStat = new TotalStat("DT Hit");
            var dtcHitStat = new TotalStat("DT C Hit");
            var missStat = new TotalStat("Miss");
            var accuracyStat = new AccuracyStat("Acc", hitStat, missStat);
            var counterStat = new CounterStat("Counter");
            var blockStat = new CounterStat("Block");
            var parryStat = new CounterStat("Parry");
            var resistStat = new CounterStat("Resist");
            var evadeStat = new CounterStat("Evade");
            var damagePctStat = new PercentStat("% of Dmg", totalStat, StatMonitor.TotalDamage);
            var cDamagePctStat = new PercentStat("% of Crit", critTotalStat, StatMonitor.PartyCritDamage);
            var damageDtPctStat = new PercentStat("% of DT Dmg", totalDtStat, StatMonitor.PartyTotalTaken);
            var cDamageDtPctStat = new PercentStat("% of DT C Dmg", totalDtcStat, StatMonitor.PartyTotalCTaken);
            var critPctStat = new PercentStat("Crit %", cHitStat, hitStat);
            var healingPctStat = new PercentStat("% of Heal", healTotalStat, StatMonitor.PartyHealing);
            var minStat = new MinStat("Low", regularTotalStat);
            var maxStat = new MaxStat("High", regularTotalStat);
            var minCStat = new MinStat("C Low", critTotalStat);
            var maxCStat = new MaxStat("C High", critTotalStat);
            var minHStat = new MinStat("Heal Low", healTotalStat);
            var maxHStat = new MaxStat("Heal High", healTotalStat);
            var minDtStat = new MinStat("DT Low", totalDtStat);
            var maxDtStat = new MaxStat("DT High", totalDtStat);
            var minDtcStat = new MinStat("DT C Low", totalDtcStat);
            var maxDtcStat = new MaxStat("DT C High", totalDtcStat);
            var avgDamageStat = new AverageStat("Avg", totalStat);
            var avgCDamageStat = new AverageStat("C Avg", critTotalStat);
            var avgDtDamageStat = new AverageStat("DT Avg", totalDtStat);
            var avgDtcDamageStat = new AverageStat("DT C Avg", totalDtcStat);
            var avgHealingStat = new AverageStat("Heal Avg", healTotalStat);
            var dpsStat = new PerSecondAverageStat("DPS", totalStat);
            var hpsStat = new PerSecondAverageStat("HPS", healTotalStat);
            var dtpsStat = new PerSecondAverageStat("DTPS", totalDtStat);
            return new Stat<decimal>[] {totalStat, regularTotalStat, critTotalStat, healTotalStat, hitStat, cHitStat, missStat, accuracyStat, damagePctStat, cDamagePctStat, critPctStat, healingPctStat, minStat, maxStat, minCStat, maxCStat, minHStat, maxHStat, avgDamageStat, avgCDamageStat, avgHealingStat, dpsStat, hpsStat, totalDtStat, totalDtrStat, totalDtcStat, dtHitStat, dtcHitStat, damageDtPctStat, cDamageDtPctStat, minDtStat, maxDtStat, minDtcStat, maxDtcStat, avgDtDamageStat, avgDtcDamageStat, dtpsStat, counterStat, blockStat, parryStat, resistStat, evadeStat};
        }

        /// <summary>
        /// </summary>
        /// <param name="type"> </param>
        /// <param name="sub"> </param>
        /// <returns> </returns>
        private Stat<Decimal>[] AbilityStatList(string type, StatGroup sub)
        {
            var totalStat = new CounterStat("Total");
            var totalAttempt = new CounterStat("Used");
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
            var chitStat = new CounterStat("C Hit");
            var accuracyStat = new AccuracyStat("Acc", totalAttempt, missStat);
            var minStat = new MinStat("Low", regularTotalStat);
            var maxStat = new MaxStat("High", regularTotalStat);
            var cminStat = new MinStat("C Low", critTotalStat);
            var cmaxStat = new MaxStat("C High", critTotalStat);
            var avgDamageStat = new AverageStat("Avg", regularTotalStat);
            var avgCDamageStat = new AverageStat("C Avg", critTotalStat);
            var counterStat = new CounterStat("Counter");
            var counterPctStat = new PercentStat("Counter %", counterStat, totalAttempt);
            var blockStat = new CounterStat("Block");
            var blockPctStat = new PercentStat("Block %", blockStat, totalAttempt);
            var parryStat = new CounterStat("Parry");
            var parryPctStat = new PercentStat("Parry %", parryStat, totalAttempt);
            var resistStat = new CounterStat("Resist");
            var resistPctStat = new PercentStat("Resist %", resistStat, totalAttempt);
            var evadeStat = new CounterStat("Evade");
            var evadePctStat = new PercentStat("Evade %", evadeStat, hitStat);
            return new[] {totalStat, dpsStat, regularTotalStat, critTotalStat, abilityPctStat, abilityCPctStat, hitStat, chitStat, missStat, accuracyStat, minStat, cminStat, maxStat, cmaxStat, avgDamageStat, avgCDamageStat, totalAttempt, counterStat, counterPctStat, blockStat, blockPctStat, parryStat, parryPctStat, resistStat, resistPctStat, evadeStat, evadePctStat};
        }

        /// <summary>
        /// </summary>
        /// <param name="type"> </param>
        /// <param name="sub"> </param>
        /// <returns> </returns>
        private Stat<Decimal>[] HealingStatList(string type, StatGroup sub)
        {
            var totalStat = new CounterStat("Total");
            var dpsStat = new PerSecondAverageStat("HPS", totalStat);
            Stat<Decimal> healingPctStat = null;
            switch (type)
            {
                case "a":
                    healingPctStat = new PercentStat("% of Heal", totalStat, Stats.GetStat("H Total"));
                    break;
                case "p":
                    healingPctStat = new PercentStat("% of Heal", totalStat, Stats.GetStat("H Total"));
                    break;
                case "pa":
                    healingPctStat = new PercentStat("% of Heal", totalStat, sub.Stats.GetStat("Total"));
                    break;
            }
            var minStat = new MinStat("Low", totalStat);
            var maxStat = new MaxStat("High", totalStat);
            var avgHealingStat = new AverageStat("Avg", totalStat);
            return new[] {totalStat, dpsStat, healingPctStat, minStat, maxStat, avgHealingStat};
        }

        /// <summary>
        /// </summary>
        /// <param name="type"> </param>
        /// <param name="sub"> </param>
        /// <returns> </returns>
        private Stat<Decimal>[] DamageStatList(string type, StatGroup sub)
        {
            var totalStat = new CounterStat("Total");
            var totalAttempt = new CounterStat("Used");
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
            var chitStat = new CounterStat("C Hit");
            var minStat = new MinStat("Low", regularTotalStat);
            var maxStat = new MaxStat("High", regularTotalStat);
            var cminStat = new MinStat("C Low", critTotalStat);
            var cmaxStat = new MaxStat("C High", critTotalStat);
            var avgDamageStat = new AverageStat("Avg", regularTotalStat);
            var avgCDamageStat = new AverageStat("C Avg", critTotalStat);
            var counterStat = new CounterStat("Counter");
            var counterPctStat = new PercentStat("Counter %", counterStat, totalAttempt);
            var blockStat = new CounterStat("Block");
            var blockPctStat = new PercentStat("Block %", blockStat, totalAttempt);
            var parryStat = new CounterStat("Parry");
            var parryPctStat = new PercentStat("Parry %", parryStat, totalAttempt);
            var resistStat = new CounterStat("Resist");
            var resistPctStat = new PercentStat("Resist %", resistStat, totalAttempt);
            var evadeStat = new CounterStat("Evade");
            var evadePctStat = new PercentStat("Evade %", evadeStat, totalAttempt);
            return new[] {totalStat, dpsStat, regularTotalStat, critTotalStat, abilityPctStat, abilityCPctStat, hitStat, chitStat, minStat, cminStat, maxStat, cmaxStat, avgDamageStat, avgCDamageStat, totalAttempt, counterStat, counterPctStat, blockStat, blockPctStat, parryStat, parryPctStat, resistStat, resistPctStat, evadeStat, evadePctStat};
        }
    }
}