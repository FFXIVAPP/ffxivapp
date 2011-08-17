using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;

namespace ParseModXIV.Stats
{
    public interface IStatContainer
    {
        String Name
        {
            get;
            set;
        }
        ObservableCollection<NumericStat> Stats
        {
            get;
        }

        Boolean HasStat(string name);
        NumericStat GetStat(string name);
        Boolean TryGetStat(string name, out object result);
        Decimal SetOrAddStat(String name, Decimal v);
        Decimal GetStatValue(string name);
        void AddStat(NumericStat s);
        void AddStats(params NumericStat[] stats);
    }

    public abstract class StatContainer : IStatContainer, IDynamicMetaObjectProvider, INotifyCollectionChanged, INotifyPropertyChanged
    {
        public String Name
        {
            get;
            set;
        }
        public abstract ObservableCollection<NumericStat> Stats { get; }

        public abstract Boolean HasStat(string name);
        public abstract NumericStat GetStat(string name);
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

        public Decimal SetOrAddStat(string name, Decimal value)
        {
            NumericStat stat = null;
            if (HasStat(name))
            {
                stat = GetStat(name);
            }
            else
            {
                stat = new NumericStat(name, value);
                AddStat(stat);
            }
            return stat.Value;
        }

        public Decimal GetStatValue(string name)
        {
            /*if(HasStat(name))*/
            return GetStat(name).Value;
            //return 0;
        }

        public void AddStat(NumericStat s)
        {
            AddStats(s);
        }
        public abstract void AddStats(params NumericStat[] stats);

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
                const string methodName = "SetOrUpdateStat";
                var args = new Expression[2];
                args[0] = Expression.Constant(binder.Name);
                args[1] = Expression.Convert(value.Expression, typeof(Decimal));
                var self = Expression.Convert(Expression, LimitType);
                var methodCall = Expression.Call(self, typeof(IStatContainer).GetMethod(methodName), args);
                return new DynamicMetaObject(methodCall, restrictions);
            }

            public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
            {
                const string methodName = "GetStat";
                var args = new Expression[1];
                args[0] = Expression.Constant(binder.Name);
                var self = Expression.Convert(Expression, LimitType);
                var methodCall = Expression.Call(self, typeof(IStatContainer).GetMethod(methodName), args);
                return new DynamicMetaObject(Expression.Convert(methodCall, binder.ReturnType), BindingRestrictions.GetTypeRestriction(Expression, LimitType));
            }

            public override IEnumerable<string> GetDynamicMemberNames()
            {
                var statContainer = (IStatContainer)Value;
                return from stat in statContainer.Stats select stat.Name;
            }
        }

        protected virtual void DoCollectionChanged()
        {
            var handler = CollectionChanged;
            if (handler != null)
            {
                handler(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, this));
            }
        }

        protected virtual void DoPropertyChanged()
        {

        }

        #region Implementation of INotifyCollectionChanged

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}