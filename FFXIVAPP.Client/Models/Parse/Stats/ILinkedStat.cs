// FFXIVAPP.Plugin.Parse
// ILinkedStat.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;

#endregion

namespace FFXIVAPP.Client.Models.Parse.Stats
{
    public interface ILinkedStat
    {
        void DoDependencyValueChanged(object sender, object previousValue, object newValue);
        event EventHandler<StatChangedEvent> OnDependencyValueChanged;
        void AddDependency(Stat<decimal> dependency);
        IEnumerable<Stat<decimal>> GetDependencies();
    }
}
