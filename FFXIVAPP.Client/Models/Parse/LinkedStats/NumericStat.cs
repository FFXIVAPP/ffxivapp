// FFXIVAPP.Client
// NumericStat.cs
// 
// © 2013 Ryan Wilson

#region Usings

using FFXIVAPP.Client.Models.Parse.Stats;

#endregion

namespace FFXIVAPP.Client.Models.Parse.LinkedStats
{
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
