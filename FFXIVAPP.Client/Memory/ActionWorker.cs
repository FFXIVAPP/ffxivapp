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
                            Stopwatch.Reset();
                            Stopwatch.Start();

                            var characterAddressMap = MemoryHandler.Instance.GetByteArray(MemoryHandler.Instance.SigScanner.Locations["CHARMAP"], 4000);
                            for (var i = 0; i <= 1000; i += 4)
                            {
                                var characterAddress = BitConverter.ToUInt32(characterAddressMap, i);
                                if (characterAddress == 0)
                                {
                                    continue;
                                }
                                var source = MemoryHandler.Instance.GetByteArray(characterAddress, 0x3F40);

                                var combatEntry = new CombatEntry();

                                var targetID = BitConverter.ToUInt32(source, 0x74);
                                var targetType = (Actor.Type) source[0x8A];
                                var targetName = MemoryHandler.Instance.GetStringFromBytes(source, 48);

                                #region Incoming Handler

                                IntPtr incomingAddress;
                                if ((incomingAddress = MemoryHandler.Instance.ReadPointer(IntPtr.Add((IntPtr) characterAddress, 0x2FD4))) == IntPtr.Zero)
                                {
                                    continue;
                                }
                                var limit = MemoryHandler.Instance.GetInt32((uint) IntPtr.Add((IntPtr) characterAddress, 0x2FD8));
                                MemoryHandler.Instance.ReadInt32((IntPtr) characterAddress, 0x2FDC);
                                MemoryHandler.Instance.ReadInt32((IntPtr) characterAddress, 0x2FE0);
                                MemoryHandler.Instance.ReadInt32((IntPtr) characterAddress, 0x2FF0);
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
                                    var type1 = MemoryHandler.Instance.ReadInt32((IntPtr) MemoryHandler.Instance.ReadInt32(ptr6, 4));
                                    var type2 = MemoryHandler.Instance.ReadInt32(ptr3);
                                    var skillID = MemoryHandler.Instance.ReadInt32((IntPtr) MemoryHandler.Instance.ReadInt32(ptr6, 12));
                                    var amount = MemoryHandler.Instance.ReadInt32((IntPtr) MemoryHandler.Instance.ReadInt32(ptr6, 0x10));
                                    var comboAmount = MemoryHandler.Instance.ReadInt32((IntPtr) MemoryHandler.Instance.ReadInt32(ptr6, 20));
                                    var unk1 = MemoryHandler.Instance.ReadInt32((IntPtr) MemoryHandler.Instance.ReadInt32(ptr6));
                                    var unk2 = MemoryHandler.Instance.ReadInt32((IntPtr) MemoryHandler.Instance.ReadInt32(ptr6, 8));
                                    var unk3 = MemoryHandler.Instance.ReadInt32((IntPtr) MemoryHandler.Instance.ReadInt32(ptr6, 0x18));
                                    var unk4 = MemoryHandler.Instance.ReadInt32((IntPtr) MemoryHandler.Instance.ReadInt32(ptr6, 0x1C));
                                    //if (((type1 == 0) || (type2 == 0)) && (((type1 == 5) || (type2 == 12)) || (skillID == 0)))
                                    //{
                                    //    continue;
                                    //}
                                    if (skillID == 0)
                                    {
                                        continue;
                                    }
                                    var incomingEntry = new IncomingEntry
                                    {
                                        ID = (uint) ptr3.ToInt32(),
                                        Amount = amount,
                                        ComboAmount = comboAmount,
                                        Type1 = type1,
                                        Type2 = type2,
                                        SkillID = skillID,
                                        UNK1 = unk1,
                                        UNK2 = unk2,
                                        UNK3 = unk3,
                                        UNK4 = unk4,
                                        TargetID = targetID,
                                        TargetName = targetName,
                                        TargetType = targetType
                                    };
                                    incomingEntries.Add(incomingEntry);
                                }

                                #endregion

                                combatEntry.IncomingEntries = incomingEntries;

                                #region IncomingAction Handler

                                var incomingActionEntries = new List<IncomingActionEntry>();

                                switch (targetType)
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
                                                    TargetID = targetID,
                                                    TargetName = targetName,
                                                    TargetType = targetType
                                                };
                                                incomingActionEntries.Add(incomingActionEntry);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                        if (incomingActionEntries.Any())
                                        {
                                            CombatTracker.EnsureHistoryItem(incomingActionEntries);
                                        }
                                        break;
                                }

                                #endregion

                                combatEntry.IncomingActionEntries = incomingActionEntries;
                            }

                            Stopwatch.Stop();
                            Logging.Log(Logger, Stopwatch.ElapsedMilliseconds.ToString());
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
