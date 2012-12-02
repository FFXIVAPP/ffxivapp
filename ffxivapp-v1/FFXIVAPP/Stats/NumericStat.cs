// FFXIVAPP
// NumericStat.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

using System;

namespace FFXIVAPP.Stats
{
    public class NumericStat : Stat<decimal>
    {
        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="value"> </param>
        public NumericStat(string name, Decimal value) : base(name, value)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        public NumericStat(string name) : base(name, 0m)
        {
        }
    }
}