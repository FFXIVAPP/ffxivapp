// FFXIVAPP.Plugin.Parse
// IStatContainer.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

#endregion

namespace FFXIVAPP.Plugin.Parse.Models.Stats
{
    public interface IStatContainer : ICollection<Stat<decimal>>, INotifyPropertyChanged, INotifyCollectionChanged
    {
        string Name { get; set; }
        bool HasStat(string name);
        Stat<decimal> GetStat(string name);
        bool TryGetStat(string name, out object result);
        decimal SetOrAddStat(string name, decimal value);
        decimal GetStatValue(string name);
        void AddStats(IEnumerable<Stat<decimal>> stats);
    }
}
