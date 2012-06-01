// ParseModXIV
// TabControlView.xaml.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

namespace ParseModXIV.View
{
    /// <summary>
    /// Interaction logic for TabControlViewModel.xaml
    /// </summary>
    public partial class TabControlView
    {
        public static TabControlView View;

        public TabControlView()
        {
            InitializeComponent();
            // Insert code required on object creation below this point.
            View = this;
        }

        //private void List_KeyUp(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Delete)
        //    {
        //        e.Handled = true;
        //    }
        //}
    }
}