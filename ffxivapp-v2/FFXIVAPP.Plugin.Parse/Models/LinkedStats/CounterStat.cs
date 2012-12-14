// FFXIVAPP.Plugin.Parse
// CounterStat.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

namespace FFXIVAPP.Plugin.Parse.Models.LinkedStats
{
    public class CounterStat : NumericStat
    {
        public CounterStat(string name, decimal value) : base(name, 0m)
        {
        }

        public CounterStat(string name) : base(name, 0m)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="amount"> </param>
        /// <returns> </returns>
        private decimal Increment(decimal amount)
        {
            Value += amount;
            return Value;
        }
    }
}
