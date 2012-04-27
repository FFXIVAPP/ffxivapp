// Project: ParseModXIV
// File: StatGroupAbilities.cs
// 
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;

namespace ParseModXIV.Stats
{
    internal class StatGroupAbilities : StatGroup
    {
        private TotalStat _abilityTotalDamage;
        private TotalStat _abilityRegularTotalDamage;
        private TotalStat _abilityCritTotalDamage;
        private AccuracyStat _percentOfOverallDamage;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="overallDamage"></param>
        /// <param name="children"></param>
        public StatGroupAbilities(string name, Stat<Decimal> overallDamage, params StatGroup[] children) : base(name, children)
        {
            InitStats(overallDamage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="overallDamage"></param>
        public StatGroupAbilities(string name, Stat<Decimal> overallDamage) : base(name)
        {
            InitStats(overallDamage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="overallDamage"></param>
        private void InitStats(Stat<Decimal> overallDamage)
        {
            IncludeSelf = false;
            _abilityTotalDamage = new TotalStat("Overall");
            _abilityRegularTotalDamage = new TotalStat("Regular");
            _abilityCritTotalDamage = new TotalStat("Critical");
            _percentOfOverallDamage = new AccuracyStat("PercentOfOverall", _abilityTotalDamage, overallDamage);
            Stats.AddStats(_abilityTotalDamage, _percentOfOverallDamage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abilityName"></param>
        /// <returns></returns>
        public StatGroup AddOrGetAbility(String abilityName)
        {
            if (HasGroup(abilityName))
            {
                return GetGroup(abilityName);
            }
            var newGroup = new StatGroup(abilityName);
            newGroup.Stats.AddStats(NewStatList());
            AddGroup(newGroup);
            return newGroup;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Stat<Decimal>[] NewStatList()
        {
            var totalStat = new CounterStat("Total");
            var regularTotalStat = new CounterStat("Reg");
            var critTotalStat = new CounterStat("Crit");
            _abilityTotalDamage.AddDependency(totalStat);
            _abilityRegularTotalDamage.AddDependency(regularTotalStat);
            _abilityCritTotalDamage.AddDependency(critTotalStat);
            var abilityPctStat = new PercentStat("% of Reg", totalStat, _abilityTotalDamage);
            var abilityCPctStat = new PercentStat("% of Crit", critTotalStat, _abilityCritTotalDamage);
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
            return new Stat<decimal>[] { totalStat, regularTotalStat, critTotalStat, abilityPctStat, abilityCPctStat, hitStat, chitStat, missStat, resistStat, resistPctStat, accuracyStat, minStat, cminStat, maxStat, cmaxStat, avgDamageStat, avgCDamageStat };
        }
    }
}