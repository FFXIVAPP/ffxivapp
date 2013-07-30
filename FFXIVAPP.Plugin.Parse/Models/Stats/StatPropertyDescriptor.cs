// FFXIVAPP.Plugin.Parse
// StatPropertyDescriptor.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.ComponentModel;

#endregion

namespace FFXIVAPP.Plugin.Parse.Models.Stats
{
    public class StatPropertyDescriptor : PropertyDescriptor
    {
        public StatPropertyDescriptor(string name) : base(name, null)
        {
        }

        #region Overrides of PropertyDescriptor

        public override Type ComponentType
        {
            get { return typeof (StatGroup); }
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
            var statGroup = (StatGroup) component;
            return Name.ToLower() == "name" ? statGroup.Name : statGroup.GetStatValue(Name);
        }

        public override void ResetValue(object component)
        {
            var statGroup = (StatGroup) component;
            if (statGroup.Stats.HasStat(Name))
            {
                statGroup.Stats.GetStat(Name)
                    .Value = 0;
            }
        }

        public override void SetValue(object component, object value)
        {
            var statGroup = (StatGroup) component;
            statGroup.Stats.SetOrAddStat(Name, (decimal) value);
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

        #endregion
    }
}
