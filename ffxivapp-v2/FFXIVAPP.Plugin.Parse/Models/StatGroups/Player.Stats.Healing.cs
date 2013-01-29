// FFXIVAPP.Plugin.Parse
// Player.Stats.Healing.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

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
            var abilityGroup = GetGroup("Healing");
            StatGroup subAbilityGroup;
            if (!abilityGroup.TryGetGroup(line.Action, out subAbilityGroup))
            {
                subAbilityGroup = new StatGroup(line.Action);
                subAbilityGroup.Stats.AddStats(HealingStatList("a", null));
                abilityGroup.AddGroup(subAbilityGroup);
            }
            var playerGroup = GetGroup("Players");
            StatGroup subPlayerGroup;
            if (!playerGroup.TryGetGroup(line.Target, out subPlayerGroup))
            {
                subPlayerGroup = new StatGroup(line.Target);
                subPlayerGroup.Stats.AddStats(HealingStatList("p", null));
                playerGroup.AddGroup(subPlayerGroup);
            }
            var abilities = subPlayerGroup.GetGroup("Abilities");
            StatGroup subPlayerAbilityGroup;
            if (!abilities.TryGetGroup(line.Action, out subPlayerAbilityGroup))
            {
                subPlayerAbilityGroup = new StatGroup(line.Action);
                subPlayerAbilityGroup.Stats.AddStats(HealingStatList("pa", subPlayerGroup));
                abilities.AddGroup(subPlayerAbilityGroup);
            }
            Stats.GetStat("HTotal")
                 .Value += line.Amount;
            subAbilityGroup.Stats.GetStat("Total")
                           .Value += line.Amount;
            subPlayerGroup.Stats.GetStat("Total")
                          .Value += line.Amount;
            subPlayerAbilityGroup.Stats.GetStat("Total")
                                 .Value += line.Amount;
        }
    }
}
