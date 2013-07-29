// FFXIVAPP.Client
// xMetroWindow.xaml.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System.Windows;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Properties;

#endregion

namespace FFXIVAPP.Client.Windows
{
    /// <summary>
    ///     Interaction logic for xMetroWindow.xaml
    /// </summary>
    public partial class xMetroWindow
    {
        public xMetroWindow()
        {
            InitializeComponent();
        }

        private void XMetroWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            ThemeHelper.ChangeTheme(Settings.Default.Theme, this);
        }
    }
}
