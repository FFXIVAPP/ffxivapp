// FFXIVAPP.Client
// PluginViewModel.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FFXIVAPP.Client.Plugins.Event.Models;

#endregion

namespace FFXIVAPP.Client.Plugins.Event
{
    internal sealed class PluginViewModel : INotifyPropertyChanged
    {
        #region Property Bindings

        private static PluginViewModel _instance;
        private ObservableCollection<SoundEvent> _events;
        private ObservableCollection<string> _soundFiles;

        public static PluginViewModel Instance
        {
            get { return _instance ?? (_instance = new PluginViewModel()); }
        }

        public static Dictionary<string, string> PluginInfo
        {
            get
            {
                var pluginInfo = new Dictionary<string, string>();
                pluginInfo.Add("Name", "FFXIVAPP.Plugin.Event");
                pluginInfo.Add("Description", "Final Fantasy XIV Event Monitor");
                pluginInfo.Add("Copyright", "Copyright © 2013 Ryan Wilson");
                return pluginInfo;
            }
        }

        public ObservableCollection<SoundEvent> Events
        {
            get { return _events ?? (_events = new ObservableCollection<SoundEvent>()); }
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
