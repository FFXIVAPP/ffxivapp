// FFXIVAPP.Plugin.Log
// PluginViewModel.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using FFXIVAPP.Common.Events;
using FFXIVAPP.Common.Helpers;

#endregion

namespace FFXIVAPP.Plugin.Log
{
    internal sealed class PluginViewModel : INotifyPropertyChanged
    {
        //used for global static properties

        public event EventHandler<PopupResultEvent> PopupResultChanged = delegate { };

        public void OnPopupResultChanged(PopupResultEvent e)
        {
            PopupResultChanged(this, e);
        }

        #region Property Bindings

        private static PluginViewModel _instance;
        private Dictionary<string, string> _locale;
        private bool _enableHelpLabels;
        private ObservableCollection<UIElement> _tabs;

        public static PluginViewModel Instance
        {
            get { return _instance ?? (_instance = new PluginViewModel()); }
        }

        public Dictionary<string, string> Locale
        {
            get { return _locale ?? (_locale = new Dictionary<string, string>()); }
            set
            {
                _locale = value;
                RaisePropertyChanged();
            }
        }

        public bool EnableHelpLabels
        {
            get { return _enableHelpLabels; }
            set
            {
                _enableHelpLabels = value;
                RaisePropertyChanged();
            }
        }

        public static Dictionary<string, string> PluginInfo
        {
            get
            {
                var pluginInfo = new Dictionary<string, string>();
                pluginInfo.Add("Icon", "Logo.png");
                pluginInfo.Add("Name", AssemblyHelper.Name);
                pluginInfo.Add("Description", AssemblyHelper.Description);
                pluginInfo.Add("Copyright", AssemblyHelper.Copyright);
                pluginInfo.Add("Version", AssemblyHelper.Version.ToString());
                return pluginInfo;
            }
        }

        public ObservableCollection<UIElement> Tabs
        {
            get { return _tabs ?? (_tabs = new ObservableCollection<UIElement>()); }
            set
            {
                _tabs = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Declarations

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
