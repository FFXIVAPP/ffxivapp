// FFXIVAPP.Client
// TargetWorker.cs
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
using FFXIVAPP.Client.Delegates;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Common.Core.Memory;
using NLog;

namespace FFXIVAPP.Client.Memory
{
    internal class TargetWorker : INotifyPropertyChanged, IDisposable
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        public TargetWorker()
        {
            _scanTimer = new Timer(100);
            _scanTimer.Elapsed += ScanTimerElapsed;
        }

        #region Property Bindings

        private TargetEntity LastTargetEntity { get; set; }

        #endregion

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
            double refresh = 100;
            if (Double.TryParse(Settings.Default.TargetWorkerRefresh.ToString(CultureInfo.InvariantCulture), out refresh))
            {
                _scanTimer.Interval = refresh;
            }
            Func<bool> scannerWorker = delegate
            {
                if (MemoryHandler.Instance.SigScanner.Locations.ContainsKey("GAMEMAIN"))
                {
                    if (MemoryHandler.Instance.SigScanner.Locations.ContainsKey("CHARMAP"))
                    {
                        try
                        {
                            long targetHateStructure = 0;
                            switch (Settings.Default.GameLanguage)
                            {
                                case "Chinese":
                                    targetHateStructure = MemoryHandler.Instance.SigScanner.Locations["CHARMAP"] + 1136;
                                    break;
                                default:
                                    if (MemoryHandler.Instance.SigScanner.Locations.ContainsKey("ENMITYMAP"))
                                    {
                                        targetHateStructure = MemoryHandler.Instance.SigScanner.Locations["ENMITYMAP"];
                                    }
                                    break;
                            }
                            var enmityEntries = new List<EnmityEntry>();
                            var targetEntity = new TargetEntity();
                            if (MemoryHandler.Instance.SigScanner.Locations.ContainsKey("TARGET"))
                            {
                                var targetAddress = MemoryHandler.Instance.SigScanner.Locations["TARGET"];
                                var somethingFound = false;
                                if (targetAddress > 0)
                                {
                                    //var targetInfo = MemoryHandler.Instance.GetStructure<Structures.Target>(targetAddress);
                                    uint currentTarget = 0;
                                    uint mouseOverTarget = 0;
                                    uint focusTarget = 0;
                                    uint previousTarget = 0;
                                    uint currentTargetID = 0;
                                    var targetInfoSource = MemoryHandler.Instance.GetByteArray(targetAddress, 192);
                                    switch (Settings.Default.GameLanguage)
                                    {
                                        case "Chinese":
                                            currentTarget = BitConverter.ToUInt32(targetInfoSource, 0x0);
                                            mouseOverTarget = BitConverter.ToUInt32(targetInfoSource, 0xC);
                                            focusTarget = BitConverter.ToUInt32(targetInfoSource, 0x3C);
                                            previousTarget = BitConverter.ToUInt32(targetInfoSource, 0x48);
                                            currentTargetID = BitConverter.ToUInt32(targetInfoSource, 0x5C);
                                            break;
                                        default:
                                            currentTarget = BitConverter.ToUInt32(targetInfoSource, 0x0);
                                            if (MemoryHandler.Instance.ProcessModel.IsWin64)
                                            {
                                                mouseOverTarget = BitConverter.ToUInt32(targetInfoSource, 0x10);
                                                focusTarget = BitConverter.ToUInt32(targetInfoSource, 0x50);
                                                previousTarget = BitConverter.ToUInt32(targetInfoSource, 0x68);
                                                currentTargetID = BitConverter.ToUInt32(targetInfoSource, 0x80);
                                            }
                                            else
                                            {
                                                mouseOverTarget = BitConverter.ToUInt32(targetInfoSource, 0x8);
                                                focusTarget = BitConverter.ToUInt32(targetInfoSource, 0x38);
                                                previousTarget = BitConverter.ToUInt32(targetInfoSource, 0x44);
                                                currentTargetID = BitConverter.ToUInt32(targetInfoSource, 0x58);
                                            }
                                            break;
                                    }
                                    if (currentTarget > 0)
                                    {
                                        try
                                        {
                                            var source = MemoryHandler.Instance.GetByteArray(currentTarget, 0x3F40);
                                            var entry = ActorEntityHelper.ResolveActorFromBytes(source);
                                            if (MemoryHandler.Instance.SigScanner.Locations.ContainsKey("MAP"))
                                            {
                                                try
                                                {
                                                    entry.MapIndex = (uint) MemoryHandler.Instance.GetPlatformUInt(MemoryHandler.Instance.SigScanner.Locations["MAP"]);
                                                }
                                                catch (Exception ex)
                                                {
                                                }
                                            }
                                            if (entry.IsValid)
                                            {
                                                somethingFound = true;
                                                targetEntity.CurrentTarget = entry;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                    if (mouseOverTarget > 0)
                                    {
                                        try
                                        {
                                            var source = MemoryHandler.Instance.GetByteArray(mouseOverTarget, 0x3F40);
                                            var entry = ActorEntityHelper.ResolveActorFromBytes(source);
                                            if (MemoryHandler.Instance.SigScanner.Locations.ContainsKey("MAP"))
                                            {
                                                try
                                                {
                                                    entry.MapIndex = (uint) MemoryHandler.Instance.GetPlatformUInt(MemoryHandler.Instance.SigScanner.Locations["MAP"]);
                                                }
                                                catch (Exception ex)
                                                {
                                                }
                                            }
                                            if (entry.IsValid)
                                            {
                                                somethingFound = true;
                                                targetEntity.MouseOverTarget = entry;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                    if (focusTarget > 0)
                                    {
                                        var source = MemoryHandler.Instance.GetByteArray(focusTarget, 0x3F40);
                                        var entry = ActorEntityHelper.ResolveActorFromBytes(source);
                                        if (MemoryHandler.Instance.SigScanner.Locations.ContainsKey("MAP"))
                                        {
                                            try
                                            {
                                                entry.MapIndex = (uint) MemoryHandler.Instance.GetPlatformUInt(MemoryHandler.Instance.SigScanner.Locations["MAP"]);
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                        if (entry.IsValid)
                                        {
                                            somethingFound = true;
                                            targetEntity.FocusTarget = entry;
                                        }
                                    }
                                    if (previousTarget > 0)
                                    {
                                        try
                                        {
                                            var source = MemoryHandler.Instance.GetByteArray(previousTarget, 0x3F40);
                                            var entry = ActorEntityHelper.ResolveActorFromBytes(source);
                                            if (MemoryHandler.Instance.SigScanner.Locations.ContainsKey("MAP"))
                                            {
                                                try
                                                {
                                                    entry.MapIndex = (uint) MemoryHandler.Instance.GetPlatformUInt(MemoryHandler.Instance.SigScanner.Locations["MAP"]);
                                                }
                                                catch (Exception ex)
                                                {
                                                }
                                            }
                                            if (entry.IsValid)
                                            {
                                                somethingFound = true;
                                                targetEntity.PreviousTarget = entry;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                    if (currentTargetID > 0)
                                    {
                                        somethingFound = true;
                                        targetEntity.CurrentTargetID = currentTargetID;
                                    }
                                }
                                if (targetEntity.CurrentTargetID > 0 && targetHateStructure > 0)
                                {
                                    for (uint i = 0; i < 16; i++)
                                    {
                                        try
                                        {
                                            var address = targetHateStructure + (i * 72);
                                            var enmityEntry = new EnmityEntry
                                            {
                                                Name = MemoryHandler.Instance.GetString(address),
                                                ID = (uint) MemoryHandler.Instance.GetPlatformInt(address, 64),
                                                Enmity = (uint) MemoryHandler.Instance.GetPlatformInt(address, 68)
                                            };
                                            if (enmityEntry.ID <= 0)
                                            {
                                                continue;
                                            }
                                            if (String.IsNullOrWhiteSpace(enmityEntry.Name))
                                            {
                                                var pc = PCWorkerDelegate.GetNPCEntity(enmityEntry.ID);
                                                var npc = NPCWorkerDelegate.GetNPCEntity(enmityEntry.ID);
                                                var monster = MonsterWorkerDelegate.GetNPCEntity(enmityEntry.ID);
                                                try
                                                {
                                                    enmityEntry.Name = (pc ?? npc).Name ?? monster.Name;
                                                }
                                                catch (Exception ex)
                                                {
                                                }
                                            }
                                            enmityEntries.Add(enmityEntry);
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                }
                                targetEntity.EnmityEntries = enmityEntries;
                                if (somethingFound)
                                {
                                    AppContextHelper.Instance.RaiseNewTargetEntity(targetEntity);
                                }
                            }
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
