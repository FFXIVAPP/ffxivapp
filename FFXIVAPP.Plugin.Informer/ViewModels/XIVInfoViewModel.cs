// FFXIVAPP.Plugin.Informer
// XIVInfoViewModel.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using FFXIVAPP.Common.Core.Memory;

#endregion

namespace FFXIVAPP.Plugin.Informer.ViewModels
{
    public class XIVInfoViewModel : INotifyPropertyChanged
    {
        #region Property Bindings

        private static XIVInfoViewModel _instance;
        private IList<ActorEntity> _currentMonsters;
        private IList<ActorEntity> _currentNPCs;
        private IList<ActorEntity> _currentPCs;
        private ActorEntity _currentTarget;
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

        public ActorEntity CurrentTarget
        {
            get { return _currentTarget ?? (_currentTarget = new ActorEntity()); }
            set
            {
                _currentTarget = value;
                RaisePropertyChanged();
            }
        }

        public IList<ActorEntity> CurrentNPCs
        {
            get { return _currentNPCs ?? (_currentNPCs = new List<ActorEntity>()); }
            set
            {
                _currentNPCs = value;
                RaisePropertyChanged();
            }
        }

        public IList<ActorEntity> CurrentMonsters
        {
            get { return _currentMonsters ?? (_currentMonsters = new List<ActorEntity>()); }
            set
            {
                _currentMonsters = value;
                RaisePropertyChanged();
            }
        }

        public IList<ActorEntity> CurrentPCs
        {
            get { return _currentPCs ?? (_currentPCs = new List<ActorEntity>()); }
            set
            {
                _currentPCs = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Declarations

        public Timer InfoTimer = new Timer(100);
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
            if (CurrentUser.TargetID > 0)
            {
                if (CurrentMonsters.Any(m => m.ID == CurrentUser.TargetID))
                {
                    CurrentTarget = CurrentMonsters.FirstOrDefault(m => m.ID == CurrentUser.TargetID);
                }
                else if (CurrentNPCs.Any(n => n.NPCID1 == CurrentUser.TargetID))
                {
                    CurrentTarget = CurrentNPCs.FirstOrDefault(n => n.NPCID1 == CurrentUser.TargetID);
                }
                else if (CurrentPCs.Any(p => p.ID == CurrentUser.TargetID))
                {
                    CurrentTarget = CurrentPCs.FirstOrDefault(p => p.ID == CurrentUser.TargetID);
                }
                else
                {
                    CurrentTarget = new ActorEntity
                    {
                        Name = "NotFound"
                    };
                }
            }
            else
            {
                CurrentTarget = new ActorEntity();
            }
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
