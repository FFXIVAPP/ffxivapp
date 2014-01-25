// FFXIVAPP.Client
// IHistoryContainer.cs
// 
// © 2013 Ryan Wilson

using System.Collections.Generic;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Models.Parse.History
{
    [DoNotObfuscate]
    public interface IHistoryContainer : ICollection<HistoryStat<decimal>>
    {
        string Name { get; set; }
        HistoryStat<decimal> GetStat(string name);
        decimal GetStatValue(string name);
        HistoryStat<decimal> EnsureStatValue(string name, decimal value);
    }
}
