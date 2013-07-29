// ParseModXIV
// MaxStat.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;

namespace ParseModXIV.Stats
{
    public sealed class MaxStat : LinkedStat
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dependency"></param>
        public MaxStat(string name, Stat<decimal> dependency) : base(name, 0)
        {
            AddDependency(dependency);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="o"></param>
        /// <param name="n"></param>
        public override void DoDependencyValueChanged(object sender, object o, object n)
        {
            var oldValue = (Decimal) o;
            var newValue = (Decimal) n;
            var delta = Math.Max(oldValue, newValue) - Math.Min(oldValue, newValue);
            if (delta > Value)
            {
                Value = delta;
            }
        }
    }
}