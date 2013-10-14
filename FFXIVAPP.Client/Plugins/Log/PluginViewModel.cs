// FFXIVAPP.Client
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

#endregion

namespace FFXIVAPP.Client.Plugins.Log
{
    internal sealed class PluginViewModel : INotifyPropertyChanged
    {
        public event EventHandler<PopupResultEvent> PopupResultChanged = delegate { };

        public void OnPopupResultChanged(PopupResultEvent e)
        {
            PopupResultChanged(this, e);
        }

        #region Property Bindings

        private static PluginViewModel _instance;
        private ObservableCollection<UIElement> _tabs;

        public static PluginViewModel Instance
        {
            get { return _instance ?? (_instance = new PluginViewModel()); }
        }

        public static Dictionary<string, string> PluginInfo
        {
            get
            {
                var pluginInfo = new Dictionary<string, string>();
                pluginInfo.Add("Name", "FFXIVAPP.Plugin.Log");
                pluginInfo.Add("Description", "Final Fantasy XIV Logger & Translator");
                pluginInfo.Add("Copyright", "Copyright © 2013 Ryan Wilson");
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
