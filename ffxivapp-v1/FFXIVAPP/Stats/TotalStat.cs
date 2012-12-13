// FFXIVAPP
// TotalStat.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

#region Usings

using System;

#endregion

namespace FFXIVAPP.Stats
{
    public class TotalStat : LinkedStat
    {
        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="dependencies"> </param>
        public TotalStat(string name, params Stat<decimal>[] dependencies) : base(name, dependencies) {}

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        public TotalStat(string name) : base(name) {}

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="value"> </param>
        public TotalStat(string name, Decimal value) : base(name, value) {}

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="oldValue"> </param>
        /// <param name="newValue"> </param>
        public override void DoDependencyValueChanged(object sender, object oldValue, object newValue)
        {
            Value += ((Decimal) newValue - (Decimal) oldValue);
        }
    }
}
