// FFXIVAPP.Plugin.Parse
// Player.Stats.DamageOverTime.cs
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
        /// <param name="line"></param>
        private void SetupDamageOverTimeAction(Line line)
        {
            if (DamageOverTimeActions.ContainsKey(line.Action))
            {
                DamageOverTimeActions[line.Action].Dispose();
                DamageOverTimeActions.Remove(line.Action);
            }
            DamageOverTimeActions.Add(line.Action, new DamageOverTimeAction(line));
        }

        /// <summary>
        /// </summary>
        /// <param name="line"></param>
        public void SetDamageOverTime(Line line)
        {
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
            Stats.IncrementStat("TotalOverallDamage", line.Amount);
            subAbilityGroup.Stats.IncrementStat("TotalOverallDamage", line.Amount);
            subMonsterGroup.Stats.IncrementStat("TotalOverallDamage", line.Amount);
            subMonsterAbilityGroup.Stats.IncrementStat("TotalOverallDamage", line.Amount);
            Stats.IncrementStat("DamageDOT", line.Amount);
            subAbilityGroup.Stats.IncrementStat("DamageDOT", line.Amount);
            subMonsterGroup.Stats.IncrementStat("DamageDOT", line.Amount);
            subMonsterAbilityGroup.Stats.IncrementStat("DamageDOT", line.Amount);
        }
    }
}
