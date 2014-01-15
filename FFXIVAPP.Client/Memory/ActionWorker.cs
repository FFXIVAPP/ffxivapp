// FFXIVAPP.Client
// ActionWorker.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using FFXIVAPP.Client.Utilities;
using FFXIVAPP.Common.Core.Memory.Enums;
using FFXIVAPP.Common.Utilities;
using NLog;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Memory
{
    [DoNotObfuscate]
    internal class ActionWorker : INotifyPropertyChanged, IDisposable
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

        public ActionWorker()
        {
            _scanTimer = new Timer(5);
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

        public Stopwatch Stopwatch = new Stopwatch();

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
                            //Stopwatch.Reset();
                            //Stopwatch.Start();

                            var characterAddressMap = MemoryHandler.Instance.SigScanner.Locations["CHARMAP"];
                            var characterAddresses = MemoryHandler.Instance.GetStructure<Structures.CHARMAP>(characterAddressMap);
                            var sources = characterAddresses.Locations.Where(c => c.BaseAddress > 0)
                                                            .ToList()
                                                            .Select(c => MemoryHandler.Instance.GetByteArray(c.BaseAddress, 0x3F40))
                                                            .ToList();
                            foreach (var source in sources.AsParallel()
                                                          .Where(s => s.Count() > 0))
                            {
                                var combatEntry = new CombatEntry();

                                var entryID = BitConverter.ToUInt32(source, 0x74);
                                var entryType = (Actor.Type) source[0x8A];
                                var entryName = MemoryHandler.Instance.GetStringFromBytes(source, 48);

                                #region Incoming Handler

                                IntPtr incomingAddress;
                                if ((incomingAddress = MemoryHandler.Instance.ReadPointer((IntPtr) BitConverter.ToUInt32(source, 0x2FD4))) == IntPtr.Zero)
                                {
                                    continue;
                                }
                                var limit = MemoryHandler.Instance.GetInt32(BitConverter.ToUInt32(source, 0x2FD8));
                                //MemoryHandler.Instance.ReadInt32((IntPtr) characterAddress, 0x2FDC);
                                //MemoryHandler.Instance.ReadInt32((IntPtr) characterAddress, 0x2FE0);
                                //MemoryHandler.Instance.ReadInt32((IntPtr) characterAddress, 0x2FF0);
                                if (limit >= 0x100)
                                {
                                    continue;
                                }
                                var incomingEntries = new List<IncomingEntry>();
                                for (var m = 0; m < limit; m++)
                                {
                                    IntPtr ptr3;
                                    if ((ptr3 = MemoryHandler.Instance.ReadPointer(IntPtr.Add(incomingAddress, m * 4))) == IntPtr.Zero)
                                    {
                                        continue;
                                    }
                                    IntPtr ptr4;
                                    if ((ptr4 = MemoryHandler.Instance.ReadPointer(IntPtr.Add(ptr3, 4))) == IntPtr.Zero)
                                    {
                                        continue;
                                    }
                                    IntPtr ptr5;
                                    if ((ptr5 = MemoryHandler.Instance.ReadPointer(IntPtr.Add(ptr4, 0))) == IntPtr.Zero)
                                    {
                                        continue;
                                    }
                                    IntPtr ptr6;
                                    if ((ptr6 = MemoryHandler.Instance.ReadPointer(IntPtr.Add(ptr5, 4))) == IntPtr.Zero)
                                    {
                                        continue;
                                    }
                                    var incomingEntry = new IncomingEntry
                                    {
                                        ID = (uint) ptr3.ToInt64(),
                                        Amount = MemoryHandler.Instance.ReadInt32(MemoryHandler.Instance.ReadPointer(ptr6, 0x10)),
                                        ComboAmount = MemoryHandler.Instance.ReadInt32(MemoryHandler.Instance.ReadPointer(ptr6, 20)),
                                        Type1 = MemoryHandler.Instance.ReadInt32(MemoryHandler.Instance.ReadPointer(ptr6, 4)),
                                        Type2 = MemoryHandler.Instance.ReadInt32(ptr3),
                                        SkillID = MemoryHandler.Instance.ReadInt32(MemoryHandler.Instance.ReadPointer(ptr6, 12)),
                                        UNK1 = MemoryHandler.Instance.ReadInt32(MemoryHandler.Instance.ReadPointer(ptr6)),
                                        UNK2 = MemoryHandler.Instance.ReadInt32(MemoryHandler.Instance.ReadPointer(ptr6, 8)),
                                        UNK3 = MemoryHandler.Instance.ReadInt32(MemoryHandler.Instance.ReadPointer(ptr6, 0x18)),
                                        UNK4 = MemoryHandler.Instance.ReadInt32(MemoryHandler.Instance.ReadPointer(ptr6, 0x1C)),
                                        TargetID = entryID,
                                        TargetName = entryName,
                                        TargetType = entryType
                                    };
                                    if (incomingEntry.SkillID > 0)
                                    {
                                        incomingEntries.Add(incomingEntry);
                                    }
                                }

                                combatEntry.IncomingEntries = incomingEntries;

                                #endregion

                                #region IncomingAction Handler

                                var incomingActionEntries = new List<IncomingActionEntry>();

                                switch (entryType)
                                {
                                    case Actor.Type.Monster:
                                    case Actor.Type.PC:
                                        try
                                        {
                                            var sourceArray = new byte[0xBB8];
                                            Buffer.BlockCopy(source, 0x32C0, sourceArray, 0, 0xBB8);
                                            for (uint d = 0; d < 30; d++)
                                            {
                                                var destinationArray = new byte[100];
                                                var index = d * 100;
                                                Array.Copy(sourceArray, index, destinationArray, 0, 100);
                                                var incomingActionEntry = new IncomingActionEntry
                                                {
                                                    Code = BitConverter.ToInt32(destinationArray, 0),
                                                    SequenceID = BitConverter.ToInt32(destinationArray, 4),
                                                    SkillID = BitConverter.ToInt32(destinationArray, 12),
                                                    SourceID = BitConverter.ToUInt32(destinationArray, 20),
                                                    Type = destinationArray[66],
                                                    Amount = BitConverter.ToInt16(destinationArray, 70),
                                                    TargetID = entryID,
                                                    TargetName = entryName,
                                                    TargetType = entryType
                                                };
                                                if (incomingActionEntry.SequenceID > 0 && incomingActionEntry.SkillID > 0 && incomingActionEntry.SourceID > 0)
                                                {
                                                    incomingActionEntries.Add(incomingActionEntry);
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                        break;
                                }

                                combatEntry.IncomingActionEntries = incomingActionEntries;

                                #endregion

                                #region OutGoing Handler

                                var outGoingCount = source[0x32B8];
                                if (outGoingCount > 0)
                                {
                                    var outGoingEntry = new OutGoingEntry
                                    {
                                        Counter = outGoingCount,
                                        SkillID = source[0x31B0],
                                        SequenceID = BitConverter.ToInt32(source, 0x31BC),
                                        SourceID = entryID,
                                        SourceName = entryName,
                                        SourceType = entryType
                                    };
                                    for (var j = 0; j < outGoingCount; j++)
                                    {
                                        outGoingEntry.TargetIDs.Add(BitConverter.ToUInt32(source, 0x31C8 + j * 8));
                                    }
                                    if (outGoingEntry.SkillID > 0)
                                    {
                                        CombatTracker.EnsureOutGoingEntries(outGoingEntry);
                                    }
                                }

                                #endregion

                                if (incomingEntries.Any())
                                {
                                    CombatTracker.EnsureIncomingEntries(incomingEntries);
                                }

                                if (incomingActionEntries.Any())
                                {
                                    CombatTracker.EnsureIncomingActionEntries(incomingActionEntries);
                                }
                            }

                            //Stopwatch.Stop();
                            //Logging.Log(Logger, Stopwatch.ElapsedMilliseconds.ToString());
                        }
                        catch (Exception ex)
                        {
                            Logging.Log(Logger, "", ex);
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
