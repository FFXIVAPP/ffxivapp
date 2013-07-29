// FFXIVAPP.Plugin.Parse
// PercentStat.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using FFXIVAPP.Plugin.Parse.Models.Stats;

#endregion

namespace FFXIVAPP.Plugin.Parse.Models.LinkedStats
{
    public class PercentStat : LinkedStat
    {
        private readonly Stat<decimal> _denominator;
        private readonly Stat<decimal> _numerator;

        public PercentStat(string name, params Stat<decimal>[] dependencies) : base(name, 0m)
        {
            _numerator = dependencies[0];
            _denominator = dependencies[1];
            SetupDepends();
        }

        public PercentStat(string name, decimal value) : base(name, 0m)
        {
        }

        public PercentStat(string name) : base(name, 0m)
        {
        }

        /// <summary>
        /// </summary>
        private void SetupDepends()
        {
            AddDependency(_numerator);
            AddDependency(_denominator);
            if (_numerator.Value > 0 && _denominator.Value > 0)
            {
                UpdatePercent();
            }
        }

        /// <summary>
        /// </summary>
        private void UpdatePercent()
        {
            if (_numerator.Value == 0 || _denominator.Value == 0)
            {
                Value = 0m;
                return;
            }
            Value = (_numerator.Value / _denominator.Value);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="previousValue"> </param>
        /// <param name="newValue"> </param>
        public override void DoDependencyValueChanged(object sender, object previousValue, object newValue)
        {
            UpdatePercent();
        }
    }
}
