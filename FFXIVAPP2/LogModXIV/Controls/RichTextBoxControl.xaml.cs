// LogModXIV
// RichTextBoxControl.xaml.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

namespace LogModXIV.Controls
{
    /// <summary>
    /// Interaction logic for RichTextBox_Control.xaml
    /// </summary>
    public partial class RichTextBoxControl
    {
        public static RichTextBoxControl View;

        public RichTextBoxControl()
        {
            InitializeComponent();
            // Insert code required on object creation below this point.
            View = this;
        }
    }
}