// FFXIVAPP.Plugin.Parse
// Player.Stats.Damage.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

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
            var damageGroup = GetGroup("Damage");
            StatGroup subMonsterGroup;
            if (!damageGroup.TryGetGroup(line.Source, out subMonsterGroup))
            {
                subMonsterGroup = new StatGroup(line.Source);
                subMonsterGroup.Stats.AddStats(DamageStatList("m", null));
                damageGroup.AddGroup(subMonsterGroup);
            }
            var abilities = subMonsterGroup.GetGroup("Abilities");
            StatGroup subMonsterAbilityGroup;
            if (!abilities.TryGetGroup(line.Action, out subMonsterAbilityGroup))
            {
                subMonsterAbilityGroup = new StatGroup(line.Action);
                subMonsterAbilityGroup.Stats.AddStats(DamageStatList("a", subMonsterGroup));
                abilities.AddGroup(subMonsterAbilityGroup);
            }
            subMonsterGroup.Stats.IncrementStat("TotalDamageTakenActionsUsed");
            subMonsterAbilityGroup.Stats.IncrementStat("TotalDamageTakenActionsUsed");
            if (line.Hit)
            {
                subMonsterGroup.Stats.IncrementStat("TotalOverallDamageTaken", line.Amount);
                subMonsterAbilityGroup.Stats.IncrementStat("TotalOverallDamageTaken", line.Amount);
                Stats.IncrementStat("TotalOverallDamageTaken", line.Amount);
                if (line.Crit)
                {
                    subMonsterGroup.Stats.IncrementStat("CriticalDamageTaken", line.Amount);
                    subMonsterGroup.Stats.IncrementStat("DamageTakenCritHit");
                    subMonsterAbilityGroup.Stats.IncrementStat("CriticalDamageTaken", line.Amount);
                    subMonsterAbilityGroup.Stats.IncrementStat("DamageTakenCritHit");
                    Stats.IncrementStat("CriticalDamageTaken", line.Amount);
                    Stats.IncrementStat("DamageTakenCritHit");
                }
                else
                {
                    subMonsterGroup.Stats.IncrementStat("RegularDamageTaken", line.Amount);
                    subMonsterGroup.Stats.IncrementStat("DamageTakenRegHit");
                    subMonsterAbilityGroup.Stats.IncrementStat("RegularDamageTaken", line.Amount);
                    subMonsterAbilityGroup.Stats.IncrementStat("DamageTakenRegHit");
                    Stats.IncrementStat("RegularDamageTaken", line.Amount);
                    Stats.IncrementStat("DamageTakenRegHit");
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
                subMonsterGroup.Stats.IncrementStat("DamageTaken" + stat.Name);
                subMonsterAbilityGroup.Stats.IncrementStat("DamageTaken" + stat.Name);
            }
        }
    }
}
