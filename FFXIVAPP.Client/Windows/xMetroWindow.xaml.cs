// FFXIVAPP.Client
// xMetroWindow.xaml.cs
// 
// © 2013 Ryan Wilson

using System.Collections.Generic;
using System.Windows;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Properties;
using MahApps.Metro.Controls;

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
            ThemeHelper.ChangeTheme(Settings.Default.Theme, new List<MetroWindow>
            {
                this
            });
        }
    }
}
