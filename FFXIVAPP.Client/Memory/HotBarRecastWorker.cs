// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HotBarRecastWorker.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   HotBarRecastWorker.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.Memory {
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Timers;

    using FFXIVAPP.Client.Helpers;
    using FFXIVAPP.Client.Properties;

    using NLog;

    using Sharlayan;
    using Sharlayan.Models.ReadResults;

    internal class HotBarRecastWorker : INotifyPropertyChanged, IDisposable {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public Stopwatch Stopwatch = new Stopwatch();

        private readonly Timer _scanTimer;

        private bool _isScanning;

        private Reader _reader;

        public HotBarRecastWorker(MemoryHandler memoryHandler) {
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

            double refresh = 100;
            if (double.TryParse(Settings.Default.HotBarRecastWorkerRefresh.ToString(CultureInfo.InvariantCulture), out refresh)) {
                this._scanTimer.Interval = refresh;
            }

            Func<bool> scanner = delegate {
                ActionResult readResult = this._reader.GetActions();

                AppContextHelper.Instance.RaiseActionContainersUpdated(readResult.ActionContainers);

                this._isScanning = false;
                return true;
            };
            scanner.BeginInvoke(delegate { }, scanner);
        }
    }
}