// FFXIVAPP.Client
// StatGroupPropertyDescriptor.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;

#endregion

namespace FFXIVAPP.Client.Plugins.Parse.Models.Stats {
    public class StatGroupPropertyDescriptor : StatPropertyDescriptor {
        public StatGroupPropertyDescriptor(string name) : base(name) {}

        #region Overrides of StatPropertyDescriptor

        public override Type PropertyType {
            get { return Name.ToLower() == "name" ? typeof (string) : typeof (StatGroup); }
        }

        public override object GetValue(object component) {
            if (Name.ToLower() == "name") {
                return ((StatGroup) component).Name;
            }
            return ((StatGroup) component).GetGroup(Name);
        }

        #endregion
    }
}
