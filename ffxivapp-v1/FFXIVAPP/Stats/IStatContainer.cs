// FFXIVAPP
// IStatContainer.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Dynamic;

#endregion

namespace FFXIVAPP.Stats
{
    public interface IStatContainer : ICollection<Stat<decimal>>, INotifyPropertyChanged, INotifyCollectionChanged, IDynamicMetaObjectProvider
    {
        String Name { get; set; }
        Boolean HasStat(string name);
        Stat<Decimal> GetStat(string name);
        Boolean TryGetStat(string name, out object result);
        Decimal SetOrAddStat(String name, Decimal v);
        Decimal GetStatValue(string name);
        void AddStats(params Stat<Decimal>[] stats);
    }
}
