// FFXIVAPP.Client
// TargetWorker.cs
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
using FFXIVAPP.Common.Core.Memory;
using FFXIVAPP.Common.Utilities;
using NLog;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Memory
{
    [DoNotObfuscate]
    internal class TargetWorker : INotifyPropertyChanged, IDisposable
    {
        #region Property Bindings

        private TargetEntity LastTargetEntity { get; set; }

        #endregion

        #region Declarations

        private static readonly Logger Tracer = LogManager.GetCurrentClassLogger();
        private readonly Timer _scanTimer;
        private bool _isScanning;

        #endregion

        public TargetWorker()
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
                    if (MemoryHandler.Instance.SigScanner.Locations.ContainsKey("CHARMAP"))
                    {
                        try
                        {
                            var targetHateStructure = MemoryHandler.Instance.SigScanner.Locations["CHARMAP"] + 1064;
                            var enmityEntries = new List<EnmityEntry>();
                            var targetEntity = new TargetEntity();
                            if (MemoryHandler.Instance.SigScanner.Locations.ContainsKey("TARGET"))
                            {
                                var targetAddress = MemoryHandler.Instance.SigScanner.Locations["TARGET"];
                                var somethingFound = false;
                                if (targetAddress > 0)
                                {
                                    var targetInfo = MemoryHandler.Instance.GetStructure<Structures.Target>(targetAddress);
                                    if (targetInfo.CurrentTarget > 0)
                                    {
                                        try
                                        {
                                            var actor = MemoryHandler.Instance.GetStructure<Structures.NPCEntry>(targetInfo.CurrentTarget);
                                            var entry = new ActorEntity
                                            {
                                                Name = MemoryHandler.Instance.GetString(targetInfo.CurrentTarget, 48),
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
                                    if (targetInfo.MouseOverTarget > 0)
                                    {
                                        try
                                        {
                                            var actor = MemoryHandler.Instance.GetStructure<Structures.NPCEntry>(targetInfo.MouseOverTarget);
                                            var entry = new ActorEntity
                                            {
                                                Name = MemoryHandler.Instance.GetString(targetInfo.MouseOverTarget, 48),
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
                                    if (targetInfo.FocusTarget > 0)
                                    {
                                        somethingFound = true;
                                        var actor = MemoryHandler.Instance.GetStructure<Structures.NPCEntry>(targetInfo.FocusTarget);
                                        var entry = new ActorEntity
                                        {
                                            Name = MemoryHandler.Instance.GetString(targetInfo.FocusTarget, 48),
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
                                        if (entry.IsValid)
                                        {
                                            targetEntity.FocusTarget = entry;
                                        }
                                    }
                                    if (targetInfo.PreviousTarget > 0)
                                    {
                                        try
                                        {
                                            var actor = MemoryHandler.Instance.GetStructure<Structures.NPCEntry>(targetInfo.PreviousTarget);
                                            var entry = new ActorEntity
                                            {
                                                Name = MemoryHandler.Instance.GetString(targetInfo.PreviousTarget, 48),
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
                                    if (targetInfo.CurrentTargetID > 0)
                                    {
                                        somethingFound = true;
                                        targetEntity.CurrentTargetID = targetInfo.CurrentTargetID;
                                    }
                                }
                                if (targetEntity.CurrentTargetID > 0)
                                {
                                    for (uint i = 0; i < 16; i++)
                                    {
                                        var address = targetHateStructure + (i * 72);
                                        var enmityEntry = new EnmityEntry
                                        {
                                            Name = MemoryHandler.Instance.GetString(address),
                                            ID = (uint) MemoryHandler.Instance.GetInt32(address + 64),
                                            Enmity = (uint) MemoryHandler.Instance.GetInt16(address + 68)
                                        };
                                        if (enmityEntry.Enmity > 32768)
                                        {
                                            enmityEntry.Enmity = 32768;
                                        }
                                        if (enmityEntry.ID > 0)
                                        {
                                            enmityEntries.Add(enmityEntry);
                                        }
                                    }
                                }
                                targetEntity.EnmityEntries = enmityEntries;
                                if (somethingFound)
                                {
                                    ApplicationContextHelper.GetContext()
                                                            .TargetWorker.RaiseEntityEvent(targetEntity);
                                }
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
