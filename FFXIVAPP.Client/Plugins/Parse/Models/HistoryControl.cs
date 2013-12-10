// FFXIVAPP.Client
// HistoryControl.cs
// 
// © 2013 Ryan Wilson

using System.ComponentModel;
using System.Runtime.CompilerServices;
using FFXIVAPP.Client.Plugins.Parse.Models.Timelines;
using FFXIVAPP.Client.Plugins.Parse.Monitors;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Plugins.Parse.Models
{
    [DoNotObfuscate]
    public class HistoryControl : INotifyPropertyChanged
    {
        #region Auto Properties

        private ParseControl _controller;

        public ParseControl Controller
        {
            get { return _controller ?? (_controller = new ParseControl(true)); }
            set
            {
                if (_controller == null)
                {
                    _controller = new ParseControl(true);
                }
                _controller = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Implementation of IParsingControl

        private static HistoryControl _instance;

        public static HistoryControl Instance
        {
            get { return _instance ?? (_instance = new HistoryControl()); }
            set { _instance = value; }
        }

        public Timeline Timeline
        {
            get { return Controller.Timeline; }
            set
            {
                Controller.Timeline = value;
                RaisePropertyChanged();
            }
        }

        public StatMonitor StatMonitor
        {
            get { return Controller.StatMonitor; }
            set
            {
                Controller.StatMonitor = value;
                RaisePropertyChanged();
            }
        }

        public TimelineMonitor TimelineMonitor
        {
            get { return Controller.TimelineMonitor; }
            set
            {
                Controller.TimelineMonitor = value;
                RaisePropertyChanged();
            }
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
