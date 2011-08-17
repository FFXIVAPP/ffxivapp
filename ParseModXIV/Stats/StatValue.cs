using System;
using System.Collections.Generic;
using System.Text;

namespace ParseModXIV.Stats
{
    //public abstract class IStatValue<T>
    //{
    //    public T Value { get; set; }

    //    protected IStatValue(T value)
    //    {
    //        Value = value;
    //    } 

    //    //public override StatContainer this[string name]
    //    //{
    //    //    get
    //    //    {
    //    //        var prop = this.GetType().GetProperty(name, typeof (T));
    //    //        if ((prop == null) || !prop.CanRead)
    //    //        {
    //    //            return null;
    //    //        }
    //    //        else
    //    //        {
    //    //            var constructorArgs = new object[] {prop.GetValue(this, null)};
    //    //            // TODO: this seems really dangerous and error-prone if I happen to make a mistake somewhere
    //    //            return (IStatValue<T>) this.GetType().TypeInitializer.Invoke(constructorArgs);
    //    //        }
    //    //    }
    //    //}
    //}

    //public class NumericStatValue : IStatValue<UInt64>
    //{
    //    public NumericStatValue(UInt64 value) : base(value)
    //    {
    //    }

    //}

    //public class CounterStatValue : NumericStatValue
    //{
    //    public new UInt64 Value { get; protected set; }

    //    public CounterStatValue(UInt64 value) : base(value)
    //    {

    //    }

    //    public CounterStatValue() : base(0)
    //    {

    //    }

    //    public virtual UInt64 Increment()
    //    {
    //        return Increment(1);
    //    }

    //    public virtual UInt64 Increment(UInt64 amount)
    //    {
    //        Value += amount;
    //        return Value;
    //    }
    //}

    //public class TimedCalculatedStatValue : CounterStatValue, IStatContainer
    //{
    //    private UInt64 _max = 0;
    //    public DateTime StartTime { get; set; }

    //    public string Name
    //    {
    //        get
    //        {
    //            throw new NotImplementedException();
    //        }
    //        set
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }

    //    public KeyValuePair<string, IStatValue<object>>[] Stats
    //    {
    //        get
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }

    //    public UInt64 PerSecond
    //    {
    //        get
    //        {
    //            try
    //            {
    //                var timeSinceStart = Convert.ToUInt64(DateTime.Now.Subtract(StartTime).TotalSeconds);
    //                if (timeSinceStart > 0)
    //                {
    //                    return Value/timeSinceStart;
    //                }
    //            } catch(OverflowException)
    //            {
    //                return 0;
    //            }
    //            return 0;
    //        }
    //    }
    //    public UInt64 Max
    //    {
    //        get { return _max; }
    //        protected set { if(value > _max) _max = value;  }
    //    }

    //    public TimedCalculatedStatValue()
    //    {
    //        StartTime = DateTime.Now;
    //    }

    //    public override ulong Increment(UInt64 amount)
    //    {
    //        if(amount > Max)
    //        {
    //            Max = amount;
    //        }
    //        return base.Increment(amount);
    //    }

    //    public bool HasStat(string name)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public IStatValue<object> GetStat(string name)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public bool TryGetStat(string name, out object result)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
