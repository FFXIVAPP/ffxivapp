// FFXIVAPP.Plugin.Parse
// MinStat.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using FFXIVAPP.Client.Models.Parse.Stats;

#endregion

namespace FFXIVAPP.Client.Models.Parse.LinkedStats
{
    public sealed class MinStat : LinkedStat
    {
        public MinStat(string name, params Stat<decimal>[] dependencies) : base(name, 0m)
        {
            AddDependency(dependencies[0]);
            GotValue = false;
        }

        public MinStat(string name, decimal value) : base(name, 0m)
        {
        }

        public MinStat(string name) : base(name, 0m)
        {
        }

        private bool GotValue { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="previousValue"> </param>
        /// <param name="newValue"> </param>
        public override void DoDependencyValueChanged(object sender, object previousValue, object newValue)
        {
            var ovalue = (decimal) previousValue;
            var nvalue = (decimal) newValue;
            var delta = Math.Max(ovalue, nvalue) - Math.Min(ovalue, nvalue);
            if ((delta >= Value) && GotValue)
            {
                return;
            }
            Value = delta;
            GotValue = true;
        }
    }
}
