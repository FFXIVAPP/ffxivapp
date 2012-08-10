// FFXIVAPP
// Accent.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Windows;

namespace FFXIVAPP.Classes
{
    public class Accent
    {
        public readonly ResourceDictionary Resources;
        public string Name { get; set; }

        public Accent()
        {
        }

        public Accent(string name, Uri resourceAddress)
        {
            Name = name;
            Resources = new ResourceDictionary {Source = resourceAddress};
        }
    }
}