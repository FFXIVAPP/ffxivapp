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
using FFXIVAPP.Client.Models.Parse.LinkedStats;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Models.Parse.Stats
{
    [DoNotObfuscate]
    public class StatGroup : StatGroupTypeDescriptor, ICollection<StatGroup>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        #region Property Bindings

        public string Name { get; set; }
        public bool IncludeSelf { private get; set; }

        public List<StatGroup> Children
        {
            get { return new List<StatGroup>(_children.Values); }
        }

        public StatContainer Stats
        {
            get { return _statList; }
        }

        #endregion

        #region Events

        #endregion

        #region Declarations

        private readonly ConcurrentDictionary<string, StatGroup> _children = new ConcurrentDictionary<string, StatGroup>();
        private readonly StatContainer _statList = new StatContainer();

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="children"> </param>
        protected StatGroup(string name, params StatGroup[] children)
        {
            IncludeSelf = true;
            var valuePairs = children.Select(c => new KeyValuePair<string, StatGroup>(c.Name, c));
            _children = new ConcurrentDictionary<string, StatGroup>(valuePairs);
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
            _statList.PropertyChanged += (sender, e) => RaisePropertyChanged(e.PropertyName);
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

        /// <summary>
        /// </summary>
        /// <param name="child"> </param>
        public void AddGroup(StatGroup child)
        {
            if (_children.TryAdd(child.Name, child))
            {
                DoCollectionChanged(NotifyCollectionChangedAction.Add, child);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <returns> </returns>
        public bool HasGroup(string name)
        {
            return _children.ContainsKey(name);
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
            if (_children.TryGetValue(name, out statGroup))
            {
                result = statGroup;
                return true;
            }
            result = null;
            return false;
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public string GetName()
        {
            return Name;
        }

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        public void AddNewSubGroup(string name)
        {
            var subGroup = new StatGroup(name);
            foreach (var stat in Stats)
            {
                var linkedStat = stat as LinkedStat;
                if (linkedStat == null)
                {
                    continue;
                }
                var constructorInfo = linkedStat.GetType()
                                                .GetConstructor(new[]
                                                {
                                                    typeof (String)
                                                });
                if (constructorInfo == null)
                {
                    continue;
                }
                var subStat = (NumericStat) constructorInfo.Invoke(new object[]
                {
                    linkedStat.Name
                });
                if (subStat == null)
                {
                    continue;
                }
                linkedStat.AddDependency(subStat);
                subGroup.Stats.Add(subStat);
            }
            if (subGroup.Stats.Any())
            {
                AddGroup(subGroup);
            }
        }

        #region Implementation of IEnumerable

        public IEnumerator<StatGroup> GetEnumerator()
        {
            var list = new List<StatGroup>();
            if (IncludeSelf)
            {
                list.Add(this);
            }
            list.AddRange(_children.Values);
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
            _children.Clear();
            foreach (var stat in _statList)
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
            return _children.ContainsKey(item.Name);
        }

        /// <summary>
        /// </summary>
        /// <param name="array"> </param>
        /// <param name="arrayIndex"> </param>
        public void CopyTo(StatGroup[] array, int arrayIndex)
        {
            _children.Values.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// </summary>
        /// <param name="item"> </param>
        /// <returns> </returns>
        public bool Remove(StatGroup item)
        {
            StatGroup result;
            if (_children.TryRemove(item.Name, out result))
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
                var count = Children.Count();
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
