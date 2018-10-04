// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChatLogWorker.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   ChatLogWorker.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.Memory {
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Timers;

    using FFXIVAPP.Client.Helpers;
    using FFXIVAPP.Client.SettingsProviders.Application;
    using NLog;

    using Sharlayan;
    using Sharlayan.Core;
    using Sharlayan.Models.ReadResults;

    internal class ChatLogWorker : INotifyPropertyChanged, IDisposable {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static readonly Logger Tracer = Logger;

        private readonly BackgroundWorker _scanner = new BackgroundWorker();

        private readonly Timer _scanTimer;

        private bool _isScanning;

        private int _previousArrayIndex;

        private int _previousOffset;

        public ChatLogWorker() {
            this._scanTimer = new Timer(250);
            this._scanTimer.Elapsed += this.ScanTimerElapsed;
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private IntPtr ChatPointerMap { get; set; }

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

        private void RaisePropertyChanged([CallerMemberName] string caller = "") {
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

            double refresh = 250;
            if (double.TryParse(Settings.Default.ChatLogWorkerRefresh.ToString(CultureInfo.InvariantCulture), out refresh)) {
                this._scanTimer.Interval = refresh;

                Func<bool> scanner = delegate {
                    ChatLogResult readResult = Reader.GetChatLog(this._previousArrayIndex, this._previousOffset);

                    this._previousArrayIndex = readResult.PreviousArrayIndex;
                    this._previousOffset = readResult.PreviousOffset;

                    foreach (ChatLogItem chatLogEntry in readResult.ChatLogItems) {
                        AppContextHelper.Instance.RaiseChatLogItemReceived(chatLogEntry);
                    }

                    this._isScanning = false;
                    return true;
                };
                System.Threading.Tasks.Task.Run(() => scanner());
            }
        }
    }
}