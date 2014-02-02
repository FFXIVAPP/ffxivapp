// FFXIVAPP.Client
// StatGroup.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Models.Parse.Stats
{
    [DoNotObfuscate]
    public class StatGroup : StatGroupTypeDescriptor, ICollection<StatGroup>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        #region Property Bindings

        public string Name { get; set; }
        public bool IncludeSelf { private get; set; }

        #endregion

        #region Declarations

        private readonly ConcurrentDictionary<string, StatGroup> ChildContainer = new ConcurrentDictionary<string, StatGroup>();

        public StatContainer Stats = new StatContainer();

        public List<StatGroup> Children
        {
            get { return new List<StatGroup>(ChildContainer.Values); }
        }

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="children"> </param>
        protected StatGroup(string name, params StatGroup[] children)
        {
            IncludeSelf = true;
            var valuePairs = children.Select(c => new KeyValuePair<string, StatGroup>(c.Name, c));
            ChildContainer = new ConcurrentDictionary<string, StatGroup>(valuePairs);
            DoInit(name);
        }

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        public StatGroup(string name)
        {
            IncludeSelf = true;
            DoInit(name);
        }

        public StatGroup this[int i]
        {
            get { return Children[i]; }
            set { Children[i] = value; }
        }

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        private void DoInit(string name)
        {
            StatGroup = this;
            Name = name;
            Stats.PropertyChanged += (sender, e) => RaisePropertyChanged(e.PropertyName);
        }

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <returns> </returns>
        public bool HasGroup(string name)
        {
            return ChildContainer.ContainsKey(name);
        }

        /// <summary>
        /// </summary>
        /// <param name="child"> </param>
        public void AddGroup(StatGroup child)
        {
            if (ChildContainer.TryAdd(child.Name, child))
            {
                DoCollectionChanged(NotifyCollectionChangedAction.Add, child);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <returns> </returns>
        public StatGroup GetGroup(string name)
        {
            StatGroup result;
            TryGetGroup(name, out result);
            if (result == null)
            {
                AddGroup(new StatGroup(name)
                {
                    IncludeSelf = false
                });
            }
            return TryGetGroup(name, out result) ? result : null;
        }

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="result"> </param>
        /// <returns> </returns>
        public bool TryGetGroup(string name, out StatGroup result)
        {
            StatGroup statGroup;
            if (ChildContainer.TryGetValue(name, out statGroup))
            {
                result = statGroup;
                return true;
            }
            result = null;
            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <returns> </returns>
        public object GetStatValue(string name)
        {
            if (name.ToLower() == "name")
            {
                return Name;
            }
            return Stats.GetStatValue(name);
        }

        #region Implementation of IEnumerable

        public IEnumerator<StatGroup> GetEnumerator()
        {
            var list = new List<StatGroup>();
            if (IncludeSelf)
            {
                list.Add(this);
            }
            list.AddRange(ChildContainer.Values);
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<StatGroup>

        /// <summary>
        /// </summary>
        /// <param name="item"> </param>
        public void Add(StatGroup item)
        {
            AddGroup(item);
        }

        /// <summary>
        /// </summary>
        public virtual void Clear()
        {
            foreach (var statGroup in ChildContainer.Values)
            {
                statGroup.Stats.Clear();
                statGroup.Clear();
            }
            ChildContainer.Clear();
            Stats.Clear();
            foreach (var stat in Stats)
            {
                stat.Reset();
            }
            DoCollectionChanged(NotifyCollectionChangedAction.Reset, null);
        }

        /// <summary>
        /// </summary>
        /// <param name="item"> </param>
        /// <returns> </returns>
        public bool Contains(StatGroup item)
        {
            return ChildContainer.ContainsKey(item.Name);
        }

        /// <summary>
        /// </summary>
        /// <param name="array"> </param>
        /// <param name="arrayIndex"> </param>
        public void CopyTo(StatGroup[] array, int arrayIndex)
        {
            ChildContainer.Values.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// </summary>
        /// <param name="item"> </param>
        /// <returns> </returns>
        public bool Remove(StatGroup item)
        {
            StatGroup result;
            if (ChildContainer.TryRemove(item.Name, out result))
            {
                DoCollectionChanged(NotifyCollectionChangedAction.Remove, result);
                return true;
            }
            return false;
        }

        /// <summary>
        /// </summary>
        public int Count
        {
            get
            {
                var count = ChildContainer.Count();
                if (IncludeSelf)
                {
                    count++;
                }
                return count;
            }
        }

        /// <summary>
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        #endregion

        #region Implementation of INotifyCollectionChanged

        public event NotifyCollectionChangedEventHandler CollectionChanged = delegate { };

        private void DoCollectionChanged(NotifyCollectionChangedAction action, StatGroup statGroup)
        {
            Dispatcher dispatcher = null;
            foreach (var @delegate in CollectionChanged.GetInvocationList())
            {
                var dispatcherObject = @delegate.Target as DispatcherObject;
                if (dispatcherObject == null)
                {
                    continue;
                }
                dispatcher = dispatcherObject.Dispatcher;
                break;
            }
            if (dispatcher != null && dispatcher.CheckAccess() == false)
            {
                dispatcher.Invoke(DispatcherPriority.DataBind, (Action) (() => DoCollectionChanged(action, statGroup)));
            }
            else
            {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, statGroup));
            }
        }

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion
    }
}
