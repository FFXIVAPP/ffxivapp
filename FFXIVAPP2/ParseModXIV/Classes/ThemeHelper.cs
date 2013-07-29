// ParseModXIV
// ThemeHelper.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Collections.Generic;
using MahApps.Metro;

namespace ParseModXIV.Classes
{
    public class ThemeHelper
    {
        private static IEnumerable<Accent> _accents;

        public static IEnumerable<Accent> DefaultAccents
        {
            get { return _accents ?? (_accents = new List<Accent> {new Accent("Red", new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Red.xaml")), new Accent("Green", new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Green.xaml")), new Accent("Blue", new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Blue.xaml")), new Accent("Purple", new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Purple.xaml")), new Accent("Orange", new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Orange.xaml")),}); }
        }
    }
}