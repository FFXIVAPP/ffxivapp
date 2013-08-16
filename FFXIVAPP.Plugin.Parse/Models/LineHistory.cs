// FFXIVAPP.Plugin.Parse
// LineHistory.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;

#endregion

namespace FFXIVAPP.Plugin.Parse.Models
{
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
