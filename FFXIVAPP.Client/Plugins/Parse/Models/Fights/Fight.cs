// FFXIVAPP.Client
// Fight.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.ComponentModel;
using System.Runtime.CompilerServices;

#endregion

namespace FFXIVAPP.Client.Plugins.Parse.Models.Fights
{
    public sealed class Fight : INotifyPropertyChanged
    {
        #region Property Bindings

        private string _mobName;

        public string MobName
        {
            get { return _mobName; }
            private set
            {
                _mobName = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public Fight(string mobName = "")
        {
            MobName = mobName;
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
