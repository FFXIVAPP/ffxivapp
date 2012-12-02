// FFXIVAPP
// StatGroupTypeDescriptor.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

using System;
using System.ComponentModel;
using System.Linq;

namespace FFXIVAPP.Stats
{
    public abstract class StatGroupTypeDescriptor : CustomTypeDescriptor
    {
        protected StatGroup StatGroup;

        /// <summary>
        /// </summary>
        /// <param name="attributes"> </param>
        /// <returns> </returns>
        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            var props = StatGroup.Stats.Select(p => new StatPropertyDescriptor(p.Name)).Cast<PropertyDescriptor>().ToList();
            props.Add(new StatPropertyDescriptor("Name"));
            props.AddRange(StatGroup.Children.Select(p => new StatGroupPropertyDescriptor(p.Name)));
            return new PropertyDescriptorCollection(props.ToArray());
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public override PropertyDescriptorCollection GetProperties()
        {
            return GetProperties(null);
        }
    }
}