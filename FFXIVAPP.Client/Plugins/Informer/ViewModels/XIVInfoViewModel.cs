// FFXIVAPP.Client
// XIVInfoViewModel.cs
// 
// © 2013 Ryan Wilson

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using FFXIVAPP.Client.Delegates;
using FFXIVAPP.Client.Memory;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Plugins.Informer.ViewModels
{
    [DoNotObfuscate]
    public class XIVInfoViewModel : INotifyPropertyChanged
    {
        #region Property Bindings

        private static XIVInfoViewModel _instance;
        private IList<NPCEntry> _currentMonsters;
        private IList<NPCEntry> _currentNPCs;
        private NPCEntry _currentTarget;
        private NPCEntry _currentUser;

        public static XIVInfoViewModel Instance
        {
            get { return _instance ?? (_instance = new XIVInfoViewModel()); }
            set { _instance = value; }
        }

        public NPCEntry CurrentUser
        {
            get { return _currentUser ?? (_currentUser = new NPCEntry()); }
            set
            {
                _currentUser = value;
                RaisePropertyChanged();
            }
        }

        public NPCEntry CurrentTarget
        {
            get { return _currentTarget ?? (_currentTarget = new NPCEntry()); }
            set
            {
                _currentTarget = value;
                RaisePropertyChanged();
            }
        }

        public IList<NPCEntry> CurrentNPCs
        {
            get { return _currentNPCs ?? (_currentNPCs = new List<NPCEntry>()); }
            set
            {
                _currentNPCs = value;
                RaisePropertyChanged();
            }
        }

        public IList<NPCEntry> CurrentMonsters
        {
            get { return _currentMonsters ?? (_currentMonsters = new List<NPCEntry>()); }
            set
            {
                _currentMonsters = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Declarations

        public Timer InfoTimer = new Timer(1000);

        #endregion

        public XIVInfoViewModel()
        {
            InfoTimer.Elapsed += InfoTimerOnElapsed;
            InfoTimer.Start();
        }

        private void InfoTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            CurrentUser = MonsterWorkerDelegate.CurrentUser;
            if (CurrentUser.TargetID > 0)
            {
                if (NPCWorkerDelegate.NPCEntries.Any())
                {
                    if (NPCWorkerDelegate.NPCEntries.Any(n => n.ID == CurrentUser.TargetID))
                    {
                        CurrentTarget = NPCWorkerDelegate.NPCEntries.FirstOrDefault(n => n.ID == CurrentUser.TargetID);
                    }
                }
                if (MonsterWorkerDelegate.NPCEntries.Any())
                {
                    if (MonsterWorkerDelegate.NPCEntries.Any(m => m.ID == CurrentUser.TargetID))
                    {
                        CurrentTarget = MonsterWorkerDelegate.NPCEntries.FirstOrDefault(m => m.ID == CurrentUser.TargetID);
                    }
                }
            }
            else
            {
                CurrentTarget = new NPCEntry();
            }
            CurrentNPCs = NPCWorkerDelegate.NPCEntries;
            CurrentMonsters = MonsterWorkerDelegate.NPCEntries;
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
