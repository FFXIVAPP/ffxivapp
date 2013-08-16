// FFXIVAPP.Plugin.Event
// PluginViewModel.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FFXIVAPP.Common.Events;
using FFXIVAPP.Common.Models;

#endregion

namespace FFXIVAPP.Plugin.Event
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
        private ObservableCollection<XValuePair> _events;
        private Dictionary<string, string> _locale;
        private ObservableCollection<string> _soundFiles;

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

        public static Dictionary<string, string> PluginInfo
        {
            get
            {
                var pluginInfo = new Dictionary<string, string>();
                pluginInfo.Add("Icon", "Logo.png");
                pluginInfo.Add("Name", Common.Constants.Name);
                pluginInfo.Add("Description", Common.Constants.Description);
                pluginInfo.Add("Copyright", Common.Constants.Copyright);
                pluginInfo.Add("Version", Common.Constants.Version.ToString());
                return pluginInfo;
            }
        }

        public ObservableCollection<XValuePair> Events
        {
            get { return _events ?? (_events = new ObservableCollection<XValuePair>()); }
            set
            {
                _events = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<string> SoundFiles
        {
            get { return _soundFiles ?? (_soundFiles = new ObservableCollection<string>()); }
            set
            {
                _soundFiles = value;
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
