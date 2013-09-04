// FFXIVAPP.Plugin.Parse
// Monster.Stats.Damage.cs
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
        /// <param name="line"></param>
        public void SetDamage(Line line)
        {
            LineHistory.Add(new LineHistory(line));
            var fields = line.GetType()
                             .GetProperties();
            var abilityGroup = GetGroup("DamageByAction");
            StatGroup subAbilityGroup;
            if (!abilityGroup.TryGetGroup(line.Action, out subAbilityGroup))
            {
                subAbilityGroup = new StatGroup(line.Action);
                subAbilityGroup.Stats.AddStats(DamageStatList(null));
                abilityGroup.AddGroup(subAbilityGroup);
            }
            var playerGroup = GetGroup("DamageToPlayers");
            StatGroup subPlayerGroup;
            if (!playerGroup.TryGetGroup(line.Target, out subPlayerGroup))
            {
                subPlayerGroup = new StatGroup(line.Target);
                subPlayerGroup.Stats.AddStats(DamageStatList(null));
                playerGroup.AddGroup(subPlayerGroup);
            }
            var monsters = subPlayerGroup.GetGroup("DamageToPlayersByAction");
            StatGroup subPlayerAbilityGroup;
            if (!monsters.TryGetGroup(line.Action, out subPlayerAbilityGroup))
            {
                subPlayerAbilityGroup = new StatGroup(line.Action);
                subPlayerAbilityGroup.Stats.AddStats(DamageStatList(subPlayerGroup, true));
                monsters.AddGroup(subPlayerAbilityGroup);
            }
            Stats.IncrementStat("TotalDamageActionsUsed");
            subAbilityGroup.Stats.IncrementStat("TotalDamageActionsUsed");
            subPlayerGroup.Stats.IncrementStat("TotalDamageActionsUsed");
            subPlayerAbilityGroup.Stats.IncrementStat("TotalDamageActionsUsed");
            if (line.Hit)
            {
                LastDamageAmount = line.Crit ? line.Amount > 0 ? ParseHelper.GetOriginalDamage(line.Amount, 50) : 0 : line.Amount;
                if (DamageOverTimeHelper.MonsterActions.ContainsKey(line.Action.ToLower()))
                {
                    SetupDamageOverTimeAction(line);
                }
                Stats.IncrementStat("TotalOverallDamage", line.Amount);
                subAbilityGroup.Stats.IncrementStat("TotalOverallDamage", line.Amount);
                subPlayerGroup.Stats.IncrementStat("TotalOverallDamage", line.Amount);
                subPlayerAbilityGroup.Stats.IncrementStat("TotalOverallDamage", line.Amount);
                if (line.Crit)
                {
                    Stats.IncrementStat("DamageCritHit");
                    subAbilityGroup.Stats.IncrementStat("DamageCritHit");
                    subPlayerGroup.Stats.IncrementStat("DamageCritHit");
                    subPlayerAbilityGroup.Stats.IncrementStat("DamageCritHit");
                    Stats.IncrementStat("CriticalDamage", line.Amount);
                    subAbilityGroup.Stats.IncrementStat("CriticalDamage", line.Amount);
                    subPlayerGroup.Stats.IncrementStat("CriticalDamage", line.Amount);
                    subPlayerAbilityGroup.Stats.IncrementStat("CriticalDamage", line.Amount);
                    if (line.Modifier != 0)
                    {
                        var mod = ParseHelper.GetOriginalDamage(line.Amount, line.Modifier);
                        var modStat = "DamageCritMod";
                        Stats.IncrementStat(modStat, mod);
                        subAbilityGroup.Stats.IncrementStat(modStat, mod);
                        subPlayerGroup.Stats.IncrementStat(modStat, mod);
                        subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
                    }
                }
                else
                {
                    Stats.IncrementStat("DamageRegHit");
                    subAbilityGroup.Stats.IncrementStat("DamageRegHit");
                    subPlayerGroup.Stats.IncrementStat("DamageRegHit");
                    subPlayerAbilityGroup.Stats.IncrementStat("DamageRegHit");
                    Stats.IncrementStat("RegularDamage", line.Amount);
                    subAbilityGroup.Stats.IncrementStat("RegularDamage", line.Amount);
                    subPlayerGroup.Stats.IncrementStat("RegularDamage", line.Amount);
                    subPlayerAbilityGroup.Stats.IncrementStat("RegularDamage", line.Amount);
                    if (line.Modifier != 0)
                    {
                        var mod = ParseHelper.GetOriginalDamage(line.Amount, line.Modifier);
                        var modStat = "DamageRegMod";
                        Stats.IncrementStat(modStat, mod);
                        subAbilityGroup.Stats.IncrementStat(modStat, mod);
                        subPlayerGroup.Stats.IncrementStat(modStat, mod);
                        subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
                    }
                }
            }
            else
            {
                Stats.IncrementStat("DamageRegMiss");
                subAbilityGroup.Stats.IncrementStat("DamageRegMiss");
                subPlayerGroup.Stats.IncrementStat("DamageRegMiss");
                subPlayerAbilityGroup.Stats.IncrementStat("DamageRegMiss");
            }
            foreach (var stat in fields.Where(stat => LD.Contains(stat.Name))
                                       .Where(stat => Equals(stat.GetValue(line), true)))
            {
                var regStat = String.Format("Damage{0}", stat.Name);
                Stats.IncrementStat(regStat);
                subAbilityGroup.Stats.IncrementStat(regStat);
                subPlayerGroup.Stats.IncrementStat(regStat);
                subPlayerAbilityGroup.Stats.IncrementStat(regStat);
                if (line.Modifier == 0)
                {
                    continue;
                }
                var mod = ParseHelper.GetOriginalDamage(line.Amount, line.Modifier);
                var modStat = String.Format("Damage{0}Mod", stat.Name);
                Stats.IncrementStat(modStat, mod);
                subAbilityGroup.Stats.IncrementStat(modStat, mod);
                subPlayerGroup.Stats.IncrementStat(modStat, mod);
                subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
            }
        }
    }
}
