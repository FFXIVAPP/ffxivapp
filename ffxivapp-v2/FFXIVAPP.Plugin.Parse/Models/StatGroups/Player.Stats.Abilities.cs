// FFXIVAPP.Plugin.Parse
// Player.Stats.Abilities.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

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
        public void SetAbilityStat(Line line)
        {
            var fields = line.GetType().GetProperties();
            var abilityGroup = GetGroup("Abilities");
            StatGroup subAbilityGroup;
            if (!abilityGroup.TryGetGroup(line.Action, out subAbilityGroup))
            {
                subAbilityGroup = new StatGroup(line.Action);
                subAbilityGroup.Stats.AddStats(AbilityStatList("a", null));
                abilityGroup.AddGroup(subAbilityGroup);
            }
            var monsterGroup = GetGroup("Monsters");
            StatGroup subMonsterGroup;
            if (!monsterGroup.TryGetGroup(line.Target, out subMonsterGroup))
            {
                subMonsterGroup = new StatGroup(line.Target);
                subMonsterGroup.Stats.AddStats(AbilityStatList("m", null));
                monsterGroup.AddGroup(subMonsterGroup);
            }
            var monsters = subMonsterGroup.GetGroup("Abilities");
            StatGroup subMonsterAbilityGroup;
            if (!monsters.TryGetGroup(line.Action, out subMonsterAbilityGroup))
            {
                subMonsterAbilityGroup = new StatGroup(line.Action);
                subMonsterAbilityGroup.Stats.AddStats(AbilityStatList("ma", subMonsterGroup));
                monsters.AddGroup(subMonsterAbilityGroup);
            }
            subAbilityGroup.Stats.GetStat("Used").Value += 1;
            subMonsterGroup.Stats.GetStat("Used").Value += 1;
            subMonsterAbilityGroup.Stats.GetStat("Used").Value += 1;
            if (line.Hit)
            {
                Stats.GetStat("Total").Value += line.Amount;
                subAbilityGroup.Stats.GetStat("Total").Value += line.Amount;
                subMonsterGroup.Stats.GetStat("Total").Value += line.Amount;
                subMonsterAbilityGroup.Stats.GetStat("Total").Value += line.Amount;
                if (line.Crit)
                {
                    Stats.GetStat("Crit").Value += line.Amount;
                    subAbilityGroup.Stats.GetStat("Crit").Value += line.Amount;
                    subMonsterGroup.Stats.GetStat("Crit").Value += line.Amount;
                    subMonsterAbilityGroup.Stats.GetStat("Crit").Value += line.Amount;
                    Stats.GetStat("CHit").Value += 1;
                    subAbilityGroup.Stats.GetStat("CHit").Value += 1;
                    subMonsterGroup.Stats.GetStat("CHit").Value += 1;
                    subMonsterAbilityGroup.Stats.GetStat("CHit").Value += 1;
                }
                else
                {
                    Stats.GetStat("Reg").Value += line.Amount;
                    subAbilityGroup.Stats.GetStat("Reg").Value += line.Amount;
                    subMonsterGroup.Stats.GetStat("Reg").Value += line.Amount;
                    subMonsterAbilityGroup.Stats.GetStat("Reg").Value += line.Amount;
                    Stats.GetStat("Hit").Value += 1;
                    subAbilityGroup.Stats.GetStat("Hit").Value += 1;
                    subMonsterGroup.Stats.GetStat("Hit").Value += 1;
                    subMonsterAbilityGroup.Stats.GetStat("Hit").Value += 1;
                }
            }
            else
            {
                Stats.GetStat("Miss").Value += 1;
                subAbilityGroup.Stats.GetStat("Miss").Value += 1;
                subMonsterGroup.Stats.GetStat("Miss").Value += 1;
                subMonsterAbilityGroup.Stats.GetStat("Miss").Value += 1;
            }
            foreach (var stat in fields.Where(stat => LD.Contains(stat.Name)).Where(stat => Equals(stat.GetValue(line), true)))
            {
                subAbilityGroup.Stats.GetStat(stat.Name).Value += 1;
                subMonsterGroup.Stats.GetStat(stat.Name).Value += 1;
                subMonsterAbilityGroup.Stats.GetStat(stat.Name).Value += 1;
            }
        }
    }
}
