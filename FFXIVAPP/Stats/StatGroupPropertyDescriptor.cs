// FFXIVAPP
// StatGroupPropertyDescriptor.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.
// 

using System;

namespace FFXIVAPP.Stats
{
    public class StatGroupPropertyDescriptor : StatPropertyDescriptor
    {
        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        public StatGroupPropertyDescriptor(string name) : base(name)
        {
        }

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
            return ((StatGroup) component).GetGroup(Name);
        }

        /// <summary>
        /// </summary>
        /// <param name="component"> </param>
        public override void ResetValue(object component)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        public override Type PropertyType
        {
            get { return Name.ToLower() == "name" ? typeof (String) : typeof (StatGroup); }
        }
    }
}