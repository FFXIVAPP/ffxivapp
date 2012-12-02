// FFXIVAPP.Plugin.Event
// MainView.xaml.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

namespace FFXIVAPP.Plugin.Event.Views
{
    /// <summary>
    ///   Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView
    {
        public static MainView View;

        public MainView()
        {
            InitializeComponent();
            View = this;
        }
    }
}