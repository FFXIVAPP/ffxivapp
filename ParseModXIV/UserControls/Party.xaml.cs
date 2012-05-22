using System.Windows;

using ParseModXIV.Classes;

namespace ParseModXIV.UserControls
{
    /// <summary>
    /// Interaction logic for Party.xaml
    /// </summary>
    public partial class Party
    {
        public static Party View;

        public Party()
        {
            InitializeComponent();
            View = this;
        }

        private void Sort(object sender, RoutedEventArgs e)
        {
            SortHandler.Handler(sender, e);
        }
    }
}
