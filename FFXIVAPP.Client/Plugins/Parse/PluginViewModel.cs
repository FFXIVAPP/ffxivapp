// FFXIVAPP.Client
// PluginViewModel.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FFXIVAPP.Common.Events;

#endregion

namespace FFXIVAPP.Client.Plugins.Parse {
    public sealed class PluginViewModel : INotifyPropertyChanged {
        //used for global static properties

        public event EventHandler<PopupResultEvent> PopupResultChanged = delegate { };

        public void OnPopupResultChanged(PopupResultEvent e) {
            PopupResultChanged(this, e);
        }

        #region Property Bindings

        private static PluginViewModel _instance;
        private bool _enableHelpLabels;
        private Dictionary<string, string> _locale;

        public static PluginViewModel Instance {
            get { return _instance ?? (_instance = new PluginViewModel()); }
        }

        public static Dictionary<string, string> PluginInfo {
            get {
                var pluginInfo = new Dictionary<string, string>();
                pluginInfo.Add("Name", "FFXIVAPP.Plugin.Parse");
                pluginInfo.Add("Description", "Final Fantasy XIV Battle Parser");
                pluginInfo.Add("Copyright", "Copyright © 2013 Ryan Wilson");
                return pluginInfo;
            }
        }

        #endregion

        #region Declarations

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "") {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion
    }
}
