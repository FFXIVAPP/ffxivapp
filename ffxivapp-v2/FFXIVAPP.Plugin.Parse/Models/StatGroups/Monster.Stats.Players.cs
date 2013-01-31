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
            subGroup.Stats.GetStat("Total")
                    .Value += line.Amount;
            Stats.GetStat("Total")
                 .Value += line.Amount;
            subGroup.Stats.GetStat("Used")
                    .Value += 1;
            if (line.Crit)
            {
                subGroup.Stats.GetStat("CHit")
                        .Value += 1;
                subGroup.Stats.GetStat("Crit")
                        .Value += line.Amount;
                Stats.GetStat("CHit")
                     .Value += 1;
                Stats.GetStat("Crit")
                     .Value += line.Amount;
            }
            else
            {
                subGroup.Stats.GetStat("Hit")
                        .Value += 1;
                subGroup.Stats.GetStat("Reg")
                        .Value += line.Amount;
                Stats.GetStat("Hit")
                     .Value += 1;
                Stats.GetStat("Reg")
                     .Value += line.Amount;
            }
            foreach (var stat in fields.Where(stat => LD.Contains(stat.Name))
                                       .Where(stat => Equals(stat.GetValue(line), true)))
            {
                subGroup.Stats.GetStat(stat.Name)
                        .Value += 1;
            }
        }
    }
}
