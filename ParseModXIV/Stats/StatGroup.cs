using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Dynamic;
using System.Linq.Expressions;

namespace ParseModXIV.Stats
{
    public class StatGroup : ICollection<StatGroup>, INotifyCollectionChanged
    {
        protected readonly ConcurrentDictionary<string, StatGroup> Children = new ConcurrentDictionary<string, StatGroup>();
        private readonly ConcurrentDictionary<string, Stat<Decimal>> stats = new ConcurrentDictionary<string, Stat<Decimal>>();
        private readonly StatContainer statList = new StatContainer();
        public String Name
        {
            get; set; 
        }

        public StatContainer Stats
        {
            get { return statList; }
        }

        public StatGroup(string Name, params StatGroup[] children)
        {
            this.Name = Name;
            this.Children = new ConcurrentDictionary<string, StatGroup>(from c in children select new KeyValuePair<string, StatGroup>(c.Name,c));
        }

        public StatGroup(string Name)
        {
            this.Name = Name;
        }

        public void AddGroup(StatGroup child)
        {
            if(Children.TryAdd(child.Name, child)) DoCollectionChanged(NotifyCollectionChangedAction.Add, child);
        }

        public Boolean HasGroup(string name)
        {
            return Children.ContainsKey(name);
        }

        public StatGroup GetGroup(string name)
        {
            StatGroup result;
            return TryGetGroup(name, out result) ? result : null;
        }

        public Boolean TryGetGroup(string name, out StatGroup result)
        {
            StatGroup g;
            if(Children.TryGetValue(name, out g))
            {
                result = (StatGroup) g;
                return true;
            }
            result = null;
            return false;
        }

        public void AddNewSubGroup(String name)
        {
            var sub = new StatGroup(name);
            foreach(var s in Stats)
            {
                if (!(s is LinkedStat)) continue;
                var stat = (LinkedStat) s;
                var constructorInfo = stat.GetType().GetConstructor(new Type[] { typeof(String) });
                if (constructorInfo == null) continue;
                // XXX: Need to actually make a Stat method that allows for a type-specific clone
                var subStat = (NumericStat)constructorInfo.Invoke(new[] { stat.Name });
                if (subStat == null) continue;
                stat.AddDependency(subStat);
                sub.Stats.Add(subStat);
            }
            if(sub.Stats.Any()) AddGroup(sub);
        }


        #region Implementation of IEnumerable

        public IEnumerator<StatGroup> GetEnumerator()
        {
            return Children.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<StatGroup>

        public void Add(StatGroup item)
        {
            AddGroup(item);
        }

        public void Clear()
        {
            Children.Clear();
            DoCollectionChanged(NotifyCollectionChangedAction.Reset, null);
        }

        public bool Contains(StatGroup item)
        {
            return Children.ContainsKey(item.Name);
        }

        public void CopyTo(StatGroup[] array, int arrayIndex)
        {
            Children.Values.CopyTo(array, arrayIndex);
        }

        public bool Remove(StatGroup item)
        {
            StatGroup result;
            if(Children.TryRemove(item.Name, out result))
            {
                DoCollectionChanged(NotifyCollectionChangedAction.Remove, result);
                return true;
            }
            return false;
        }

        public int Count
        {
            get { return Children.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        #endregion

        #region Implementation of INotifyCollectionChanged

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion

        protected virtual void DoCollectionChanged(NotifyCollectionChangedAction action, StatGroup whichGroup)
        {
            var handler = CollectionChanged;
            if (handler != null)
            {
                handler(this, new NotifyCollectionChangedEventArgs(action, whichGroup));
            }
        }
    }
}