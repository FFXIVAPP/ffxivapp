// ParseModXIV
// Damage.xaml.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using ParseModXIV.Classes;

namespace ParseModXIV.UserControls
{
    /// <summary>
    /// Interaction logic for Damage.xaml
    /// </summary>
    public partial class Damage
    {
        public static Damage View;

        public Damage()
        {
            InitializeComponent();
            View = this;
        }
    }
}