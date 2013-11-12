// FFXIVAPP.Client
// HistoryControl.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.ComponentModel;
using System.Runtime.CompilerServices;
using FFXIVAPP.Client.Plugins.Parse.Models.Timelines;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Plugins.Parse.Models
{
    [DoNotObfuscate]
    public class HistoryControl : INotifyPropertyChanged
    {
        #region Property Bindings

        private static HistoryControl _instance;
        private Timeline _timeline;

        public static HistoryControl Instance
        {
            get { return _instance ?? (_instance = new HistoryControl()); }
            set { _instance = value; }
        }

        public Timeline Timeline
        {
            get { return _timeline ?? (_timeline = new Timeline(true)); }
            set
            {
                _timeline = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Declarations

        #endregion

        #region Loading Functions

        #endregion

        public HistoryControl()
        {
            Timeline = new Timeline(true);
        }

        #region Utility Functions

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
