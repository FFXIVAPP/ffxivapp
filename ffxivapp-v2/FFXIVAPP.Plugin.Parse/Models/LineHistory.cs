// FFXIVAPP.Plugin.Parse
// LineHistory.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;

#endregion

namespace FFXIVAPP.Plugin.Parse.Models
{
    public class LineHistory
    {
        public DateTime TimeStamp { get; set; }
        public Line Line { get; set; }

        public LineHistory(Line line)
        {
            TimeStamp = DateTime.Now;
            Line = line;
        }
    }
}
