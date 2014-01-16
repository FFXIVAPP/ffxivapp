// FFXIVAPP.Client
// NumericStat.cs
// 
// © 2013 Ryan Wilson

using FFXIVAPP.Client.Models.Parse.Stats;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Models.Parse.LinkedStats
{
    [DoNotObfuscate]
    public class NumericStat : Stat<decimal>
    {
        public NumericStat(string name, decimal value) : base(name, 0m)
        {
        }

        public NumericStat(string name) : base(name, 0m)
        {
        }
    }
}
