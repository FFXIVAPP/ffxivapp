// FFXIVAPP
// ILinkedStat.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.
// 

using System;
using System.Collections.Generic;

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