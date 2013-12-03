// FFXIVAPP.Client
// ActorWorker.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using FFXIVAPP.Common.Core.Memory;
using FFXIVAPP.Common.Utilities;
using NLog;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Memory
{
    [DoNotObfuscate]
    internal class ActorWorker : INotifyPropertyChanged, IDisposable
    {
        #region Property Bindings

        #endregion

        #region Declarations

        private static readonly Logger Tracer = LogManager.GetCurrentClassLogger();
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
                    var actorEntities = new List<ActorEntity>();
                    if (MemoryHandler.Instance.SigScanner.Locations.ContainsKey("CHARMAP"))
                    {
                        try
                        {
                            for (uint i = 0; i <= 1000; i += 4)
                            {
                                var characterAddress = (uint) MemoryHandler.Instance.GetInt32(MemoryHandler.Instance.SigScanner.Locations["CHARMAP"] + i);
                                if (characterAddress == 0)
                                {
                                    continue;
                                }
                                var npcEntry = MemoryHandler.Instance.GetStructure<Structures.NPCEntry>(characterAddress);
                                var actorEntity = new ActorEntity
                                {
                                    Name = MemoryHandler.Instance.GetString(characterAddress, 48),
                                    ID = npcEntry.ID,
                                    NPCID1 = npcEntry.NPCID1,
                                    NPCID2 = npcEntry.NPCID2,
                                    Type = npcEntry.Type,
                                    Coordinate = new Coordinate(npcEntry.X, npcEntry.Z, npcEntry.Y),
                                    X = npcEntry.X,
                                    Z = npcEntry.Z,
                                    Y = npcEntry.Y,
                                    Heading = npcEntry.Heading,
                                    Fate = npcEntry.Fate,
                                    ModelID = npcEntry.ModelID,
                                    Icon = npcEntry.Icon,
                                    Claimed = npcEntry.Claimed,
                                    TargetID = npcEntry.TargetID,
                                    Level = npcEntry.Level,
                                    HPCurrent = npcEntry.HPCurrent,
                                    HPMax = npcEntry.HPMax,
                                    MPCurrent = npcEntry.MPCurrent,
                                    MPMax = npcEntry.MPMax,
                                    TPCurrent = npcEntry.TPCurrent,
                                    TPMax = 1000,
                                    GPCurrent = npcEntry.GPCurrent,
                                    GPMax = npcEntry.GPMax,
                                    CPCurrent = npcEntry.CPCurrent,
                                    CPMax = npcEntry.CPMax
                                };
                                if (actorEntity.HPMax == 0)
                                {
                                    actorEntity.HPMax = 1;
                                }
                                if (i == 0)
                                {
                                    if (MemoryHandler.Instance.SigScanner.Locations.ContainsKey("TARGET"))
                                    {
                                        var targetAddress = MemoryHandler.Instance.SigScanner.Locations["TARGET"];
                                        if (targetAddress > 0)
                                        {
                                            actorEntity.TargetID = MemoryHandler.Instance.GetInt32(targetAddress);
                                        }
                                    }
                                }
                                if (actorEntity.TargetID == -536870912)
                                {
                                    actorEntity.TargetID = -1;
                                }
                                actorEntity.MapIndex = 0;
                                if (MemoryHandler.Instance.SigScanner.Locations.ContainsKey("MAP"))
                                {
                                    try
                                    {
                                        actorEntity.MapIndex = MemoryHandler.Instance.GetUInt32(MemoryHandler.Instance.SigScanner.Locations["MAP"]);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                                // setup DoT: +12104
                                foreach (var status in npcEntry.Statuses.Where(s => s.StatusID > 0))
                                {
                                    actorEntity.StatusEntries.Add(new StatusEntry
                                    {
                                        TargetName = actorEntity.Name,
                                        StatusID = status.StatusID,
                                        Duration = status.Duration,
                                        CasterID = status.CasterID
                                    });
                                }
                                if (!actorEntity.IsValid)
                                {
                                    continue;
                                }
                                actorEntities.Add(actorEntity);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
                        }
                    }
                    if (MemoryHandler.Instance.SigScanner.Locations.ContainsKey("NPCMAP"))
                    {
                        try
                        {
                            for (uint i = 0; i <= 256; i += 4)
                            {
                                var characterAddress = (uint) MemoryHandler.Instance.GetInt32(MemoryHandler.Instance.SigScanner.Locations["NPCMAP"] + i);
                                if (characterAddress == 0)
                                {
                                    continue;
                                }
                                var npcEntry = MemoryHandler.Instance.GetStructure<Structures.NPCEntry>(characterAddress);
                                var actorEntity = new ActorEntity
                                {
                                    Name = MemoryHandler.Instance.GetString(characterAddress, 48),
                                    ID = npcEntry.ID,
                                    NPCID1 = npcEntry.NPCID1,
                                    NPCID2 = npcEntry.NPCID2,
                                    Type = npcEntry.Type,
                                    Coordinate = new Coordinate(npcEntry.X, npcEntry.Z, npcEntry.Y),
                                    X = npcEntry.X,
                                    Z = npcEntry.Z,
                                    Y = npcEntry.Y,
                                    Heading = npcEntry.Heading,
                                    Fate = npcEntry.Fate,
                                    ModelID = npcEntry.ModelID,
                                    Icon = npcEntry.Icon,
                                    Claimed = npcEntry.Claimed,
                                    TargetID = npcEntry.TargetID,
                                    Level = npcEntry.Level,
                                    HPCurrent = npcEntry.HPCurrent,
                                    HPMax = npcEntry.HPMax,
                                    MPCurrent = npcEntry.MPCurrent,
                                    MPMax = npcEntry.MPMax,
                                    TPCurrent = npcEntry.TPCurrent,
                                    TPMax = 1000,
                                    GPCurrent = npcEntry.GPCurrent,
                                    GPMax = npcEntry.GPMax,
                                    CPCurrent = npcEntry.CPCurrent,
                                    CPMax = npcEntry.CPMax
                                };
                                if (actorEntity.HPMax == 0)
                                {
                                    actorEntity.HPMax = 1;
                                }
                                if (i == 0)
                                {
                                    if (MemoryHandler.Instance.SigScanner.Locations.ContainsKey("TARGET"))
                                    {
                                        var targetAddress = MemoryHandler.Instance.SigScanner.Locations["TARGET"];
                                        if (targetAddress > 0)
                                        {
                                            actorEntity.TargetID = MemoryHandler.Instance.GetInt32(targetAddress);
                                        }
                                    }
                                }
                                if (actorEntity.TargetID == -536870912)
                                {
                                    actorEntity.TargetID = -1;
                                }
                                actorEntity.MapIndex = 0;
                                if (MemoryHandler.Instance.SigScanner.Locations.ContainsKey("MAP"))
                                {
                                    try
                                    {
                                        actorEntity.MapIndex = MemoryHandler.Instance.GetUInt32(MemoryHandler.Instance.SigScanner.Locations["MAP"]);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                                // setup DoT: +12104
                                foreach (var status in npcEntry.Statuses.Where(s => s.StatusID > 0))
                                {
                                    actorEntity.StatusEntries.Add(new StatusEntry
                                    {
                                        TargetName = actorEntity.Name,
                                        StatusID = status.StatusID,
                                        Duration = status.Duration,
                                        CasterID = status.CasterID
                                    });
                                }
                                if (!actorEntity.IsValid)
                                {
                                    continue;
                                }
                                actorEntities.Add(actorEntity);
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
