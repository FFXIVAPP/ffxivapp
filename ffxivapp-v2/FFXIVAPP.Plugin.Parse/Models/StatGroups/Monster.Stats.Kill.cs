// FFXIVAPP.Plugin.Parse
// Monster.Stats.Kill.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.Plugin.Parse.Models.Fights;
using NLog;

#endregion

namespace FFXIVAPP.Plugin.Parse.Models.StatGroups
{
    public partial class Monster
    {
        /// <summary>
        /// </summary>
        /// <param name="fight"> </param>
        public void SetKillStat(Fight fight)
        {
            if (fight.MobName != Name)
            {
                Logging.Log(LogManager.GetCurrentClassLogger(), String.Format("KillEvent : Got request to add kill stats for {0}, but my name is {1}!", fight.MobName, Name));
                return;
            }
            if (fight.MobName == "")
            {
                Logging.Log(LogManager.GetCurrentClassLogger(), String.Format("KillEvent : Got request to add kill stats for {0}, but no name!", fight.MobName));
                return;
            }
            ParseControl.Instance.LastKilled = fight.MobName;
            Stats.IncrementStat("TotalKilled");
            Stats.SetOrAddStat("AverageHP", Stats.GetStatValue("TotalOverallDamage") / Stats.GetStatValue("TotalKilled"));
        }
    }
}
