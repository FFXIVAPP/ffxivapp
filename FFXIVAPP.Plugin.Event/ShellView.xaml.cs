// FFXIVAPP.Plugin.Event
// ShellView.xaml.cs
// 
// © 2013 Ryan Wilson

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
    }
}
