// FFXIVAPP.Client
// InventoryWorker.cs
// 
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
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
using System.Runtime.CompilerServices;
using System.Timers;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Common.Core.Memory;
using FFXIVAPP.Common.Core.Memory.Enums;
using Newtonsoft.Json;
using NLog;

namespace FFXIVAPP.Client.Memory
{
    internal class InventoryWorker : INotifyPropertyChanged, IDisposable
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Property Bindings

        public uint InventoryPointerMap { get; set; }
        public List<InventoryEntity> LastInventoryEntities { get; set; }

        #endregion

        #region Declarations

        private readonly Timer _scanTimer;
        private bool _isScanning;

        #endregion

        public InventoryWorker()
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
            double refresh = 100;
            if (Double.TryParse(Settings.Default.InventoryWorkerRefresh.ToString(CultureInfo.InvariantCulture), out refresh))
            {
                _scanTimer.Interval = refresh;
            }
            Func<bool> scannerWorker = delegate
            {
                if (MemoryHandler.Instance.SigScanner.Locations.ContainsKey("INVENTORY"))
                {
                    try
                    {
                        InventoryPointerMap = MemoryHandler.Instance.GetUInt32(MemoryHandler.Instance.SigScanner.Locations["INVENTORY"]);

                        var inventoryEntities = new List<InventoryEntity>
                        {
                            GetItems(InventoryPointerMap, Inventory.Container.INVENTORY_1),
                            GetItems(InventoryPointerMap, Inventory.Container.INVENTORY_2),
                            GetItems(InventoryPointerMap, Inventory.Container.INVENTORY_3),
                            GetItems(InventoryPointerMap, Inventory.Container.INVENTORY_4),
                            GetItems(InventoryPointerMap, Inventory.Container.CURRENT_EQ),
                            GetItems(InventoryPointerMap, Inventory.Container.EXTRA_EQ),
                            GetItems(InventoryPointerMap, Inventory.Container.CRYSTALS),
                            GetItems(InventoryPointerMap, Inventory.Container.QUESTS_KI),
                            GetItems(InventoryPointerMap, Inventory.Container.HIRE_1),
                            GetItems(InventoryPointerMap, Inventory.Container.HIRE_2),
                            GetItems(InventoryPointerMap, Inventory.Container.HIRE_3),
                            GetItems(InventoryPointerMap, Inventory.Container.HIRE_4),
                            GetItems(InventoryPointerMap, Inventory.Container.HIRE_5),
                            GetItems(InventoryPointerMap, Inventory.Container.HIRE_6),
                            GetItems(InventoryPointerMap, Inventory.Container.HIRE_7),
                            GetItems(InventoryPointerMap, Inventory.Container.COMPANY_1),
                            GetItems(InventoryPointerMap, Inventory.Container.COMPANY_2),
                            GetItems(InventoryPointerMap, Inventory.Container.COMPANY_3),
                            GetItems(InventoryPointerMap, Inventory.Container.COMPANY_CRYSTALS),
                            GetItems(InventoryPointerMap, Inventory.Container.AC_MH),
                            GetItems(InventoryPointerMap, Inventory.Container.AC_OH),
                            GetItems(InventoryPointerMap, Inventory.Container.AC_HEAD),
                            GetItems(InventoryPointerMap, Inventory.Container.AC_BODY),
                            GetItems(InventoryPointerMap, Inventory.Container.AC_HANDS),
                            GetItems(InventoryPointerMap, Inventory.Container.AC_BELT),
                            GetItems(InventoryPointerMap, Inventory.Container.AC_LEGS),
                            GetItems(InventoryPointerMap, Inventory.Container.AC_FEET),
                            GetItems(InventoryPointerMap, Inventory.Container.AC_EARRINGS),
                            GetItems(InventoryPointerMap, Inventory.Container.AC_NECK),
                            GetItems(InventoryPointerMap, Inventory.Container.AC_WRISTS),
                            GetItems(InventoryPointerMap, Inventory.Container.AC_RINGS),
                            GetItems(InventoryPointerMap, Inventory.Container.AC_SOULS)
                        };
                        var notify = false;
                        if (LastInventoryEntities == null)
                        {
                            LastInventoryEntities = inventoryEntities;
                            notify = true;
                        }
                        else
                        {
                            var hash1 = JsonConvert.SerializeObject(LastInventoryEntities)
                                                   .GetHashCode();
                            var hash2 = JsonConvert.SerializeObject(inventoryEntities)
                                                   .GetHashCode();
                            if (!hash1.Equals(hash2))
                            {
                                LastInventoryEntities = inventoryEntities;
                                notify = true;
                            }
                            // Get Latest Character Name
                            if (MemoryHandler.Instance.SigScanner.Locations.ContainsKey("CHARMAP"))
                            {
                                try
                                {
                                    var charMapAddress = MemoryHandler.Instance.SigScanner.Locations["CHARMAP"];
                                    var characterAddressMap = MemoryHandler.Instance.GetByteArray(charMapAddress, 4);
                                    var characterAddress = BitConverter.ToUInt32(characterAddressMap, 0);
                                    var name = MemoryHandler.Instance.GetString(characterAddress, 48);
                                    if (Settings.Default.CharacterName != name || String.IsNullOrWhiteSpace(Settings.Default.CharacterName))
                                    {
                                        Settings.Default.CharacterName = name;
                                        notify = true;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Settings.Default.CharacterName = "";
                                }
                            }
                            else
                            {
                                Settings.Default.CharacterName = "";
                            }
                        }
                        if (notify)
                        {
                            AppContextHelper.Instance.RaiseNewInventoryEntries(inventoryEntities);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }

                _isScanning = false;
                return true;
            };
            scannerWorker.BeginInvoke(delegate { }, scannerWorker);
        }

        private InventoryEntity GetItems(uint address, Inventory.Container type)
        {
            var offset = (uint) ((int) type * 24);
            var containerAddress = MemoryHandler.Instance.GetUInt32(address, offset);

            var container = new InventoryEntity
            {
                Amount = MemoryHandler.Instance.GetByte(address, offset + 0x8),
                Items = new List<ItemInfo>(),
                Type = type
            };
            // The number of item is 50 in COMPANY's locker
            int limit;
            switch (type)
            {
                case Inventory.Container.COMPANY_1:
                case Inventory.Container.COMPANY_2:
                case Inventory.Container.COMPANY_3:
                    limit = 3200;
                    break;
                default:
                    limit = 1600;
                    break;
            }

            for (var ci = 0; ci < limit; ci += 64)
            {
                var itemOffset = (uint) (containerAddress + ci);
                var id = MemoryHandler.Instance.GetUInt32(itemOffset, 0x8);
                if (id > 0)
                {
                    container.Items.Add(new ItemInfo
                    {
                        ID = id,
                        Slot = MemoryHandler.Instance.GetByte(itemOffset, 0x4),
                        Amount = MemoryHandler.Instance.GetByte(itemOffset, 0xC),
                        SB = MemoryHandler.Instance.GetUInt16(itemOffset, 0x10),
                        Durability = MemoryHandler.Instance.GetUInt16(itemOffset, 0x12),
                        GlamourID = MemoryHandler.Instance.GetUInt32(itemOffset, 0x30),
                        //get the flag that show if the item is hq or not
                        IsHQ = (MemoryHandler.Instance.GetByte(itemOffset, 0x14) == 0x01)
                    });
                }
            }

            return container;
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
