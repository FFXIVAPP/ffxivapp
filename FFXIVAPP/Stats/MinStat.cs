// FFXIVAPP
// MinStat.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.
// 

using System;

namespace FFXIVAPP.Stats
{
    public sealed class MinStat : LinkedStat
    {
        private Boolean _gotValue;

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="dependency"> </param>
        public MinStat(string name, Stat<decimal> dependency) : base(name, 0)
        {
            AddDependency(dependency);
            _gotValue = false;
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="o"> </param>
        /// <param name="n"> </param>
        public override void DoDependencyValueChanged(object sender, object o, object n)
        {
            var oldValue = (Decimal) o;
            var newValue = (Decimal) n;
            var delta = Math.Max(oldValue, newValue) - Math.Min(oldValue, newValue);
            if ((delta < Value) || !_gotValue)
            {
                Value = delta;
                _gotValue = true;
            }
        }
    }
}