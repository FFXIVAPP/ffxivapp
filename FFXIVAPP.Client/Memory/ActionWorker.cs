// FFXIVAPP.Client
// ActionWorker.cs
// 
// Copyright © 2007 - 2014 Ryan Wilson - All Rights Reserved
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions are met: 
// 
//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright 
//    notice, this list of conditions and the following disclaimer in the 
//    documentation and/or other materials provided with the distribution. 
//  * Neither the name of SyndicatedLife nor the names of its contributors may 
//    be used to endorse or promote products derived from this software 
//    without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE. 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Client.Utilities;
using FFXIVAPP.Common.Core.Memory.Enums;
using FFXIVAPP.Common.Utilities;
using NLog;

namespace FFXIVAPP.Client.Memory
{
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
            double refresh = 5;
            if (Double.TryParse(Settings.Default.ActionWorkerRefresh.ToString(CultureInfo.InvariantCulture), out refresh))
            {
                _scanTimer.Interval = refresh;
            }
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
                                var entryID = BitConverter.ToUInt32(source, 0x74);
                                var entryType = (Actor.Type) source[0x8A];
                                var entryName = MemoryHandler.Instance.GetStringFromBytes(source, 48);

                                #region FlyingText Handler

                                try
                                {
                                    var flyingTextEntries = new List<FlyingTextEntry>();

                                    switch (entryType)
                                    {
                                        case Actor.Type.Monster:
                                        case Actor.Type.PC:
                                            IntPtr flyingTextAddress;
                                            if ((flyingTextAddress = MemoryHandler.Instance.ReadPointer((IntPtr) BitConverter.ToUInt32(source, 0x3194))) == IntPtr.Zero)
                                            {
                                                continue;
                                            }
                                            var limit = MemoryHandler.Instance.GetInt32(BitConverter.ToUInt32(source, 0x3198));
                                            //MemoryHandler.Instance.ReadInt32((IntPtr) characterAddress, 0x319C);
                                            //MemoryHandler.Instance.ReadInt32((IntPtr) characterAddress, 0x31A0);
                                            //MemoryHandler.Instance.ReadInt32((IntPtr) characterAddress, 0x31B0);
                                            if (limit < 0x100)
                                            {
                                                for (var m = 0; m < limit; m++)
                                                {
                                                    try
                                                    {
                                                        IntPtr ptr3;
                                                        if ((ptr3 = MemoryHandler.Instance.ReadPointer(IntPtr.Add(flyingTextAddress, m * 4))) == IntPtr.Zero)
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
                                                        var flyingTextEntry = new FlyingTextEntry
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
                                                        if (flyingTextEntry.SkillID > 0)
                                                        {
                                                            flyingTextEntries.Add(flyingTextEntry);
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                    }
                                                }
                                            }
                                            break;
                                    }
                                }
                                catch (Exception ex)
                                {
                                }

                                #endregion

                                #region IncomingAction Handler

                                var incomingActionEntries = new List<IncomingActionEntry>();

                                try
                                {
                                    switch (entryType)
                                    {
                                        case Actor.Type.Monster:
                                        case Actor.Type.PC:
                                            var sourceArray = new byte[0xBB8];
                                            Buffer.BlockCopy(source, 0x3480, sourceArray, 0, 0xBB8);
                                            for (uint d = 0; d < 30; d++)
                                            {
                                                try
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
                                                catch (Exception ex)
                                                {
                                                }
                                            }
                                            break;
                                    }
                                }
                                catch (Exception ex)
                                {
                                }

                                #endregion

                                #region OutGoing Handler

                                var outGoingActionEntries = new List<OutGoingActionEntry>();

                                try
                                {
                                    switch (entryType)
                                    {
                                        case Actor.Type.Monster:
                                        case Actor.Type.PC:
                                            var sourceArray = new byte[0x108];
                                            Buffer.BlockCopy(source, 0x3334, sourceArray, 0, 0x108);
                                            var outGoingActionEntry = new OutGoingActionEntry
                                            {
                                                Amount = 0,
                                                SequenceID = BitConverter.ToInt32(sourceArray, 8),
                                                SkillID = BitConverter.ToInt32(sourceArray, 0),
                                                SourceID = entryID,
                                                TargetID = BitConverter.ToUInt32(sourceArray, 12),
                                                Type = source[0x8A]
                                            };
                                            if (outGoingActionEntry.SequenceID > 0 && outGoingActionEntry.SkillID > 0 && outGoingActionEntry.SourceID > 0 && outGoingActionEntry.TargetID != 0xE0000000)
                                            {
                                                outGoingActionEntries.Add(outGoingActionEntry);
                                            }
                                            break;
                                    }
                                }
                                catch (Exception ex)
                                {
                                }

                                #endregion
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
