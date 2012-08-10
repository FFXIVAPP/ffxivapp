// FFXIVAPP
// StatPropertyDescriptor.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.ComponentModel;

namespace FFXIVAPP.Stats
{
    public class StatPropertyDescriptor : PropertyDescriptor
    {
        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        public StatPropertyDescriptor(string name) : base(name, null)
        {
        }

        #region Overrides of PropertyDescriptor

        /// <summary>
        /// </summary>
        /// <param name="component"> </param>
        /// <returns> </returns>
        public override bool CanResetValue(object component)
        {
            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="component"> </param>
        /// <returns> </returns>
        public override object GetValue(object component)
        {
            if (Name.ToLower() == "name")
            {
                return ((StatGroup) component).Name;
            }
            return ((StatGroup) component).GetStatValue(Name);
        }

        /// <summary>
        /// </summary>
        /// <param name="component"> </param>
        public override void ResetValue(object component)
        {
            var sg = (StatGroup) component;
            if (sg.Stats.HasStat(Name))
            {
                sg.Stats.GetStat(Name).Value = 0;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="component"> </param>
        /// <param name="value"> </param>
        public override void SetValue(object component, object value)
        {
            ((StatGroup) component).Stats.SetOrAddStat(Name, (Decimal) value);
        }

        /// <summary>
        /// </summary>
        /// <param name="component"> </param>
        /// <returns> </returns>
        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

        /// <summary>
        /// </summary>
        public override Type ComponentType
        {
            get { return typeof (StatGroup); }
        }

        /// <summary>
        /// </summary>
        public override bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// </summary>
        public override Type PropertyType
        {
            get { return Name.ToLower() == "name" ? typeof (String) : typeof (Decimal); }
        }

        #endregion
    }
}