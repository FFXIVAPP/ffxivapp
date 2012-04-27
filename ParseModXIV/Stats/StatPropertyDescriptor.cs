// Project: ParseModXIV
// File: StatPropertyDescriptor.cs
// 
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.ComponentModel;

namespace ParseModXIV.Stats
{
    public class StatPropertyDescriptor : PropertyDescriptor
    {
        public StatPropertyDescriptor(string name) : base(name, null)
        {
        }

        #region Overrides of PropertyDescriptor

        public override bool CanResetValue(object component)
        {
            return true;
        }

        public override object GetValue(object component)
        {
            if (Name.ToLower() == "name")
            {
                return ((StatGroup) component).Name;
            }
            return ((StatGroup) component).GetStatValue(Name);
        }

        public override void ResetValue(object component)
        {
            var sg = (StatGroup) component;
            if (sg.Stats.HasStat(Name))
            {
                sg.Stats.GetStat(Name).Value = 0;
            }
        }

        public override void SetValue(object component, object value)
        {
            ((StatGroup) component).Stats.SetOrAddStat(Name, (Decimal) value);
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

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
            get { return Name.ToLower() == "name" ? typeof (String) : typeof (Decimal); }
        }

        #endregion
    }
}