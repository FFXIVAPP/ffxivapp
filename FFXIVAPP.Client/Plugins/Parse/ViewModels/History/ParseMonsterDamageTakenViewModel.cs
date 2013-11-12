// FFXIVAPP.Client
// ParseMonsterDamageTakenViewModel.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.ComponentModel;
using System.Runtime.CompilerServices;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Plugins.Parse.ViewModels.History
{
    [DoNotObfuscate]
    internal sealed class ParseMonsterDamageTakenViewModel : INotifyPropertyChanged
    {
        #region Property Bindings

        private static ParseMonsterDamageTakenViewModel _instance;

        public static ParseMonsterDamageTakenViewModel Instance
        {
            get { return _instance ?? (_instance = new ParseMonsterDamageTakenViewModel()); }
        }

        #endregion

        #region Declarations

        #endregion

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
