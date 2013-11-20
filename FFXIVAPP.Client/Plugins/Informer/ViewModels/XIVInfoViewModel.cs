// FFXIVAPP.Client
// XIVInfoViewModel.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using FFXIVAPP.Client.Delegates;
using FFXIVAPP.Common.Core.Memory;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Plugins.Informer.ViewModels
{
    [DoNotObfuscate]
    public class XIVInfoViewModel : INotifyPropertyChanged
    {
        #region Property Bindings

        private static XIVInfoViewModel _instance;
        private IList<ActorEntity> _currentMonsters;
        private IList<ActorEntity> _currentNPCs;
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

        #endregion

        #region Declarations

        public Timer InfoTimer = new Timer(100);

        #endregion

        public XIVInfoViewModel()
        {
            InfoTimer.Elapsed += InfoTimerOnElapsed;
            InfoTimer.Start();
        }

        private void InfoTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            CurrentUser = MemoryDelegates.Instance.CurrentUser;
            CurrentNPCs = NPCWorkerDelegate.NPCEntries;
            CurrentMonsters = MonsterWorkerDelegate.NPCEntries;
            if (CurrentUser.TargetID > 0)
            {
                if (MonsterWorkerDelegate.NPCEntries.Any(m => m.ID == CurrentUser.TargetID))
                {
                    CurrentTarget = MonsterWorkerDelegate.NPCEntries.FirstOrDefault(m => m.ID == CurrentUser.TargetID);
                }
                else if (NPCWorkerDelegate.NPCEntries.Any(n => n.NPCID1 == CurrentUser.TargetID))
                {
                    CurrentTarget = NPCWorkerDelegate.NPCEntries.FirstOrDefault(n => n.NPCID1 == CurrentUser.TargetID);
                }
                else if (PCWorkerDelegate.NPCEntries.Any(p => p.ID == CurrentUser.TargetID))
                {
                    CurrentTarget = PCWorkerDelegate.NPCEntries.FirstOrDefault(p => p.ID == CurrentUser.TargetID);
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
