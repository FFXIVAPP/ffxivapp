// ParseModXIV
// StatGroup.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Threading;

namespace ParseModXIV.Stats
{
    public class StatGroup : StatGroupTypeDescriptor, ICollection<StatGroup>, INotifyCollectionChanged, IDynamicMetaObjectProvider, INotifyPropertyChanged
    {
        private readonly ConcurrentDictionary<string, StatGroup> _children = new ConcurrentDictionary<string, StatGroup>();
        public Boolean IncludeSelf { private get; set; }
        private readonly StatContainer _statList = new StatContainer();
        public String Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<StatGroup> Children
        {
            get { return _children.Values; }
        }

        /// <summary>
        /// 
        /// </summary>
        public StatContainer Stats
        {
            get { return _statList; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="children"></param>
        protected StatGroup(string name, params StatGroup[] children)
        {
            IncludeSelf = true;
            _children = new ConcurrentDictionary<string, StatGroup>(from c in children select new KeyValuePair<string, StatGroup>(c.Name, c));
            DoInit(name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public StatGroup(string name)
        {
            IncludeSelf = true;
            DoInit(name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        private void DoInit(string name)
        {
            StatGroup = this;
            Name = name;
            _statList.PropertyChanged += (sender, e) => DoPropertyChanged(e.PropertyName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object GetStatValue(string name)
        {
            if (name.ToLower() == "name")
            {
                return Name;
            }
            return Stats.GetStatValue(name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="child"></param>
        public void AddGroup(StatGroup child)
        {
            if (_children.TryAdd(child.Name, child))
            {
                DoCollectionChanged(NotifyCollectionChangedAction.Add, child);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Boolean HasGroup(string name)
        {
            return _children.ContainsKey(name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public StatGroup GetGroup(string name)
        {
            StatGroup result;
            TryGetGroup(name, out result);
            if (result == null)
            {
                AddGroup(new StatGroup(name) {IncludeSelf = false});
            }
            return TryGetGroup(name, out result) ? result : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public Boolean TryGetGroup(string name, out StatGroup result)
        {
            StatGroup g;
            if (_children.TryGetValue(name, out g))
            {
                result = g;
                return true;
            }
            result = null;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String GetName()
        {
            return Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public void AddNewSubGroup(String name)
        {
            var sub = new StatGroup(name);
            foreach (var s in Stats)
            {
                var stat = s as LinkedStat;
                if (stat == null)
                {
                    continue;
                }
                var constructorInfo = stat.GetType().GetConstructor(new[] {typeof (String)});
                if (constructorInfo == null)
                {
                    continue;
                }
                // XXX: Need to actually make a Stat method that allows for a type-specific clone
                var subStat = (NumericStat) constructorInfo.Invoke(new[] {stat.Name});
                if (subStat == null)
                {
                    continue;
                }
                stat.AddDependency(subStat);
                sub.Stats.Add(subStat);
            }
            if (sub.Stats.Any())
            {
                AddGroup(sub);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return new StatGroupMetaObject(parameter, this);
        }

        /// <summary>
        /// 
        /// </summary>
        private class StatGroupMetaObject : DynamicMetaObject
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="expression"></param>
            /// <param name="restrictions"></param>
            internal StatGroupMetaObject(Expression expression, BindingRestrictions restrictions) : base(expression, restrictions)
            {
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="expression"></param>
            /// <param name="value"></param>
            internal StatGroupMetaObject(Expression expression, StatGroup value) : base(expression, BindingRestrictions.Empty, value)
            {
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="binder"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public override DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value)
            {
                var restrictions = BindingRestrictions.GetTypeRestriction(Expression, LimitType);
                const string methodName = "SetOrAddStat";
                var args = new Expression[2];
                args[0] = Expression.Constant(binder.Name);
                args[1] = Expression.Convert(value.Expression, typeof (Decimal));
                var self = Expression.Convert(Expression, LimitType);
                var methodCall = Expression.Call(self, typeof (IStatContainer).GetMethod(methodName), args);
                return new DynamicMetaObject(methodCall, restrictions);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="binder"></param>
            /// <returns></returns>
            public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
            {
                var self = Expression.Convert(Expression, LimitType);
                var thisGroup = (StatGroup) Value;
                var methodName = thisGroup.Stats.HasStat(binder.Name) ? "GetStatValue" : "GetGroup";
                var args = new Expression[1];
                args[0] = Expression.Constant(binder.Name);
                var methodCall = Expression.Call(self, typeof (StatGroup).GetMethod(methodName), args);
                return new DynamicMetaObject(Expression.Convert(methodCall, binder.ReturnType), BindingRestrictions.GetTypeRestriction(Expression, LimitType));
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override IEnumerable<string> GetDynamicMemberNames()
            {
                var sg = (StatGroup) Value;
                var groupNames = from g in sg select g.Name;
                var statNames = from stat in sg.Stats select stat.Name;
                return statNames.Union(groupNames);
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
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Add(StatGroup item)
        {
            AddGroup(item);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Clear()
        {
            _children.Clear();
            DoCollectionChanged(NotifyCollectionChangedAction.Reset, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(StatGroup item)
        {
            return _children.ContainsKey(item.Name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(StatGroup[] array, int arrayIndex)
        {
            _children.Values.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        public int Count
        {
            get
            {
                var howMany = Children.Count();
                if (IncludeSelf)
                {
                    howMany++;
                }
                return howMany;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        #endregion

        #region Implementation of INotifyCollectionChanged

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion

        private void DoCollectionChanged(NotifyCollectionChangedAction action, StatGroup whichGroup)
        {
            var handler = CollectionChanged;
            if (handler != null)
            {
                MainView.View.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(delegate
                {
                    handler(this, new NotifyCollectionChangedEventArgs(action, whichGroup));
                    return null;
                }), null);
            }
        }

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void DoPropertyChanged(string name)
        {
            var propChanged = PropertyChanged;
            if (propChanged != null)
            {
                propChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion
    }
}