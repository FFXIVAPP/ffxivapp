// FFXIVAPP.Client
// ParseControl.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FFXIVAPP.Client.Plugins.Parse.Models.Timelines;
using FFXIVAPP.Client.Plugins.Parse.Monitors;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Plugins.Parse.Models
{
    [DoNotObfuscate]
    public class ParseControl : IParsingControl, INotifyPropertyChanged
    {
        #region Auto Properties

        public DateTime StartTime { get; set; }
        public bool IsHistoryBased { get; set; }

        #endregion

        public ParseControl(bool isHistoryBased = false)
        {
            IsHistoryBased = isHistoryBased;
            Timeline = new Timeline(this);
            TimelineMonitor = new TimelineMonitor(this);
            StatMonitor = new StatMonitor(this);
            StartTime = DateTime.Now;
        }

        #region Implementation of IParsingControl

        private static ParseControl _instance;
        private StatMonitor _statMonitor;
        private Timeline _timeline;
        private TimelineMonitor _timelineMonitor;

        public static ParseControl Instance
        {
            get { return _instance ?? (_instance = new ParseControl()); }
            set { _instance = value; }
        }

        IParsingControl IParsingControl.Instance { get { return Instance; } }

        public Timeline Timeline
        {
            get { return _timeline ?? (_timeline = new Timeline(this)); }
            set
            {
                _timeline = value;
                RaisePropertyChanged();
            }
        }

        public StatMonitor StatMonitor
        {
            get { return _statMonitor ?? (_statMonitor = new StatMonitor(this)); }
            set
            {
                _statMonitor = value;
                RaisePropertyChanged();
            }
        }

        public TimelineMonitor TimelineMonitor
        {
            get { return _timelineMonitor ?? (_timelineMonitor = new TimelineMonitor(this)); }
            set
            {
                _timelineMonitor = value;
                RaisePropertyChanged();
            }
        }
        
        public void Initialize()
        {
            
        }

        public void Reset()
        {
            StatMonitor.Clear();
            Timeline.Clear();
            StartTime = DateTime.Now;
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
