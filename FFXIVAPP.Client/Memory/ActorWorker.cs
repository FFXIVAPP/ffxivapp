// FFXIVAPP.Client ~ ActorWorker.cs
// 
// Copyright © 2007 - 2017 Ryan Wilson - All Rights Reserved
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

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Memory;
using NLog;

namespace FFXIVAPP.Client.Memory
{
    internal class ActorWorker : INotifyPropertyChanged, IDisposable
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        public ActorWorker()
        {
            _scanTimer = new Timer(100);
            _scanTimer.Elapsed += ScanTimerElapsed;
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            _scanTimer.Elapsed -= ScanTimerElapsed;
        }

        #endregion

        #region Property Bindings

        public bool ReferencesSet { get; set; }
        public bool PCReferencesSet { get; set; }
        public bool NPCReferencesSet { get; set; }
        public bool MonsterReferencesSet { get; set; }

        #endregion

        #region Declarations

        private readonly Timer _scanTimer;
        private bool _isScanning;

        #endregion

        #region Timer Controls

        /// <summary>
        /// </summary>
        public void StartScanning()
        {
            _scanTimer.Enabled = true;
        }

        /// <summary>
        /// </summary>
        public void StopScanning()
        {
            _scanTimer.Enabled = false;
        }

        #endregion

        #region Threads

        public Stopwatch Stopwatch = new Stopwatch();

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void ScanTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (_isScanning)
            {
                return;
            }
            _isScanning = true;
            double refresh = 100;
            if (Double.TryParse(Settings.Default.ActorWorkerRefresh.ToString(CultureInfo.InvariantCulture), out refresh))
            {
                _scanTimer.Interval = refresh;
            }
            Func<bool> scannerWorker = delegate
            {
                var readResult = Reader.GetActors();

                #region Notifications

                if (!MonsterReferencesSet && readResult.MonsterEntities.Any())
                {
                    MonsterReferencesSet = true;
                    AppContextHelper.Instance.RaiseNewMonsterEntries(readResult.MonsterEntities);
                }
                if (!NPCReferencesSet && readResult.NPCEntities.Any())
                {
                    NPCReferencesSet = true;
                    AppContextHelper.Instance.RaiseNewNPCEntries(readResult.NPCEntities);
                }
                if (!PCReferencesSet && readResult.PCEntities.Any())
                {
                    PCReferencesSet = true;
                    AppContextHelper.Instance.RaiseNewPCEntries(readResult.PCEntities);
                }

                if (MonsterReferencesSet && NPCReferencesSet && PCReferencesSet)
                {
                    ReferencesSet = true;
                }

                if (readResult.NewMonster.Any())
                {
                    AppContextHelper.Instance.RaiseNewMonsterAddedEntries(readResult.NewMonster);
                }
                if (readResult.NewNPC.Any())
                {
                    AppContextHelper.Instance.RaiseNewNPCAddedEntries(readResult.NewNPC);
                }
                if (readResult.NewPC.Any())
                {
                    AppContextHelper.Instance.RaiseNewPCAddedEntries(readResult.NewPC);
                }

                if (readResult.RemovedMonster.Any())
                {
                    AppContextHelper.Instance.RaiseNewMonsterRemovedEntries(readResult.RemovedMonster.Keys.ToList());
                }
                if (readResult.RemovedNPC.Any())
                {
                    AppContextHelper.Instance.RaiseNewNPCRemovedEntries(readResult.RemovedNPC.Keys.ToList());
                }
                if (readResult.RemovedPC.Any())
                {
                    AppContextHelper.Instance.RaiseNewPCRemovedEntries(readResult.RemovedPC.Keys.ToList());
                }

                #endregion

                _isScanning = false;
                return true;
            };
            scannerWorker.BeginInvoke(delegate { }, scannerWorker);
        }

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
