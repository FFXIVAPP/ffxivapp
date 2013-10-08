// FFXIVAPP.Client
// LineHistory.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;

#endregion

namespace FFXIVAPP.Client.Models.Parse
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
