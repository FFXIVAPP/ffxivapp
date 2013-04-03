// FFXIVAPP.Plugin.Parse
// Monster.Stats.Players.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System.Linq;
using FFXIVAPP.Plugin.Parse.Models.Stats;

#endregion

namespace FFXIVAPP.Plugin.Parse.Models.StatGroups
{
    public partial class Monster
    {
        /// <summary>
        /// </summary>
        /// <param name="line"> </param>
        public void SetPlayerStat(Line line)
        {
            var fields = line.GetType()
                             .GetProperties();
            var abilityGroup = GetGroup("Abilities");
            StatGroup subGroup;
            if (!abilityGroup.TryGetGroup(line.Action, out subGroup))
            {
                subGroup = new StatGroup(line.Action);
                subGroup.Stats.AddStats(DamageStatList());
                abilityGroup.AddGroup(subGroup);
            }
            subGroup.Stats.IncrementStat("Used");
            if (line.Hit)
            {
                subGroup.Stats.IncrementStat("Total", line.Amount);
                Stats.IncrementStat("Total", line.Amount);
                if (line.Crit)
                {
                    subGroup.Stats.IncrementStat("CHit");
                    subGroup.Stats.IncrementStat("Crit", line.Amount);
                    Stats.IncrementStat("CHit");
                    Stats.IncrementStat("Crit", line.Amount);
                }
                else
                {
                    subGroup.Stats.IncrementStat("Hit");
                    subGroup.Stats.IncrementStat("Reg", line.Amount);
                    Stats.IncrementStat("Hit");
                    Stats.IncrementStat("Reg", line.Amount);
                }
            }
            else
            {
                subGroup.Stats.IncrementStat("Miss");
            }
            foreach (var stat in fields.Where(stat => LD.Contains(stat.Name))
                                       .Where(stat => Equals(stat.GetValue(line), true)))
            {
                subGroup.Stats.IncrementStat(stat.Name);
            }
        }
    }
}
