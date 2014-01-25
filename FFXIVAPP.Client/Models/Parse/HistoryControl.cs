// FFXIVAPP.Client
// HistoryControl.cs
// 
// © 2013 Ryan Wilson

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FFXIVAPP.Client.Models.Parse.History;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Models.Parse
{
    [DoNotObfuscate]
    public class HistoryControl : INotifyPropertyChanged
    {
        private static HistoryControl _instance;
        private HistoryTimeline _timeline;

        public HistoryControl()
        {
            Timeline = new HistoryTimeline();
        }

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion

        public static HistoryControl Instance
        {
            get { return _instance ?? (_instance = new HistoryControl()); }
            set { _instance = value; }
        }

        public HistoryTimeline Timeline
        {
            get { return _timeline ?? (_timeline = new HistoryTimeline()); }
            set { _timeline = value; }
        }

        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
