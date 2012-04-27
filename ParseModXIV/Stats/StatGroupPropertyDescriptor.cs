// Project: ParseModXIV
// File: StatGroupPropertyDescriptor.cs
// 
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;

namespace ParseModXIV.Stats
{
    public class StatGroupPropertyDescriptor : StatPropertyDescriptor
    {
        public StatGroupPropertyDescriptor(string name) : base(name)
        {
        }

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
            return ((StatGroup) component).GetGroup(Name);
        }

        public override void ResetValue(object component)
        {
            throw new NotImplementedException();
        }

        public override Type PropertyType
        {
            get { return Name.ToLower() == "name" ? typeof (String) : typeof (StatGroup); }
        }
    }
}