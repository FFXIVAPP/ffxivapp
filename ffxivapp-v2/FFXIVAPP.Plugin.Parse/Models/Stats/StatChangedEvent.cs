// FFXIVAPP.Plugin.Parse
// StatChangedEvent.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

using System;

namespace FFXIVAPP.Plugin.Parse.Models.Stats
{
    public class StatChangedEvent : EventArgs
    {
        #region Property Bindings

        private Stat<decimal> SourceStat { get; set; }
        public object PreviousValue { get; private set; }
        public object NewValue { get; private set; }

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="sourceStat"> </param>
        /// <param name="previousValue"> </param>
        /// <param name="newValue"> </param>
        public StatChangedEvent(object sourceStat, object previousValue, object newValue)
        {
            SourceStat = (Stat<decimal>) sourceStat;
            PreviousValue = previousValue;
            NewValue = newValue;
        }
    }
}