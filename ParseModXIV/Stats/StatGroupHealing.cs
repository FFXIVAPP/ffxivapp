// Project: ParseModXIV
// File: StatGroupHealing.cs
// 
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;

namespace ParseModXIV.Stats
{
    internal class StatGroupHealing : StatGroup
    {
        private TotalStat _healingTotal;
        private AccuracyStat _percentOfOverallHealing;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="overallHealing"></param>
        /// <param name="children"></param>
        public StatGroupHealing(string name, Stat<Decimal> overallHealing, params StatGroup[] children) : base(name, children)
        {
            InitStats(overallHealing);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="overallHealing"></param>
        public StatGroupHealing(string name, Stat<Decimal> overallHealing) : base(name)
        {
            InitStats(overallHealing);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="overallHealing"></param>
        private void InitStats(Stat<Decimal> overallHealing)
        {
            IncludeSelf = false;
            _healingTotal = new TotalStat("Overall");
            _percentOfOverallHealing = new AccuracyStat("PercentOfOverall", _healingTotal, overallHealing);
            Stats.AddStats(_healingTotal, _percentOfOverallHealing);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="healingName"></param>
        /// <returns></returns>
        public StatGroup AddOrGetHealing(String healingName)
        {
            if (HasGroup(healingName))
            {
                return GetGroup(healingName);
            }
            var newGroup = new StatGroup(healingName);
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
            _healingTotal.AddDependency(totalStat);
            var healingPctStat = new PercentStat("% of Heal", totalStat, _healingTotal);
            var minStat = new MinStat("Low", totalStat);
            var maxStat = new MaxStat("High", totalStat);
            var avgHealingStat = new AverageStat("Avg", totalStat);
            return new Stat<decimal>[] { totalStat, healingPctStat, minStat, maxStat, avgHealingStat };
        }
    }
}