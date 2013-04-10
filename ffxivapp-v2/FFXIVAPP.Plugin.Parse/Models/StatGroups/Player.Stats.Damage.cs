// FFXIVAPP.Plugin.Parse
// Player.Stats.Abilities.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Linq;
using FFXIVAPP.Plugin.Parse.Models.Stats;

#endregion

namespace FFXIVAPP.Plugin.Parse.Models.StatGroups
{
    public partial class Player
    {
        /// <summary>
        /// </summary>
        /// <param name="line"> </param>
        public void SetAbilityStat(Line line)
        {
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
            var monsterGroup = GetGroup("DamageToMonsters");
            StatGroup subMonsterGroup;
            if (!monsterGroup.TryGetGroup(line.Target, out subMonsterGroup))
            {
                subMonsterGroup = new StatGroup(line.Target);
                subMonsterGroup.Stats.AddStats(DamageStatList(null));
                monsterGroup.AddGroup(subMonsterGroup);
            }
            var monsters = subMonsterGroup.GetGroup("DamageToMonstersByAction");
            StatGroup subMonsterAbilityGroup;
            if (!monsters.TryGetGroup(line.Action, out subMonsterAbilityGroup))
            {
                subMonsterAbilityGroup = new StatGroup(line.Action);
                subMonsterAbilityGroup.Stats.AddStats(DamageStatList(subMonsterGroup, true));
                monsters.AddGroup(subMonsterAbilityGroup);
            }
            Stats.IncrementStat("TotalDamageActionsUsed");
            subAbilityGroup.Stats.IncrementStat("TotalDamageActionsUsed");
            subMonsterGroup.Stats.IncrementStat("TotalDamageActionsUsed");
            subMonsterAbilityGroup.Stats.IncrementStat("TotalDamageActionsUsed");
            if (line.Hit)
            {
                Stats.IncrementStat("TotalOverallDamage", line.Amount);
                subAbilityGroup.Stats.IncrementStat("TotalOverallDamage", line.Amount);
                subMonsterGroup.Stats.IncrementStat("TotalOverallDamage", line.Amount);
                subMonsterAbilityGroup.Stats.IncrementStat("TotalOverallDamage", line.Amount);
                if (line.Crit)
                {
                    Stats.IncrementStat("DamageCritHit");
                    subAbilityGroup.Stats.IncrementStat("DamageCritHit");
                    subMonsterGroup.Stats.IncrementStat("DamageCritHit");
                    subMonsterAbilityGroup.Stats.IncrementStat("DamageCritHit");
                    Stats.IncrementStat("CriticalDamage", line.Amount);
                    subAbilityGroup.Stats.IncrementStat("CriticalDamage", line.Amount);
                    subMonsterGroup.Stats.IncrementStat("CriticalDamage", line.Amount);
                    subMonsterAbilityGroup.Stats.IncrementStat("CriticalDamage", line.Amount);
                }
                else
                {
                    Stats.IncrementStat("DamageRegHit");
                    subAbilityGroup.Stats.IncrementStat("DamageRegHit");
                    subMonsterGroup.Stats.IncrementStat("DamageRegHit");
                    subMonsterAbilityGroup.Stats.IncrementStat("DamageRegHit");
                    Stats.IncrementStat("RegularDamage", line.Amount);
                    subAbilityGroup.Stats.IncrementStat("RegularDamage", line.Amount);
                    subMonsterGroup.Stats.IncrementStat("RegularDamage", line.Amount);
                    subMonsterAbilityGroup.Stats.IncrementStat("RegularDamage", line.Amount);
                }
            }
            else
            {
                Stats.IncrementStat("DamageRegMiss");
                subAbilityGroup.Stats.IncrementStat("DamageRegMiss");
                subMonsterGroup.Stats.IncrementStat("DamageRegMiss");
                subMonsterAbilityGroup.Stats.IncrementStat("DamageRegMiss");
            }
            foreach (var stat in fields.Where(stat => LD.Contains(stat.Name))
                                       .Where(stat => Equals(stat.GetValue(line), true)))
            {
                var regStat = String.Format("Damage{0}", stat.Name);
                Stats.IncrementStat(regStat);
                subAbilityGroup.Stats.IncrementStat(regStat);
                subMonsterGroup.Stats.IncrementStat(regStat);
                subMonsterAbilityGroup.Stats.IncrementStat(regStat);
                if (line.Modifier == 0)
                {
                    continue;
                }
                var reduction = Math.Abs((line.Amount * (line.Amount / (100 + line.Modifier))) - line.Amount);
                var reductionStat = String.Format("Damage{0}Reduction", stat.Name);
                Stats.IncrementStat(reductionStat, reduction);
                subAbilityGroup.Stats.IncrementStat(reductionStat, reduction);
                subMonsterGroup.Stats.IncrementStat(reductionStat, reduction);
                subMonsterAbilityGroup.Stats.IncrementStat(reductionStat, reduction);
            }
        }
    }
}
