// ParseModXIV
// PercentStat.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;

namespace ParseModXIV.Stats
{
    /// <summary>
    /// Stat which automatically generates a percentage value based on the numerator and denominator
    /// stats that are linked to it.  Any time the linked numerator or denominator stat's value is updated,
    /// the PercentStat value will automatically update with the changes and publish its own <seealso cref="Stat{T}.OnValueChanged">OnValueChanged</seealso> event.
    /// </summary>
    public class PercentStat : LinkedStat
    {
        private readonly Stat<Decimal> _numerator;
        private readonly Stat<Decimal> _denominator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="n"></param>
        /// <param name="d"></param>
        public PercentStat(string name, Stat<Decimal> n, Stat<Decimal> d) : base(name, n, d)
        {
            _numerator = n;
            _denominator = d;
        }

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="o"></param>
        /// <param name="n"></param>
        public override void DoDependencyValueChanged(object sender, object o, object n)
        {
            UpdatePercent();
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdatePercent()
        {
            if (_numerator.Value == 0 || _denominator.Value == 0)
            {
                Value = 0m;
                return;
            }
            Value = (_numerator.Value/_denominator.Value);
        }
    }
}