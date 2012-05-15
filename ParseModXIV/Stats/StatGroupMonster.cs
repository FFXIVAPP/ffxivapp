// ParseModXIV
// StatGroupMonster.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using NLog;
using ParseModXIV.Classes;
using ParseModXIV.Model;

namespace ParseModXIV.Stats
{
    public class StatGroupMonster : StatGroup
    {
        private TotalStat TotalDrops { get; set; }
        private CounterStat NumKilled { get; set; }
        private new static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public StatGroupMonster(string name) : base(name)
        {
            InitStats();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitStats()
        {
            Stats.AddStats(TotalStatList());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        public void AddKillStats(Fight f)
        {
            if (f.MobName != Name)
            {
                Logger.Trace("KillEvent : Got request to add kill stats for {0}, but my name is {1}!", f.MobName, Name);
                return;
            }
            if (f.MobName == "")
            {
                Logger.Trace("KillEvent : Got request to add kill stats for {0}, but no name!", f.MobName);
                return;
            }
            ParseMod.LastKilled = f.MobName;
            Stats.GetStat("Killed").Value++;
            Stats.GetStat("Avg HP").Value = Stats.GetStat("Total").Value/Stats.GetStat("Killed").Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ability"></param>
        /// <param name="amount"></param>
        /// <param name="resisted"></param>
        /// <param name="evaded"></param>
        /// <param name="critical"></param>
        public void AddAbilityStats(string ability, decimal amount, bool resisted, bool evaded, bool critical)
        {
            var abilityGroup = GetGroup("Abilities");
            StatGroup subGroup;
            if (!abilityGroup.TryGetGroup(ability, out subGroup))
            {
                subGroup = new StatGroup(ability);
                subGroup.Stats.AddStats(DamageStatList());
                abilityGroup.AddGroup(subGroup);
            }
            subGroup.Stats.GetStat("Total").Value += amount;
            Stats.GetStat("Total").Value += amount;
            subGroup.Stats.GetStat("Used").Value += 1;
            if (critical)
            {
                subGroup.Stats.GetStat("C Hit").Value += 1;
                subGroup.Stats.GetStat("Crit").Value += amount;
                Stats.GetStat("C Hit").Value += 1;
                Stats.GetStat("Crit").Value += amount;
            }
            else
            {
                subGroup.Stats.GetStat("Hit").Value += 1;
                subGroup.Stats.GetStat("Reg").Value += amount;
                Stats.GetStat("Hit").Value += 1;
                Stats.GetStat("Reg").Value += amount;
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
                subGroup.Stats.AddStats(DropStatList());
                dropGroup.AddGroup(subGroup);
            }
            subGroup.Stats.GetStat("Total").Value += 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Stat<Decimal>[] TotalStatList()
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

            return new Stat<Decimal>[] {TotalDrops, NumKilled, totalStat, avgHp, regularTotalStat, critTotalStat, hitStat, chitStat, minStat, maxStat, cminStat, cmaxStat, avgDamageStat, avgCDamageStat};
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Stat<Decimal>[] DamageStatList()
        {
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

            return new Stat<Decimal>[] {totalStat, regularTotalStat, critTotalStat, hitStat, chitStat, minStat, cminStat, maxStat, cmaxStat, avgDamageStat, avgCDamageStat, totalAttempt, resistStat, evadeStat, resistPct, evadePct};
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Stat<Decimal>[] DropStatList()
        {
            var totalStat = new CounterStat("Total");
            TotalDrops.AddDependency(totalStat);
            var dropPctStat = new PercentStat("Drop %", totalStat, NumKilled);

            return new Stat<Decimal>[] {totalStat, dropPctStat};
        }
    }
}