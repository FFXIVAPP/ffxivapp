// FFXIVAPP.Plugin.Parse
// Player.Stats.Healing.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using FFXIVAPP.Plugin.Parse.Models.Stats;

#endregion

namespace FFXIVAPP.Plugin.Parse.Models.StatGroups
{
    public partial class Player
    {
        /// <summary>
        /// </summary>
        /// <param name="line"> </param>
        public void SetHealingStat(Line line)
        {
            var abilityGroup = GetGroup("HealingByAction");
            StatGroup subAbilityGroup;
            if (!abilityGroup.TryGetGroup(line.Action, out subAbilityGroup))
            {
                subAbilityGroup = new StatGroup(line.Action);
                subAbilityGroup.Stats.AddStats(HealingStatList(null));
                abilityGroup.AddGroup(subAbilityGroup);
            }
            var playerGroup = GetGroup("HealingToPlayers");
            StatGroup subPlayerGroup;
            if (!playerGroup.TryGetGroup(line.Target, out subPlayerGroup))
            {
                subPlayerGroup = new StatGroup(line.Target);
                subPlayerGroup.Stats.AddStats(HealingStatList(null));
                playerGroup.AddGroup(subPlayerGroup);
            }
            var abilities = subPlayerGroup.GetGroup("HealingToPlayersByAction");
            StatGroup subPlayerAbilityGroup;
            if (!abilities.TryGetGroup(line.Action, out subPlayerAbilityGroup))
            {
                subPlayerAbilityGroup = new StatGroup(line.Action);
                subPlayerAbilityGroup.Stats.AddStats(HealingStatList(subPlayerGroup, true));
                abilities.AddGroup(subPlayerAbilityGroup);
            }
            Stats.IncrementStat("TotalHealingActionsUsed");
            subAbilityGroup.Stats.IncrementStat("TotalHealingActionsUsed");
            subPlayerGroup.Stats.IncrementStat("TotalHealingActionsUsed");
            subPlayerAbilityGroup.Stats.IncrementStat("TotalHealingActionsUsed");
            Stats.IncrementStat("TotalOverallHealing", line.Amount);
            subAbilityGroup.Stats.IncrementStat("TotalOverallHealing", line.Amount);
            subPlayerGroup.Stats.IncrementStat("TotalOverallHealing", line.Amount);
            subPlayerAbilityGroup.Stats.IncrementStat("TotalOverallHealing", line.Amount);
            if (line.Crit)
            {
                Stats.IncrementStat("HealingCritHit");
                subAbilityGroup.Stats.IncrementStat("HealingCritHit");
                subPlayerGroup.Stats.IncrementStat("HealingCritHit");
                subPlayerAbilityGroup.Stats.IncrementStat("HealingCritHit");
                Stats.IncrementStat("CriticalHealing", line.Amount);
                subAbilityGroup.Stats.IncrementStat("CriticalHealing", line.Amount);
                subPlayerGroup.Stats.IncrementStat("CriticalHealing", line.Amount);
                subPlayerAbilityGroup.Stats.IncrementStat("CriticalHealing", line.Amount);
            }
            else
            {
                Stats.IncrementStat("HealingRegHit");
                subAbilityGroup.Stats.IncrementStat("HealingRegHit");
                subPlayerGroup.Stats.IncrementStat("HealingRegHit");
                subPlayerAbilityGroup.Stats.IncrementStat("HealingRegHit");
                Stats.IncrementStat("RegularHealing", line.Amount);
                subAbilityGroup.Stats.IncrementStat("RegularHealing", line.Amount);
                subPlayerGroup.Stats.IncrementStat("RegularHealing", line.Amount);
                subPlayerAbilityGroup.Stats.IncrementStat("RegularHealing", line.Amount); 
            }
        }
    }
}
