// FFXIVAPP.Plugin.Parse
// Player.Stats.Damage.cs
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
            subMonsterGroup.Stats.GetStat("Used")
                           .Value += 1;
            subMonsterAbilityGroup.Stats.GetStat("Used")
                                  .Value += 1;
            if (line.Hit)
            {
                subMonsterGroup.Stats.GetStat("Total")
                               .Value += line.Amount;
                subMonsterAbilityGroup.Stats.GetStat("Total")
                                      .Value += line.Amount;
                Stats.GetStat("DTTotal")
                     .Value += line.Amount;
                if (line.Crit)
                {
                    subMonsterGroup.Stats.GetStat("Crit")
                                   .Value += line.Amount;
                    subMonsterGroup.Stats.GetStat("CHit")
                                   .Value += 1;
                    subMonsterAbilityGroup.Stats.GetStat("Crit")
                                          .Value += line.Amount;
                    subMonsterAbilityGroup.Stats.GetStat("CHit")
                                          .Value += 1;
                    Stats.GetStat("DTCrit")
                         .Value += line.Amount;
                    Stats.GetStat("DTCHit")
                         .Value += 1;
                }
                else
                {
                    subMonsterGroup.Stats.GetStat("Reg")
                                   .Value += line.Amount;
                    subMonsterGroup.Stats.GetStat("Hit")
                                   .Value += 1;
                    subMonsterAbilityGroup.Stats.GetStat("Reg")
                                          .Value += line.Amount;
                    subMonsterAbilityGroup.Stats.GetStat("Hit")
                                          .Value += 1;
                    Stats.GetStat("DTReg")
                         .Value += line.Amount;
                    Stats.GetStat("DTHit")
                         .Value += 1;
                }
            }
            foreach (var stat in fields.Where(stat => LD.Contains(stat.Name))
                                       .Where(stat => Equals(stat.GetValue(line), true)))
            {
                subMonsterGroup.Stats.GetStat(stat.Name)
                               .Value += 1;
                subMonsterAbilityGroup.Stats.GetStat(stat.Name)
                                      .Value += 1;
                ;
            }
        }
    }
}
