// FFXIVAPP.Plugin.Parse
// AverageStat.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

using FFXIVAPP.Plugin.Parse.Models.Stats;

namespace FFXIVAPP.Plugin.Parse.Models.LinkedStats
{
    public class AverageStat : LinkedStat
    {
        private int _numUpdates;

        public AverageStat(string name, params Stat<decimal>[] dependencies) : base(name, 0m)
        {
            SetupDepends(dependencies[0]);
        }

        public AverageStat(string name, decimal value) : base(name, 0m)
        {
        }

        public AverageStat(string name) : base(name, 0m)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="total"> </param>
        private void SetupDepends(Stat<decimal> total)
        {
            AddDependency(total);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="previousValue"> </param>
        /// <param name="newValue"> </param>
        public override void DoDependencyValueChanged(object sender, object previousValue, object newValue)
        {
            var value = (decimal) newValue;
            Value = value/++_numUpdates;
        }
    }
}