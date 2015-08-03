﻿// FFXIVAPP.Client
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
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using FFXIVAPP.Client.Delegates;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Common.Core.Memory;
using NLog;

namespace FFXIVAPP.Client.Memory
{
    internal class PartyInfoWorker : INotifyPropertyChanged, IDisposable
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        public PartyInfoWorker()
        {
            _scanTimer = new Timer(1000);
            _scanTimer.Elapsed += ScanTimerElapsed;
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            _scanTimer.Elapsed -= ScanTimerElapsed;
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
                                var partyCount = MemoryHandler.Instance.GetByte(PartyCountMap);

                                var currentPartyEntries = PartyInfoWorkerDelegate.NPCEntities.Keys.ToDictionary(key => key);

                                var newPartyEntries = new List<UInt32>();

                                if (partyCount > 1 && partyCount < 9)
                                {
                                    for (uint i = 0; i < partyCount; i++)
                                    {
                                        UInt32 ID;
                                        uint size;
                                        switch (Settings.Default.GameLanguage)
                                        {
                                            case "Chinese":
                                                size = 594;
                                                break;
                                            default:
                                                size = 544;
                                                break;
                                        }
                                        var address = PartyInfoMap + (i * size);
                                        var source = MemoryHandler.Instance.GetByteArray(address, (int) size);
                                        switch (Settings.Default.GameLanguage)
                                        {
                                            case "Chinese":
                                                ID = BitConverter.ToUInt32(source, 0x10);
                                                break;
                                            default:
                                                ID = BitConverter.ToUInt32(source, 0x10);
                                                break;
                                        }
                                        ActorEntity existing = null;
                                        if (currentPartyEntries.ContainsKey(ID))
                                        {
                                            currentPartyEntries.Remove(ID);
                                            if (MonsterWorkerDelegate.NPCEntities.ContainsKey(ID))
                                            {
                                                existing = MonsterWorkerDelegate.GetNPCEntity(ID);
                                            }
                                            if (PCWorkerDelegate.NPCEntities.ContainsKey(ID))
                                            {
                                                existing = PCWorkerDelegate.GetNPCEntity(ID);
                                            }
                                        }
                                        else
                                        {
                                            newPartyEntries.Add(ID);
                                        }
                                        var entry = PartyEntityHelper.ResolvePartyMemberFromBytes(source, existing);
                                        if (!entry.IsValid)
                                        {
                                            continue;
                                        }
                                        if (existing != null)
                                        {
                                            continue;
                                        }
                                        PartyInfoWorkerDelegate.EnsureNPCEntity(entry.ID, entry);
                                    }
                                }
                                else if (partyCount == 0 || partyCount == 1)
                                {
                                    var entry = PartyEntityHelper.ResolvePartyMemberFromBytes(new byte[0], PCWorkerDelegate.CurrentUser);
                                    if (entry.IsValid)
                                    {
                                        var exists = false;
                                        if (currentPartyEntries.ContainsKey(entry.ID))
                                        {
                                            currentPartyEntries.Remove(entry.ID);
                                            exists = true;
                                        }
                                        else
                                        {
                                            newPartyEntries.Add(entry.ID);
                                        }
                                        if (!exists)
                                        {
                                            PartyInfoWorkerDelegate.EnsureNPCEntity(entry.ID, entry);
                                        }
                                    }
                                }

                                if (!ReferencesSet)
                                {
                                    ReferencesSet = true;
                                    AppContextHelper.Instance.RaiseNewPartyEntries(PartyInfoWorkerDelegate.NPCEntities);
                                }

                                if (newPartyEntries.Any())
                                {
                                    AppContextHelper.Instance.RaiseNewPartyAddedEntries(newPartyEntries);
                                }

                                if (currentPartyEntries.Any())
                                {
                                    AppContextHelper.Instance.RaiseNewPartyRemovedEntries(currentPartyEntries.Keys.ToList());
                                    foreach (var key in currentPartyEntries.Keys)
                                    {
                                        PartyInfoWorkerDelegate.RemoveNPCEntity(key);
                                    }
                                }
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

        #region Property Bindings

        private long PartyInfoMap { get; set; }
        private long PartyCountMap { get; set; }

        public bool ReferencesSet { get; set; }

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

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion
    }
}
