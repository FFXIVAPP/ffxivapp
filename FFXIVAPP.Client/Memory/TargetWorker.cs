// FFXIVAPP.Client
// TargetWorker.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Common.Core.Memory;
using FFXIVAPP.Common.Utilities;
using Newtonsoft.Json;
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
                                if (targetAddress > 0)
                                {
                                    targetEntity.ID = (uint) MemoryHandler.Instance.GetInt32(targetAddress);
                                }
                            }
                            if (targetEntity.ID > 0)
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
                                    if (enmityEntry.ID > 0)
                                    {
                                        enmityEntries.Add(enmityEntry);
                                    }
                                }
                                targetEntity.EnmityEntries = enmityEntries;
                                var notify = false;
                                if (LastTargetEntity == null)
                                {
                                    LastTargetEntity = targetEntity;
                                    notify = true;
                                }
                                else
                                {
                                    var hash1 = JsonConvert.SerializeObject(LastTargetEntity)
                                                           .GetHashCode();
                                    var hash2 = JsonConvert.SerializeObject(targetEntity)
                                                           .GetHashCode();
                                    if (!hash1.Equals(hash2))
                                    {
                                        LastTargetEntity = targetEntity;
                                        notify = true;
                                    }
                                }
                                if (notify)
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
