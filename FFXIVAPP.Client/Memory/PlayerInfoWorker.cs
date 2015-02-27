// FFXIVAPP.Client
// PlayerInfoWorker.cs
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
using Newtonsoft.Json;
using NLog;

namespace FFXIVAPP.Client.Memory
{
    internal class PlayerInfoWorker : INotifyPropertyChanged, IDisposable
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Property Bindings

        private uint PlayerInfoMap { get; set; }

        private PlayerEntity LastPlayerEntity { get; set; }

        #endregion

        #region Declarations

        private readonly Timer _scanTimer;
        private bool _isScanning;

        #endregion

        public PlayerInfoWorker()
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
            if (Double.TryParse(Settings.Default.PlayerInfoWorkerRefresh.ToString(CultureInfo.InvariantCulture), out refresh))
            {
                _scanTimer.Interval = refresh;
            }
            Func<bool> scannerWorker = delegate
            {
                if (MemoryHandler.Instance.SigScanner.Locations.ContainsKey("CHARMAP"))
                {
                    if (MemoryHandler.Instance.SigScanner.Locations.ContainsKey("PLAYERINFO"))
                    {
                        PlayerInfoMap = MemoryHandler.Instance.SigScanner.Locations["PLAYERINFO"];
                        if (PlayerInfoMap <= 6496)
                        {
                            return false;
                        }
                        try
                        {
                            short enmityCount;
                            uint enmityStructure;
                            switch (Settings.Default.GameLanguage)
                            {
                                case "Chinese":
                                    enmityCount = MemoryHandler.Instance.GetInt16(MemoryHandler.Instance.SigScanner.Locations["CHARMAP"] + 5688);
                                    enmityStructure = MemoryHandler.Instance.SigScanner.Locations["CHARMAP"] + 3384;
                                    break;
                                default:
                                    enmityCount = MemoryHandler.Instance.GetInt16(MemoryHandler.Instance.SigScanner.Locations["CHARMAP"] - 0x1C590); // 116032
                                    enmityStructure = MemoryHandler.Instance.SigScanner.Locations["CHARMAP"] - 0x1CE94; // 118340;
                                    break;
                            }
                            var enmityEntries = new List<EnmityEntry>();
                            if (enmityCount > 0 && enmityCount < 32 && enmityStructure > 0)
                            {
                                for (uint i = 0; i < enmityCount; i++)
                                {
                                    var address = enmityStructure + (i * 72);
                                    var enmityEntry = new EnmityEntry
                                    {
                                        ID = (uint) MemoryHandler.Instance.GetInt32(address),
                                        Name = MemoryHandler.Instance.GetString(address + 4),
                                        Enmity = (uint) MemoryHandler.Instance.GetInt16(address + 72)
                                    };
                                    if (enmityEntry.ID > 0)
                                    {
                                        enmityEntries.Add(enmityEntry);
                                    }
                                }
                            }
                            var source = MemoryHandler.Instance.GetByteArray(PlayerInfoMap, 0x256);
                            try
                            {
                                var entry = PlayerEntityHelper.ResolvePlayerFromBytes(source);
                                entry.EnmityEntries = enmityEntries;
                                var notify = false;
                                if (LastPlayerEntity == null)
                                {
                                    LastPlayerEntity = entry;
                                    notify = true;
                                }
                                else
                                {
                                    var hash1 = JsonConvert.SerializeObject(LastPlayerEntity)
                                                           .GetHashCode();
                                    var hash2 = JsonConvert.SerializeObject(entry)
                                                           .GetHashCode();
                                    if (!hash1.Equals(hash2))
                                    {
                                        LastPlayerEntity = entry;
                                        notify = true;
                                    }
                                }
                                if (notify)
                                {
                                    AppContextHelper.Instance.RaiseNewPlayerEntity(entry);
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    else
                    {
                        try
                        {
                            switch (Settings.Default.GameLanguage)
                            {
                                case "Chinese":
                                    PlayerInfoMap = MemoryHandler.Instance.SigScanner.Locations["CHARMAP"] + 5724;
                                    break;
                                default:
                                    PlayerInfoMap = MemoryHandler.Instance.SigScanner.Locations["CHARMAP"] - 115996;
                                    break;
                            }
                            MemoryHandler.Instance.SigScanner.Locations.Add("PLAYERINFO", PlayerInfoMap);
                        }
                        catch (Exception ex)
                        {
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
