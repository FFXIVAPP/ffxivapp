// FFXIVAPP
// TimelineEventArgs.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

#region Usings

using System;

#endregion

namespace FFXIVAPP.Models
{
    public class TimelineEventArgs : EventArgs
    {
        /// <summary>
        /// </summary>
        /// <param name="t"> </param>
        /// <param name="args"> </param>
        public TimelineEventArgs(TimelineEventType t, params object[] args)
        {
            Type = t;
            Args = args;
        }

        private TimelineEventType Type { get; set; }
        private object[] Args { get; set; }
    }
}
