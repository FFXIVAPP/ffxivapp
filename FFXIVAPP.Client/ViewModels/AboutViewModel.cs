// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AboutViewModel.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   AboutViewModel.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.ViewModels {
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;

    using FFXIVAPP.Common.Helpers;
    using FFXIVAPP.Common.ViewModelBase;

    internal sealed class AboutViewModel : INotifyPropertyChanged {
        private static Lazy<AboutViewModel> _instance = new Lazy<AboutViewModel>(() => new AboutViewModel());

        public AboutViewModel() {
            this.ManualUpdateCommand = new DelegateCommand(this.ManualUpdate);
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public static AboutViewModel Instance {
            get {
                return _instance.Value;
            }
        }

        public ICommand ManualUpdateCommand { get; private set; }

        /// <summary>
        /// </summary>
        private void ManualUpdate() {
            // TODO: CloseApplication DispatcherHelper.Invoke(() => ShellView.CloseApplication(true), DispatcherPriority.Send);
        }

        private void RaisePropertyChanged([CallerMemberName] string caller = "") {
            this.PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }
    }
}