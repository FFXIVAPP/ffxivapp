// FFXIVAPP.Client
// StatGeneration.cs
// 
// © 2013 Ryan Wilson

using System.Collections.Generic;
using FFXIVAPP.Client.Models.Parse.LinkedStats;
using FFXIVAPP.Client.Models.Parse.Stats;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Models.Parse.StatGroups
{
    [DoNotObfuscate]
    public static class StatGeneration
    {
        #region Stat Generation Methods

        #region Damage

        public static Dictionary<string, Stat<decimal>> DamageStats()
        {
            var stats = new Dictionary<string, Stat<decimal>>();

            stats.Add("TotalOverallDamage", new TotalStat("TotalOverallDamage"));
            stats.Add("RegularDamage", new TotalStat("RegularDamage"));
            stats.Add("CriticalDamage", new TotalStat("CriticalDamage"));
            stats.Add("TotalDamageActionsUsed", new CounterStat("TotalDamageActionsUsed"));
            stats.Add("DPS", new PerSecondAverageStat("DPS", stats["TotalOverallDamage"]));
            stats.Add("DamageRegHit", new TotalStat("DamageRegHit"));
            stats.Add("DamageRegMiss", new TotalStat("DamageRegMiss"));
            stats.Add("DamageRegAccuracy", new AccuracyStat("DamageRegAccuracy", stats["TotalDamageActionsUsed"], stats["DamageRegMiss"]));
            stats.Add("DamageRegLow", new MinStat("DamageRegLow", stats["RegularDamage"]));
            stats.Add("DamageRegHigh", new MaxStat("DamageRegHigh", stats["RegularDamage"]));
            stats.Add("DamageRegAverage", new AverageStat("DamageRegAverage", stats["RegularDamage"]));
            stats.Add("DamageRegMod", new TotalStat("DamageRegMod"));
            stats.Add("DamageRegModAverage", new AverageStat("DamageRegModAverage", stats["DamageRegMod"]));
            stats.Add("DamageCritHit", new TotalStat("DamageCritHit"));
            stats.Add("DamageCritPercent", new PercentStat("DamageCritPercent", stats["DamageCritHit"], stats["TotalDamageActionsUsed"]));
            stats.Add("DamageCritLow", new MinStat("DamageCritLow", stats["CriticalDamage"]));
            stats.Add("DamageCritHigh", new MaxStat("DamageCritHigh", stats["CriticalDamage"]));
            stats.Add("DamageCritAverage", new AverageStat("DamageCritAverage", stats["CriticalDamage"]));
            stats.Add("DamageCritMod", new TotalStat("DamageCritMod"));
            stats.Add("DamageCritModAverage", new AverageStat("DamageCritModAverage", stats["DamageCritMod"]));

            stats.Add("DamageCounter", new CounterStat("DamageCounter"));
            stats.Add("DamageCounterPercent", new PercentStat("DamageCounterPercent", stats["DamageCounter"], stats["TotalDamageActionsUsed"]));
            stats.Add("DamageCounterMod", new TotalStat("DamageCounterMod"));
            stats.Add("DamageCounterModAverage", new AverageStat("DamageCounterModAverage", stats["DamageCounterMod"]));
            stats.Add("DamageBlock", new CounterStat("DamageBlock"));
            stats.Add("DamageBlockPercent", new PercentStat("DamageBlockPercent", stats["DamageBlock"], stats["TotalDamageActionsUsed"]));
            stats.Add("DamageBlockMod", new TotalStat("DamageBlockMod"));
            stats.Add("DamageBlockModAverage", new AverageStat("DamageBlockModAverage", stats["DamageBlockMod"]));
            stats.Add("DamageParry", new CounterStat("DamageParry"));
            stats.Add("DamageParryPercent", new PercentStat("DamageParryPercent", stats["DamageParry"], stats["TotalDamageActionsUsed"]));
            stats.Add("DamageParryMod", new TotalStat("DamageParryMod"));
            stats.Add("DamageParryModAverage", new AverageStat("DamageParryModAverage", stats["DamageParryMod"]));
            stats.Add("DamageResist", new CounterStat("DamageResist"));
            stats.Add("DamageResistPercent", new PercentStat("DamageResistPercent", stats["DamageResist"], stats["TotalDamageActionsUsed"]));
            stats.Add("DamageResistMod", new TotalStat("DamageResistMod"));
            stats.Add("DamageResistModAverage", new AverageStat("DamageResistModAverage", stats["DamageResistMod"]));
            stats.Add("DamageEvade", new CounterStat("DamageEvade"));
            stats.Add("DamageEvadePercent", new PercentStat("DamageEvadePercent", stats["DamageEvade"], stats["TotalDamageActionsUsed"]));
            stats.Add("DamageEvadeMod", new TotalStat("DamageEvadeMod"));
            stats.Add("DamageEvadeModAverage", new AverageStat("DamageEvadeModAverage", stats["DamageEvadeMod"]));

            return stats;
        }

        public static Dictionary<string, Stat<decimal>> DamageOverTimeStats()
        {
            var stats = new Dictionary<string, Stat<decimal>>();

            stats.Add("TotalOverallDamageOverTime", new TotalStat("TotalOverallDamageOverTime"));
            stats.Add("RegularDamageOverTime", new TotalStat("RegularDamageOverTime"));
            stats.Add("CriticalDamageOverTime", new TotalStat("CriticalDamageOverTime"));
            stats.Add("TotalDamageOverTimeActionsUsed", new CounterStat("TotalDamageOverTimeActionsUsed"));
            stats.Add("DOTPS", new PerSecondAverageStat("DOTPS", stats["TotalOverallDamageOverTime"]));
            stats.Add("DamageOverTimeRegHit", new TotalStat("DamageOverTimeRegHit"));
            stats.Add("DamageOverTimeRegMiss", new TotalStat("DamageOverTimeRegMiss"));
            stats.Add("DamageOverTimeRegAccuracy", new AccuracyStat("DamageOverTimeRegAccuracy", stats["TotalDamageOverTimeActionsUsed"], stats["DamageOverTimeRegMiss"]));
            stats.Add("DamageOverTimeRegLow", new MinStat("DamageOverTimeRegLow", stats["RegularDamageOverTime"]));
            stats.Add("DamageOverTimeRegHigh", new MaxStat("DamageOverTimeRegHigh", stats["RegularDamageOverTime"]));
            stats.Add("DamageOverTimeRegAverage", new AverageStat("DamageOverTimeRegAverage", stats["RegularDamageOverTime"]));
            stats.Add("DamageOverTimeRegMod", new TotalStat("DamageOverTimeRegMod"));
            stats.Add("DamageOverTimeRegModAverage", new AverageStat("DamageOverTimeRegModAverage", stats["DamageOverTimeRegMod"]));
            stats.Add("DamageOverTimeCritHit", new TotalStat("DamageOverTimeCritHit"));
            stats.Add("DamageOverTimeCritPercent", new PercentStat("DamageOverTimeCritPercent", stats["DamageOverTimeCritHit"], stats["TotalDamageOverTimeActionsUsed"]));
            stats.Add("DamageOverTimeCritLow", new MinStat("DamageOverTimeCritLow", stats["CriticalDamageOverTime"]));
            stats.Add("DamageOverTimeCritHigh", new MaxStat("DamageOverTimeCritHigh", stats["CriticalDamageOverTime"]));
            stats.Add("DamageOverTimeCritAverage", new AverageStat("DamageOverTimeCritAverage", stats["CriticalDamageOverTime"]));
            stats.Add("DamageOverTimeCritMod", new TotalStat("DamageOverTimeCritMod"));
            stats.Add("DamageOverTimeCritModAverage", new AverageStat("DamageOverTimeCritModAverage", stats["DamageOverTimeCritMod"]));

            return stats;
        }

        #endregion

        #region Healing

        public static Dictionary<string, Stat<decimal>> HealingStats()
        {
            var stats = new Dictionary<string, Stat<decimal>>();

            stats.Add("TotalOverallHealing", new TotalStat("TotalOverallHealing"));
            stats.Add("RegularHealing", new TotalStat("RegularHealing"));
            stats.Add("CriticalHealing", new TotalStat("CriticalHealing"));
            stats.Add("TotalHealingActionsUsed", new CounterStat("TotalHealingActionsUsed"));
            stats.Add("HPS", new PerSecondAverageStat("HPS", stats["TotalOverallHealing"]));
            stats.Add("HealingRegHit", new TotalStat("HealingRegHit"));
            stats.Add("HealingRegLow", new MinStat("HealingRegLow", stats["RegularHealing"]));
            stats.Add("HealingRegHigh", new MaxStat("HealingRegHigh", stats["RegularHealing"]));
            stats.Add("HealingRegAverage", new AverageStat("HealingRegAverage", stats["RegularHealing"]));
            stats.Add("HealingRegMod", new TotalStat("HealingRegMod"));
            stats.Add("HealingRegModAverage", new AverageStat("HealingRegModAverage", stats["HealingRegMod"]));
            stats.Add("HealingCritHit", new TotalStat("HealingCritHit"));
            stats.Add("HealingCritPercent", new PercentStat("HealingCritPercent", stats["HealingCritHit"], stats["TotalHealingActionsUsed"]));
            stats.Add("HealingCritLow", new MinStat("HealingCritLow", stats["CriticalHealing"]));
            stats.Add("HealingCritHigh", new MaxStat("HealingCritHigh", stats["CriticalHealing"]));
            stats.Add("HealingCritAverage", new AverageStat("HealingCritAverage", stats["CriticalHealing"]));
            stats.Add("HealingCritMod", new TotalStat("HealingCritMod"));
            stats.Add("HealingCritModAverage", new AverageStat("HealingCritModAverage", stats["HealingCritMod"]));

            return stats;
        }

        public static Dictionary<string, Stat<decimal>> HealingOverHealingStats()
        {
            var stats = new Dictionary<string, Stat<decimal>>();

            stats.Add("TotalOverallHealingOverHealing", new TotalStat("TotalOverallHealingOverHealing"));
            stats.Add("RegularHealingOverHealing", new TotalStat("RegularHealingOverHealing"));
            stats.Add("CriticalHealingOverHealing", new TotalStat("CriticalHealingOverHealing"));
            stats.Add("TotalHealingOverHealingActionsUsed", new CounterStat("TotalHealingOverHealingActionsUsed"));
            stats.Add("HOHPS", new PerSecondAverageStat("HOHPS", stats["TotalOverallHealingOverHealing"]));
            stats.Add("HealingOverHealingRegHit", new TotalStat("HealingOverHealingRegHit"));
            stats.Add("HealingOverHealingRegLow", new MinStat("HealingOverHealingRegLow", stats["RegularHealingOverHealing"]));
            stats.Add("HealingOverHealingRegHigh", new MaxStat("HealingOverHealingRegHigh", stats["RegularHealingOverHealing"]));
            stats.Add("HealingOverHealingRegAverage", new AverageStat("HealingOverHealingRegAverage", stats["RegularHealingOverHealing"]));
            stats.Add("HealingOverHealingRegMod", new TotalStat("HealingOverHealingRegMod"));
            stats.Add("HealingOverHealingRegModAverage", new AverageStat("HealingOverHealingRegModAverage", stats["HealingOverHealingRegMod"]));
            stats.Add("HealingOverHealingCritHit", new TotalStat("HealingOverHealingCritHit"));
            stats.Add("HealingOverHealingCritPercent", new PercentStat("HealingOverHealingCritPercent", stats["HealingOverHealingCritHit"], stats["TotalHealingOverHealingActionsUsed"]));
            stats.Add("HealingOverHealingCritLow", new MinStat("HealingOverHealingCritLow", stats["CriticalHealingOverHealing"]));
            stats.Add("HealingOverHealingCritHigh", new MaxStat("HealingOverHealingCritHigh", stats["CriticalHealingOverHealing"]));
            stats.Add("HealingOverHealingCritAverage", new AverageStat("HealingOverHealingCritAverage", stats["CriticalHealingOverHealing"]));
            stats.Add("HealingOverHealingCritMod", new TotalStat("HealingOverHealingCritMod"));
            stats.Add("HealingOverHealingCritModAverage", new AverageStat("HealingOverHealingCritModAverage", stats["HealingOverHealingCritMod"]));

            return stats;
        }

        public static Dictionary<string, Stat<decimal>> HealingOverTimeStats()
        {
            var stats = new Dictionary<string, Stat<decimal>>();

            stats.Add("TotalOverallHealingOverTime", new TotalStat("TotalOverallHealingOverTime"));
            stats.Add("RegularHealingOverTime", new TotalStat("RegularHealingOverTime"));
            stats.Add("CriticalHealingOverTime", new TotalStat("CriticalHealingOverTime"));
            stats.Add("TotalHealingOverTimeActionsUsed", new CounterStat("TotalHealingOverTimeActionsUsed"));
            stats.Add("HOTPS", new PerSecondAverageStat("HOTPS", stats["TotalOverallHealingOverTime"]));
            stats.Add("HealingOverTimeRegHit", new TotalStat("HealingOverTimeRegHit"));
            stats.Add("HealingOverTimeRegLow", new MinStat("HealingOverTimeRegLow", stats["RegularHealingOverTime"]));
            stats.Add("HealingOverTimeRegHigh", new MaxStat("HealingOverTimeRegHigh", stats["RegularHealingOverTime"]));
            stats.Add("HealingOverTimeRegAverage", new AverageStat("HealingOverTimeRegAverage", stats["RegularHealingOverTime"]));
            stats.Add("HealingOverTimeRegMod", new TotalStat("HealingOverTimeRegMod"));
            stats.Add("HealingOverTimeRegModAverage", new AverageStat("HealingOverTimeRegModAverage", stats["HealingOverTimeRegMod"]));
            stats.Add("HealingOverTimeCritHit", new TotalStat("HealingOverTimeCritHit"));
            stats.Add("HealingOverTimeCritPercent", new PercentStat("HealingOverTimeCritPercent", stats["HealingOverTimeCritHit"], stats["TotalHealingOverTimeActionsUsed"]));
            stats.Add("HealingOverTimeCritLow", new MinStat("HealingOverTimeCritLow", stats["CriticalHealingOverTime"]));
            stats.Add("HealingOverTimeCritHigh", new MaxStat("HealingOverTimeCritHigh", stats["CriticalHealingOverTime"]));
            stats.Add("HealingOverTimeCritAverage", new AverageStat("HealingOverTimeCritAverage", stats["CriticalHealingOverTime"]));
            stats.Add("HealingOverTimeCritMod", new TotalStat("HealingOverTimeCritMod"));
            stats.Add("HealingOverTimeCritModAverage", new AverageStat("HealingOverTimeCritModAverage", stats["HealingOverTimeCritMod"]));

            return stats;
        }

        public static Dictionary<string, Stat<decimal>> HealingMitigatedStats()
        {
            var stats = new Dictionary<string, Stat<decimal>>();

            stats.Add("TotalOverallHealingMitigated", new TotalStat("TotalOverallHealingMitigated"));
            stats.Add("RegularHealingMitigated", new TotalStat("RegularHealingMitigated"));
            stats.Add("CriticalHealingMitigated", new TotalStat("CriticalHealingMitigated"));
            stats.Add("TotalHealingMitigatedActionsUsed", new CounterStat("TotalHealingMitigatedActionsUsed"));
            stats.Add("HMPS", new PerSecondAverageStat("HMPS", stats["TotalOverallHealingMitigated"]));
            stats.Add("HealingMitigatedRegHit", new TotalStat("HealingMitigatedRegHit"));
            stats.Add("HealingMitigatedRegLow", new MinStat("HealingMitigatedRegLow", stats["RegularHealingMitigated"]));
            stats.Add("HealingMitigatedRegHigh", new MaxStat("HealingMitigatedRegHigh", stats["RegularHealingMitigated"]));
            stats.Add("HealingMitigatedRegAverage", new AverageStat("HealingMitigatedRegAverage", stats["RegularHealingMitigated"]));
            stats.Add("HealingMitigatedRegMod", new TotalStat("HealingMitigatedRegMod"));
            stats.Add("HealingMitigatedRegModAverage", new AverageStat("HealingMitigatedRegModAverage", stats["HealingMitigatedRegMod"]));
            stats.Add("HealingMitigatedCritHit", new TotalStat("HealingMitigatedCritHit"));
            stats.Add("HealingMitigatedCritPercent", new PercentStat("HealingMitigatedCritPercent", stats["HealingMitigatedCritHit"], stats["TotalHealingMitigatedActionsUsed"]));
            stats.Add("HealingMitigatedCritLow", new MinStat("HealingMitigatedCritLow", stats["CriticalHealingMitigated"]));
            stats.Add("HealingMitigatedCritHigh", new MaxStat("HealingMitigatedCritHigh", stats["CriticalHealingMitigated"]));
            stats.Add("HealingMitigatedCritAverage", new AverageStat("HealingMitigatedCritAverage", stats["CriticalHealingMitigated"]));
            stats.Add("HealingMitigatedCritMod", new TotalStat("HealingMitigatedCritMod"));
            stats.Add("HealingMitigatedCritModAverage", new AverageStat("HealingMitigatedCritModAverage", stats["HealingMitigatedCritMod"]));

            return stats;
        }

        #endregion

        #region Damage Taken

        public static Dictionary<string, Stat<decimal>> DamageTakenStats()
        {
            var stats = new Dictionary<string, Stat<decimal>>();

            stats.Add("TotalOverallDamageTaken", new TotalStat("TotalOverallDamageTaken"));
            stats.Add("RegularDamageTaken", new TotalStat("RegularDamageTaken"));
            stats.Add("CriticalDamageTaken", new TotalStat("CriticalDamageTaken"));
            stats.Add("TotalDamageTakenActionsUsed", new CounterStat("TotalDamageTakenActionsUsed"));
            stats.Add("DTPS", new PerSecondAverageStat("DTPS", stats["TotalOverallDamageTaken"]));
            stats.Add("DamageTakenRegHit", new TotalStat("DamageTakenRegHit"));
            stats.Add("DamageTakenRegMiss", new TotalStat("DamageTakenRegMiss"));
            stats.Add("DamageTakenRegAccuracy", new AccuracyStat("DamageTakenRegAccuracy", stats["TotalDamageTakenActionsUsed"], stats["DamageTakenRegMiss"]));
            stats.Add("DamageTakenRegLow", new MinStat("DamageTakenRegLow", stats["RegularDamageTaken"]));
            stats.Add("DamageTakenRegHigh", new MaxStat("DamageTakenRegHigh", stats["RegularDamageTaken"]));
            stats.Add("DamageTakenRegAverage", new AverageStat("DamageTakenRegAverage", stats["RegularDamageTaken"]));
            stats.Add("DamageTakenRegMod", new TotalStat("DamageTakenRegMod"));
            stats.Add("DamageTakenRegModAverage", new AverageStat("DamageTakenRegModAverage", stats["DamageTakenRegMod"]));
            stats.Add("DamageTakenCritHit", new TotalStat("DamageTakenCritHit"));
            stats.Add("DamageTakenCritPercent", new PercentStat("DamageTakenCritPercent", stats["DamageTakenCritHit"], stats["TotalDamageTakenActionsUsed"]));
            stats.Add("DamageTakenCritLow", new MinStat("DamageTakenCritLow", stats["CriticalDamageTaken"]));
            stats.Add("DamageTakenCritHigh", new MaxStat("DamageTakenCritHigh", stats["CriticalDamageTaken"]));
            stats.Add("DamageTakenCritAverage", new AverageStat("DamageTakenCritAverage", stats["CriticalDamageTaken"]));
            stats.Add("DamageTakenCritMod", new TotalStat("DamageTakenCritMod"));
            stats.Add("DamageTakenCritModAverage", new AverageStat("DamageTakenCritModAverage", stats["DamageTakenCritMod"]));

            stats.Add("DamageTakenCounter", new CounterStat("DamageTakenCounter"));
            stats.Add("DamageTakenCounterPercent", new PercentStat("DamageTakenCounterPercent", stats["DamageTakenCounter"], stats["TotalDamageTakenActionsUsed"]));
            stats.Add("DamageTakenCounterMod", new TotalStat("DamageTakenCounterMod"));
            stats.Add("DamageTakenCounterModAverage", new AverageStat("DamageTakenCounterModAverage", stats["DamageTakenCounterMod"]));
            stats.Add("DamageTakenBlock", new CounterStat("DamageTakenBlock"));
            stats.Add("DamageTakenBlockPercent", new PercentStat("DamageTakenBlockPercent", stats["DamageTakenBlock"], stats["TotalDamageTakenActionsUsed"]));
            stats.Add("DamageTakenBlockMod", new TotalStat("DamageTakenBlockMod"));
            stats.Add("DamageTakenBlockModAverage", new AverageStat("DamageTakenBlockModAverage", stats["DamageTakenBlockMod"]));
            stats.Add("DamageTakenParry", new CounterStat("DamageTakenParry"));
            stats.Add("DamageTakenParryPercent", new PercentStat("DamageTakenParryPercent", stats["DamageTakenParry"], stats["TotalDamageTakenActionsUsed"]));
            stats.Add("DamageTakenParryMod", new TotalStat("DamageTakenParryMod"));
            stats.Add("DamageTakenParryModAverage", new AverageStat("DamageTakenParryModAverage", stats["DamageTakenParryMod"]));
            stats.Add("DamageTakenResist", new CounterStat("DamageTakenResist"));
            stats.Add("DamageTakenResistPercent", new PercentStat("DamageTakenResistPercent", stats["DamageTakenResist"], stats["TotalDamageTakenActionsUsed"]));
            stats.Add("DamageTakenResistMod", new TotalStat("DamageTakenResistMod"));
            stats.Add("DamageTakenResistModAverage", new AverageStat("DamageTakenResistModAverage", stats["DamageTakenResistMod"]));
            stats.Add("DamageTakenEvade", new CounterStat("DamageTakenEvade"));
            stats.Add("DamageTakenEvadePercent", new PercentStat("DamageTakenEvadePercent", stats["DamageTakenEvade"], stats["TotalDamageTakenActionsUsed"]));
            stats.Add("DamageTakenEvadeMod", new TotalStat("DamageTakenEvadeMod"));
            stats.Add("DamageTakenEvadeModAverage", new AverageStat("DamageTakenEvadeModAverage", stats["DamageTakenEvadeMod"]));

            return stats;
        }

        public static Dictionary<string, Stat<decimal>> DamageTakenOverTimeStats()
        {
            var stats = new Dictionary<string, Stat<decimal>>();

            stats.Add("TotalOverallDamageTakenOverTime", new TotalStat("TotalOverallDamageTakenOverTime"));
            stats.Add("RegularDamageTakenOverTime", new TotalStat("RegularDamageTakenOverTime"));
            stats.Add("CriticalDamageTakenOverTime", new TotalStat("CriticalDamageTakenOverTime"));
            stats.Add("TotalDamageTakenOverTimeActionsUsed", new CounterStat("TotalDamageTakenOverTimeActionsUsed"));
            stats.Add("DTOTPS", new PerSecondAverageStat("DTOTPS", stats["TotalOverallDamageTakenOverTime"]));
            stats.Add("DamageTakenOverTimeRegHit", new TotalStat("DamageTakenOverTimeRegHit"));
            stats.Add("DamageTakenOverTimeRegMiss", new TotalStat("DamageTakenOverTimeRegMiss"));
            stats.Add("DamageTakenOverTimeRegAccuracy", new AccuracyStat("DamageTakenOverTimeRegAccuracy", stats["TotalDamageTakenOverTimeActionsUsed"], stats["DamageTakenOverTimeRegMiss"]));
            stats.Add("DamageTakenOverTimeRegLow", new MinStat("DamageTakenOverTimeRegLow", stats["RegularDamageTakenOverTime"]));
            stats.Add("DamageTakenOverTimeRegHigh", new MaxStat("DamageTakenOverTimeRegHigh", stats["RegularDamageTakenOverTime"]));
            stats.Add("DamageTakenOverTimeRegAverage", new AverageStat("DamageTakenOverTimeRegAverage", stats["RegularDamageTakenOverTime"]));
            stats.Add("DamageTakenOverTimeRegMod", new TotalStat("DamageTakenOverTimeRegMod"));
            stats.Add("DamageTakenOverTimeRegModAverage", new AverageStat("DamageTakenOverTimeRegModAverage", stats["DamageTakenOverTimeRegMod"]));
            stats.Add("DamageTakenOverTimeCritHit", new TotalStat("DamageTakenOverTimeCritHit"));
            stats.Add("DamageTakenOverTimeCritPercent", new PercentStat("DamageTakenOverTimeCritPercent", stats["DamageTakenOverTimeCritHit"], stats["TotalDamageTakenOverTimeActionsUsed"]));
            stats.Add("DamageTakenOverTimeCritLow", new MinStat("DamageTakenOverTimeCritLow", stats["CriticalDamageTakenOverTime"]));
            stats.Add("DamageTakenOverTimeCritHigh", new MaxStat("DamageTakenOverTimeCritHigh", stats["CriticalDamageTakenOverTime"]));
            stats.Add("DamageTakenOverTimeCritAverage", new AverageStat("DamageTakenOverTimeCritAverage", stats["CriticalDamageTakenOverTime"]));
            stats.Add("DamageTakenOverTimeCritMod", new TotalStat("DamageTakenOverTimeCritMod"));
            stats.Add("DamageTakenOverTimeCritModAverage", new AverageStat("DamageTakenOverTimeCritModAverage", stats["DamageTakenOverTimeCritMod"]));

            return stats;
        }

        #endregion

        #region Buff

        public static Dictionary<string, Stat<decimal>> BuffStats()
        {
            var stats = new Dictionary<string, Stat<decimal>>();

            stats.Add("TotalBuffTime", new TotalStat("TotalBuffTime"));
            stats.Add("TotalBuffHours", new TotalStat("TotalBuffHours"));
            stats.Add("TotalBuffMinutes", new TotalStat("TotalBuffMinutes"));
            stats.Add("TotalBuffSeconds", new TotalStat("TotalBuffSeconds"));

            return stats;
        }

        #endregion

        #region Combined

        public static Dictionary<string, Stat<decimal>> CombinedStats()
        {
            var stats = new Dictionary<string, Stat<decimal>>();

            stats.Add("CombinedTotalOverallDamage", new TotalStat("CombinedTotalOverallDamage"));
            stats.Add("CombinedRegularDamage", new TotalStat("CombinedRegularDamage"));
            stats.Add("CombinedCriticalDamage", new TotalStat("CombinedCriticalDamage"));
            stats.Add("CombinedTotalDamageActionsUsed", new CounterStat("CombinedTotalDamageActionsUsed"));
            stats.Add("CombinedDPS", new PerSecondAverageStat("CombinedDPS", stats["CombinedTotalOverallDamage"]));
            stats.Add("CombinedDamageRegHit", new TotalStat("CombinedDamageRegHit"));
            stats.Add("CombinedDamageRegMiss", new TotalStat("CombinedDamageRegMiss"));
            stats.Add("CombinedDamageRegAccuracy", new AccuracyStat("CombinedDamageRegAccuracy", stats["CombinedTotalDamageActionsUsed"], stats["CombinedDamageRegMiss"]));
            stats.Add("CombinedDamageRegLow", new MinStat("CombinedDamageRegLow", stats["CombinedRegularDamage"]));
            stats.Add("CombinedDamageRegHigh", new MaxStat("CombinedDamageRegHigh", stats["CombinedRegularDamage"]));
            stats.Add("CombinedDamageRegAverage", new AverageStat("CombinedDamageRegAverage", stats["CombinedRegularDamage"]));
            stats.Add("CombinedDamageRegMod", new TotalStat("CombinedDamageRegMod"));
            stats.Add("CombinedDamageRegModAverage", new AverageStat("CombinedDamageRegModAverage", stats["CombinedDamageRegMod"]));
            stats.Add("CombinedDamageCritHit", new TotalStat("CombinedDamageCritHit"));
            stats.Add("CombinedDamageCritPercent", new PercentStat("CombinedDamageCritPercent", stats["CombinedDamageCritHit"], stats["CombinedTotalDamageActionsUsed"]));
            stats.Add("CombinedDamageCritLow", new MinStat("CombinedDamageCritLow", stats["CombinedCriticalDamage"]));
            stats.Add("CombinedDamageCritHigh", new MaxStat("CombinedDamageCritHigh", stats["CombinedCriticalDamage"]));
            stats.Add("CombinedDamageCritAverage", new AverageStat("CombinedDamageCritAverage", stats["CombinedCriticalDamage"]));
            stats.Add("CombinedDamageCritMod", new TotalStat("CombinedDamageCritMod"));
            stats.Add("CombinedDamageCritModAverage", new AverageStat("CombinedDamageCritModAverage", stats["CombinedDamageCritMod"]));

            stats.Add("CombinedDamageCounter", new CounterStat("CombinedDamageCounter"));
            stats.Add("CombinedDamageCounterPercent", new PercentStat("CombinedDamageCounterPercent", stats["CombinedDamageCounter"], stats["CombinedTotalDamageActionsUsed"]));
            stats.Add("CombinedDamageCounterMod", new TotalStat("CombinedDamageCounterMod"));
            stats.Add("CombinedDamageCounterModAverage", new AverageStat("CombinedDamageCounterModAverage", stats["CombinedDamageCounterMod"]));
            stats.Add("CombinedDamageBlock", new CounterStat("CombinedDamageBlock"));
            stats.Add("CombinedDamageBlockPercent", new PercentStat("CombinedDamageBlockPercent", stats["CombinedDamageBlock"], stats["CombinedTotalDamageActionsUsed"]));
            stats.Add("CombinedDamageBlockMod", new TotalStat("CombinedDamageBlockMod"));
            stats.Add("CombinedDamageBlockModAverage", new AverageStat("CombinedDamageBlockModAverage", stats["CombinedDamageBlockMod"]));
            stats.Add("CombinedDamageParry", new CounterStat("CombinedDamageParry"));
            stats.Add("CombinedDamageParryPercent", new PercentStat("CombinedDamageParryPercent", stats["CombinedDamageParry"], stats["CombinedTotalDamageActionsUsed"]));
            stats.Add("CombinedDamageParryMod", new TotalStat("CombinedDamageParryMod"));
            stats.Add("CombinedDamageParryModAverage", new AverageStat("CombinedDamageParryModAverage", stats["CombinedDamageParryMod"]));
            stats.Add("CombinedDamageResist", new CounterStat("CombinedDamageResist"));
            stats.Add("CombinedDamageResistPercent", new PercentStat("CombinedDamageResistPercent", stats["CombinedDamageResist"], stats["CombinedTotalDamageActionsUsed"]));
            stats.Add("CombinedDamageResistMod", new TotalStat("CombinedDamageResistMod"));
            stats.Add("CombinedDamageResistModAverage", new AverageStat("CombinedDamageResistModAverage", stats["CombinedDamageResistMod"]));
            stats.Add("CombinedDamageEvade", new CounterStat("CombinedDamageEvade"));
            stats.Add("CombinedDamageEvadePercent", new PercentStat("CombinedDamageEvadePercent", stats["CombinedDamageEvade"], stats["CombinedTotalDamageActionsUsed"]));
            stats.Add("CombinedDamageEvadeMod", new TotalStat("CombinedDamageEvadeMod"));
            stats.Add("CombinedDamageEvadeModAverage", new AverageStat("CombinedDamageEvadeModAverage", stats["CombinedDamageEvadeMod"]));

            stats.Add("CombinedTotalOverallHealing", new TotalStat("CombinedTotalOverallHealing"));
            stats.Add("CombinedRegularHealing", new TotalStat("CombinedRegularHealing"));
            stats.Add("CombinedCriticalHealing", new TotalStat("CombinedCriticalHealing"));
            stats.Add("CombinedTotalHealingActionsUsed", new CounterStat("CombinedTotalHealingActionsUsed"));
            stats.Add("CombinedHPS", new PerSecondAverageStat("CombinedHPS", stats["CombinedTotalOverallHealing"]));
            stats.Add("CombinedHealingRegHit", new TotalStat("CombinedHealingRegHit"));
            stats.Add("CombinedHealingRegLow", new MinStat("CombinedHealingRegLow", stats["CombinedRegularHealing"]));
            stats.Add("CombinedHealingRegHigh", new MaxStat("CombinedHealingRegHigh", stats["CombinedRegularHealing"]));
            stats.Add("CombinedHealingRegAverage", new AverageStat("CombinedHealingRegAverage", stats["CombinedRegularHealing"]));
            stats.Add("CombinedHealingRegMod", new TotalStat("CombinedHealingRegMod"));
            stats.Add("CombinedHealingRegModAverage", new AverageStat("CombinedHealingRegModAverage", stats["CombinedHealingRegMod"]));
            stats.Add("CombinedHealingCritHit", new TotalStat("CombinedHealingCritHit"));
            stats.Add("CombinedHealingCritPercent", new PercentStat("CombinedHealingCritPercent", stats["CombinedHealingCritHit"], stats["CombinedTotalHealingActionsUsed"]));
            stats.Add("CombinedHealingCritLow", new MinStat("CombinedHealingCritLow", stats["CombinedCriticalHealing"]));
            stats.Add("CombinedHealingCritHigh", new MaxStat("CombinedHealingCritHigh", stats["CombinedCriticalHealing"]));
            stats.Add("CombinedHealingCritAverage", new AverageStat("CombinedHealingCritAverage", stats["CombinedCriticalHealing"]));
            stats.Add("CombinedHealingCritMod", new TotalStat("CombinedHealingCritMod"));
            stats.Add("CombinedHealingCritModAverage", new AverageStat("CombinedHealingCritModAverage", stats["CombinedHealingCritMod"]));

            stats.Add("CombinedTotalOverallDamageTaken", new TotalStat("CombinedTotalOverallDamageTaken"));
            stats.Add("CombinedRegularDamageTaken", new TotalStat("CombinedRegularDamageTaken"));
            stats.Add("CombinedCriticalDamageTaken", new TotalStat("CombinedCriticalDamageTaken"));
            stats.Add("CombinedTotalDamageTakenActionsUsed", new CounterStat("CombinedTotalDamageTakenActionsUsed"));
            stats.Add("CombinedDTPS", new PerSecondAverageStat("CombinedDTPS", stats["CombinedTotalOverallDamageTaken"]));
            stats.Add("CombinedDamageTakenRegHit", new TotalStat("CombinedDamageTakenRegHit"));
            stats.Add("CombinedDamageTakenRegMiss", new TotalStat("CombinedDamageTakenRegMiss"));
            stats.Add("CombinedDamageTakenRegAccuracy", new AccuracyStat("CombinedDamageTakenRegAccuracy", stats["CombinedTotalDamageTakenActionsUsed"], stats["CombinedDamageTakenRegMiss"]));
            stats.Add("CombinedDamageTakenRegLow", new MinStat("CombinedDamageTakenRegLow", stats["CombinedRegularDamageTaken"]));
            stats.Add("CombinedDamageTakenRegHigh", new MaxStat("CombinedDamageTakenRegHigh", stats["CombinedRegularDamageTaken"]));
            stats.Add("CombinedDamageTakenRegAverage", new AverageStat("CombinedDamageTakenRegAverage", stats["CombinedRegularDamageTaken"]));
            stats.Add("CombinedDamageTakenRegMod", new TotalStat("CombinedDamageTakenRegMod"));
            stats.Add("CombinedDamageTakenRegModAverage", new AverageStat("CombinedDamageTakenRegModAverage", stats["CombinedDamageTakenRegMod"]));
            stats.Add("CombinedDamageTakenCritHit", new TotalStat("CombinedDamageTakenCritHit"));
            stats.Add("CombinedDamageTakenCritPercent", new PercentStat("CombinedDamageTakenCritPercent", stats["CombinedDamageTakenCritHit"], stats["CombinedTotalDamageTakenActionsUsed"]));
            stats.Add("CombinedDamageTakenCritLow", new MinStat("CombinedDamageTakenCritLow", stats["CombinedCriticalDamageTaken"]));
            stats.Add("CombinedDamageTakenCritHigh", new MaxStat("CombinedDamageTakenCritHigh", stats["CombinedCriticalDamageTaken"]));
            stats.Add("CombinedDamageTakenCritAverage", new AverageStat("CombinedDamageTakenCritAverage", stats["CombinedCriticalDamageTaken"]));
            stats.Add("CombinedDamageTakenCritMod", new TotalStat("CombinedDamageTakenCritMod"));
            stats.Add("CombinedDamageTakenCritModAverage", new AverageStat("CombinedDamageTakenCritModAverage", stats["CombinedDamageTakenCritMod"]));

            stats.Add("CombinedDamageTakenCounter", new CounterStat("CombinedDamageTakenCounter"));
            stats.Add("CombinedDamageTakenCounterPercent", new PercentStat("CombinedDamageTakenCounterPercent", stats["CombinedDamageTakenCounter"], stats["CombinedTotalDamageTakenActionsUsed"]));
            stats.Add("CombinedDamageTakenCounterMod", new TotalStat("CombinedDamageTakenCounterMod"));
            stats.Add("CombinedDamageTakenCounterModAverage", new AverageStat("CombinedDamageTakenCounterModAverage", stats["CombinedDamageTakenCounterMod"]));
            stats.Add("CombinedDamageTakenBlock", new CounterStat("CombinedDamageTakenBlock"));
            stats.Add("CombinedDamageTakenBlockPercent", new PercentStat("CombinedDamageTakenBlockPercent", stats["CombinedDamageTakenBlock"], stats["CombinedTotalDamageTakenActionsUsed"]));
            stats.Add("CombinedDamageTakenBlockMod", new TotalStat("CombinedDamageTakenBlockMod"));
            stats.Add("CombinedDamageTakenBlockModAverage", new AverageStat("CombinedDamageTakenBlockModAverage", stats["CombinedDamageTakenBlockMod"]));
            stats.Add("CombinedDamageTakenParry", new CounterStat("CombinedDamageTakenParry"));
            stats.Add("CombinedDamageTakenParryPercent", new PercentStat("CombinedDamageTakenParryPercent", stats["CombinedDamageTakenParry"], stats["CombinedTotalDamageTakenActionsUsed"]));
            stats.Add("CombinedDamageTakenParryMod", new TotalStat("CombinedDamageTakenParryMod"));
            stats.Add("CombinedDamageTakenParryModAverage", new AverageStat("CombinedDamageTakenParryModAverage", stats["CombinedDamageTakenParryMod"]));
            stats.Add("CombinedDamageTakenResist", new CounterStat("CombinedDamageTakenResist"));
            stats.Add("CombinedDamageTakenResistPercent", new PercentStat("CombinedDamageTakenResistPercent", stats["CombinedDamageTakenResist"], stats["CombinedTotalDamageTakenActionsUsed"]));
            stats.Add("CombinedDamageTakenResistMod", new TotalStat("CombinedDamageTakenResistMod"));
            stats.Add("CombinedDamageTakenResistModAverage", new AverageStat("CombinedDamageTakenResistModAverage", stats["CombinedDamageTakenResistMod"]));
            stats.Add("CombinedDamageTakenEvade", new CounterStat("CombinedDamageTakenEvade"));
            stats.Add("CombinedDamageTakenEvadePercent", new PercentStat("CombinedDamageTakenEvadePercent", stats["CombinedDamageTakenEvade"], stats["CombinedTotalDamageTakenActionsUsed"]));
            stats.Add("CombinedDamageTakenEvadeMod", new TotalStat("CombinedDamageTakenEvadeMod"));
            stats.Add("CombinedDamageTakenEvadeModAverage", new AverageStat("CombinedDamageTakenEvadeModAverage", stats["CombinedDamageTakenEvadeMod"]));

            return stats;
        }

        #endregion

        #endregion
    }
}
