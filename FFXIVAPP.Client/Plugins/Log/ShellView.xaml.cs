// FFXIVAPP.Client
// ShellView.xaml.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.Web.UI;
using System.Windows;
using FFXIVAPP.Client.Plugins.Log.Views;

#endregion

namespace FFXIVAPP.Client.Plugins.Log
{
    /// <summary>
    ///     Interaction logic for DefaultView.xaml
    /// </summary>
    public partial class ShellView
    {
        #region Declarations

        private bool IsRendered { get; set; }

        #endregion

        public static ShellView View;

        public ShellView()
        {
            InitializeComponent();
            View = this;
        }

        private void ShellView_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (IsRendered)
            {
                return;
            }
            IsRendered = true;
            PluginInitializer.Log.ApplyTheming();
            ((MainView) MainTI.Content).MainViewTC.SelectedIndex = Constants.Log.PluginSettings.EnableAll ? 0 : 1;
        }
    }
}
