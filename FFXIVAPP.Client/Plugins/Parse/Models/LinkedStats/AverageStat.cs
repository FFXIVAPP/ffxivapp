// FFXIVAPP.Client
// AverageStat.cs
// 
// © 2013 Ryan Wilson

#region Usings

using FFXIVAPP.Client.Plugins.Parse.Models.Stats;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Plugins.Parse.Models.LinkedStats
{
    [DoNotObfuscate]
    public class AverageStat : LinkedStat
    {
        private int _numUpdates;

        public AverageStat(string name, params Stat<decimal>[] dependencies) : base(name, 0m)
        {
            SetupDepends(dependencies[0]);
        }

        public AverageStat(string name, decimal value) : base(name, 0m)
        {
        }

        public AverageStat(string name) : base(name, 0m)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="total"> </param>
        private void SetupDepends(Stat<decimal> total)
        {
            AddDependency(total);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="previousValue"> </param>
        /// <param name="newValue"> </param>
        public override void DoDependencyValueChanged(object sender, object previousValue, object newValue)
        {
            var value = (decimal) newValue;
            Value = value / ++_numUpdates;
        }
    }
}
