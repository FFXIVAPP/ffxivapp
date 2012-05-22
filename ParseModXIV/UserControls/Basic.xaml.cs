using System.Windows;

using ParseModXIV.Classes;

namespace ParseModXIV.UserControls
{
    /// <summary>
    /// Interaction logic for Basic.xaml
    /// </summary>
    public partial class Basic
    {
        public static Basic View;

        public Basic()
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
