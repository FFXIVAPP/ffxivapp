// ParseModXIV
// Monster.xaml.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

namespace ParseModXIV.UserControls
{
    /// <summary>
    /// Interaction logic for Monster.xaml
    /// </summary>
    public partial class Monster
    {
        public static Monster View;

        public Monster()
        {
            InitializeComponent();
            // Insert code required on object creation below this point.
            View = this;
        }
    }
}