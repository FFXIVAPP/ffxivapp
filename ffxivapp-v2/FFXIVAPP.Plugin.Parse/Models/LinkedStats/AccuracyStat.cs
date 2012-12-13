// FFXIVAPP.Plugin.Parse
// AccuracyStat.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using FFXIVAPP.Plugin.Parse.Models.Stats;

#endregion

namespace FFXIVAPP.Plugin.Parse.Models.LinkedStats
{
    public class AccuracyStat : LinkedStat
    {
        public AccuracyStat(string name, params Stat<decimal>[] dependencies) : base(name, 0m)
        {
            HitStat = dependencies[0];
            MissStat = dependencies[1];
            SetupDepends();
        }

        public AccuracyStat(string name, decimal value) : base(name, 0m) {}

        public AccuracyStat(string name) : base(name, 0m) {}
        private Stat<decimal> HitStat { get; set; }
        private Stat<decimal> MissStat { get; set; }

        /// <summary>
        /// </summary>
        private void SetupDepends()
        {
            AddDependency(HitStat);
            AddDependency(MissStat);
            if (HitStat.Value > 0 && MissStat.Value > 0)
            {
                UpdateAccuracy();
            }
        }

        /// <summary>
        /// </summary>
        private void UpdateAccuracy()
        {
            if (HitStat.Value == 0 && MissStat.Value == 0)
            {
                Value = 0;
                return;
            }
            var total = Convert.ToDouble(HitStat.Value + MissStat.Value);
            Value = Convert.ToDecimal((Convert.ToDouble(HitStat.Value) / total));
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="previousValue"> </param>
        /// <param name="newValue"> </param>
        public override void DoDependencyValueChanged(object sender, object previousValue, object newValue)
        {
            UpdateAccuracy();
        }
    }
}
