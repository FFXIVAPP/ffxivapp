// FFXIVAPP.Plugin.Parse
// NumericStat.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

#region Usings

using FFXIVAPP.Plugin.Parse.Models.Stats;

#endregion

namespace FFXIVAPP.Plugin.Parse.Models.LinkedStats
{
    public class NumericStat : Stat<decimal>
    {
        public NumericStat(string name, decimal value) : base(name, 0m) {}

        public NumericStat(string name) : base(name, 0m) {}
    }
}
