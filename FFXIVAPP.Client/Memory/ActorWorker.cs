// FFXIVAPP.Client
// ActorWorker.cs
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
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using FFXIVAPP.Client.Delegates;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Common.Core.Memory;
using FFXIVAPP.Common.Core.Memory.Enums;
using NLog;

namespace FFXIVAPP.Client.Memory
{
    internal class ActorWorker : INotifyPropertyChanged, IDisposable
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Property Bindings

        public bool ReferencesSet { get; set; }

        #endregion

        #region Declarations

        private readonly Timer _scanTimer;
        private bool _isScanning;

        #endregion

        public ActorWorker()
        {
            _scanTimer = new Timer(100);
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
                if (MemoryHandler.Instance.SigScanner.Locations.ContainsKey("GAMEMAIN"))
                {
                    if (MemoryHandler.Instance.SigScanner.Locations.ContainsKey("CHARMAP"))
                    {
                        try
                        {
                            #region Ensure Target & Map

                            uint targetAddress = 0;
                            uint mapIndex = 0;
                            if (MemoryHandler.Instance.SigScanner.Locations.ContainsKey("TARGET"))
                            {
                                try
                                {
                                    targetAddress = MemoryHandler.Instance.SigScanner.Locations["TARGET"];
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                            if (MemoryHandler.Instance.SigScanner.Locations.ContainsKey("MAP"))
                            {
                                try
                                {
                                    mapIndex = MemoryHandler.Instance.GetUInt32(MemoryHandler.Instance.SigScanner.Locations["MAP"]);
                                }
                                catch (Exception ex)
                                {
                                }
                            }

                            #endregion

                            var endianSize = 4;
                            var limit = 1372;
                            var chunkSize = endianSize * limit;

                            var characterAddressMap = MemoryHandler.Instance.GetByteArray(MemoryHandler.Instance.SigScanner.Locations["CHARMAP"], chunkSize);

                            var uniqueAddresses = new Dictionary<UInt32, UInt32>();

                            for (var i = 0; i <= (chunkSize / endianSize); i += endianSize)
                            {
                                var characterAddress = BitConverter.ToUInt32(characterAddressMap, i);
                                if (characterAddress == 0)
                                {
                                    continue;
                                }
                                uniqueAddresses[characterAddress] = characterAddress;
                            }

                            var sourceData = uniqueAddresses.Select(kvp => MemoryHandler.Instance.GetByteArray(kvp.Value, 0x3F40))
                                                            .ToList();

                            #region ActorEntity Handlers

                            var firstTime = true;

                            var currentMonsterEntries = MonsterWorkerDelegate.NPCEntities.Keys.ToDictionary(key => key);
                            var currentNPCEntries = NPCWorkerDelegate.NPCEntities.Keys.ToDictionary(key => key);
                            var currentPCEntries = PCWorkerDelegate.NPCEntities.Keys.ToDictionary(key => key);

                            var newMonsterEntries = new List<UInt32>();
                            var newNPCEntries = new List<UInt32>();
                            var newPCEntries = new List<UInt32>();

                            for (var i = 0; i < sourceData.Count; i++)
                            {
                                try
                                {
                                    var source = sourceData[i];
                                    //var source = MemoryHandler.Instance.GetByteArray(characterAddress, 0x3F40);

                                    UInt32 ID;
                                    UInt32 NPCID2;
                                    Actor.Type Type;

                                    switch (Settings.Default.GameLanguage)
                                    {
                                        case "Chinese":
                                            ID = BitConverter.ToUInt32(source, 0x74);
                                            NPCID2 = BitConverter.ToUInt32(source, 0x80);
                                            Type = (Actor.Type) source[0x8A];
                                            break;
                                        default:
                                            ID = BitConverter.ToUInt32(source, 0x74);
                                            NPCID2 = BitConverter.ToUInt32(source, 0x80);
                                            Type = (Actor.Type) source[0x8A];
                                            break;
                                    }

                                    ActorEntity existing = null;

                                    switch (Type)
                                    {
                                        case Actor.Type.Monster:
                                            if (currentMonsterEntries.ContainsKey(ID))
                                            {
                                                currentMonsterEntries.Remove(ID);
                                                existing = MonsterWorkerDelegate.GetNPCEntity(ID);
                                            }
                                            else
                                            {
                                                newMonsterEntries.Add(ID);
                                            }
                                            break;
                                        case Actor.Type.PC:
                                            if (currentPCEntries.ContainsKey(ID))
                                            {
                                                currentPCEntries.Remove(ID);
                                                existing = PCWorkerDelegate.GetNPCEntity(ID);
                                            }
                                            else
                                            {
                                                newPCEntries.Add(ID);
                                            }
                                            break;
                                        case Actor.Type.NPC:
                                            if (currentNPCEntries.ContainsKey(NPCID2))
                                            {
                                                currentNPCEntries.Remove(NPCID2);
                                                existing = NPCWorkerDelegate.GetNPCEntity(NPCID2);
                                            }
                                            else
                                            {
                                                newNPCEntries.Add(NPCID2);
                                            }
                                            break;
                                        default:
                                            if (currentNPCEntries.ContainsKey(ID))
                                            {
                                                currentNPCEntries.Remove(ID);
                                                existing = NPCWorkerDelegate.GetNPCEntity(ID);
                                            }
                                            else
                                            {
                                                newNPCEntries.Add(ID);
                                            }
                                            break;
                                    }

                                    var entry = ActorEntityHelper.ResolveActorFromBytes(source, firstTime, existing);

                                    firstTime = false;

                                    //var actor = MemoryHandler.Instance.GetStructureFromBytes<Structures.NPCEntry>(source);
                                    //var actor = MemoryHandler.Instance.GetStructure<Structures.NPCEntry>(characterAddress);
                                    //var name = MemoryHandler.Instance.GetString(characterAddress, 48);
                                    //var entry = ActorEntityHelper.ResolveActorFromMemory(actor, name);
                                    entry.MapIndex = mapIndex;
                                    if (i == 0)
                                    {
                                        var name = Settings.Default.CharacterName;
                                        if (name != entry.Name || String.IsNullOrWhiteSpace(name))
                                        {
                                            Settings.Default.CharacterName = entry.Name;
                                        }
                                        if (targetAddress > 0)
                                        {
                                            uint currentTargetID;
                                            var targetInfoSource = MemoryHandler.Instance.GetByteArray(targetAddress, 128);
                                            switch (Settings.Default.GameLanguage)
                                            {
                                                case "Chinese":
                                                    currentTargetID = BitConverter.ToUInt32(targetInfoSource, 0x68);
                                                    break;
                                                default:
                                                    currentTargetID = BitConverter.ToUInt32(targetInfoSource, 0x68);
                                                    break;
                                            }
                                            entry.TargetID = (int) currentTargetID;
                                        }
                                    }
                                    if (!entry.IsValid)
                                    {
                                        continue;
                                    }
                                    if (existing != null)
                                    {
                                        continue;
                                    }
                                    switch (entry.Type)
                                    {
                                        case Actor.Type.Monster:
                                            MonsterWorkerDelegate.EnsureNPCEntity(entry.ID, entry);
                                            break;
                                        case Actor.Type.PC:
                                            PCWorkerDelegate.EnsureNPCEntity(entry.ID, entry);
                                            break;
                                        case Actor.Type.NPC:
                                            NPCWorkerDelegate.EnsureNPCEntity(entry.NPCID2, entry);
                                            break;
                                        default:
                                            NPCWorkerDelegate.EnsureNPCEntity(entry.ID, entry);
                                            break;
                                    }
                                }
                                catch (Exception ex)
                                {
                                }
                            }

                            if (!ReferencesSet)
                            {
                                ReferencesSet = true;
                                AppContextHelper.Instance.RaiseNewMonsterEntries(MonsterWorkerDelegate.NPCEntities);
                                AppContextHelper.Instance.RaiseNewNPCEntries(NPCWorkerDelegate.NPCEntities);
                                AppContextHelper.Instance.RaiseNewPCEntries(PCWorkerDelegate.NPCEntities);
                            }

                            if (newMonsterEntries.Any())
                            {
                                AppContextHelper.Instance.RaiseNewMonsterAddedEntries(newMonsterEntries);
                            }
                            if (newNPCEntries.Any())
                            {
                                AppContextHelper.Instance.RaiseNewNPCAddedEntries(newNPCEntries);
                            }
                            if (newPCEntries.Any())
                            {
                                AppContextHelper.Instance.RaiseNewPCAddedEntries(newPCEntries);
                            }

                            if (currentMonsterEntries.Any())
                            {
                                AppContextHelper.Instance.RaiseNewMonsterRemovedEntries(currentMonsterEntries.Keys.ToList());
                                foreach (var key in currentMonsterEntries.Keys)
                                {
                                    MonsterWorkerDelegate.RemoveNPCEntity(key);
                                }
                            }
                            if (currentNPCEntries.Any())
                            {
                                AppContextHelper.Instance.RaiseNewNPCRemovedEntries(currentNPCEntries.Keys.ToList());
                                foreach (var key in currentNPCEntries.Keys)
                                {
                                    NPCWorkerDelegate.RemoveNPCEntity(key);
                                }
                            }
                            if (currentPCEntries.Any())
                            {
                                AppContextHelper.Instance.RaiseNewPCRemovedEntries(currentPCEntries.Keys.ToList());
                                foreach (var key in currentPCEntries.Keys)
                                {
                                    PCWorkerDelegate.RemoveNPCEntity(key);
                                }
                            }

                            #endregion
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
