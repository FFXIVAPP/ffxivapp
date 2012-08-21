// FFXIVAPP
// StatChangedEvent.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;

namespace FFXIVAPP.Stats
{
    public class StatChangedEvent : EventArgs
    {
        private Stat<Decimal> SourceStat { get; set; }
        public object OldValue { get; private set; }
        public object NewValue { get; private set; }

        /// <summary>
        /// </summary>
        /// <param name="sourceStat"> </param>
        /// <param name="oldValue"> </param>
        /// <param name="newValue"> </param>
        public StatChangedEvent(object sourceStat, object oldValue, object newValue)
        {
            SourceStat = (Stat<Decimal>) sourceStat;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}