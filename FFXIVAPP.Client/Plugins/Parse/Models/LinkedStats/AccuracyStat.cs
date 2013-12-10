// FFXIVAPP.Client
// AccuracyStat.cs
// 
// © 2013 Ryan Wilson

using System;
using FFXIVAPP.Client.Plugins.Parse.Models.Stats;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Plugins.Parse.Models.LinkedStats
{
    [DoNotObfuscate]
    public class AccuracyStat : LinkedStat
    {
        public AccuracyStat(string name, params Stat<decimal>[] dependencies) : base(name, 0m)
        {
            UsedStat = dependencies[0];
            MissStat = dependencies[1];
            SetupDepends();
        }

        public AccuracyStat(string name, decimal value) : base(name, 0m)
        {
        }

        public AccuracyStat(string name) : base(name, 0m)
        {
        }

        private Stat<decimal> UsedStat { get; set; }
        private Stat<decimal> MissStat { get; set; }

        /// <summary>
        /// </summary>
        private void SetupDepends()
        {
            AddDependency(UsedStat);
            AddDependency(MissStat);
            if (UsedStat.Value > 0 && MissStat.Value > 0)
            {
                UpdateAccuracy();
            }
        }

        /// <summary>
        /// </summary>
        private void UpdateAccuracy()
        {
            if (UsedStat.Value == 0 && MissStat.Value == 0)
            {
                Value = 0;
                return;
            }
            var totalHits = Convert.ToDouble(UsedStat.Value - MissStat.Value);
            if (totalHits > -1)
            {
                Value = Convert.ToDecimal(totalHits / (Convert.ToDouble(UsedStat.Value)));
            }
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
