// Project: ParseModXIV
// File: TimelineEventArgs.cs
// 
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;

namespace ParseModXIV.Model
{
    public class TimelineEventArgs : EventArgs
    {
        public TimelineEventType Type { get; private set; }
        public object[] Args { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="args"></param>
        public TimelineEventArgs(TimelineEventType t, params object[] args)
        {
            Type = t;
            Args = args;
        }
    }
}