// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultViewModel.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   DefaultViewModel.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.ViewModels {
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Composition;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;

    using FFXIVAPP.Common.ViewModelBase;

    [Export(typeof(DefaultViewModel)),]
    internal sealed class DefaultViewModel : INotifyPropertyChanged {
        private static Lazy<DefaultViewModel> _instance = new Lazy<DefaultViewModel>(() => new DefaultViewModel());

        public DefaultViewModel() {
            this.DefaultCommand = new DelegateCommand(Default);
            this.DefaultCommandT = new DelegateCommand<object>(DefaultT);
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public static DefaultViewModel Instance {
            get {
                return _instance.Value;
            }
        }

        public ICommand DefaultCommand { get; private set; }

        public ICommand DefaultCommandT { get; private set; }

        /// <summary>
        /// </summary>
        public static void Default() {
            // do something here
        }

        /// <summary>
        /// </summary>
        public static void DefaultT(object parameter) {
            // do something here
        }

        private void RaisePropertyChanged([CallerMemberName,] string caller = "") {
            this.PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }
    }
}