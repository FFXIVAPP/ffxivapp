// FFXIVAPP.Client
// NumericHistoryStat.cs
// 
// © 2013 Ryan Wilson

using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Models.Parse.History
{
    [DoNotObfuscate]
    public class NumericHistoryStat : HistoryStat<decimal>
    {
        public NumericHistoryStat(string name, decimal value) : base(name, 0m)
        {
        }

        public NumericHistoryStat(string name) : base(name, 0m)
        {
        }
    }
}
