// FFXIVAPP.Plugin.Parse
// ParseControl.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FFXIVAPP.Plugin.Parse.Models.Timelines;
using FFXIVAPP.Plugin.Parse.Monitors;
using FFXIVAPP.Plugin.Parse.Utilities;

#endregion

namespace FFXIVAPP.Plugin.Parse.Models
{
    public class ParseControl : INotifyPropertyChanged
    {
        #region Property Bindings

        private static ParseControl _instance;
        private string _lastKilled = "";
        private StatMonitor _statMonitor;
        private Timeline _timeline;
        private TimelineMonitor _timelineMonitor;
        private Dictionary<string, string> _totalA;
        private Dictionary<string, string> _totalD;
        private Dictionary<string, string> _totalDPS;
        private Dictionary<string, string> _totalH;

        public static ParseControl Instance
        {
            get { return _instance ?? (_instance = new ParseControl()); }
        }

        public Timeline Timeline
        {
            get { return _timeline ?? (_timeline = new Timeline()); }
            set
            {
                _timeline = value;
                RaisePropertyChanged();
            }
        }

        public StatMonitor StatMonitor
        {
            get { return _statMonitor ?? (_statMonitor = new StatMonitor(this)); }
            private set
            {
                _statMonitor = value;
                RaisePropertyChanged();
            }
        }

        private TimelineMonitor TimelineMonitor
        {
            get { return _timelineMonitor ?? (_timelineMonitor = new TimelineMonitor(this)); }
            set
            {
                _timelineMonitor = value;
                RaisePropertyChanged();
            }
        }

        public Dictionary<string, string> TotalA
        {
            get { return _totalA ?? (_totalA = new Dictionary<string, string>()); }
            set
            {
                _totalA = value;
                RaisePropertyChanged();
            }
        }

        public Dictionary<string, string> TotalD
        {
            get { return _totalD ?? (_totalD = new Dictionary<string, string>()); }
            set
            {
                _totalD = value;
                RaisePropertyChanged();
            }
        }

        public Dictionary<string, string> TotalH
        {
            get { return _totalH ?? (_totalH = new Dictionary<string, string>()); }
            set
            {
                _totalH = value;
                RaisePropertyChanged();
            }
        }

        public Dictionary<string, string> TotalDPS
        {
            get { return _totalDPS ?? (_totalDPS = new Dictionary<string, string>()); }
            set
            {
                _totalDPS = value;
                RaisePropertyChanged();
            }
        }

        public string LastKilled
        {
            get { return _lastKilled; }
            set
            {
                _lastKilled = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Declarations

        #endregion

        #region Loading Functions

        #endregion

        private ParseControl()
        {
            Timeline = new Timeline();
            TimelineMonitor = new TimelineMonitor(this);
            StatMonitor = new StatMonitor(this);
        }

        #region Utility Functions

        public void Reset()
        {
            StatMonitor.Clear();
        }

        public void Toggle()
        {
            Filter.IsEnabled = !Filter.IsEnabled;
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
