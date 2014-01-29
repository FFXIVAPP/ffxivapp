// FFXIVAPP.Plugin.Radar
// ShellViewModel.cs
// 
// © 2013 Ryan Wilson

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using FFXIVAPP.Plugin.Radar.Interop;
using FFXIVAPP.Plugin.Radar.Properties;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Plugin.Radar
{
    [DoNotObfuscate]
    public sealed class ShellViewModel : INotifyPropertyChanged
    {
        #region Property Bindings

        private static ShellViewModel _instance;

        public static ShellViewModel Instance
        {
            get { return _instance ?? (_instance = new ShellViewModel()); }
        }

        #endregion

        #region Declarations

        #endregion

        public ShellViewModel()
        {
            Initializer.LoadSettings();
            Initializer.SetupWindowTopMost();
            Settings.Default.PropertyChanged += DefaultOnPropertyChanged;
        }

        internal static void Loaded(object sender, RoutedEventArgs e)
        {
            ShellView.View.Loaded -= Loaded;
        }

        private static void DefaultOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            var propertyName = propertyChangedEventArgs.PropertyName;
            switch (propertyName)
            {
                case "WidgetClickThroughEnabled":
                    WinAPI.ToggleClickThrough(Widgets.Instance.RadarWidget);
                    break;
                case "RadarWidgetUIScale":
                    try
                    {
                        Settings.Default.RadarWidgetWidth = (int) (600 * Double.Parse(Settings.Default.RadarWidgetUIScale));
                        Settings.Default.RadarWidgetHeight = (int) (600 * Double.Parse(Settings.Default.RadarWidgetUIScale));
                    }
                    catch (Exception ex)
                    {
                        Settings.Default.RadarWidgetWidth = 600;
                        Settings.Default.RadarWidgetHeight = 600;
                    }
                    break;
            }
        }

        #region Loading Functions

        #endregion

        #region Utility Functions

        #endregion

        #region Command Bindings

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion
    }
}
