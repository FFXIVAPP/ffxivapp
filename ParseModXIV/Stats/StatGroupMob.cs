// Project: ParseModXIV
// File: StatGroupMob.cs
// 
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using NLog;
using ParseModXIV.Classes;
using ParseModXIV.Model;

namespace ParseModXIV.Stats
{
    public class StatGroupMob : StatGroup
    {
        private TotalStat TotalDrops { get; set; }
        private CounterStat NumKilled { get; set; }
        private new static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public StatGroupMob(string name) : base(name)
        {
            InitStats();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitStats()
        {
            TotalDrops = new TotalStat("Total Drops");
            NumKilled = new CounterStat("Killed");
            var totalStat = new CounterStat("Total");
            var regularTotalStat = new CounterStat("Reg");
            var avgHp = new NumericStat("Avg HP");
            var critTotalStat = new CounterStat("Crit");
            var hitStat = new CounterStat("Hit");
            var chitStat = new CounterStat("C Hit");
            var minStat = new MinStat("Low", regularTotalStat);
            var maxStat = new MaxStat("High", regularTotalStat);
            var cminStat = new MinStat("C Low", critTotalStat);
            var cmaxStat = new MaxStat("C High", critTotalStat);
            var avgDamageStat = new AverageStat("Avg", regularTotalStat);
            var avgCDamageStat = new AverageStat("C Avg", critTotalStat);
            Stats.Add(TotalDrops);
            Stats.AddStats(new Stat<decimal>[] { NumKilled });
            Stats.AddStats(new Stat<decimal>[] { totalStat });
            Stats.AddStats(new Stat<decimal>[] { avgHp });
            Stats.AddStats(new Stat<decimal>[] { regularTotalStat });
            Stats.AddStats(new Stat<decimal>[] { critTotalStat });
            Stats.AddStats(new Stat<decimal>[] { hitStat });
            Stats.AddStats(new Stat<decimal>[] { chitStat });
            Stats.AddStats(new Stat<decimal>[] { minStat });
            Stats.AddStats(new Stat<decimal>[] { maxStat });
            Stats.AddStats(new Stat<decimal>[] { cminStat });
            Stats.AddStats(new Stat<decimal>[] { cmaxStat });
            Stats.AddStats(new Stat<decimal>[] { avgDamageStat });
            Stats.AddStats(new Stat<decimal>[] { avgCDamageStat });
            var abilityStatGroup = new StatGroup("M Abilities") { IncludeSelf = false };
            var dropStatGroup = new StatGroup("Drops") { IncludeSelf = false };
            AddGroup(abilityStatGroup);
            AddGroup(dropStatGroup);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        public void AddKillStats(Fight f)
        {
            if (f.MobName != Name)
            {
                //logger.Error("Got request to add kill stats for {0}, but my name is {1}!", f.MobName, Name);
                return;
            }
            if (f.MobName == "")
            {
                //logger.Error("Got request to add kill stats for {0}, but no name!", f.MobName);
                return;
            }
            ParseMod.LastKilled = f.MobName;
            var numKilledStat = Stats.GetStat("Killed");
            numKilledStat.Value += 1;
            Stats.GetStat("Avg HP").Value = Stats.GetStat("Total").Value / Stats.GetStat("Killed").Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dmgAmount"></param>
        /// <param name="resisted"></param>
        /// <param name="evaded"></param>
        /// <param name="critical"></param>
        public void AddAbilityStats(string name, decimal dmgAmount, bool resisted, bool evaded, bool critical)
        {
            var abilityGroup = GetGroup("M Abilities");
            StatGroup subGroup;
            if (!abilityGroup.TryGetGroup(name, out subGroup))
            {
                subGroup = new StatGroup(name);
                var totalStat = new CounterStat("Total");
                var totalAttempt = new CounterStat("Used");
                var regularTotalStat = new CounterStat("Reg");
                var critTotalStat = new CounterStat("Crit");
                var hitStat = new CounterStat("Hit");
                var chitStat = new CounterStat("C Hit");
                var minStat = new MinStat("Low", regularTotalStat);
                var maxStat = new MaxStat("High", regularTotalStat);
                var cminStat = new MinStat("C Low", critTotalStat);
                var cmaxStat = new MaxStat("C High", critTotalStat);
                var avgDamageStat = new AverageStat("Avg", regularTotalStat);
                var avgCDamageStat = new AverageStat("C Avg", critTotalStat);
                var resistStat = new CounterStat("Resist");
                var resistPct = new PercentStat("Resist %", resistStat, totalAttempt);
                var evadeStat = new CounterStat("Evade");
                var evadePct = new PercentStat("Evade %", evadeStat, totalAttempt);
                subGroup.Stats.AddStats(new Stat<decimal>[] { totalStat, regularTotalStat, critTotalStat, hitStat, chitStat, minStat, cminStat, maxStat, cmaxStat, avgDamageStat, avgCDamageStat, totalAttempt, resistStat, evadeStat, resistPct, evadePct });
                abilityGroup.AddGroup(subGroup);
            }
            subGroup.Stats.GetStat("Total").Value += dmgAmount;
            Stats.GetStat("Total").Value += dmgAmount;
            subGroup.Stats.GetStat("Used").Value += 1;
            if (critical)
            {
                subGroup.Stats.GetStat("C Hit").Value += 1;
                subGroup.Stats.GetStat("Crit").Value += dmgAmount;
                Stats.GetStat("C Hit").Value += 1;
                Stats.GetStat("Crit").Value += dmgAmount;
            }
            else
            {
                subGroup.Stats.GetStat("Hit").Value += 1;
                subGroup.Stats.GetStat("Reg").Value += dmgAmount;
                Stats.GetStat("Hit").Value += 1;
                Stats.GetStat("Reg").Value += dmgAmount;
            }
            if (resisted)
            {
                subGroup.Stats.GetStat("Resist").Value += 1;
            }
            if (evaded)
            {
                subGroup.Stats.GetStat("Evade").Value += 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public void AddDropStats(string name)
        {
            var dropGroup = GetGroup("Drops");
            StatGroup subGroup = null;
            if (!dropGroup.TryGetGroup(name, out subGroup))
            {
                subGroup = new StatGroup(name);
                var totalStat = new CounterStat("Total");
                TotalDrops.AddDependency(totalStat);
                var dropPctStat = new PercentStat("Drop %", totalStat, NumKilled);
                subGroup.Stats.Add(totalStat);
                subGroup.Stats.Add(dropPctStat);
                dropGroup.AddGroup(subGroup);
            }
            subGroup.Stats.GetStat("Total").Value += 1;
        }
    }
}