// ParseModXIV
// FlowDocument.xaml.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

namespace ParseModXIV.UserControls
{
    /// <summary>
    /// Interaction logic for FlowDocument.xaml
    /// </summary>
    public partial class FlowDocument
    {
        public static FlowDocument View;

        public FlowDocument()
        {
            InitializeComponent();
            // Insert code required on object creation below this point.
            View = this;
        }
    }
}