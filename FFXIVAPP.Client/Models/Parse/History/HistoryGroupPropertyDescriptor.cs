// FFXIVAPP.Client
// HistoryGroupPropertyDescriptor.cs
// 
// © 2013 Ryan Wilson

using System;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Models.Parse.History
{
    [DoNotObfuscate]
    public class HistoryGroupPropertyDescriptor : HistoryStatPropertyDescriptor
    {
        public HistoryGroupPropertyDescriptor(string name) : base(name)
        {
        }

        #region Overrides of StatPropertyDescriptor

        public override Type PropertyType
        {
            get { return Name.ToLower() == "name" ? typeof (string) : typeof (HistoryGroup); }
        }

        public override object GetValue(object component)
        {
            if (Name.ToLower() == "name")
            {
                return ((HistoryGroup) component).Name;
            }
            return ((HistoryGroup) component).GetGroup(Name);
        }

        #endregion
    }
}
