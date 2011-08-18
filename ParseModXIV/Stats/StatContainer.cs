using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Concurrent;

namespace ParseModXIV.Stats
{
    public interface IStatContainer : ICollection<Stat<Decimal>>, INotifyPropertyChanged, INotifyCollectionChanged
    {
        String Name
        {
            get;
            set;
        }

        Boolean HasStat(string name);
        Stat<Decimal> GetStat(string name);
        Boolean TryGetStat(string name, out object result);
        Decimal SetOrAddStat(String name, Decimal v);
        Decimal GetStatValue(string name);
        void AddStats(params Stat<Decimal>[] stats);
    }

    public class StatContainer : IStatContainer, IDynamicMetaObjectProvider
    {
        private readonly ConcurrentDictionary<String, Stat<Decimal>> statDict = new ConcurrentDictionary<String, Stat<Decimal>>();
        private String _name;
        public String Name
        {
            get { return _name; }
            set
            {
                _name = value;
                DoPropertyChanged("Name");
            }
        }

        public Boolean HasStat(string name)
        {
            return statDict.ContainsKey(name);
        }

        public Stat<Decimal> GetStat(string name)
        {
            Stat<Decimal> result=null;
            statDict.TryGetValue(name, out result);
            return result;
        }
        
        public Boolean TryGetStat(string name, out object result)
        {
            if(HasStat(name))
            {
                result = GetStat(name);
                return true;
            }
            result = null;
            return false;
        }

        public Decimal SetOrAddStat(string name, Decimal value)
        {
            Stat<Decimal> stat = null;
            if(HasStat(name))
            {
                stat = GetStat(name);
            } else
            {
                 stat = new NumericStat(name, value);
                 Add(stat);
            }
            return stat.Value;
        }

        public Decimal GetStatValue(string name)
        {
            return GetStat(name).Value;
        }

        public void AddStats(params Stat<Decimal>[] stats)
        {
            foreach(var s in stats)
            {
                Add(s);
            }
        }

        protected virtual void HandleStatValueChanged(object sender, StatChangedEvent e)
        {
            var s = (NumericStat)sender;
            DoPropertyChanged(s.Name);
        }

        public DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return new StatContainerMetaObject(parameter, this);
        }

        protected class StatContainerMetaObject : DynamicMetaObject
        {
            internal StatContainerMetaObject(Expression expression, BindingRestrictions restrictions)
                : base(expression, restrictions)
            {
            }

            internal StatContainerMetaObject(Expression expression, IStatContainer value)
                : base(expression, BindingRestrictions.Empty, value)
            {
            }

            public override DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value)
            {
                var restrictions = BindingRestrictions.GetTypeRestriction(Expression, LimitType);
                const string methodName = "SetOrAddStat";
                var args = new Expression[2];
                args[0] = Expression.Constant(binder.Name);
                args[1] = Expression.Convert(value.Expression, typeof(Decimal));
                var self = Expression.Convert(Expression, LimitType);
                var methodCall = Expression.Call(self, typeof(IStatContainer).GetMethod(methodName), args);
                return new DynamicMetaObject(methodCall, restrictions);
            }

            public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
            {
                const string methodName = "GetStatValue";
                var args = new Expression[1];
                args[0] = Expression.Constant(binder.Name);
                var self = Expression.Convert(Expression, LimitType);
                var methodCall = Expression.Call(self, typeof(Decimal).GetMethod(methodName), args);
                return new DynamicMetaObject(Expression.Convert(methodCall,binder.ReturnType), BindingRestrictions.GetTypeRestriction(Expression, LimitType));
            }

            public override IEnumerable<string> GetDynamicMemberNames()
            {
                var statContainer = (IStatContainer)Value;
                return from stat in statContainer select stat.Name;
            }
        }

        protected virtual void DoCollectionChanged(NotifyCollectionChangedAction action, Stat<decimal> whichStat)
        {
            var handler = CollectionChanged;
            if(handler != null)
            {
                handler(this, new NotifyCollectionChangedEventArgs(action, whichStat));
            }
        }

        protected virtual void DoPropertyChanged(string whichProp)
        {
            var handler = PropertyChanged;
            if(handler != null)
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
            return statDict.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<NumericStat>

        public void Add(Stat<Decimal> item)
        {
            if(statDict.TryAdd(item.Name, item))
            {
                DoCollectionChanged(NotifyCollectionChangedAction.Add, item);
            }
        }

        public void Clear()
        {
            foreach(var s in statDict.Values)
            {
                s.OnValueChanged -= HandleStatValueChanged;
            }
            statDict.Clear();
            DoCollectionChanged(NotifyCollectionChangedAction.Reset, null);
        }

        public bool Contains(Stat<Decimal> item)
        {
            return statDict.ContainsKey(item.Name);
        }

        public void CopyTo(Stat<Decimal>[] array, int arrayIndex)
        {
            statDict.Values.CopyTo(array, arrayIndex);
        }

        public bool Remove(Stat<Decimal> item)
        {
            Stat<Decimal> removed;
            if(statDict.TryRemove(item.Name, out removed))
            {
                removed.OnValueChanged -= HandleStatValueChanged;
                DoCollectionChanged(NotifyCollectionChangedAction.Remove, removed);
                return true;
            }
            return false;
        }

        public int Count
        {
            get { return statDict.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        #endregion
    }
}