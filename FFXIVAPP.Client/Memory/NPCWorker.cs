// FFXIVAPP.Client
// NPCWorker.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Common.Core.Memory;
using FFXIVAPP.Common.Core.Memory.Enums;
using NLog;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Memory
{
    [DoNotObfuscate]
    internal class NPCWorker : INotifyPropertyChanged, IDisposable
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Property Bindings

        #endregion

        #region Declarations

        private readonly Timer _scanTimer;
        private bool _isScanning;

        #endregion

        public NPCWorker()
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
            Func<bool> scannerWorker = delegate
            {
                if (MemoryHandler.Instance.SigScanner.Locations.ContainsKey("GAMEMAIN"))
                {
                    if (MemoryHandler.Instance.SigScanner.Locations.ContainsKey("NPCMAP"))
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

                            var characterAddressMap = MemoryHandler.Instance.GetByteArray(MemoryHandler.Instance.SigScanner.Locations["NPCMAP"], 1024);
                            var sourceData = new List<byte[]>();
                            for (var i = 0; i <= 256; i += 4)
                            {
                                var characterAddress = BitConverter.ToUInt32(characterAddressMap, i);
                                if (characterAddress == 0)
                                {
                                    continue;
                                }
                                sourceData.Add(MemoryHandler.Instance.GetByteArray(characterAddress, 0x3F40));
                            }

                            #region ActorEntity Handlers

                            var monsterEntries = new List<ActorEntity>();
                            var npcEntries = new List<ActorEntity>();
                            var pcEntries = new List<ActorEntity>();
                            for (var i = 0; i < sourceData.Count; i++)
                            {
                                var source = sourceData[i];
                                //var source = MemoryHandler.Instance.GetByteArray(characterAddress, 0x3F40);
                                var entry = ActorEntityHelper.ResolveActorFromBytes(source);
                                //var actor = MemoryHandler.Instance.GetStructureFromBytes<Structures.NPCEntry>(source);
                                //var actor = MemoryHandler.Instance.GetStructure<Structures.NPCEntry>(characterAddress);
                                //var name = MemoryHandler.Instance.GetString(characterAddress, 48);
                                //var entry = ActorEntityHelper.ResolveActorFromMemory(actor, name);
                                entry.MapIndex = mapIndex;
                                if (!entry.IsValid)
                                {
                                    continue;
                                }
                                switch (entry.Type)
                                {
                                    case Actor.Type.Monster:
                                        monsterEntries.Add(entry);
                                        break;
                                    case Actor.Type.NPC:
                                        npcEntries.Add(entry);
                                        break;
                                    case Actor.Type.PC:
                                        pcEntries.Add(entry);
                                        break;
                                    default:
                                        npcEntries.Add(entry);
                                        break;
                                }
                            }
                            if (monsterEntries.Any())
                            {
                                AppContextHelper.Instance.RaiseNewMonsterEntries(monsterEntries);
                            }
                            if (npcEntries.Any())
                            {
                                AppContextHelper.Instance.RaiseNewNPCEntries(npcEntries);
                            }
                            if (pcEntries.Any())
                            {
                                AppContextHelper.Instance.RaiseNewPCEntries(pcEntries);
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
