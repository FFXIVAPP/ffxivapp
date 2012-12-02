// FFXIVAPP.Plugin.Chat
// ShellView.xaml.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

namespace FFXIVAPP.Plugin.Chat
{
    /// <summary>
    ///   Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView
    {
        public static ShellView View;

        public ShellView()
        {
            InitializeComponent();
            View = this;
        }
    }
}