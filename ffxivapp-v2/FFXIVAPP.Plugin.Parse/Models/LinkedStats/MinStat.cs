// FFXIVAPP.Plugin.Parse
// MinStat.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

using System;
using FFXIVAPP.Plugin.Parse.Models.Stats;

namespace FFXIVAPP.Plugin.Parse.Models.LinkedStats
{
    public sealed class MinStat : LinkedStat
    {
        private bool GotValue { get; set; }

        public MinStat(string name, params Stat<decimal>[] dependencies) : base(name, 0m)
        {
            AddDependency(dependencies[0]);
            GotValue = false;
        }

        public MinStat(string name, decimal value) : base(name, 0m)
        {
        }

        public MinStat(string name) : base(name, 0m)
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
            if ((delta >= Value) && GotValue)
            {
                return;
            }
            Value = delta;
            GotValue = true;
        }
    }
}