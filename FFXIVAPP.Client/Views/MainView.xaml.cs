using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace FFXIVAPP.Client.Views
{
    public class MainView : UserControlBase
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}