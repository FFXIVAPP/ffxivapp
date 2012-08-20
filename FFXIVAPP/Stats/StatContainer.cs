// FFXIVAPP
// StatContainer.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.
// 

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;

namespace FFXIVAPP.Stats
{
    public sealed class StatContainer : IStatContainer
    {
        private readonly ConcurrentDictionary<String, Stat<Decimal>> _statDict = new ConcurrentDictionary<String, Stat<Decimal>>();
        private String _name;

        /// <summary>
        /// </summary>
        public String Name
        {
            get { return _name; }
            set
            {
                _name = value;
                DoPropertyChanged("Name");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <returns> </returns>
        public Boolean HasStat(string name)
        {
            return _statDict.ContainsKey(name);
        }

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <returns> </returns>
        public Stat<Decimal> GetStat(string name)
        {
            Stat<Decimal> result = null;
            _statDict.TryGetValue(name, out result);
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="result"> </param>
        /// <returns> </returns>
        public Boolean TryGetStat(string name, out object result)
        {
            if (HasStat(name))
            {
                result = GetStat(name);
                return true;
            }
            result = null;
            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="value"> </param>
        /// <returns> </returns>
        public Decimal SetOrAddStat(string name, Decimal value)
        {
            Stat<Decimal> stat;
            if (HasStat(name))
            {
                stat = GetStat(name);
            }
            else
            {
                stat = new NumericStat(name, value);
                Add(stat);
            }
            return stat.Value;
        }

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <returns> </returns>
        public Decimal GetStatValue(string name)
        {
            return HasStat(name) ? GetStat(name).Value : -1;
        }

        /// <summary>
        /// </summary>
        /// <param name="stats"> </param>
        public void AddStats(params Stat<Decimal>[] stats)
        {
            foreach (var s in stats)
            {
                Add(s);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void HandleStatValueChanged(object sender, StatChangedEvent e)
        {
            var s = (Stat<Decimal>) sender;
            DoPropertyChanged(s.Name);
        }

        /// <summary>
        /// </summary>
        /// <param name="parameter"> </param>
        /// <returns> </returns>
        public DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return new StatContainerMetaObject(parameter, this);
        }

        private class StatContainerMetaObject : DynamicMetaObject
        {
            /// <summary>
            /// </summary>
            /// <param name="expression"> </param>
            /// <param name="restrictions"> </param>
            internal StatContainerMetaObject(Expression expression, BindingRestrictions restrictions) : base(expression, restrictions)
            {
            }

            /// <summary>
            /// </summary>
            /// <param name="expression"> </param>
            /// <param name="value"> </param>
            internal StatContainerMetaObject(Expression expression, IStatContainer value) : base(expression, BindingRestrictions.Empty, value)
            {
            }

            /// <summary>
            /// </summary>
            /// <param name="binder"> </param>
            /// <param name="value"> </param>
            /// <returns> </returns>
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
            /// </summary>
            /// <param name="binder"> </param>
            /// <returns> </returns>
            public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
            {
                const string methodName = "GetStatValue";
                var args = new Expression[1];
                args[0] = Expression.Constant(binder.Name);
                var self = Expression.Convert(Expression, LimitType);
                var methodCall = Expression.Call(self, typeof (IStatContainer).GetMethod(methodName), args);
                return new DynamicMetaObject(Expression.Convert(methodCall, binder.ReturnType), BindingRestrictions.GetTypeRestriction(Expression, LimitType));
            }

            /// <summary>
            /// </summary>
            /// <returns> </returns>
            public override IEnumerable<string> GetDynamicMemberNames()
            {
                var statContainer = (IStatContainer) Value;
                return from stat in statContainer select stat.Name;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="action"> </param>
        /// <param name="whichStat"> </param>
        private void DoCollectionChanged(NotifyCollectionChangedAction action, Stat<decimal> whichStat)
        {
            var handler = CollectionChanged;
            if (handler != null)
            {
                handler(this, new NotifyCollectionChangedEventArgs(action, whichStat));
            }
        }

        /// <summary>
        /// </summary>
        public void ResetAll()
        {
            foreach (var s in _statDict.Values)
            {
                s.Reset();
            }
            DoCollectionChanged(NotifyCollectionChangedAction.Reset, _statDict.Values.First());
        }

        private void DoPropertyChanged(string whichProp)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(whichProp));
            }
        }

        #region Implementation of INotifyCollectionChanged

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Implementation of IEnumerable

        public IEnumerator<Stat<Decimal>> GetEnumerator()
        {
            return _statDict.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<NumericStat>

        /// <summary>
        /// </summary>
        /// <param name="item"> </param>
        public void Add(Stat<Decimal> item)
        {
            if (!_statDict.TryAdd(item.Name, item))
            {
                return;
            }
            item.OnValueChanged += HandleStatValueChanged;
            DoCollectionChanged(NotifyCollectionChangedAction.Add, item);
        }

        /// <summary>
        /// </summary>
        public void Clear()
        {
            foreach (var s in _statDict.Values)
            {
                s.OnValueChanged -= HandleStatValueChanged;
            }
            _statDict.Clear();
            DoCollectionChanged(NotifyCollectionChangedAction.Reset, null);
        }

        /// <summary>
        /// </summary>
        /// <param name="item"> </param>
        /// <returns> </returns>
        public bool Contains(Stat<Decimal> item)
        {
            return _statDict.ContainsKey(item.Name);
        }

        /// <summary>
        /// </summary>
        /// <param name="array"> </param>
        /// <param name="arrayIndex"> </param>
        public void CopyTo(Stat<Decimal>[] array, int arrayIndex)
        {
            _statDict.Values.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// </summary>
        /// <param name="item"> </param>
        /// <returns> </returns>
        public bool Remove(Stat<Decimal> item)
        {
            Stat<Decimal> removed;
            if (_statDict.TryRemove(item.Name, out removed))
            {
                removed.OnValueChanged -= HandleStatValueChanged;
                DoCollectionChanged(NotifyCollectionChangedAction.Remove, removed);
                return true;
            }
            return false;
        }

        /// <summary>
        /// </summary>
        public int Count
        {
            get { return _statDict.Count; }
        }

        /// <summary>
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        #endregion
    }
}