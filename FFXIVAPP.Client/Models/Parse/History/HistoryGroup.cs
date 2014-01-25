// FFXIVAPP.Client
// HistoryGroup.cs
// 
// © 2013 Ryan Wilson

using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Models.Parse.History
{
    [DoNotObfuscate]
    public class HistoryGroup : HistoryGroupTypeDescriptor, ICollection<HistoryGroup>
    {
        #region Property Bindings

        public string Name { get; set; }

        #endregion

        #region Declarations

        private ConcurrentDictionary<string, HistoryGroup> _childContainer;
        private List<HistoryGroup> _children;
        private HistoryContainer _stats;

        private ConcurrentDictionary<string, HistoryGroup> ChildContainer
        {
            get { return _childContainer ?? (_childContainer = new ConcurrentDictionary<string, HistoryGroup>()); }
            set { _childContainer = value; }
        }

        public List<HistoryGroup> Children
        {
            get { return _children ?? (_children = new List<HistoryGroup>(ChildContainer.Values)); }
            set { _children = value; }
        }

        public HistoryContainer Stats
        {
            get { return _stats ?? (_stats = new HistoryContainer()); }
        }

        #endregion

        public HistoryGroup(string name)
        {
            HistoryGroup = this;
            Name = name;
            Last20DamageActions = new List<LineHistory>();
            Last20DamageTakenActions = new List<LineHistory>();
            Last20HealingActions = new List<LineHistory>();
        }

        public List<LineHistory> Last20DamageActions { get; set; }
        public List<LineHistory> Last20DamageTakenActions { get; set; }
        public List<LineHistory> Last20HealingActions { get; set; }

        public HistoryGroup this[int i]
        {
            get { return Children[i]; }
            set { Children[i] = value; }
        }

        public bool HasGroup(string name)
        {
            return ChildContainer.ContainsKey(name);
        }

        public void AddGroup(HistoryGroup item)
        {
            ChildContainer.TryAdd(item.Name, item);
        }

        public HistoryGroup GetGroup(string name)
        {
            HistoryGroup result;
            TryGetGroup(name, out result);
            if (result == null)
            {
                AddGroup(new HistoryGroup(name));
            }
            return TryGetGroup(name, out result) ? result : null;
        }

        public bool TryGetGroup(string name, out HistoryGroup result)
        {
            HistoryGroup historyGroup;
            if (ChildContainer.TryGetValue(name, out historyGroup))
            {
                result = historyGroup;
                return true;
            }
            result = null;
            return false;
        }

        public object GetStatValue(string name)
        {
            if (name.ToLower() == "name")
            {
                return Name;
            }
            return Stats.GetStatValue(name);
        }

        #region Implementation of ICollection<HistoryGroup>

        public IEnumerator<HistoryGroup> GetEnumerator()
        {
            var list = new List<HistoryGroup>();
            list.AddRange(ChildContainer.Values);
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(HistoryGroup item)
        {
            ChildContainer.TryAdd(item.Name, item);
        }

        public virtual void Clear()
        {
            ChildContainer.Clear();
        }

        public bool Contains(HistoryGroup item)
        {
            return ChildContainer.ContainsKey(item.Name);
        }

        public void CopyTo(HistoryGroup[] array, int arrayIndex)
        {
            ChildContainer.Values.CopyTo(array, arrayIndex);
        }

        public bool Remove(HistoryGroup item)
        {
            HistoryGroup result;
            return ChildContainer.TryRemove(item.Name, out result);
        }

        public int Count
        {
            get { return ChildContainer.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        #endregion
    }
}
