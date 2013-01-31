// FFXIVAPP.Plugin.Parse
// ILinkedStat.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Collections.Generic;

#endregion

namespace FFXIVAPP.Plugin.Parse.Models.Stats
{
    public interface ILinkedStat
    {
        void DoDependencyValueChanged(object sender, object previousValue, object newValue);
        event EventHandler<StatChangedEvent> OnDependencyValueChanged;
        void AddDependency(Stat<decimal> dependency);
        IEnumerable<Stat<decimal>> GetDependencies();
    }
}
