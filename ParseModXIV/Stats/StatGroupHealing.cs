// ParseModXIV
// StatGroupHealing.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using ParseModXIV.Monitors;

namespace ParseModXIV.Stats
{
    public class StatGroupHealing : StatGroup
    {
        public StatGroupHealing(string name, Stat<Decimal> overall) : base(name)
        {
            InitStats(overall);
        }

        private void InitStats(Stat<Decimal> overall)
        {
            Stats.AddStats(TotalStatList());
        }

        #region " GETSET HEALING"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ability"></param>
        /// <param name="caston"></param>
        /// <param name="amount"></param>
        public void GetSetHealing(string ability, string caston, decimal amount)
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

        #endregion

        #region " STAT LISTS "

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static Stat<Decimal>[] TotalStatList()
        {
            var healTotalStat = new TotalStat("H Total");
            StatMonitor.PartyHealing.AddDependency(healTotalStat);
            var healingPctStat = new PercentStat("% of Heal", healTotalStat, StatMonitor.PartyHealing);
            var minHStat = new MinStat("Heal Low", healTotalStat);
            var maxHStat = new MaxStat("Heal High", healTotalStat);
            var avgHealingStat = new AverageStat("Heal Avg", healTotalStat);
            var hpsStat = new PerSecondAverageStat("HPS", healTotalStat);
            return new Stat<decimal>[] {healTotalStat, healingPctStat, minHStat, maxHStat, avgHealingStat, hpsStat};
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sub"></param>
        /// <returns></returns>
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

        #endregion
    }
}