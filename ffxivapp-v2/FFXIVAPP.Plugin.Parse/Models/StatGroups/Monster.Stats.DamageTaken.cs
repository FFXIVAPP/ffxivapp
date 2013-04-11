// FFXIVAPP.Plugin.Parse
// Monster.Stats.Players.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
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
            var abilityGroup = GetGroup("DamageTakenByAction");
            StatGroup subGroup;
            if (!abilityGroup.TryGetGroup(line.Action, out subGroup))
            {
                subGroup = new StatGroup(line.Action);
                subGroup.Stats.AddStats(DamageTakenStatList());
                abilityGroup.AddGroup(subGroup);

            }
            Stats.IncrementStat("TotalDamageTakenActionsUsed");
            subGroup.Stats.IncrementStat("TotalDamageTakenActionsUsed");
            if (line.Hit)
            {
                Stats.IncrementStat("TotalOverallDamageTaken", line.Amount);
                subGroup.Stats.IncrementStat("TotalOverallDamageTaken", line.Amount);
                if (line.Crit)
                {
                    Stats.IncrementStat("DamageTakenCritHit");
                    subGroup.Stats.IncrementStat("DamageTakenCritHit");
                    Stats.IncrementStat("CriticalDamageTaken", line.Amount);
                    subGroup.Stats.IncrementStat("CriticalDamageTaken", line.Amount);
                }
                else
                {
                    Stats.IncrementStat("DamageTakenRegHit");
                    subGroup.Stats.IncrementStat("DamageTakenRegHit");
                    Stats.IncrementStat("RegularDamageTaken", line.Amount);
                    subGroup.Stats.IncrementStat("RegularDamageTaken", line.Amount);
                }
            }
            else
            {
                Stats.IncrementStat("DamageTakenRegMiss");
                subGroup.Stats.IncrementStat("DamageTakenRegMiss");
            }
            foreach (var stat in fields.Where(stat => LD.Contains(stat.Name))
                                       .Where(stat => Equals(stat.GetValue(line), true)))
            {
                var regStat = String.Format("DamageTaken{0}", stat.Name);
                Stats.IncrementStat(regStat);
                subGroup.Stats.IncrementStat(regStat);
                if (line.Modifier == 0)
                {
                    continue;
                }
                var reduction = (line.Amount * (line.Amount / (100 + line.Modifier))) - line.Amount;
                var reductionStat = String.Format("DamageTaken{0}Reduction", stat.Name);
                Stats.IncrementStat(reductionStat, reduction);
                subGroup.Stats.IncrementStat(reductionStat, reduction);
            }
        }
    }
}
