// FFXIVAPP.Client
// HistoryGroupTypeDescriptor.cs
// 
// © 2013 Ryan Wilson

using System;
using System.ComponentModel;
using System.Linq;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Models.Parse.History
{
    [DoNotObfuscate]
    public abstract class HistoryGroupTypeDescriptor : CustomTypeDescriptor
    {
        protected HistoryGroup HistoryGroup;

        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            var descriptors = HistoryGroup.Stats.Select(stat => new HistoryStatPropertyDescriptor(stat.Name))
                                          .Cast<PropertyDescriptor>()
                                          .ToList();
            descriptors.Add(new HistoryStatPropertyDescriptor("Name"));
            descriptors.AddRange(HistoryGroup.Children.Select(p => new HistoryGroupPropertyDescriptor(p.Name)));
            return new PropertyDescriptorCollection(descriptors.ToArray());
        }

        public override PropertyDescriptorCollection GetProperties()
        {
            return GetProperties(null);
        }
    }
}
