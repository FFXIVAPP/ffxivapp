// FFXIVAPP.Plugin.Parse
// ParsePartyDamageTakenViewModel.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.ComponentModel;
using System.Runtime.CompilerServices;

#endregion

namespace FFXIVAPP.Plugin.Parse.ViewModels
{
    internal sealed class ParsePartyDamageTakenViewModel : INotifyPropertyChanged
    {
        #region Property Bindings

        private static ParsePartyDamageTakenViewModel _instance;

        public static ParsePartyDamageTakenViewModel Instance
        {
            get { return _instance ?? (_instance = new ParsePartyDamageTakenViewModel()); }
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
