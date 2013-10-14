// FFXIVAPP.Client
// XIVDBViewModel.cs
// 
// © 2013 Ryan Wilson

using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;

namespace FFXIVAPP.Client.ViewModels {
    [Export(typeof (XIVDBViewModel))]
    internal sealed class XIVDBViewModel : INotifyPropertyChanged {
        #region Property Bindings

        private static XIVDBViewModel _instance;
        private int _killProcessed;
        private int _killSeen;
        private int _lootProcessed;
        private int _lootSeen;
        private int _mobProcessed;
        private int _mobSeen;
        private int _npcProcessed;
        private int _npcSeen;
        private int _playerProcessed;
        private int _playerSeen;

        public static XIVDBViewModel Instance {
            get { return _instance ?? (_instance = new XIVDBViewModel()); }
        }

        public int PlayerSeen {
            get { return _playerSeen; }
            set {
                _playerSeen = value;
                RaisePropertyChanged();
            }
        }

        public int MobSeen {
            get { return _mobSeen; }
            set {
                _mobSeen = value;
                RaisePropertyChanged();
            }
        }

        public int NPCSeen {
            get { return _npcSeen; }
            set {
                _npcSeen = value;
                RaisePropertyChanged();
            }
        }

        public int KillSeen {
            get { return _killSeen; }
            set {
                _killSeen = value;
                RaisePropertyChanged();
            }
        }

        public int LootSeen {
            get { return _lootSeen; }
            set {
                _lootSeen = value;
                RaisePropertyChanged();
            }
        }

        public int PlayerProcessed {
            get { return _playerProcessed; }
            set {
                _playerProcessed = value;
                RaisePropertyChanged();
            }
        }

        public int MobProcessed {
            get { return _mobProcessed; }
            set {
                _mobProcessed = value;
                RaisePropertyChanged();
            }
        }

        public int NPCProcessed {
            get { return _npcProcessed; }
            set {
                _npcProcessed = value;
                RaisePropertyChanged();
            }
        }

        public int KillProcessed {
            get { return _killProcessed; }
            set {
                _killProcessed = value;
                RaisePropertyChanged();
            }
        }

        public int LootProcessed {
            get { return _lootProcessed; }
            set {
                _lootProcessed = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Declarations

        #endregion

        public XIVDBViewModel() {
            PlayerSeen = 0;
            MobSeen = 0;
            NPCSeen = 0;
            KillSeen = 0;
            LootSeen = 0;
            PlayerProcessed = 0;
            MobProcessed = 0;
            NPCProcessed = 0;
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

        private void RaisePropertyChanged([CallerMemberName] string caller = "") {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion
    }
}
