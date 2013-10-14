// FFXIVAPP.Client
// Monster.Stats.Kill.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using FFXIVAPP.Client.Plugins.Parse.Models.Fights;
using FFXIVAPP.Common.Utilities;
using NLog;

#endregion

namespace FFXIVAPP.Client.Plugins.Parse.Models.StatGroups {
    public partial class Monster {
        /// <summary>
        /// </summary>
        /// <param name="fight"> </param>
        public void SetKill(Fight fight) {
            if (fight.MobName != Name) {
                Logging.Log(LogManager.GetCurrentClassLogger(), String.Format("KillEvent : Got request to add kill stats for {0}, but my name is {1}!", fight.MobName, Name));
                return;
            }
            if (fight.MobName == "") {
                Logging.Log(LogManager.GetCurrentClassLogger(), String.Format("KillEvent : Got request to add kill stats for {0}, but no name!", fight.MobName));
                return;
            }
            Stats.IncrementStat("TotalKilled");
            var avghp = Stats.GetStatValue("TotalOverallDamageTaken") / Stats.GetStatValue("TotalKilled");
            Stats.SetOrAddStat("AverageHP", avghp);
        }
    }
}
