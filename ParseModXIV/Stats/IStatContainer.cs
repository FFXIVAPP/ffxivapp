// Project: ParseModXIV
// File: IStatContainer.cs
// 
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Dynamic;

namespace ParseModXIV.Stats
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