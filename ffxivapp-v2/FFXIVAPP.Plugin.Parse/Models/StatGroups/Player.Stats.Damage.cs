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
            subMonsterGroup.Stats.IncrementStat("Used");
            subMonsterAbilityGroup.Stats.IncrementStat("Used");
            if (line.Hit)
            {
                subMonsterGroup.Stats.IncrementStat("Total", line.Amount);
                subMonsterAbilityGroup.Stats.IncrementStat("Total", line.Amount);
                Stats.IncrementStat("DTTotal", line.Amount);
                if (line.Crit)
                {
                    subMonsterGroup.Stats.IncrementStat("Crit", line.Amount);
                    subMonsterGroup.Stats.IncrementStat("CHit");
                    subMonsterAbilityGroup.Stats.IncrementStat("Crit", line.Amount);
                    subMonsterAbilityGroup.Stats.IncrementStat("CHit");
                    Stats.IncrementStat("DTCrit", line.Amount);
                    Stats.IncrementStat("DTCHit");
                }
                else
                {
                    subMonsterGroup.Stats.IncrementStat("Reg", line.Amount);
                    subMonsterGroup.Stats.IncrementStat("Hit");
                    subMonsterAbilityGroup.Stats.IncrementStat("Reg", line.Amount);
                    subMonsterAbilityGroup.Stats.IncrementStat("Hit");
                    Stats.IncrementStat("DTReg", line.Amount);
                    Stats.IncrementStat("DTHit");
                }
            }
            else
            {
                Stats.IncrementStat("Miss");
                subMonsterGroup.Stats.IncrementStat("Miss");
                subMonsterAbilityGroup.Stats.IncrementStat("Miss");
            }
            foreach (var stat in fields.Where(stat => LD.Contains(stat.Name))
                                       .Where(stat => Equals(stat.GetValue(line), true)))
            {
                subMonsterGroup.Stats.IncrementStat(stat.Name);
                subMonsterAbilityGroup.Stats.IncrementStat(stat.Name);
            }
        }
    }
}
