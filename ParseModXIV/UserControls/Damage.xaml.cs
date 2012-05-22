using System.Windows;

using ParseModXIV.Classes;

namespace ParseModXIV.UserControls
{
    /// <summary>
    /// Interaction logic for Damage.xaml
    /// </summary>
    public partial class Damage
    {
        public static Damage View;

        public Damage()
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
