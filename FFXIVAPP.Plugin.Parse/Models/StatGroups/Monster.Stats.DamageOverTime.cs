// FFXIVAPP.Plugin.Parse
// Monster.Stats.DamageOverTime.cs
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
        /// <summary>
        /// </summary>
        /// <param name="line"></param>
        private void SetupDamageOverTimeAction(Line line)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="line"></param>
        public void SetDamageOverTime(Line line)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="line"></param>
        public void SetDamageOverTimeFromPlayer(Line line)
        {
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
            Stats.IncrementStat("TotalOverallDamageTaken", line.Amount);
            subPlayerGroup.Stats.IncrementStat("TotalOverallDamageTaken", line.Amount);
            subPlayerAbilityGroup.Stats.IncrementStat("TotalOverallDamageTaken", line.Amount);
            Stats.IncrementStat("DamageTakenDOT", line.Amount);
            subPlayerGroup.Stats.IncrementStat("DamageTakenDOT", line.Amount);
            subPlayerAbilityGroup.Stats.IncrementStat("DamageTakenDOT", line.Amount);
        }
    }
}
