// FFXIVAPP.Plugin.Parse
// Monster.Stats.DamageTaken.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Linq;
using FFXIVAPP.Plugin.Parse.Helpers;
using FFXIVAPP.Plugin.Parse.Models.Stats;

#endregion

namespace FFXIVAPP.Plugin.Parse.Models.StatGroups
{
    public partial class Monster
    {
        /// <summary>
        /// </summary>
        /// <param name="line"> </param>
        public void SetDamageTaken(Line line)
        {
            LineHistory.Add(new LineHistory(line));
            var fields = line.GetType()
                             .GetProperties();
            var abilityGroup = GetGroup("DamageTakenByAction");
            StatGroup subAbilityGroup;
            if (!abilityGroup.TryGetGroup(line.Action, out subAbilityGroup))
            {
                subAbilityGroup = new StatGroup(line.Action);
                subAbilityGroup.Stats.AddStats(DamageTakenStatList(null));
                abilityGroup.AddGroup(subAbilityGroup);
            }
            var damageGroup = GetGroup("DamageTakenByPlayers");
            StatGroup subPlayerGroup;
            if (!damageGroup.TryGetGroup(line.Source, out subPlayerGroup))
            {
                subPlayerGroup = new StatGroup(line.Source);
                subPlayerGroup.Stats.AddStats(DamageTakenStatList(null));
                damageGroup.AddGroup(subPlayerGroup);
            }
            var abilities = subPlayerGroup.GetGroup("DamageTakenByPlayersByAction");
            StatGroup subPlayerAbilityGroup;
            if (!abilities.TryGetGroup(line.Action, out subPlayerAbilityGroup))
            {
                subPlayerAbilityGroup = new StatGroup(line.Action);
                subPlayerAbilityGroup.Stats.AddStats(DamageTakenStatList(subPlayerGroup, true));
                abilities.AddGroup(subPlayerAbilityGroup);
            }
            Stats.IncrementStat("TotalDamageTakenActionsUsed");
            subAbilityGroup.Stats.IncrementStat("TotalDamageTakenActionsUsed");
            subPlayerGroup.Stats.IncrementStat("TotalDamageTakenActionsUsed");
            subPlayerAbilityGroup.Stats.IncrementStat("TotalDamageTakenActionsUsed");
            if (line.Hit)
            {
                Stats.IncrementStat("TotalOverallDamageTaken", line.Amount);
                subAbilityGroup.Stats.IncrementStat("TotalOverallDamageTaken", line.Amount);
                subPlayerGroup.Stats.IncrementStat("TotalOverallDamageTaken", line.Amount);
                subPlayerAbilityGroup.Stats.IncrementStat("TotalOverallDamageTaken", line.Amount);
                if (line.Crit)
                {
                    Stats.IncrementStat("DamageTakenCritHit");
                    subAbilityGroup.Stats.IncrementStat("DamageTakenCritHit");
                    subPlayerGroup.Stats.IncrementStat("DamageTakenCritHit");
                    subPlayerAbilityGroup.Stats.IncrementStat("DamageTakenCritHit");
                    Stats.IncrementStat("CriticalDamageTaken", line.Amount);
                    subAbilityGroup.Stats.IncrementStat("CriticalDamageTaken", line.Amount);
                    subPlayerGroup.Stats.IncrementStat("CriticalDamageTaken", line.Amount);
                    subPlayerAbilityGroup.Stats.IncrementStat("CriticalDamageTaken", line.Amount);
                    if (line.Modifier != 0)
                    {
                        var mod = ParseHelper.GetBonusDamage(line.Amount, line.Modifier);
                        var modStat = "DamageTakenCritMod";
                        Stats.IncrementStat(modStat, mod);
                        subAbilityGroup.Stats.IncrementStat(modStat, mod);
                        subPlayerGroup.Stats.IncrementStat(modStat, mod);
                        subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
                    }
                }
                else
                {
                    Stats.IncrementStat("DamageTakenRegHit");
                    subAbilityGroup.Stats.IncrementStat("DamageTakenRegHit");
                    subPlayerGroup.Stats.IncrementStat("DamageTakenRegHit");
                    subPlayerAbilityGroup.Stats.IncrementStat("DamageTakenRegHit");
                    Stats.IncrementStat("RegularDamageTaken", line.Amount);
                    subAbilityGroup.Stats.IncrementStat("RegularDamageTaken", line.Amount);
                    subPlayerGroup.Stats.IncrementStat("RegularDamageTaken", line.Amount);
                    subPlayerAbilityGroup.Stats.IncrementStat("RegularDamageTaken", line.Amount);
                    if (line.Modifier != 0)
                    {
                        var mod = ParseHelper.GetBonusDamage(line.Amount, line.Modifier);
                        var modStat = "DamageTakenRegMod";
                        Stats.IncrementStat(modStat, mod);
                        subAbilityGroup.Stats.IncrementStat(modStat, mod);
                        subPlayerGroup.Stats.IncrementStat(modStat, mod);
                        subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
                    }
                }
            }
            else
            {
                Stats.IncrementStat("DamageTakenRegMiss");
                subAbilityGroup.Stats.IncrementStat("DamageTakenRegMiss");
                subPlayerGroup.Stats.IncrementStat("DamageTakenRegMiss");
                subPlayerAbilityGroup.Stats.IncrementStat("DamageTakenRegMiss");
            }
            foreach (var stat in fields.Where(stat => LD.Contains(stat.Name))
                                       .Where(stat => Equals(stat.GetValue(line), true)))
            {
                var regStat = String.Format("DamageTaken{0}", stat.Name);
                Stats.IncrementStat(regStat);
                subAbilityGroup.Stats.IncrementStat(regStat);
                subPlayerGroup.Stats.IncrementStat(regStat);
                subPlayerAbilityGroup.Stats.IncrementStat(regStat);
                if (line.Modifier == 0)
                {
                    continue;
                }
                var mod = ParseHelper.GetBonusDamage(line.Amount, line.Modifier);
                var modStat = String.Format("DamageTaken{0}Mod", stat.Name);
                Stats.IncrementStat(modStat, mod);
                subAbilityGroup.Stats.IncrementStat(modStat, mod);
                subPlayerGroup.Stats.IncrementStat(modStat, mod);
                subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
            }
        }
    }
}
