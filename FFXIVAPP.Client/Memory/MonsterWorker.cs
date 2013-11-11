// FFXIVAPP.Client
// MonsterWorker.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Timers;
using FFXIVAPP.Common.Utilities;
using NLog;
using SmartAssembly.Attributes;
using Timer = System.Timers.Timer;

#endregion

namespace FFXIVAPP.Client.Memory
{
    [DoNotObfuscate]
    internal class MonsterWorker : INotifyPropertyChanged, IDisposable
    {
        #region Property Bindings

        #endregion

        #region Declarations

        private static readonly Logger Tracer = LogManager.GetCurrentClassLogger();
        private readonly Timer _scanTimer;
        private readonly SynchronizationContext _sync = SynchronizationContext.Current;
        private bool _isScanning;

        #endregion

        #region Events

        public event NewNPCEventHandler OnNewNPC = delegate { };

        /// <summary>
        /// </summary>
        /// <param name="npcEntry"> </param>
        private void PostNPCEvent(List<NPCEntry> npcEntry)
        {
            _sync.Post(RaiseNPCEvent, npcEntry);
        }

        /// <summary>
        /// </summary>
        /// <param name="state"> </param>
        private void RaiseNPCEvent(object state)
        {
            OnNewNPC((List<NPCEntry>) state);
        }

        #endregion

        #region Delegates

        public delegate void NewNPCEventHandler(List<NPCEntry> npcEntry);

        #endregion

        public MonsterWorker()
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
                        _isScanning = true;
                        try
                        {
                            var npcEntries = new List<NPCEntry>();
                            for (uint i = 0; i <= 1000; i += 4)
                            {
                                var characterAddress = (uint) MemoryHandler.Instance.GetInt32(MemoryHandler.Instance.SigScanner.Locations["CHARMAP"] + i);
                                if (characterAddress == 0)
                                {
                                    continue;
                                }
                                var npc = MemoryHandler.Instance.GetStructure<Structures.NPCEntry>(characterAddress);
                                var entry = new NPCEntry
                                {
                                    Name = npc.Name,
                                    ID = npc.ID,
                                    NPCID1 = npc.NPCID1,
                                    NPCID2 = npc.NPCID2,
                                    Type = npc.Type,
                                    Coordinate = new Coordinate
                                    {
                                        X = npc.X,
                                        Y = npc.Y,
                                        Z = npc.Z
                                    },
                                    Heading = npc.Heading,
                                    Fate = npc.Fate,
                                    ModelID = npc.ModelID,
                                    Icon = npc.Icon,
                                    Claimed = npc.Claimed,
                                    TargetID = npc.TargetID,
                                    Level = npc.Level,
                                    HPCurrent = npc.HPCurrent,
                                    HPMax = npc.HPMax,
                                    MPCurrent = npc.MPCurrent,
                                    MPMax = npc.MPMax,
                                    TPCurrent = npc.TPCurrent,
                                    TPMax = 1000,
                                    GPCurrent = npc.GPCurrent,
                                    GPMax = npc.GPMax,
                                    CPCurrent = npc.CPCurrent,
                                    CPMax = npc.CPMax
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
                                            entry.TargetID = MemoryHandler.Instance.GetInt32(targetAddress);
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
                                foreach (var status in npc.Statuses.Where(s => s.StatusID > 0))
                                {
                                    entry.StatusList.Add(new StatusEntry
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
                                npcEntries.Add(entry);
                            }
                            PostNPCEvent(npcEntries);
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
