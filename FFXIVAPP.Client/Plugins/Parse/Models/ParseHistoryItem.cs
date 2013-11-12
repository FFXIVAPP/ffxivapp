// FFXIVAPP.Client
// ParseHistoryItem.cs
// 
// © 2013 Ryan Wilson

using System;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Plugins.Parse.Models
{
    [DoNotObfuscate]
    public class ParseHistoryItem
    {
        #region "Auto Properties"

        public string Name { get; set; }
        public HistoryControl HistoryControl { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public TimeSpan ParseLength { get; set; }

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="zoneID"></param>
        public ParseHistoryItem(string name = "UnknownEvent")
        {
            HistoryControl = new HistoryControl();
        }
    }
}
