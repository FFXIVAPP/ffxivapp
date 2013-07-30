// FFXIVAPP.Plugin.Parse
// StatContainer.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FFXIVAPP.Plugin.Parse.Models.LinkedStats;

#endregion

namespace FFXIVAPP.Plugin.Parse.Models.Stats
{
    public sealed class StatContainer : IStatContainer
    {
        private readonly ConcurrentDictionary<string, Stat<decimal>> _statDict = new ConcurrentDictionary<string, Stat<decimal>>();

        #region Implementation of IEnumerable

        public IEnumerator<Stat<decimal>> GetEnumerator()
        {
            return _statDict.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<Stat<decimal>>

        public void Add(Stat<decimal> stat)
        {
            if (!_statDict.TryAdd(stat.Name, stat))
            {
                return;
            }
            stat.OnValueChanged += HandleStatValueChanged;
            DoCollectionChanged(NotifyCollectionChangedAction.Add, stat);
        }

        public void Clear()
        {
            foreach (var s in _statDict.Values)
            {
                s.OnValueChanged -= HandleStatValueChanged;
            }
            _statDict.Clear();
            DoCollectionChanged(NotifyCollectionChangedAction.Reset, null);
        }

        public bool Contains(Stat<decimal> stat)
        {
            return _statDict.ContainsKey(stat.Name);
        }

        public void CopyTo(Stat<decimal>[] array, int arrayIndex)
        {
            _statDict.Values.CopyTo(array, arrayIndex);
        }

        public bool Remove(Stat<decimal> stat)
        {
            Stat<decimal> removed;
            if (_statDict.TryRemove(stat.Name, out removed))
            {
                removed.OnValueChanged -= HandleStatValueChanged;
                DoCollectionChanged(NotifyCollectionChangedAction.Remove, removed);
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

        private void HandleStatValueChanged(object sender, StatChangedEvent e)
        {
            var stat = (Stat<decimal>) sender;
            RaisePropertyChanged(stat.Name);
        }

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion

        #region Implementation of INotifyCollectionChanged

        public event NotifyCollectionChangedEventHandler CollectionChanged = delegate { };

        private void DoCollectionChanged(NotifyCollectionChangedAction action, Stat<decimal> whichStat)
        {
            CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, whichStat));
        }

        #endregion

        #region Implementation of IStatContainer

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }

        public bool HasStat(string name)
        {
            return _statDict.ContainsKey(name);
        }

        public Stat<decimal> GetStat(string name)
        {
            Stat<decimal> result;
            _statDict.TryGetValue(name, out result);
            return result;
        }

        public bool TryGetStat(string name, out object result)
        {
            if (HasStat(name))
            {
                result = GetStat(name);
                return true;
            }
            result = null;
            return false;
        }

        public decimal SetOrAddStat(string name, decimal value)
        {
            Stat<decimal> stat;
            if (HasStat(name))
            {
                stat = GetStat(name);
                stat.Value = value;
            }
            else
            {
                stat = new NumericStat(name, value);
                Add(stat);
            }
            return stat.Value;
        }

        public decimal GetStatValue(string name)
        {
            return HasStat(name) ? GetStat(name)
                .Value : -1;
        }

        public void IncrementStat(string name, decimal value = 1)
        {
            if (!HasStat(name))
            {
                return;
            }
            var result = GetStat(name);
            result.Value += value;
        }

        public void AddStats(IEnumerable<Stat<decimal>> stats)
        {
            foreach (var stat in stats)
            {
                Add(stat);
            }
        }

        public void ResetAll()
        {
            foreach (var s in _statDict.Values)
            {
                s.Reset();
            }
            DoCollectionChanged(NotifyCollectionChangedAction.Reset, _statDict.Values.First());
        }

        #endregion
    }
}
