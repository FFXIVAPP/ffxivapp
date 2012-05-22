using System.Windows;

using ParseModXIV.Classes;

namespace ParseModXIV.UserControls
{
    /// <summary>
    /// Interaction logic for Healing.xaml
    /// </summary>
    public partial class Healing
    {
        public static Healing View;

        public Healing()
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
