// FFXIVAPP.Client
// AboutViewModel.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FFXIVAPP.Client.Plugins.Parse.Models;
using FFXIVAPP.Client.Plugins.Parse.Models.Stats;
using FFXIVAPP.Common.ViewModelBase;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Plugins.Parse.ViewModels
{
    [DoNotObfuscate]
    internal sealed class HistoryViewModel : INotifyPropertyChanged
    {
        #region Property Bindings

        private static HistoryViewModel _instance;
        private List<ParseHistoryItem> _parseHistory;
        private ParseControl _historyControl;

        public static HistoryViewModel Instance
        {
            get { return _instance ?? (_instance = new HistoryViewModel()); }
        }

        public List<ParseHistoryItem> ParseHistory
        {
            get { return _parseHistory ?? (_parseHistory = new List<ParseHistoryItem>()); }
            set
            {
                if (_parseHistory == null)
                {
                    _parseHistory = new List<ParseHistoryItem>();
                }
                _parseHistory = value;
                RaisePropertyChanged();
            }
        }

        public ParseControl HistoryControl
        {
            get { return _historyControl ?? (_historyControl = new ParseControl()); }
            set
            {
                _historyControl = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Declarations

        public ICommand LoadHistoryItemCommand { get; private set; }

        #endregion

        public HistoryViewModel()
        {
            LoadHistoryItemCommand = new DelegateCommand(LoadHistoryItem);
        }

        #region Loading Functions

        #endregion

        #region Utility Functions

        #endregion

        #region Command Bindings

        public void LoadHistoryItem()
        {
            HistoryControl = new ParseControl();
            if (!ParseHistory.Any())
            {
                return;
            }
            var historyItem = ParseHistory.Last();
            foreach (var stat in historyItem.Overall.Stats)
            {
                HistoryControl.Timeline.Overall.Stats.Add(stat); 
            }
            foreach (var player in historyItem.Party.Children)
            {
                HistoryControl.Timeline.Party.Add(player);
            }
            foreach (var monster in historyItem.Monster.Children)
            {
                HistoryControl.Timeline.Monster.Add(monster);
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
