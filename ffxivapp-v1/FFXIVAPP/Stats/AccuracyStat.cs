// FFXIVAPP
// AccuracyStat.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

#region Usings

using System;

#endregion

namespace FFXIVAPP.Stats
{
    public class AccuracyStat : LinkedStat
    {
        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="hitStat"> </param>
        /// <param name="missStat"> </param>
        public AccuracyStat(string name, Stat<decimal> hitStat, Stat<decimal> missStat) : base(name, 0)
        {
            HitStat = hitStat;
            MissStat = missStat;
            SetupDepends(hitStat, missStat);
        }

        private Stat<decimal> HitStat { get; set; }
        private Stat<decimal> MissStat { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="hitStat"> </param>
        /// <param name="missStat"> </param>
        private void SetupDepends(Stat<decimal> hitStat, Stat<decimal> missStat)
        {
            AddDependency(hitStat);
            AddDependency(missStat);
            if (hitStat.Value > 0 && missStat.Value > 0)
            {
                UpdateAccuracy();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="o"> </param>
        /// <param name="n"> </param>
        public override void DoDependencyValueChanged(object sender, object o, object n)
        {
            UpdateAccuracy();
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
    }
}
