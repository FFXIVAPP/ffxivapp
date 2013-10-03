// FFXIVAPP.Client
// NPCWorker.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Timers;
using FFXIVAPP.Common.Helpers;
using NLog;
using Timer = System.Timers.Timer;

#endregion

namespace FFXIVAPP.Client.Memory
{
    internal class NPCWorker : INotifyPropertyChanged, IDisposable
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

        public NPCWorker()
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
            Func<bool> scannerWorker = delegate
            {
                if (!MemoryHandler.Instance.SigScanner.Locations.ContainsKey("GAMEMAIN"))
                {
                    return false;
                }
                if (!MemoryHandler.Instance.SigScanner.Locations.ContainsKey("NPCMAP"))
                {
                    return false;
                }
                _isScanning = true;
                try
                {
                    var npcEntries = new List<NPCEntry>();
                    for (uint i = 0; i <= 256; i += 4)
                    {
                        var characterAddress = (uint) MemoryHandler.Instance.GetInt32(MemoryHandler.Instance.SigScanner.Locations["NPCMAP"] + i);
                        if (characterAddress == 0)
                        {
                            continue;
                        }
                        var npcEntry = new NPCEntry
                        {
                            Name = MemoryHandler.Instance.GetString(characterAddress, 48),
                            ID = MemoryHandler.Instance.GetUInt32(characterAddress, 116),
                            NPCID = MemoryHandler.Instance.GetUInt32(characterAddress, 128),
                            Type = MemoryHandler.Instance.GetByte(characterAddress, 138),
                            Coordinate = new Coordinate
                            {
                                X = MemoryHandler.Instance.GetFloat(characterAddress, 160),
                                Y = MemoryHandler.Instance.GetFloat(characterAddress, 168),
                                Z = MemoryHandler.Instance.GetFloat(characterAddress, 164)
                            },
                            Heading = MemoryHandler.Instance.GetFloat(characterAddress, 176),
                            Fate = MemoryHandler.Instance.GetUInt32(characterAddress, 228),
                            ModelID = MemoryHandler.Instance.GetUInt32(characterAddress, 388),
                            Icon = MemoryHandler.Instance.GetByte(characterAddress, 394),
                            Claimed = MemoryHandler.Instance.GetByte(characterAddress, 405),
                            TargetID = MemoryHandler.Instance.GetInt32(characterAddress, 416),
                            HPCurrent = MemoryHandler.Instance.GetInt32(characterAddress, 5776),
                            HPMax = MemoryHandler.Instance.GetInt32(characterAddress, 5780),
                            MPCurrent = MemoryHandler.Instance.GetInt32(characterAddress, 5784),
                            MPMax = MemoryHandler.Instance.GetInt32(characterAddress, 5788),
                            TPCurrent = MemoryHandler.Instance.GetInt32(characterAddress, 5792),
                            TPMax = 1000
                        };
                        if (npcEntry.HPMax == 0)
                        {
                            npcEntry.HPMax = 1;
                        }
                        if (npcEntry.TargetID == -536870912)
                        {
                            npcEntry.TargetID = -1;
                        }
                        npcEntry.MapIndex = 0;
                        if (MemoryHandler.Instance.SigScanner.Locations.ContainsKey("MAP"))
                        {
                            try
                            {
                                npcEntry.MapIndex = MemoryHandler.Instance.GetUInt32(MemoryHandler.Instance.SigScanner.Locations["MAP"]);
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        // setup DoTee: +12104
                        if (!npcEntry.IsValid)
                        {
                            continue;
                        }
                        npcEntries.Add(npcEntry);
                    }
                    try
                    {
                        RaiseNPCEvent(npcEntries);
                    }
                    catch (Exception raiseEx)
                    {
                        try
                        {
                            PostNPCEvent(npcEntries);
                        }
                        catch (Exception postEx)
                        {
                            DispatcherHelper.Invoke(() => PostNPCEvent(npcEntries));
                        }
                    }
                }
                catch (Exception ex)
                {
                    //Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
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
