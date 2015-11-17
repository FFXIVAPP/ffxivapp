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

        public ActorWorker()
        {
            _scanTimer = new Timer(100);
            _scanTimer.Elapsed += ScanTimerElapsed;
        }

        #region Property Bindings

        public bool ReferencesSet { get; set; }

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            _scanTimer.Elapsed -= ScanTimerElapsed;
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

                            long targetAddress = 0;
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
                                    mapIndex = (uint) MemoryHandler.Instance.GetPlatformUInt(MemoryHandler.Instance.SigScanner.Locations["MAP"]);
                                }
                                catch (Exception ex)
                                {
                                }
                            }

                            #endregion

                            var endianSize = MemoryHandler.Instance.ProcessModel.IsWin64 ? 8 : 4;
                            const int limit = 1372;

                            var characterAddressMap = MemoryHandler.Instance.GetByteArray(MemoryHandler.Instance.SigScanner.Locations["CHARMAP"], endianSize * limit);

                            var uniqueAddresses = new Dictionary<IntPtr, IntPtr>();

                            var firstAddress = IntPtr.Zero;


                            var firstTime = true;


                            for (var i = 0; i < limit; i++)
                            {
                                IntPtr characterAddress;
                                if (MemoryHandler.Instance.ProcessModel.IsWin64)
                                {
                                    characterAddress = new IntPtr(BitConverter.ToInt64(characterAddressMap, i * endianSize));
                                }
                                else
                                {
                                    characterAddress = new IntPtr(BitConverter.ToInt32(characterAddressMap, i * endianSize));
                                }
                                if (characterAddress == IntPtr.Zero)
                                {
                                    continue;
                                }

                                if (firstTime)
                                {
                                    firstAddress = characterAddress;
                                    firstTime = false;
                                }
                                uniqueAddresses[characterAddress] = characterAddress;
                            }

                            //var sourceData = uniqueAddresses.Select(kvp => MemoryHandler.Instance.GetByteArray((long)kvp.Value, 0x23F0)).ToList(); // old size: 0x3F40

                            #region ActorEntity Handlers

                            var currentMonsterEntries = MonsterWorkerDelegate.MonsterEntities.Keys.ToDictionary(key => key);
                            var currentNPCEntries = NPCWorkerDelegate.NPCEntities.Keys.ToDictionary(key => key);
                            var currentPCEntries = PCWorkerDelegate.PCEntities.Keys.ToDictionary(key => key);

                            var newMonsterEntries = new List<UInt32>();
                            var newNPCEntries = new List<UInt32>();
                            var newPCEntries = new List<UInt32>();

                            foreach (var kvp in uniqueAddresses)
                            {
                                try
                                {
                                    var source = MemoryHandler.Instance.GetByteArray(kvp.Value.ToInt64(), 0x23F0);
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
                                    var newEntry = false;

                                    switch (Type)
                                    {
                                        case Actor.Type.Monster:
                                            if (currentMonsterEntries.ContainsKey(ID))
                                            {
                                                currentMonsterEntries.Remove(ID);
                                                existing = MonsterWorkerDelegate.GetMonsterEntity(ID);
                                            }
                                            else
                                            {
                                                newMonsterEntries.Add(ID);
                                                newEntry = true;
                                            }
                                            break;
                                        case Actor.Type.PC:
                                            if (currentPCEntries.ContainsKey(ID))
                                            {
                                                currentPCEntries.Remove(ID);
                                                existing = PCWorkerDelegate.GetPCEntity(ID);
                                            }
                                            else
                                            {
                                                newPCEntries.Add(ID);
                                                newEntry = true;
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
                                                newEntry = true;
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
                                                newEntry = true;
                                            }
                                            break;
                                    }

                                    var isFirstEntry = kvp.Value.ToInt64() == firstAddress.ToInt64();

                                    var entry = ActorEntityHelper.ResolveActorFromBytes(source, isFirstEntry, existing);

                                    //firstTime = false;

                                    //var actor = MemoryHandler.Instance.GetStructureFromBytes<Structures.NPCEntry>(source);
                                    //var actor = MemoryHandler.Instance.GetStructure<Structures.NPCEntry>(characterAddress);
                                    //var name = MemoryHandler.Instance.GetString(characterAddress, 48);
                                    //var entry = ActorEntityHelper.ResolveActorFromMemory(actor, name);

                                    entry.MapIndex = mapIndex;
                                    if (isFirstEntry)
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
                                                    currentTargetID = BitConverter.ToUInt32(targetInfoSource, 0x74);
                                                    break;
                                            }
                                            entry.TargetID = (int) currentTargetID;
                                        }
                                    }
                                    if (!entry.IsValid)
                                    {
                                        newMonsterEntries.Remove(entry.ID);
                                        newMonsterEntries.Remove(entry.NPCID2);
                                        newNPCEntries.Remove(entry.ID);
                                        newNPCEntries.Remove(entry.NPCID2);
                                        newPCEntries.Remove(entry.ID);
                                        newPCEntries.Remove(entry.NPCID2);
                                        continue;
                                    }
                                    if (existing != null)
                                    {
                                        continue;
                                    }

                                    if (newEntry)
                                    {
                                        switch (entry.Type)
                                        {
                                            case Actor.Type.Monster:
                                                MonsterWorkerDelegate.EnsureMonsterEntity(entry.ID, entry);
                                                break;
                                            case Actor.Type.PC:
                                                PCWorkerDelegate.EnsurePCEntity(entry.ID, entry);
                                                break;
                                            case Actor.Type.NPC:
                                                NPCWorkerDelegate.EnsureNPCEntity(entry.NPCID2, entry);
                                                break;
                                            default:
                                                NPCWorkerDelegate.EnsureNPCEntity(entry.ID, entry);
                                                break;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                }
                            }

                            MemoryHandler._scanCount++;


                            //if (!ReferencesSet)
                            {
                                ReferencesSet = true;
                                AppContextHelper.Instance.RaiseNewMonsterEntries(MonsterWorkerDelegate.MonsterEntities);
                                AppContextHelper.Instance.RaiseNewNPCEntries(NPCWorkerDelegate.NPCEntities);
                                AppContextHelper.Instance.RaiseNewPCEntries(PCWorkerDelegate.PCEntities);
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
                                    MonsterWorkerDelegate.RemoveMonsterEntity(key);
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
                                    PCWorkerDelegate.RemovePCEntity(key);
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
    }
}
