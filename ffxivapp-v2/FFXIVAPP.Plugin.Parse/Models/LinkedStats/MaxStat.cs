// FFXIVAPP.Plugin.Parse
// MaxStat.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using FFXIVAPP.Plugin.Parse.Models.Stats;

#endregion

namespace FFXIVAPP.Plugin.Parse.Models.LinkedStats
{
    public sealed class MaxStat : LinkedStat
    {
        public MaxStat(string name, params Stat<decimal>[] dependencies) : base(name, 0m)
        {
            AddDependency(dependencies[0]);
        }

        public MaxStat(string name, decimal value) : base(name, 0m)
        {
        }

        public MaxStat(string name) : base(name, 0m)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="previousValue"> </param>
        /// <param name="newValue"> </param>
        public override void DoDependencyValueChanged(object sender, object previousValue, object newValue)
        {
            var ovalue = (decimal) previousValue;
            var nvalue = (decimal) newValue;
            var delta = Math.Max(ovalue, nvalue) - Math.Min(ovalue, nvalue);
            if (delta > Value)
            {
                Value = delta;
            }
        }
    }
}
