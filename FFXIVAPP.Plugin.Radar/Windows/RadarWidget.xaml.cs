// FFXIVAPP.Plugin.Radar
// RadarWidget.xaml.cs
// 
// © 2013 Ryan Wilson

using System;
using System.ComponentModel;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Plugin.Radar.Interop;
using FFXIVAPP.Plugin.Radar.Properties;

namespace FFXIVAPP.Plugin.Radar.Windows
{
    /// <summary>
    ///     Interaction logic for RadarWidget.xaml
    /// </summary>
    public partial class RadarWidget
    {
        #region Radar Declarations

        private readonly Timer RefreshTimer = new Timer(100);
        public bool IsRendered { get; set; }

        #endregion

        public static RadarWidget View;

        public RadarWidget()
        {
            View = this;
            InitializeComponent();
            View.SourceInitialized += delegate { WinAPI.ToggleClickThrough(this); };
        }

        private void RadarWidget_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (IsRendered)
            {
                return;
            }
            IsRendered = true;
            RefreshTimer.Elapsed += RefreshTimerTick;
            RefreshTimer.Start();
        }

        private void RefreshTimerTick(object sender, EventArgs e)
        {
            if (View.IsVisible)
            {
                DispatcherHelper.Invoke(() => View.RadarControl.Refresh());
            }
        }

        private void TitleBar_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void WidgetClose_OnClick(object sender, RoutedEventArgs e)
        {
            Settings.Default.ShowRadarWidgetOnLoad = false;
            Close();
        }

        private void RadarWidget_OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
