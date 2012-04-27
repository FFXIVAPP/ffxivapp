// Project: Launcher
// File: MainToolBarView.xaml.cs
// 
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System.Windows.Input;

namespace Launcher.View
{
    /// <summary>
    /// Interaction logic for Main_Toolbar_View.xaml
    /// </summary>
    public partial class MainToolBarView
    {
        public static MainToolBarView View;

        public MainToolBarView()
        {
            InitializeComponent();
            // Insert code required on object creation below this point.
            View = this;
        }

        private void LayoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.View.DragMove();
        }
    }
}