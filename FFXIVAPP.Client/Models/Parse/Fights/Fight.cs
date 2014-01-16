// FFXIVAPP.Client
// Fight.cs
// 
// © 2013 Ryan Wilson

using System.ComponentModel;
using System.Runtime.CompilerServices;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Models.Parse.Fights
{
    [DoNotObfuscate]
    public sealed class Fight : INotifyPropertyChanged
    {
        #region Property Bindings

        private string _monsterName;

        public string MonsterName
        {
            get { return _monsterName; }
            private set
            {
                _monsterName = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public Fight(string monsterName = "")
        {
            MonsterName = monsterName;
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
