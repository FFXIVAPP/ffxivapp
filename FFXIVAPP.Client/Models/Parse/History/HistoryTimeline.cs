// FFXIVAPP.Client
// HistoryTimeline.cs
// 
// © 2013 Ryan Wilson

using NLog;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Models.Parse.History
{
    [DoNotObfuscate]
    public class HistoryTimeline
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        public HistoryTimeline()
        {
            Overall = new HistoryGroup("Overall");
            Party = new HistoryGroup("Party");
            Monster = new HistoryGroup("Monster");
        }

        public HistoryGroup Overall { get; set; }
        public HistoryGroup Party { get; set; }
        public HistoryGroup Monster { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="monsterName"> </param>
        /// <returns> </returns>
        public HistoryGroup GetSetMonster(string monsterName)
        {
            if (!Monster.HasGroup(monsterName))
            {
                Monster.AddGroup(new HistoryGroup(monsterName));
            }
            var monster = Monster.GetGroup(monsterName);
            return monster;
        }

        /// <summary>
        /// </summary>
        /// <param name="playerName"> </param>
        /// <returns> </returns>
        public HistoryGroup GetSetPlayer(string playerName)
        {
            if (!Party.HasGroup(playerName))
            {
                Party.AddGroup(new HistoryGroup(playerName));
            }
            var player = Party.GetGroup(playerName);
            return player;
        }
    }
}
