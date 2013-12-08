// FFXIVAPP.Client
// NPCWorker.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Reflection;
using FFXIVAPP.Common.Core.Memory;
using FFXIVAPP.Common.Utilities;
using NLog;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Memory
{
    [DoNotObfuscate]
    internal class NPCWorker : INotifyPropertyChanged, IDisposable
    {
        #region Property Bindings

        #endregion

        #region Declarations

        private static readonly Logger Tracer = LogManager.GetCurrentClassLogger();
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
                            var monsterEntries = new List<ActorEntity>();
                            var npcEntries = new List<ActorEntity>();
                            var pcEntries = new List<ActorEntity>();
                            for (uint i = 0; i <= 256; i += 4)
                            {
                                var characterAddress = (uint) MemoryHandler.Instance.GetInt32(MemoryHandler.Instance.SigScanner.Locations["NPCMAP"] + i);
                                if (characterAddress == 0)
                                {
                                    continue;
                                }
                                var actor = MemoryHandler.Instance.GetStructure<Structures.NPCEntry>(characterAddress);
                                var entry = new ActorEntity
                                {
                                    Name = MemoryHandler.Instance.GetString(characterAddress, 48),
                                    ID = actor.ID,
                                    NPCID1 = actor.NPCID1,
                                    NPCID2 = actor.NPCID2,
                                    Type = actor.Type,
                                    Coordinate = new Coordinate(actor.X, actor.Z, actor.Y),
                                    X = actor.X,
                                    Z = actor.Z,
                                    Y = actor.Y,
                                    Heading = actor.Heading,
                                    Fate = actor.Fate,
                                    ModelID = actor.ModelID,
                                    Icon = actor.Icon,
                                    Claimed = actor.Claimed,
                                    TargetID = actor.TargetID,
                                    Level = actor.Level,
                                    HPCurrent = actor.HPCurrent,
                                    HPMax = actor.HPMax,
                                    MPCurrent = actor.MPCurrent,
                                    MPMax = actor.MPMax,
                                    TPCurrent = actor.TPCurrent,
                                    TPMax = 1000,
                                    GPCurrent = actor.GPCurrent,
                                    GPMax = actor.GPMax,
                                    CPCurrent = actor.CPCurrent,
                                    CPMax = actor.CPMax
                                };
                                if (entry.HPMax == 0)
                                {
                                    entry.HPMax = 1;
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
                                if (entry.TargetID == -536870912)
                                {
                                    entry.TargetID = -1;
                                }
                                entry.MapIndex = 0;
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
                                // setup DoT: +12104
                                foreach (var status in actor.Statuses.Where(s => s.StatusID > 0))
                                {
                                    entry.StatusEntries.Add(new StatusEntry
                                    {
                                        TargetName = entry.Name,
                                        StatusID = status.StatusID,
                                        Duration = status.Duration,
                                        CasterID = status.CasterID
                                    });
                                }
                                if (!entry.IsValid)
                                {
                                    continue;
                                }
                                switch (entry.ActorType)
                                {
                                    case Common.Core.Memory.Enums.Actor.Type.Monster:
                                        monsterEntries.Add(entry);
                                        break;
                                    case Common.Core.Memory.Enums.Actor.Type.NPC:
                                        npcEntries.Add(entry);
                                        break;
                                    case Common.Core.Memory.Enums.Actor.Type.PC:
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
                        }
                        catch (Exception ex)
                        {
                            Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
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
