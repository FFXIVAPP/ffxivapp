// FFXIVAPP.Plugin.Parse
// Player.Stats.Damage.cs
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
        public void SetDamageStat(Line line)
        {
            var fields = line.GetType()
                             .GetProperties();
            var damageGroup = GetGroup("DamageTakenByAction");
            StatGroup subMonsterGroup;
            if (!damageGroup.TryGetGroup(line.Source, out subMonsterGroup))
            {
                subMonsterGroup = new StatGroup(line.Source);
                subMonsterGroup.Stats.AddStats(DamageStatList(null));
                damageGroup.AddGroup(subMonsterGroup);
            }
            var abilities = subMonsterGroup.GetGroup("DamageTakenByMonsterByAction");
            StatGroup subMonsterAbilityGroup;
            if (!abilities.TryGetGroup(line.Action, out subMonsterAbilityGroup))
            {
                subMonsterAbilityGroup = new StatGroup(line.Action);
                subMonsterAbilityGroup.Stats.AddStats(DamageStatList(subMonsterGroup, true));
                abilities.AddGroup(subMonsterAbilityGroup);
            }
            Stats.IncrementStat("TotalDamageTakenActionsUsed");
            subMonsterGroup.Stats.IncrementStat("TotalDamageTakenActionsUsed");
            subMonsterAbilityGroup.Stats.IncrementStat("TotalDamageTakenActionsUsed");
            if (line.Hit)
            {
                Stats.IncrementStat("TotalOverallDamageTaken", line.Amount);
                subMonsterGroup.Stats.IncrementStat("TotalOverallDamageTaken", line.Amount);
                subMonsterAbilityGroup.Stats.IncrementStat("TotalOverallDamageTaken", line.Amount);
                if (line.Crit)
                {
                    Stats.IncrementStat("DamageTakenCritHit");
                    subMonsterGroup.Stats.IncrementStat("DamageTakenCritHit");
                    subMonsterAbilityGroup.Stats.IncrementStat("DamageTakenCritHit");
                    Stats.IncrementStat("CriticalDamageTaken", line.Amount);
                    subMonsterGroup.Stats.IncrementStat("CriticalDamageTaken", line.Amount);
                    subMonsterAbilityGroup.Stats.IncrementStat("CriticalDamageTaken", line.Amount);
                }
                else
                {
                    Stats.IncrementStat("DamageTakenRegHit");
                    subMonsterGroup.Stats.IncrementStat("DamageTakenRegHit");
                    subMonsterAbilityGroup.Stats.IncrementStat("DamageTakenRegHit");
                    Stats.IncrementStat("RegularDamageTaken", line.Amount);
                    subMonsterGroup.Stats.IncrementStat("RegularDamageTaken", line.Amount);
                    subMonsterAbilityGroup.Stats.IncrementStat("RegularDamageTaken", line.Amount);
                }
            }
            else
            {
                Stats.IncrementStat("DamageTakenRegMiss");
                subMonsterGroup.Stats.IncrementStat("DamageTakenRegMiss");
                subMonsterAbilityGroup.Stats.IncrementStat("DamageTakenRegMiss");
            }
            foreach (var stat in fields.Where(stat => LD.Contains(stat.Name))
                                       .Where(stat => Equals(stat.GetValue(line), true)))
            {
                var regStat = String.Format("DamageTaken{0}", stat.Name);
                Stats.IncrementStat(regStat);
                subMonsterGroup.Stats.IncrementStat(regStat);
                subMonsterAbilityGroup.Stats.IncrementStat(regStat);
                if (line.Modifier == 0)
                {
                    continue;
                }
                var reduction = Math.Abs((line.Amount * (line.Amount / (100 + line.Modifier))) - line.Amount);
                var reductionStat = String.Format("DamageTaken{0}Reduction", stat.Name);
                Stats.IncrementStat(reductionStat, reduction);
                subMonsterGroup.Stats.IncrementStat(reductionStat, reduction);
                subMonsterAbilityGroup.Stats.IncrementStat(reductionStat, reduction);
            }
        }
    }
}
