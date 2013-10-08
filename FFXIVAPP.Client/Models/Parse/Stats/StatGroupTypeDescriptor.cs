// FFXIVAPP.Client
// StatGroupTypeDescriptor.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.ComponentModel;
using System.Linq;

#endregion

namespace FFXIVAPP.Client.Models.Parse.Stats
{
    public abstract class StatGroupTypeDescriptor : CustomTypeDescriptor
    {
        protected StatGroup StatGroup;

        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            var descriptors = StatGroup.Stats.Select(stat => new StatPropertyDescriptor(stat.Name))
                                       .Cast<PropertyDescriptor>()
                                       .ToList();
            descriptors.Add(new StatPropertyDescriptor("Name"));
            descriptors.AddRange(StatGroup.Children.Select(p => new StatGroupPropertyDescriptor(p.Name)));
            return new PropertyDescriptorCollection(descriptors.ToArray());
        }

        public override PropertyDescriptorCollection GetProperties()
        {
            return GetProperties(null);
        }
    }
}
