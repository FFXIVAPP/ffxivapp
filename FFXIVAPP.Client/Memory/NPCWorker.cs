﻿// FFXIVAPP.Client
// ChatWorker.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;
using System.Windows.Documents;
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
        private readonly MemoryHandler _handler;
        private readonly SigFinder _offsets;
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
            OnNewNPC((List<NPCEntry>)state);
        }

        #endregion

        #region Delegates

        public delegate void NewNPCEventHandler(List<NPCEntry> npcEntry);

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="process"> </param>
        /// <param name="offsets"> </param>
        public NPCWorker(Process process, SigFinder offsets)
        {
            _scanTimer = new Timer(1000);
            _scanTimer.Elapsed += ScanTimerElapsed;
            _handler = new MemoryHandler(process, 0);
            _offsets = offsets;
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
                if (!_offsets.Locations.ContainsKey("GAMEMAIN"))
                {
                    return false;
                }
                if (!_offsets.Locations.ContainsKey("CHARMAP"))
                {
                    return false;
                }
                _isScanning = true;
                
                var npcList = new List<NPCEntry>();
                for (uint i = 0; i <= 384; i += 4)
                {
                    try
                    {
                        _handler.Address = _offsets.Locations["CHARMAP"] + i;
                        var characterAddress = _handler.GetInt32();
                        if (characterAddress != 0)
                        {
                            _handler.Address = (uint)characterAddress;
                            var npcEntry = new NPCEntry
                            {
                                Name = _handler.GetString(48),
                                ID = _handler.GetInt32(116),
                                Type = _handler.GetByte(138),
                                Coordinate = new Coordinate(_handler.GetFloat(160), _handler.GetFloat(168), _handler.GetFloat(164)),
                                Heading = _handler.GetFloat(176),
                                HPCurrent = _handler.GetInt32(5776),
                                HPMax = _handler.GetInt32(5780),
                                MPCurrent = _handler.GetInt32(5784),
                                MPMax = _handler.GetInt32(5788),
                                TPCurrent = _handler.GetInt32(5792),
                                TPMax = 1000
                            };
                            npcList.Add(npcEntry); 
                        }
                    }
                    catch (Exception ex)
                    {
                        //Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
                    }
                }
                RaiseNPCEvent(npcList);
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
