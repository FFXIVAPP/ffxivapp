// FFXIVAPP.Client
// DPSWidget.xaml.cs
// 
// © 2013 Ryan Wilson

using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace FFXIVAPP.Client.Plugins.Parse.WidgetWindows
{
    /// <summary>
    ///     Interaction logic for DPSWidget.xaml
    /// </summary>
    public partial class DPSWidget : Window
    {
        public DPSWidget()
        {
            InitializeComponent();
        }

        private void DPSWidget_OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void UIElement_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
