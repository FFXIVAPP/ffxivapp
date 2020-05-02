// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActorWorker.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2020 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   ActorWorker.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.Memory {
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Timers;

    using FFXIVAPP.Client.Helpers;
    using FFXIVAPP.Client.Properties;

    using NLog;

    using Sharlayan;
    using Sharlayan.Models.ReadResults;

    internal class ActorWorker : INotifyPropertyChanged, IDisposable {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public Stopwatch Stopwatch = new Stopwatch();

        private readonly Timer _scanTimer;

        private bool _isScanning;

        public ActorWorker() {
            this._scanTimer = new Timer(100);
            this._scanTimer.Elapsed += this.ScanTimerElapsed;
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public bool MonsterReferencesSet { get; set; }

        public bool NPCReferencesSet { get; set; }

        public bool PCReferencesSet { get; set; }

        public bool ReferencesSet { get; set; }

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
            if (double.TryParse(Settings.Default.ActorWorkerRefresh.ToString(CultureInfo.InvariantCulture), out refresh)) {
                this._scanTimer.Interval = refresh;
            }

            Func<bool> scanner = delegate {
                ActorResult readResult = Reader.GetActors();

                if (!this.MonsterReferencesSet && readResult.CurrentMonsters.Any()) {
                    this.MonsterReferencesSet = true;
                    AppContextHelper.Instance.RaiseMonsterItemsUpdated(readResult.CurrentMonsters);
                }

                if (!this.NPCReferencesSet && readResult.CurrentNPCs.Any()) {
                    this.NPCReferencesSet = true;
                    AppContextHelper.Instance.RaiseNPCItemsUpdated(readResult.CurrentNPCs);
                }

                if (!this.PCReferencesSet && readResult.CurrentPCs.Any()) {
                    this.PCReferencesSet = true;
                    AppContextHelper.Instance.RaisePCItemsUpdated(readResult.CurrentPCs);
                }

                if (this.MonsterReferencesSet && this.NPCReferencesSet && this.PCReferencesSet) {
                    this.ReferencesSet = true;
                }

                if (readResult.NewMonsters.Any()) {
                    AppContextHelper.Instance.RaiseMonsterItemsAdded(readResult.NewMonsters);
                }

                if (readResult.NewNPCs.Any()) {
                    AppContextHelper.Instance.RaiseNPCItemsAdded(readResult.NewNPCs);
                }

                if (readResult.NewPCs.Any()) {
                    AppContextHelper.Instance.RaisePCItemsAdded(readResult.NewPCs);
                }

                if (readResult.RemovedMonsters.Any()) {
                    AppContextHelper.Instance.RaiseMonsterItemsRemoved(readResult.RemovedMonsters);
                }

                if (readResult.RemovedNPCs.Any()) {
                    AppContextHelper.Instance.RaiseNPCItemsRemoved(readResult.RemovedNPCs);
                }

                if (readResult.RemovedPCs.Any()) {
                    AppContextHelper.Instance.RaisePCItemsRemoved(readResult.RemovedPCs);
                }

                this._isScanning = false;
                return true;
            };
            scanner.BeginInvoke(delegate { }, scanner);
        }
    }
}