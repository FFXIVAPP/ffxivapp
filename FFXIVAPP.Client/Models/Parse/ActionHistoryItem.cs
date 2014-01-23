// FFXIVAPP.Client
// ActionHistoryItem.cs
// 
// © 2013 Ryan Wilson

using System;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Models.Parse
{
    [DoNotObfuscate]
    public class ActionHistoryItem
    {
        public string Source { get; set; }
        public string Target { get; set; }
        public string Action { get; set; }
        public decimal Amount { get; set; }
        public string Critical { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
