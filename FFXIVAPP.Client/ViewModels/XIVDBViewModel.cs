// FFXIVAPP.Client
// XIVDBViewModel.cs
// 
// © 2013 Ryan Wilson

using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.ViewModels
{
    [DoNotObfuscate]
    [Export(typeof (XIVDBViewModel))]
    internal sealed class XIVDBViewModel : INotifyPropertyChanged
    {
        #region Property Bindings

        private static XIVDBViewModel _instance;
        private int _killProcessed;
        private int _killSeen;
        private bool _killUploadEnabled;
        private int _lootProcessed;
        private int _lootSeen;
        private bool _lootUploadEnabled;
        private int _monsterProcessed;
        private int _monsterSeen;
        private bool _monsterUploadEnabled;
        private int _npcProcessed;
        private int _npcSeen;
        private bool _npcUploadEnabled;
        private int _pcProcessed;
        private int _pcSeen;
        private bool _pcUploadEnabled;

        public static XIVDBViewModel Instance
        {
            get { return _instance ?? (_instance = new XIVDBViewModel()); }
        }

        public int MonsterSeen
        {
            get { return _monsterSeen; }
            set
            {
                _monsterSeen = value;
                RaisePropertyChanged();
            }
        }

        public int NPCSeen
        {
            get { return _npcSeen; }
            set
            {
                _npcSeen = value;
                RaisePropertyChanged();
            }
        }

        public int PCSeen
        {
            get { return _pcSeen; }
            set
            {
                _pcSeen = value;
                RaisePropertyChanged();
            }
        }

        public int KillSeen
        {
            get { return _killSeen; }
            set
            {
                _killSeen = value;
                RaisePropertyChanged();
            }
        }

        public int LootSeen
        {
            get { return _lootSeen; }
            set
            {
                _lootSeen = value;
                RaisePropertyChanged();
            }
        }

        public int MonsterProcessed
        {
            get { return _monsterProcessed; }
            set
            {
                _monsterProcessed = value;
                RaisePropertyChanged();
            }
        }

        public int NPCProcessed
        {
            get { return _npcProcessed; }
            set
            {
                _npcProcessed = value;
                RaisePropertyChanged();
            }
        }

        public int PCProcessed
        {
            get { return _pcProcessed; }
            set
            {
                _pcProcessed = value;
                RaisePropertyChanged();
            }
        }

        public int KillProcessed
        {
            get { return _killProcessed; }
            set
            {
                _killProcessed = value;
                RaisePropertyChanged();
            }
        }

        public int LootProcessed
        {
            get { return _lootProcessed; }
            set
            {
                _lootProcessed = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Declarations

        public bool MonsterUploadEnabled
        {
            get { return _monsterUploadEnabled; }
            set
            {
                _monsterUploadEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool NPCUploadEnabled
        {
            get { return _npcUploadEnabled; }
            set
            {
                _npcUploadEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool PCUploadEnabled
        {
            get { return _pcUploadEnabled; }
            set
            {
                _pcUploadEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool KillUploadEnabled
        {
            get { return _killUploadEnabled; }
            set
            {
                _killUploadEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool LootUploadEnabled
        {
            get { return _lootUploadEnabled; }
            set
            {
                _lootUploadEnabled = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public XIVDBViewModel()
        {
            MonsterSeen = 0;
            NPCSeen = 0;
            PCSeen = 0;
            KillSeen = 0;
            LootSeen = 0;
            MonsterProcessed = 0;
            NPCProcessed = 0;
            PCProcessed = 0;
            KillProcessed = 0;
            LootProcessed = 0;
        }

        #region Loading Functions

        #endregion

        #region Utility Functions

        #endregion

        #region Command Bindings

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
