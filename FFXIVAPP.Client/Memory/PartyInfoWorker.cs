// FFXIVAPP.Client
// PartyInfoWorker.cs
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
using FFXIVAPP.Common.Utilities;
using NLog;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Memory
{
    [DoNotObfuscate]
    internal class PartyInfoWorker : INotifyPropertyChanged, IDisposable
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Property Bindings

        private uint PartyInfoMap { get; set; }
        private uint PartyCountMap { get; set; }

        #endregion

        #region Declarations

        private readonly Timer _scanTimer;
        private bool _isScanning;

        #endregion

        public PartyInfoWorker()
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
                if (MemoryHandler.Instance.SigScanner.Locations.ContainsKey("CHARMAP"))
                {
                    if (MemoryHandler.Instance.SigScanner.Locations.ContainsKey("PARTYMAP"))
                    {
                        if (MemoryHandler.Instance.SigScanner.Locations.ContainsKey("PARTYCOUNT"))
                        {
                            PartyInfoMap = MemoryHandler.Instance.SigScanner.Locations["PARTYMAP"];
                            PartyCountMap = MemoryHandler.Instance.SigScanner.Locations["PARTYCOUNT"];
                            try
                            {
                                var partyEntities = new List<PartyEntity>();
                                var partyCount = MemoryHandler.Instance.GetByte(PartyCountMap);
                                if (partyCount > 0 && partyCount < 9)
                                {
                                    for (uint i = 0; i < partyCount; i++)
                                    {
                                        var address = PartyInfoMap + (i * 928);
                                        var actor = MemoryHandler.Instance.GetStructure<Structures.PartyMember>(address);
                                        var entry = new PartyEntity
                                        {
                                            Name = MemoryHandler.Instance.GetString(address, 32),
                                            ID = actor.ID,
                                            Coordinate = new Coordinate(actor.X, actor.Z, actor.Y),
                                            X = actor.X,
                                            Z = actor.Z,
                                            Y = actor.Y,
                                            Level = actor.Level,
                                            HPCurrent = actor.HPCurrent,
                                            HPMax = actor.HPMax,
                                            MPCurrent = actor.MPCurrent,
                                            MPMax = actor.MPMax,
                                            Job = actor.Job
                                        };
                                        if (entry.HPMax == 0)
                                        {
                                            entry.HPMax = 1;
                                        }
                                        foreach (var statusEntry in actor.Statuses.Select(status => new StatusEntry
                                        {
                                            TargetName = entry.Name,
                                            StatusID = status.StatusID,
                                            Duration = status.Duration,
                                            CasterID = status.CasterID
                                        })
                                                                         .Where(statusEntry => statusEntry.IsValid()))
                                        {
                                            entry.StatusEntries.Add(statusEntry);
                                        }
                                        if (entry.IsValid)
                                        {
                                            partyEntities.Add(entry);
                                        }
                                    }
                                }
                                AppContextHelper.Instance.RaiseNewPartyEntries(partyEntities);
                            }
                            catch (Exception ex)
                            {
                            }
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
