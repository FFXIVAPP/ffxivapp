// FFXIVAPP.Client
// ILinkedStat.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Models.Parse.Stats
{
    [DoNotObfuscate]
    public interface ILinkedStat
    {
        event EventHandler<StatChangedEvent> OnDependencyValueChanged;
        void DoDependencyValueChanged(object sender, object previousValue, object newValue);
        void AddDependency(Stat<decimal> dependency);
        void RemoveDependency(Stat<decimal> dependency);
        IEnumerable<Stat<decimal>> GetDependencies();
        IEnumerable<Stat<decimal>> CloneDependentStats();
        void ClearDependencies();
    }
}
