// FFXIVAPP.Client
// Player.Stats.DamageTaken.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Linq;
using FFXIVAPP.Client.Plugins.Parse.Helpers;
using FFXIVAPP.Client.Plugins.Parse.Models.Stats;

#endregion

namespace FFXIVAPP.Client.Plugins.Parse.Models.StatGroups
{
    public partial class Player
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
            var damageGroup = GetGroup("DamageTakenByMonsters");
            StatGroup subMonsterGroup;
            if (!damageGroup.TryGetGroup(line.Source, out subMonsterGroup))
            {
                subMonsterGroup = new StatGroup(line.Source);
                subMonsterGroup.Stats.AddStats(DamageTakenStatList(null));
                damageGroup.AddGroup(subMonsterGroup);
            }
            var abilities = subMonsterGroup.GetGroup("DamageTakenByMonstersByAction");
            StatGroup subMonsterAbilityGroup;
            if (!abilities.TryGetGroup(line.Action, out subMonsterAbilityGroup))
            {
                subMonsterAbilityGroup = new StatGroup(line.Action);
                subMonsterAbilityGroup.Stats.AddStats(DamageTakenStatList(subMonsterGroup, true));
                abilities.AddGroup(subMonsterAbilityGroup);
            }
            Stats.IncrementStat("TotalDamageTakenActionsUsed");
            subAbilityGroup.Stats.IncrementStat("TotalDamageTakenActionsUsed");
            subMonsterGroup.Stats.IncrementStat("TotalDamageTakenActionsUsed");
            subMonsterAbilityGroup.Stats.IncrementStat("TotalDamageTakenActionsUsed");
            if (line.Hit)
            {
                Stats.IncrementStat("TotalOverallDamageTaken", line.Amount);
                subAbilityGroup.Stats.IncrementStat("TotalOverallDamageTaken", line.Amount);
                subMonsterGroup.Stats.IncrementStat("TotalOverallDamageTaken", line.Amount);
                subMonsterAbilityGroup.Stats.IncrementStat("TotalOverallDamageTaken", line.Amount);
                if (line.Crit)
                {
                    Stats.IncrementStat("DamageTakenCritHit");
                    subAbilityGroup.Stats.IncrementStat("DamageTakenCritHit");
                    subMonsterGroup.Stats.IncrementStat("DamageTakenCritHit");
                    subMonsterAbilityGroup.Stats.IncrementStat("DamageTakenCritHit");
                    Stats.IncrementStat("CriticalDamageTaken", line.Amount);
                    subAbilityGroup.Stats.IncrementStat("CriticalDamageTaken", line.Amount);
                    subMonsterGroup.Stats.IncrementStat("CriticalDamageTaken", line.Amount);
                    subMonsterAbilityGroup.Stats.IncrementStat("CriticalDamageTaken", line.Amount);
                    if (line.Modifier != 0)
                    {
                        var mod = ParseHelper.GetBonusDamage(line.Amount, line.Modifier);
                        var modStat = "DamageTakenCritMod";
                        Stats.IncrementStat(modStat, mod);
                        subAbilityGroup.Stats.IncrementStat(modStat, mod);
                        subMonsterGroup.Stats.IncrementStat(modStat, mod);
                        subMonsterAbilityGroup.Stats.IncrementStat(modStat, mod);
                    }
                }
                else
                {
                    Stats.IncrementStat("DamageTakenRegHit");
                    subAbilityGroup.Stats.IncrementStat("DamageTakenRegHit");
                    subMonsterGroup.Stats.IncrementStat("DamageTakenRegHit");
                    subMonsterAbilityGroup.Stats.IncrementStat("DamageTakenRegHit");
                    Stats.IncrementStat("RegularDamageTaken", line.Amount);
                    subAbilityGroup.Stats.IncrementStat("RegularDamageTaken", line.Amount);
                    subMonsterGroup.Stats.IncrementStat("RegularDamageTaken", line.Amount);
                    subMonsterAbilityGroup.Stats.IncrementStat("RegularDamageTaken", line.Amount);
                    if (line.Modifier != 0)
                    {
                        var mod = ParseHelper.GetBonusDamage(line.Amount, line.Modifier);
                        var modStat = "DamageTakenRegMod";
                        Stats.IncrementStat(modStat, mod);
                        subAbilityGroup.Stats.IncrementStat(modStat, mod);
                        subMonsterGroup.Stats.IncrementStat(modStat, mod);
                        subMonsterAbilityGroup.Stats.IncrementStat(modStat, mod);
                    }
                }
            }
            else
            {
                Stats.IncrementStat("DamageTakenRegMiss");
                subAbilityGroup.Stats.IncrementStat("DamageTakenRegMiss");
                subMonsterGroup.Stats.IncrementStat("DamageTakenRegMiss");
                subMonsterAbilityGroup.Stats.IncrementStat("DamageTakenRegMiss");
            }
            foreach (var stat in fields.Where(stat => LD.Contains(stat.Name))
                                       .Where(stat => Equals(stat.GetValue(line), true)))
            {
                var regStat = String.Format("DamageTaken{0}", stat.Name);
                Stats.IncrementStat(regStat);
                subAbilityGroup.Stats.IncrementStat(regStat);
                subMonsterGroup.Stats.IncrementStat(regStat);
                subMonsterAbilityGroup.Stats.IncrementStat(regStat);
                if (line.Modifier == 0)
                {
                    continue;
                }
                var mod = ParseHelper.GetBonusDamage(line.Amount, line.Modifier);
                var modStat = String.Format("DamageTaken{0}Mod", stat.Name);
                Stats.IncrementStat(modStat, mod);
                subAbilityGroup.Stats.IncrementStat(modStat, mod);
                subMonsterGroup.Stats.IncrementStat(modStat, mod);
                subMonsterAbilityGroup.Stats.IncrementStat(modStat, mod);
            }
        }
    }
}
