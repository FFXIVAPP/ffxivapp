// FFXIVAPP.Client
// PerSecondAverageStat.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using FFXIVAPP.Client.Plugins.Parse.Models.Stats;

#endregion

namespace FFXIVAPP.Client.Plugins.Parse.Models.LinkedStats
{
    public class PerSecondAverageStat : LinkedStat
    {
        public PerSecondAverageStat(string name, params Stat<decimal>[] dependencies) : base(name, 0m)
        {
            FirstEventReceived = DateTime.Now;
            SetupDepends(dependencies[0]);
        }

        public PerSecondAverageStat(string name, decimal value) : base(name, 0m)
        {
        }

        public PerSecondAverageStat(string name) : base(name, 0m)
        {
        }

        private DateTime FirstEventReceived { get; set; }
        private DateTime LastEventReceived { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="dependency"> </param>
        private void SetupDepends(Stat<decimal> dependency)
        {
            AddDependency(dependency);
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
            if (FirstEventReceived == default(DateTime))
            {
                FirstEventReceived = DateTime.Now;
            }
            LastEventReceived = DateTime.Now;
            var timeDifference = Convert.ToDecimal(LastEventReceived.Subtract(FirstEventReceived)
                                                                    .TotalSeconds);
            if (timeDifference >= 1)
            {
                Value = nvalue / timeDifference;
            }
        }
    }
}
