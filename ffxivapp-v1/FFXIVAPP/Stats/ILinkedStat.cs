// FFXIVAPP
// ILinkedStat.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Collections.Generic;

#endregion

namespace FFXIVAPP.Stats
{
    public interface ILinkedStat
    {
        void DoDependencyValueChanged(object sender, object oldValue, object newValue);
        event EventHandler<StatChangedEvent> OnDependencyValueChanged;
        void AddDependency(Stat<Decimal> dependency);
        IEnumerable<Stat<Decimal>> GetDependencies();
    }
}
