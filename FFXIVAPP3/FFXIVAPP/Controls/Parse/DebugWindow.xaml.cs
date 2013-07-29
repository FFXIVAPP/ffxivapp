// FFXIVAPP
// DebugWindow.xaml.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

namespace FFXIVAPP.Controls.Parse
{
    /// <summary>
    ///   Interaction logic for DebugWindow.xaml
    /// </summary>
    public partial class DebugWindow
    {
        public static DebugWindow View;

        public DebugWindow()
        {
            InitializeComponent();
            // Insert code required on object creation below this point.
            View = this;
        }
    }
}