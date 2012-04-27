// Project: Launcher
// File: MainView.xaml.cs
// 
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

namespace Launcher.View
{
    /// <summary>
    /// Interaction logic for Main_View.xaml
    /// </summary>
    public partial class MainView
    {
        public static MainView View;

        public MainView()
        {
            InitializeComponent();
            // Insert code required on object creation below this point.
            View = this;
        }
    }
}