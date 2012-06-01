// ParseModXIV
// MainMenuView.xaml.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using ParseModXIV.Classes;

namespace ParseModXIV.View
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView
    {

        public static SettingsView View;

        public SettingsView()
        {
            InitializeComponent();
            // Insert code required on object creation below this point.
            View = this;
        }
    }
}