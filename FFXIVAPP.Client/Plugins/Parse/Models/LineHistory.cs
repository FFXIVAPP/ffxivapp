// FFXIVAPP.Client
// LineHistory.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Plugins.Parse.Models
{
    [DoNotObfuscate]
    public class LineHistory
    {
        public LineHistory(Line line)
        {
            TimeStamp = DateTime.Now;
            Line = line;
        }

        public DateTime TimeStamp { get; set; }
        public Line Line { get; set; }
    }
}
