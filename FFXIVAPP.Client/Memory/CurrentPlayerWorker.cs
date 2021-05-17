// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CurrentPlayerWorker.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   CurrentPlayerWorker.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.Memory {
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Timers;

    using FFXIVAPP.Client.Helpers;
    using FFXIVAPP.Client.Properties;

    using NLog;

    using Sharlayan;
    using Sharlayan.Models.ReadResults;

    internal class CurrentPlayerWorker : INotifyPropertyChanged, IDisposable {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly Timer _scanTimer;

        private bool _isScanning;

        private Reader _reader;

        public CurrentPlayerWorker(MemoryHandler memoryHandler) {
            this._scanTimer = new Timer(100);
            this._scanTimer.Elapsed += this.ScanTimerElapsed;
            this._reader = memoryHandler.Reader;
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void Dispose() {
            this._scanTimer.Elapsed -= this.ScanTimerElapsed;
        }

        /// <summary>
        /// </summary>
        public void StartScanning() {
            this._scanTimer.Enabled = true;
        }

        /// <summary>
        /// </summary>
        public void StopScanning() {
            this._scanTimer.Enabled = false;
        }

        private void RaisePropertyChanged([CallerMemberName,] string caller = "") {
            this.PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void ScanTimerElapsed(object sender, ElapsedEventArgs e) {
            if (this._isScanning) {
                return;
            }

            this._isScanning = true;

            double refresh = 1000;
            if (double.TryParse(Settings.Default.PlayerInfoWorkerRefresh.ToString(CultureInfo.InvariantCulture), out refresh)) {
                this._scanTimer.Interval = refresh;
            }

            Func<bool> scanner = delegate {
                CurrentPlayerResult readResult = this._reader.GetCurrentPlayer();

                AppContextHelper.Instance.RaiseCurrentUserUpdated(readResult.Entity);
                AppContextHelper.Instance.RaisePlayerInfoUpdated(readResult.PlayerInfo);

                this._isScanning = false;
                return true;
            };
            scanner.BeginInvoke(delegate { }, scanner);
        }
    }
}