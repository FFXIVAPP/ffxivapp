using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Dynamic;
using System.Linq.Expressions;

namespace ParseModXIV.Stats
{
    public class StatGroup : ICollection<StatGroup>, INotifyCollectionChanged, IDynamicMetaObjectProvider, INotifyPropertyChanged
    {
        protected readonly ConcurrentDictionary<string, StatGroup> Children = new ConcurrentDictionary<string, StatGroup>();
        private readonly StatContainer statList = new StatContainer();
        public String Name
        {
            get; set; 
        }

        public StatContainer Stats
        {
            get { return statList; }
        }

        public StatGroup(string name, params StatGroup[] children)
        {
            this.Children = new ConcurrentDictionary<string, StatGroup>(from c in children select new KeyValuePair<string, StatGroup>(c.Name,c));
            DoInit(name);
        }

        public StatGroup(string name)
        {
            DoInit(name);
        }

        private void DoInit(string name)
        {
            Name = name;
            statList.PropertyChanged += (sender, e) => DoPropertyChanged(e.PropertyName);
        }

        public Decimal GetStatValue(string name)
        {
            return Stats.GetStatValue(name);
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

        public DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return new StatGroupMetaObject(parameter, this);
        }

        protected internal class StatGroupMetaObject : DynamicMetaObject
        {
            internal StatGroupMetaObject(Expression expression, BindingRestrictions restrictions)
                : base(expression, restrictions)
            {
            }

            internal StatGroupMetaObject(Expression expression, StatGroup value)
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
                var methodCall = Expression.Call(self, typeof(StatGroup).GetMethod(methodName), args);
                return new DynamicMetaObject(Expression.Convert(methodCall, binder.ReturnType), BindingRestrictions.GetTypeRestriction(Expression, LimitType));
            }

            public override IEnumerable<string> GetDynamicMemberNames()
            {
                var statContainer = ((StatGroup)Value).Stats;
                return from stat in statContainer
                       select stat.Name;
            }
        }

        #region Implementation of IEnumerable

        public IEnumerator<StatGroup> GetEnumerator()
        {
            var list = new List<StatGroup>();
            list.Add(this);
            list.AddRange(Children.Values);
            return list.GetEnumerator();
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

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void DoPropertyChanged(string name)
        {
            var propChanged = PropertyChanged;
            if(propChanged != null) propChanged(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}