// FFXIVAPP.Client
// IStatContainer.cs
// 
// © 2013 Ryan Wilson

using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Models.Parse.Stats
{
    [DoNotObfuscate]
    public interface IStatContainer : ICollection<Stat<decimal>>, INotifyPropertyChanged, INotifyCollectionChanged
    {
        string Name { get; set; }
        bool HasStat(string name);
        Stat<decimal> GetStat(string name);
        bool TryGetStat(string name, out object result);
        decimal SetOrAddStat(string name, decimal value);
        decimal GetStatValue(string name);
        void IncrementStat(string name, decimal value);
        void AddStats(IEnumerable<Stat<decimal>> stats);
    }
}
