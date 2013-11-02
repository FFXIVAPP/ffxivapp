// FFXIVAPP.Client
// ILinkedStat.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Plugins.Parse.Models.Stats
{
    [DoNotObfuscate]
    public interface ILinkedStat
    {
        void DoDependencyValueChanged(object sender, object previousValue, object newValue);
        event EventHandler<StatChangedEvent> OnDependencyValueChanged;
        void AddDependency(Stat<decimal> dependency);
        IEnumerable<Stat<decimal>> GetDependencies();
    }
}
