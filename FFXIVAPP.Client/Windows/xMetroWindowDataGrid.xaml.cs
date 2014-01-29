// FFXIVAPP.Client
// xMetroWindowDataGrid.xaml.cs
// 
// © 2013 Ryan Wilson

using System.Collections.Generic;
using System.Windows;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Common.Helpers;
using MahApps.Metro.Controls;

namespace FFXIVAPP.Client.Windows
{
    /// <summary>
    ///     Interaction logic for xMetroWindowDataGrid.xaml
    /// </summary>
    public partial class xMetroWindowDataGrid
    {
        public xMetroWindowDataGrid()
        {
            InitializeComponent();
        }

        private void XMetroWindowDataGrid_OnLoaded(object sender, RoutedEventArgs e)
        {
            ThemeHelper.ChangeTheme(Settings.Default.Theme, new List<MetroWindow>
            {
                this
            });
        }
    }
}
