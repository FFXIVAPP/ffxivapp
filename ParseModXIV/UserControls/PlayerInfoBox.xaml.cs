// ParseModXIV
// PlayerInfoBox.xaml.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

namespace ParseModXIV.UserControls
{
    /// <summary>
    /// Interaction logic for PlayerInfoBox.xaml
    /// </summary>
    public partial class PlayerInfoBox
    {
        public static PlayerInfoBox View;

        public PlayerInfoBox()
        {
            InitializeComponent();
            // Insert code required on object creation below this point.
            View = this;
        }
    }
}