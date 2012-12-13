// FFXIVAPP
// StatGroupMonster.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FFXIVAPP.Classes;
using FFXIVAPP.Classes.Helpers;
using FFXIVAPP.Models;
using NLog;

#endregion

namespace FFXIVAPP.Stats
{
    public class StatGroupMonster : StatGroup
    {
        private static readonly IList<string> LD = new[] {"Counter", "Block", "Parry", "Resist", "Evade"};
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        public StatGroupMonster(string name) : base(name)
        {
            InitStats();
        }

        private TotalStat TotalDrops { get; set; }
        private CounterStat NumKilled { get; set; }

        /// <summary>
        /// </summary>
        private void InitStats()
        {
            Stats.AddStats(TotalStatList());
        }

        /// <summary>
        /// </summary>
        /// <param name="f"> </param>
        public void AddKillStats(Fight f)
        {
            if (f.MobName != Name)
            {
                Logger.Debug("KillEvent : Got request to add kill stats for {0}, but my name is {1}!", f.MobName, Name);
                return;
            }
            if (f.MobName == "")
            {
                Logger.Debug("KillEvent : Got request to add kill stats for {0}, but no name!", f.MobName);
                return;
            }
            FFXIV.LastKilled = f.MobName;
            Stats.GetStat("Killed").Value++;
            Stats.GetStat("Avg HP").Value = Stats.GetStat("Total").Value / Stats.GetStat("Killed").Value;
        }

        /// <summary>
        /// </summary>
        /// <param name="d"> </param>
        public void GetSetPlayer(ParseHelper.LineData d)
        {
            var fields = d.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
            var abilityGroup = GetGroup("Abilities");
            StatGroup subGroup;
            if (!abilityGroup.TryGetGroup(d.Action, out subGroup))
            {
                subGroup = new StatGroup(d.Action);
                subGroup.Stats.AddStats(DamageStatList());
                abilityGroup.AddGroup(subGroup);
            }
            subGroup.Stats.GetStat("Total").Value += d.Amount;
            Stats.GetStat("Total").Value += d.Amount;
            subGroup.Stats.GetStat("Used").Value += 1;
            if (d.Crit)
            {
                subGroup.Stats.GetStat("C Hit").Value += 1;
                subGroup.Stats.GetStat("Crit").Value += d.Amount;
                Stats.GetStat("C Hit").Value += 1;
                Stats.GetStat("Crit").Value += d.Amount;
            }
            else
            {
                subGroup.Stats.GetStat("Hit").Value += 1;
                subGroup.Stats.GetStat("Reg").Value += d.Amount;
                Stats.GetStat("Hit").Value += 1;
                Stats.GetStat("Reg").Value += d.Amount;
            }
            foreach (var s in fields.SelectMany(f => LD.Where(t => t == f.Name && Equals(f.GetValue(d), true))))
            {
                subGroup.Stats.GetStat(s).Value += 1;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
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
        /// </summary>
        /// <returns> </returns>
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
        /// </summary>
        /// <returns> </returns>
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
            return new Stat<Decimal>[] {totalStat, regularTotalStat, critTotalStat, hitStat, chitStat, minStat, cminStat, maxStat, cmaxStat, avgDamageStat, avgCDamageStat, totalAttempt, counterStat, counterPctStat, blockStat, blockPctStat, parryStat, parryPctStat, resistStat, resistPctStat, evadeStat, evadePctStat};
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        private Stat<Decimal>[] DropStatList()
        {
            var totalStat = new CounterStat("Total");
            TotalDrops.AddDependency(totalStat);
            var dropPctStat = new PercentStat("Drop %", totalStat, NumKilled);

            return new Stat<Decimal>[] {totalStat, dropPctStat};
        }
    }
}
