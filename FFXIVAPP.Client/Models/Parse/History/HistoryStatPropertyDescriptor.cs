// FFXIVAPP.Client
// HistoryStatPropertyDescriptor.cs
// 
// © 2013 Ryan Wilson

using System;
using System.ComponentModel;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Models.Parse.History
{
    [DoNotObfuscate]
    public class HistoryStatPropertyDescriptor : PropertyDescriptor
    {
        public HistoryStatPropertyDescriptor(string name) : base(name, null)
        {
        }

        public override Type ComponentType
        {
            get { return typeof (HistoryGroup); }
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override Type PropertyType
        {
            get { return Name.ToLower() == "name" ? typeof (string) : typeof (decimal); }
        }

        public override bool CanResetValue(object component)
        {
            return true;
        }

        public override object GetValue(object component)
        {
            var historyGroup = (HistoryGroup) component;
            return Name.ToLower() == "name" ? historyGroup.Name : historyGroup.GetStatValue(Name);
        }

        public override void ResetValue(object component)
        {
            var historyGroup = (HistoryGroup) component;
            if (historyGroup.Stats.HasStat(Name))
            {
                historyGroup.Stats.GetStat(Name)
                            .Value = 0;
            }
        }

        public override void SetValue(object component, object value)
        {
            var historyGroup = (HistoryGroup) component;
            historyGroup.Stats.EnsureStatValue(Name, (decimal) value);
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
    }
}
