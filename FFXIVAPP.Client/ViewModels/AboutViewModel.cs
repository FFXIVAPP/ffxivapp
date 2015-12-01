// FFXIVAPP.Client
// FFXIVAPP & Related Plugins/Modules
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Threading;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.ViewModelBase;

namespace FFXIVAPP.Client.ViewModels
{
    [Export(typeof (AboutViewModel))]
    internal sealed class AboutViewModel : INotifyPropertyChanged
    {
        public AboutViewModel()
        {
            ManualUpdateCommand = new DelegateCommand(ManualUpdate);
        }

        #region Declarations

        public ICommand ManualUpdateCommand { get; private set; }

        #endregion

        #region Command Bindings

        /// <summary>
        /// </summary>
        private void ManualUpdate()
        {
            DispatcherHelper.Invoke(() => ShellView.CloseApplication(true), DispatcherPriority.Send);
        }

        #endregion

        #region Property Bindings

        private static AboutViewModel _instance;

        public static AboutViewModel Instance
        {
            get { return _instance ?? (_instance = new AboutViewModel()); }
        }

        #endregion

        #region Loading Functions

        #endregion

        #region Utility Functions

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
