// ParseModXIV
// StatGroupPlayer.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using ParseModXIV;
using ParseModXIV.Classes;
using ParseModXIV.Monitors;

namespace ParseModXIV.Stats
{
    public class StatGroupPlayer : StatGroup
    {
        private TotalStat _abilityTotal;
        private TotalStat _abilityRegTotal;
        private TotalStat _abilityCritTotal;
        private TotalStat _healingTotal;
        private TotalStat _abilityTotalDt;
        private TotalStat _abilityRegTotalDt;
        private TotalStat _abilityCritTotalDt;

        public StatGroupPlayer(string name, Stat<Decimal> overallDamage, Stat<Decimal> overallHealing, Stat<Decimal> overallDamageTaken) : base(name)
        {
            InitStats(overallDamage, overallHealing, overallDamageTaken);
        }

        private void InitStats(Stat<Decimal> overallDamage, Stat<Decimal> overallHealing, Stat<Decimal> overallDamageTaken)
        {
            _abilityTotal = new TotalStat("OverallAblility");
            _abilityRegTotal = new TotalStat("RegularAblility");
            _abilityCritTotal = new TotalStat("CriticalAblility");
            _healingTotal = new TotalStat("OverallHealing");
            _abilityTotalDt = new TotalStat("OverallTaken");
            _abilityRegTotalDt = new TotalStat("RegularTaken");
            _abilityCritTotalDt = new TotalStat("CriticalTaken");
            Stats.AddStats(TotalStatList());
            Stats.AddStats(_abilityTotal, _abilityRegTotal, _abilityCritTotal);
            Stats.AddStats(_healingTotal);
            Stats.AddStats(_abilityTotalDt, _abilityRegTotalDt, _abilityCritTotalDt);
        }

        public void AddAbilityStats(string monster, string ability, decimal amount, bool didhit, bool didcrit, bool resisted)
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

        public void AddHealingStats(string ability, string caston, decimal amount)
        {
            var abilityGroup = GetGroup("Healing");
            StatGroup subAGroup;
            if (!abilityGroup.TryGetGroup(ability, out subAGroup))
            {
                subAGroup = new StatGroup(ability);
                subAGroup.Stats.AddStats(HealingStatList("a", null));
                abilityGroup.AddGroup(subAGroup);
            }
            var playerGroup = GetGroup("Players");
            StatGroup subPGroup;
            if (!playerGroup.TryGetGroup(caston, out subPGroup))
            {
                subPGroup = new StatGroup(caston);
                subPGroup.Stats.AddStats(HealingStatList("p", null));
                playerGroup.AddGroup(subPGroup);
            }
            var abilities = subPGroup.GetGroup("Abilities");
            StatGroup subPaGroup;
            if (!abilities.TryGetGroup(ability, out subPaGroup))
            {
                subPaGroup = new StatGroup(ability);
                subPaGroup.Stats.AddStats(HealingStatList("pa", subPGroup));
                abilities.AddGroup(subPaGroup);
            }
            Stats.GetStat("H Total").Value += amount;
            subAGroup.Stats.GetStat("Total").Value += amount;
            subPGroup.Stats.GetStat("Total").Value += amount;
            subPaGroup.Stats.GetStat("Total").Value += amount;
        }

        public void AddDamageStats(string monster, string ability, decimal amount, bool didhit, bool didcrit, bool blocked)
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
            var evadeStat = new TotalStat("Evade");
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
            return new Stat<decimal>[] {totalStat, regularTotalStat, critTotalStat, healTotalStat, hitStat, cHitStat, missStat, accuracyStat, evadeStat, damagePctStat, cDamagePctStat, critPctStat, healingPctStat, minStat, maxStat, minCStat, maxCStat, minHStat, maxHStat, avgDamageStat, avgCDamageStat, avgHealingStat, dpsStat, hpsStat, totalDtStat, totalDtrStat, totalDtcStat, dtHitStat, dtcHitStat, damageDtPctStat, cDamageDtPctStat, minDtStat, maxDtStat, minDtcStat, maxDtcStat, avgDtDamageStat, avgDtcDamageStat, dtpsStat};
        }

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
                    _abilityTotal.AddDependency(totalStat);
                    _abilityRegTotal.AddDependency(regularTotalStat);
                    _abilityCritTotal.AddDependency(critTotalStat);
                    abilityPctStat = new PercentStat("% of Reg", regularTotalStat, Stats.GetStat("Reg"));
                    abilityCPctStat = new PercentStat("% of Crit", critTotalStat, Stats.GetStat("Crit"));
                    break;
                case "m":
                    abilityPctStat = new PercentStat("% of Reg", regularTotalStat, Stats.GetStat("RegularAblility"));
                    abilityCPctStat = new PercentStat("% of Crit", critTotalStat, Stats.GetStat("CriticalAblility"));
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
            return new Stat<decimal>[] { totalStat, dpsStat, regularTotalStat, critTotalStat, abilityPctStat, abilityCPctStat, hitStat, chitStat, missStat, resistStat, resistPctStat, accuracyStat, minStat, cminStat, maxStat, cmaxStat, avgDamageStat, avgCDamageStat };
        }

        private Stat<Decimal>[] HealingStatList(string type, StatGroup sub)
        {
            var totalStat = new CounterStat("Total");
            var dpsStat = new PerSecondAverageStat("HPS", totalStat);
            Stat<Decimal> healingPctStat = null;
            switch (type)
            {
                case "a":
                    _healingTotal.AddDependency(totalStat);
                    healingPctStat = new PercentStat("% of Heal", totalStat, Stats.GetStat("H Total"));
                    break;
                case "p":
                    healingPctStat = new PercentStat("% of Heal", totalStat, Stats.GetStat("OverallHealing"));
                    break;
                case "pa":
                    healingPctStat = new PercentStat("% of Heal", totalStat, sub.Stats.GetStat("Total"));
                    break;
            }
            var minStat = new MinStat("Low", totalStat);
            var maxStat = new MaxStat("High", totalStat);
            var avgHealingStat = new AverageStat("Avg", totalStat);
            return new Stat<decimal>[] {totalStat, dpsStat, healingPctStat, minStat, maxStat, avgHealingStat};
        }

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
                    _abilityTotal.AddDependency(totalStat);
                    _abilityRegTotal.AddDependency(regularTotalStat);
                    _abilityCritTotal.AddDependency(critTotalStat);
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
            return new Stat<decimal>[] {totalStat, dpsStat, regularTotalStat, critTotalStat, abilityPctStat, abilityCPctStat, hitStat, blockStat, blockPctStat, chitStat, minStat, cminStat, maxStat, cmaxStat, avgDamageStat, avgCDamageStat};
        }
    }
}