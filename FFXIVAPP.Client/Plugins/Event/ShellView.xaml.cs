// FFXIVAPP.Client
// ShellView.xaml.cs
// 
// © 2013 Ryan Wilson

using System.Windows;

namespace FFXIVAPP.Client.Plugins.Event {
    /// <summary>
    ///     Interaction logic for DefaultView.xaml
    /// </summary>
    public partial class ShellView {
        #region Declarations

        private bool IsRendered { get; set; }

        #endregion

        public static ShellView View;

        public ShellView() {
            InitializeComponent();
            View = this;
        }

        private void ShellView_OnLoaded(object sender, RoutedEventArgs e) {
            if (IsRendered) {
                return;
            }
            IsRendered = true;
        }
    }
}
