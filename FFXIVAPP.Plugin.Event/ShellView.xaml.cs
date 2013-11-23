// FFXIVAPP.Plugin.Event
// ShellView.xaml.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.Windows;

#endregion

namespace FFXIVAPP.Plugin.Event
{
    /// <summary>
    ///     Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView
    {
        public static ShellView View;

        public ShellView()
        {
            InitializeComponent();
            View = this;
        }

        public bool IsRendered { get; set; }

        private void ShellView_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (IsRendered)
            {
                return;
            }
            IsRendered = true;
        }
    }
}
