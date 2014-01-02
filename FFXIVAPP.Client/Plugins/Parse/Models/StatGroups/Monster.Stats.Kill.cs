// FFXIVAPP.Client
// Monster.Stats.Kill.cs
// 
// © 2013 Ryan Wilson

using System;
using FFXIVAPP.Client.Plugins.Parse.Models.Fights;
using FFXIVAPP.Common.Utilities;
using NLog;

namespace FFXIVAPP.Client.Plugins.Parse.Models.StatGroups
{
    public partial class Monster
    {
        /// <summary>
        /// </summary>
        /// <param name="fight"> </param>
        public void SetKill(Fight fight)
        {
            if (fight.MonsterName != Name)
            {
                Logging.Log(Logger, String.Format("KillEvent : Got request to add kill stats for {0}, but my name is {1}!", fight.MonsterName, Name));
                return;
            }
            if (fight.MonsterName == "")
            {
                Logging.Log(Logger, String.Format("KillEvent : Got request to add kill stats for {0}, but no name!", fight.MonsterName));
                return;
            }
            Stats.IncrementStat("TotalKilled");
            var avghp = Stats.GetStatValue("TotalOverallDamageTaken") / Stats.GetStatValue("TotalKilled");
            Stats.SetOrAddStat("AverageHP", avghp);
        }
    }
}
