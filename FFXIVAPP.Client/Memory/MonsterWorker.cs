// FFXIVAPP.Client
// MonsterWorker.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Utilities;
using FFXIVAPP.Common.Core.Memory;
using FFXIVAPP.Common.Core.Memory.Enums;
using NLog;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Memory
{
    [DoNotObfuscate]
    internal class MonsterWorker : INotifyPropertyChanged, IDisposable
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

        public MonsterWorker()
        {
            _scanTimer = new Timer(25);
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
                    if (MemoryHandler.Instance.SigScanner.Locations.ContainsKey("CHARMAP"))
                    {
                        try
                        {
                            var monsterEntries = new List<ActorEntity>();
                            var npcEntries = new List<ActorEntity>();
                            var pcEntries = new List<ActorEntity>();
                            for (uint i = 0; i <= 1000; i += 4)
                            {
                                var characterAddress = (uint) MemoryHandler.Instance.GetInt32(MemoryHandler.Instance.SigScanner.Locations["CHARMAP"] + i);
                                if (characterAddress == 0)
                                {
                                    continue;
                                }
                                var actor = MemoryHandler.Instance.GetStructure<Structures.NPCEntry>(characterAddress);
                                var name = MemoryHandler.Instance.GetString(characterAddress, 48);
                                var entry = ActorEntityHelper.ResolveActorFromMemory(actor, name);
                                if (MemoryHandler.Instance.SigScanner.Locations.ContainsKey("MAP"))
                                {
                                    try
                                    {
                                        entry.MapIndex = MemoryHandler.Instance.GetUInt32(MemoryHandler.Instance.SigScanner.Locations["MAP"]);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                                if (i == 0)
                                {
                                    if (MemoryHandler.Instance.SigScanner.Locations.ContainsKey("TARGET"))
                                    {
                                        var targetAddress = MemoryHandler.Instance.SigScanner.Locations["TARGET"];
                                        if (targetAddress > 0)
                                        {
                                            var targetInfo = MemoryHandler.Instance.GetStructure<Structures.Target>(targetAddress);
                                            entry.TargetID = (int) targetInfo.CurrentTargetID;
                                        }
                                    }
                                }
                                if (MemoryHandler.Instance.SigScanner.Locations.ContainsKey("MAP"))
                                {
                                    try
                                    {
                                        entry.MapIndex = MemoryHandler.Instance.GetUInt32(MemoryHandler.Instance.SigScanner.Locations["MAP"]);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                                var damageEntries = new List<DamageEntry>();
                                try
                                {
                                    //var source = MemoryHandler.Instance.GetByteArray(characterAddress + 0x32C0, 0xBB8);
                                    var source = actor.IncomingActions;
                                    for (uint d = 0; d < 30; d++)
                                    {
                                        var data = new byte[100];
                                        var index = d * 100;
                                        Array.Copy(source, index, data, 0, 100);
                                        var damageEntry = new DamageEntry
                                        {
                                            Code = BitConverter.ToInt32(data, 0),
                                            SequenceID = BitConverter.ToInt32(data, 4),
                                            SkillID = BitConverter.ToInt32(data, 12),
                                            SourceID = BitConverter.ToUInt32(data, 20),
                                            Type = data[66],
                                            Amount = BitConverter.ToInt16(data, 70),
                                            NPCEntry = entry
                                        };
                                        //if (damageEntry.SequenceID == 0 || damageEntry.SkillID <= 7 || damageEntry.SourceID == 0)
                                        //{
                                        //    continue;
                                        //}
                                        if (damageEntry.SequenceID == 0 || damageEntry.SourceID == 0)
                                        {
                                            continue;
                                        }
                                        damageEntries.Add(damageEntry);
                                    }
                                }
                                catch (Exception ex)
                                {
                                }
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
                                switch (entry.Type)
                                {
                                    case Actor.Type.Monster:
                                    case Actor.Type.PC:
                                        DamageTracker.EnsureHistoryItem(damageEntries);
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
