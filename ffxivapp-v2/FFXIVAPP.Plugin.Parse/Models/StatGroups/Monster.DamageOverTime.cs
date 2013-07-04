// FFXIVAPP.Plugin.Parse
// Monster.DamageOverTime.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using FFXIVAPP.Plugin.Parse.Models.Stats;

#endregion

namespace FFXIVAPP.Plugin.Parse.Models.StatGroups
{
    public partial class Monster
    {
        public void SetPlayerDamageOverTime(Line line)
        {
            var abilityGroup = GetGroup("DamageTakenByAction");
            StatGroup subGroup;
            if (!abilityGroup.TryGetGroup(line.Action, out subGroup))
            {
                subGroup = new StatGroup(line.Action);
                subGroup.Stats.AddStats(DamageTakenStatList());
                abilityGroup.AddGroup(subGroup);
            }
            Stats.IncrementStat("TotalOverallDamageTaken", line.Amount);
            subGroup.Stats.IncrementStat("TotalOverallDamageTaken", line.Amount);
            Stats.IncrementStat("DamageTakenDOT", line.Amount);
            subGroup.Stats.IncrementStat("DamageTakenDOT", line.Amount);
        }
    }
}
