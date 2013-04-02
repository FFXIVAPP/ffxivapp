// FFXIVAPP.Plugin.Parse
// Player.Stats.Abilities.cs
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
        public void SetAbilityStat(Line line)
        {
            var fields = line.GetType()
                             .GetProperties();
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
            Stats.IncrementStat("Used");
            subAbilityGroup.Stats.IncrementStat("Used");
            subMonsterGroup.Stats.IncrementStat("Used");
            subMonsterAbilityGroup.Stats.IncrementStat("Used");
            if (line.Hit)
            {
                Stats.IncrementStat("Total", line.Amount);
                subAbilityGroup.Stats.IncrementStat("Total", line.Amount);
                subMonsterGroup.Stats.IncrementStat("Total", line.Amount);
                subMonsterAbilityGroup.Stats.IncrementStat("Total", line.Amount);
                if (line.Crit)
                {
                    Stats.IncrementStat("Crit", line.Amount);
                    subAbilityGroup.Stats.IncrementStat("Crit", line.Amount);
                    subMonsterGroup.Stats.IncrementStat("Crit", line.Amount);
                    subMonsterAbilityGroup.Stats.IncrementStat("Crit", line.Amount);
                    Stats.IncrementStat("CHit");
                    subAbilityGroup.Stats.IncrementStat("CHit");
                    subMonsterGroup.Stats.IncrementStat("CHit");
                    subMonsterAbilityGroup.Stats.IncrementStat("CHit");
                }
                else
                {
                    Stats.IncrementStat("Reg", line.Amount);
                    subAbilityGroup.Stats.IncrementStat("Reg", line.Amount);
                    subMonsterGroup.Stats.IncrementStat("Reg", line.Amount);
                    subMonsterAbilityGroup.Stats.IncrementStat("Reg", line.Amount);
                    Stats.IncrementStat("Hit");
                    subAbilityGroup.Stats.IncrementStat("Hit");
                    subMonsterGroup.Stats.IncrementStat("Hit");
                    subMonsterAbilityGroup.Stats.IncrementStat("Hit");
                }
            }
            else
            {
                Stats.IncrementStat("Miss");
                subAbilityGroup.Stats.IncrementStat("Miss");
                subMonsterGroup.Stats.IncrementStat("Miss");
                subMonsterAbilityGroup.Stats.IncrementStat("Miss");
            }
            foreach (var stat in fields.Where(stat => LD.Contains(stat.Name))
                                       .Where(stat => Equals(stat.GetValue(line), true)))
            {
                subAbilityGroup.Stats.IncrementStat(stat.Name);
                subMonsterGroup.Stats.IncrementStat(stat.Name);
                subMonsterAbilityGroup.Stats.IncrementStat(stat.Name);
            }
        }
    }
}
