// FFXIVAPP.Plugin.Radar
// MainViewModel.cs
// 
// © 2013 Ryan Wilson

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FFXIVAPP.Common.ViewModelBase;
using FFXIVAPP.Plugin.Radar.Properties;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Plugin.Radar.ViewModels
{
    [DoNotObfuscate]
    internal sealed class MainViewModel : INotifyPropertyChanged
    {
        #region Property Bindings

        private static MainViewModel _instance;

        public static MainViewModel Instance
        {
            get { return _instance ?? (_instance = new MainViewModel()); }
        }

        #endregion

        #region Declarations

        public ICommand ResetRadarWidgetCommand { get; private set; }
        public ICommand OpenRadarWidgetCommand { get; private set; }

        #endregion

        #region Loading Functions

        #endregion

        public MainViewModel()
        {
            ResetRadarWidgetCommand = new DelegateCommand(ResetRadarWidget);
            OpenRadarWidgetCommand = new DelegateCommand(OpenRadarWidget);
        }

        #region Utility Functions

        #endregion

        #region Command Bindings

        public void ResetRadarWidget()
        {
            Settings.Default.RadarWidgetUIScale = Settings.Default.Properties["RadarWidgetUIScale"].DefaultValue.ToString();
            Settings.Default.RadarWidgetTop = Int32.Parse(Settings.Default.Properties["RadarWidgetTop"].DefaultValue.ToString());
            Settings.Default.RadarWidgetLeft = Int32.Parse(Settings.Default.Properties["RadarWidgetLeft"].DefaultValue.ToString());
            Settings.Default.RadarWidgetHeight = Int32.Parse(Settings.Default.Properties["RadarWidgetHeight"].DefaultValue.ToString());
            Settings.Default.RadarWidgetWidth = Int32.Parse(Settings.Default.Properties["RadarWidgetWidth"].DefaultValue.ToString());
        }

        public void OpenRadarWidget()
        {
            Settings.Default.ShowRadarWidgetOnLoad = true;
            Widgets.Instance.ShowRadarWidget();
        }

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
