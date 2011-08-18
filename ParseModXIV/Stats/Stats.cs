using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParseModXIV.Stats
{
    public class StatChangedEvent : EventArgs
    {
        public Stat<Decimal> SourceStat
        {
            get; set;
        }
        public object OldValue { get; set; }
        public object NewValue { get; set; }
 
        public StatChangedEvent(object sourceStat, object oldValue, object newValue)
        {
            SourceStat = (Stat<Decimal>)sourceStat;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }

    public abstract class Stat<T>
    {
        private T _value;
        public virtual T Value
        {
            get { return _value;  }
            set {
                var oldVal = Value;
                _value = value;
                var converter = TypeDescriptor.GetConverter(typeof (T));
                if (converter == null) return;
                if(converter.ConvertToString(oldVal) != converter.ConvertToString(_value))
                {
                    DoValueChanged(this,oldVal,Value);
                }
            }
        }

        public String Name
        {
            get; set; 
        }

        public virtual event EventHandler<StatChangedEvent> OnValueChanged;

        protected Stat()
        {
            Name = "";
            _value = default(T);
        }

        protected Stat(String name)
        {
            Name = name;
            _value = default(T);
        }

        protected Stat(String name, T value)
        {
            Name = name;
            _value = value;
        } 

        protected virtual void DoValueChanged(Stat<T> src, T oldValue, T newValue)
        {
            var changedEvent = OnValueChanged;
            if (changedEvent != null) changedEvent(this, new StatChangedEvent(this, oldValue, newValue));
        }
    }

    public interface ILinkedStat
    {
        void DoDependencyValueChanged(object sender, object oldValue, object newValue);
        event EventHandler<StatChangedEvent> OnDependencyValueChanged;
        void AddDependency(Stat<Decimal> dependency);
        IEnumerable<Stat<Decimal>> GetDependencies();
    }

    public abstract class LinkedStat : Stat<Decimal>, ILinkedStat
    {
        protected List<Stat<Decimal>> dependencyList = new List<Stat<decimal>>();
 
        protected LinkedStat(String name, params Stat<Decimal>[] dependencies) : base(name)
        {
            foreach(var d in dependencies) AddDependency(d);
        }

        protected LinkedStat(String name, Decimal value) : base(name,value)
        {
        }

        protected LinkedStat(String name) : base(name, 0)
        {
        }

        public event EventHandler<StatChangedEvent> OnDependencyValueChanged;

        public void AddDependency(Stat<Decimal> dependency)
        {
            dependency.OnValueChanged += dependency_OnValueChanged;
        }

        public IEnumerable<Stat<Decimal>> GetDependencies()
        {
            return dependencyList.AsReadOnly();
        }

        protected void dependency_OnValueChanged(object sender, StatChangedEvent e)
        {
            var onChange = OnDependencyValueChanged;
            if (onChange != null) onChange(this, new StatChangedEvent(sender, e.OldValue, e.NewValue));
            DoDependencyValueChanged(sender, e.OldValue, e.NewValue);
        }

        public virtual void DoDependencyValueChanged(object sender, object oldValue, object newValue)
        {
            Value = (Decimal)newValue;
        }
    }

    public class NumericStat : Stat<Decimal>
    {
        public NumericStat(string name, Decimal value) : base(name,value)
        {
        }

        public NumericStat(string name) : base(name,0)
        {
        }
    }

    public class CounterStat : NumericStat
    {
        public CounterStat(String name, Decimal value) : base(name,value)
        {
        }

        public CounterStat(String name) : base(name)
        {
        }

        public Decimal Increment()
        {
            return Increment(1);
        }

        public Decimal Increment(Decimal amount)
        {
            Value += amount;
            return Value;
        }
    }

    public class TotalStat : LinkedStat
    {
        public TotalStat(string name, params Stat<Decimal>[] dependencies) : base(name,dependencies)
        {
        }

        public TotalStat(string name) : base(name)
        {
            
        }

        public TotalStat(string name, Decimal value) : base(name, value)
        {
        }

        public override void DoDependencyValueChanged(object sender, object oldValue, object newValue)
        {
            
            Value += ((Decimal)newValue - (Decimal)oldValue);
        }
    }

    public class MaxStat : LinkedStat
    {
        public MaxStat(string name, NumericStat dependency) : base(name,0)
        {
            AddDependency(dependency);
        }

        public override void DoDependencyValueChanged(object sender, object o, object n)
        {
            var oldValue = (Decimal) o;
            var newValue = (Decimal) n;
            var delta = Math.Max(oldValue, newValue) - Math.Min(oldValue, newValue);
            if (delta > Value) Value = delta;
        }
    }

    public class MinStat : LinkedStat
    {
        private Boolean gotValue;
        public MinStat(string name, NumericStat dependency) : base(name,0)
        {
            AddDependency(dependency);
            gotValue = false;
        }

        public override void DoDependencyValueChanged(object sender, object o, object n)
        {
            var oldValue = (Decimal)o;
            var newValue = (Decimal)n;
            var delta = Math.Max(oldValue, newValue) - Math.Min(oldValue, newValue);
            if ((delta < Value) || !gotValue)
            {
                Value = delta;
                gotValue = true;
            }
        }
    }

    public class PerSecondAverageStat : LinkedStat
    {
        public DateTime FirstEventReceived { get; set; }
        public DateTime LastEventReceived { get; set; }
        protected Decimal total = 0;

        public PerSecondAverageStat(string name, decimal value, NumericStat dependency) : base(name, value)
        {
            FirstEventReceived = DateTime.Now;
            AddDependency(dependency);
        }

        public PerSecondAverageStat(string name, NumericStat dependency) : base(name,0)
        {
            FirstEventReceived = DateTime.Now;
            AddDependency(dependency);
        }

        public override void DoDependencyValueChanged(object sender, object o, object n)
        {
            var oldValue = (Decimal)o;
            var newValue = (Decimal)n;
            total += newValue;
            if (FirstEventReceived == default(DateTime)) FirstEventReceived = DateTime.Now;
            LastEventReceived = DateTime.Now;
            var timeDiff = Convert.ToDecimal(LastEventReceived.Subtract(FirstEventReceived).TotalSeconds);
            if (timeDiff >= 1) Value = total/timeDiff;
        }

    }

    public class AccuracyStat : LinkedStat
    {
        public NumericStat HitStat
        {
            get; set;
        }
        public NumericStat MissStat
        {
            get; set;
        }

        public AccuracyStat(string name, NumericStat hitStat, NumericStat missStat) : base(name, 0)
        {
            HitStat = hitStat;
            MissStat = missStat;
            AddDependency(hitStat);
            AddDependency(missStat);
            if(hitStat.Value > 0 && missStat.Value > 0)
            {
                UpdateAccuracy();
            }
        }

        public override void DoDependencyValueChanged(object sender, object o, object n)
        {
            var oldValue = (Decimal)o;
            var newValue = (Decimal)n;
            if ((HitStat.Value <= 0) && (Value != 0)) Value = 0;
            else UpdateAccuracy();
        }

        private void UpdateAccuracy()
        {
            if (HitStat.Value == 0 && MissStat.Value == 0)
            {
                Value = 0;
                return;
            }
            var total = Convert.ToDouble(HitStat.Value + MissStat.Value);
            Value = Convert.ToDecimal((Convert.ToDouble(HitStat.Value) / total) * 100);            
        }
    }
}
