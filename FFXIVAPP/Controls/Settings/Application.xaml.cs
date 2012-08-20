// FFXIVAPP
// Application.xaml.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.
// 

namespace FFXIVAPP.Controls.Settings
{
    /// <summary>
    ///   Interaction logic for Application.xaml
    /// </summary>
    public partial class Application
    {
        public static Application View;

        public Application()
        {
            InitializeComponent();
            // Insert code required on object creation below this point.
            View = this;
        }
    }
}