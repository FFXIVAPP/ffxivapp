// FFXIVAPP.Client
// PartyInfoWorker.cs
// 
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions are met: 
// 
//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright 
//    notice, this list of conditions and the following disclaimer in the 
//    documentation and/or other materials provided with the distribution. 
//  * Neither the name of SyndicatedLife nor the names of its contributors may 
//    be used to endorse or promote products derived from this software 
//    without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE. 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Timers;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Common.Core.Memory;
using FFXIVAPP.Common.Helpers;
using NLog;

namespace FFXIVAPP.Client.Memory
{
    internal class PartyInfoWorker : INotifyPropertyChanged, IDisposable
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Property Bindings

        private uint PartyInfoMap { get; set; }
        private uint PartyCountMap { get; set; }

        #endregion

        #region Declarations

        private readonly Timer _scanTimer;
        private bool _isScanning;

        #endregion

        public PartyInfoWorker()
        {
            _scanTimer = new Timer(1000);
            _scanTimer.Elapsed += ScanTimerElapsed;
        }

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
            double refresh = 1000;
            if (Double.TryParse(Settings.Default.PartyInfoWorkerRefresh.ToString(CultureInfo.InvariantCulture), out refresh))
            {
                _scanTimer.Interval = refresh;
            }
            Func<bool> scannerWorker = delegate
            {
                if (MemoryHandler.Instance.SigScanner.Locations.ContainsKey("CHARMAP"))
                {
                    if (MemoryHandler.Instance.SigScanner.Locations.ContainsKey("PARTYMAP"))
                    {
                        if (MemoryHandler.Instance.SigScanner.Locations.ContainsKey("PARTYCOUNT"))
                        {
                            PartyInfoMap = MemoryHandler.Instance.SigScanner.Locations["PARTYMAP"];
                            PartyCountMap = MemoryHandler.Instance.SigScanner.Locations["PARTYCOUNT"];
                            try
                            {
                                var partyEntities = new List<PartyEntity>();
                                var partyCount = MemoryHandler.Instance.GetByte(PartyCountMap);
                                if (partyCount > 0 && partyCount < 9)
                                {
                                    for (uint i = 0; i < partyCount; i++)
                                    {
                                        var address = PartyInfoMap + (i * 594);
                                        var actor = MemoryHandler.Instance.GetStructure<Structures.PartyMember>(address);
                                        var entry = new PartyEntity
                                        {
                                            Name = MemoryHandler.Instance.GetString(address, 32),
                                            ID = actor.ID,
                                            Coordinate = new Coordinate(actor.X, actor.Z, actor.Y),
                                            X = actor.X,
                                            Z = actor.Z,
                                            Y = actor.Y,
                                            Level = actor.Level,
                                            HPCurrent = actor.HPCurrent,
                                            HPMax = actor.HPMax,
                                            MPCurrent = actor.MPCurrent,
                                            MPMax = actor.MPMax,
                                            Job = actor.Job
                                        };
                                        if (entry.HPMax == 0)
                                        {
                                            entry.HPMax = 1;
                                        }
                                        foreach (var status in actor.Statuses)
                                        {
                                            var statusEntry = new StatusEntry
                                            {
                                                TargetName = entry.Name,
                                                StatusID = status.StatusID,
                                                Duration = status.Duration,
                                                CasterID = status.CasterID
                                            };
                                            try
                                            {
                                                var statusInfo = StatusEffectHelper.StatusInfo(statusEntry.StatusID);
                                                statusEntry.IsCompanyAction = statusInfo.CompanyAction;
                                                var statusKey = "";
                                                switch (Settings.Default.GameLanguage)
                                                {
                                                    case "English":
                                                        statusKey = statusInfo.Name.English;
                                                        break;
                                                    case "French":
                                                        statusKey = statusInfo.Name.French;
                                                        break;
                                                    case "German":
                                                        statusKey = statusInfo.Name.German;
                                                        break;
                                                    case "Japanese":
                                                        statusKey = statusInfo.Name.Japanese;
                                                        break;
                                                }
                                                statusEntry.StatusName = statusKey;
                                            }
                                            catch (Exception ex)
                                            {
                                                statusEntry.StatusName = "UNKNOWN";
                                            }
                                            if (statusEntry.IsValid())
                                            {
                                                entry.StatusEntries.Add(statusEntry);
                                            }
                                        }
                                        if (entry.IsValid)
                                        {
                                            partyEntities.Add(entry);
                                        }
                                    }
                                }
                                AppContextHelper.Instance.RaiseNewPartyEntries(partyEntities);
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                }
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

        #region Implementation of IDisposable

        public void Dispose()
        {
            _scanTimer.Elapsed -= ScanTimerElapsed;
        }

        #endregion
    }
}
