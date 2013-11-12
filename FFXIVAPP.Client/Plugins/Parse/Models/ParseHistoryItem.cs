// FFXIVAPP.Client
// ParseHistory.cs
// 
// © 2013 Ryan Wilson

using System;
using FFXIVAPP.Client.Plugins.Parse.Models.Stats;

namespace FFXIVAPP.Client.Plugins.Parse.Models
{
    public class ParseHistoryItem
    {
        #region "Auto Properties"

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public TimeSpan ParseLength { get; set; }
        public string Name { get; set; }
        public StatGroup Overall { get; set; }
        public StatGroup Party { get; set; }
        public StatGroup Monster { get; set; }

        #endregion

        public ParseHistoryItem(string name = "UnknownEvent")
        {
            Name = name;
            Overall = new StatGroup("Overall");
            Party = new StatGroup("Party");
            Monster = new StatGroup("Monster");
        }
    }
}
