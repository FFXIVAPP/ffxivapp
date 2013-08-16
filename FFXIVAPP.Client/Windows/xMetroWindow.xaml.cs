// FFXIVAPP.Client
// xMetroWindow.xaml.cs
// 
// © 2013 Ryan Wilson

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
