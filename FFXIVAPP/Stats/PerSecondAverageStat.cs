// FFXIVAPP
// PerSecondAverageStat.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.
// 

using System;

namespace FFXIVAPP.Stats
{
    public class PerSecondAverageStat : LinkedStat
    {
        private DateTime FirstEventReceived { get; set; }
        private DateTime LastEventReceived { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="value"> </param>
        /// <param name="dependency"> </param>
        public PerSecondAverageStat(string name, decimal value, Stat<Decimal> dependency) : base(name, value)
        {
            FirstEventReceived = DateTime.Now;
            SetupDepends(dependency);
        }

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="dependency"> </param>
        public PerSecondAverageStat(string name, Stat<Decimal> dependency) : base(name, 0)
        {
            FirstEventReceived = DateTime.Now;
            SetupDepends(dependency);
        }

        /// <summary>
        /// </summary>
        /// <param name="dependency"> </param>
        private void SetupDepends(Stat<Decimal> dependency)
        {
            AddDependency(dependency);
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
            if (FirstEventReceived == default(DateTime))
            {
                FirstEventReceived = DateTime.Now;
            }
            LastEventReceived = DateTime.Now;
            var timeDiff = Convert.ToDecimal(LastEventReceived.Subtract(FirstEventReceived).TotalSeconds);
            if (timeDiff >= 1)
            {
                Value = newValue/timeDiff;
            }
        }
    }
}