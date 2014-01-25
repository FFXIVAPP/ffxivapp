// FFXIVAPP.Client
// HistoryContainer.cs
// 
// © 2013 Ryan Wilson

using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Models.Parse.History
{
    [DoNotObfuscate]
    public class HistoryContainer : IHistoryContainer
    {
        private readonly ConcurrentDictionary<string, HistoryStat<decimal>> _statDict = new ConcurrentDictionary<string, HistoryStat<decimal>>();

        #region Implementation of IEnumerable

        public IEnumerator<HistoryStat<decimal>> GetEnumerator()
        {
            return _statDict.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public string Name { get; set; }

        public HistoryStat<decimal> GetStat(string name)
        {
            HistoryStat<decimal> result;
            _statDict.TryGetValue(name, out result);
            return result;
        }

        public HistoryStat<decimal> EnsureStatValue(string name, decimal value)
        {
            HistoryStat<decimal> stat;
            if (HasStat(name))
            {
                stat = GetStat(name);
                stat.Value = value;
            }
            else
            {
                stat = new NumericHistoryStat(name, value);
                Add(stat);
            }
            return stat;
        }

        public decimal GetStatValue(string name)
        {
            return HasStat(name) ? GetStat(name)
                .Value : 0;
        }

        #region Implementation of ICollection<Stat<decimal>>

        public void Add(HistoryStat<decimal> stat)
        {
            _statDict.TryAdd(stat.Name, stat);
        }

        public void Clear()
        {
            _statDict.Clear();
        }

        public bool Contains(HistoryStat<decimal> stat)
        {
            return _statDict.ContainsKey(stat.Name);
        }

        public void CopyTo(HistoryStat<decimal>[] array, int arrayIndex)
        {
            _statDict.Values.CopyTo(array, arrayIndex);
        }

        public bool Remove(HistoryStat<decimal> stat)
        {
            HistoryStat<decimal> removed;
            if (_statDict.TryRemove(stat.Name, out removed))
            {
                return true;
            }
            return false;
        }

        public int Count
        {
            get { return _statDict.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        #endregion

        public bool HasStat(string name)
        {
            return _statDict.ContainsKey(name);
        }
    }
}
