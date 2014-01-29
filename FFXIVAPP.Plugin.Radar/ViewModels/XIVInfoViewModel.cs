// FFXIVAPP.Plugin.Radar
// XIVInfoViewModel.cs
// 
// © 2013 Ryan Wilson

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;
using FFXIVAPP.Common.Core.Memory;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Plugin.Radar.ViewModels
{
    [DoNotObfuscate]
    public class XIVInfoViewModel : INotifyPropertyChanged
    {
        #region Property Bindings

        private static XIVInfoViewModel _instance;
        private ObservableCollection<ActorEntity> _currentMonsters;
        private ObservableCollection<ActorEntity> _currentNPCs;
        private ObservableCollection<ActorEntity> _currentPCs;
        private ActorEntity _currentUser;

        public static XIVInfoViewModel Instance
        {
            get { return _instance ?? (_instance = new XIVInfoViewModel()); }
            set { _instance = value; }
        }

        public ActorEntity CurrentUser
        {
            get { return _currentUser ?? (_currentUser = new ActorEntity()); }
            set
            {
                _currentUser = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<ActorEntity> CurrentNPCs
        {
            get { return _currentNPCs ?? (_currentNPCs = new ObservableCollection<ActorEntity>()); }
            set
            {
                _currentNPCs = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<ActorEntity> CurrentMonsters
        {
            get { return _currentMonsters ?? (_currentMonsters = new ObservableCollection<ActorEntity>()); }
            set
            {
                _currentMonsters = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<ActorEntity> CurrentPCs
        {
            get { return _currentPCs ?? (_currentPCs = new ObservableCollection<ActorEntity>()); }
            set
            {
                _currentPCs = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Declarations

        public readonly Timer InfoTimer = new Timer(100);

        public bool IsProcessing { get; set; }

        #endregion

        public XIVInfoViewModel()
        {
            InfoTimer.Elapsed += InfoTimerOnElapsed;
            InfoTimer.Start();
        }

        private void InfoTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (IsProcessing)
            {
                return;
            }
            IsProcessing = true;
            // do stuff if you have too
            IsProcessing = false;
        }

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion
    }
}
