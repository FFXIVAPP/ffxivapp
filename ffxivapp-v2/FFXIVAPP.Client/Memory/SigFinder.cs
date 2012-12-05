// FFXIVAPP.Client
// Offsets.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using FFXIVAPP.Common.Utilities;
using NLog;

namespace FFXIVAPP.Client.Memory
{
    internal class SigFinder : INotifyPropertyChanged
    {
        #region Property Bindings

        private Dictionary<string, uint> _locations;

        public Dictionary<string, uint> Locations
        {
            get { return _locations ?? (_locations = new Dictionary<string, uint>()); }
            private set
            {
                _locations = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Private Structs

        private struct MemRegion
        {
            public long Start;
            public long Length;
        }

        #endregion

        #region Declarations

        private readonly Process _process;
        private List<MemRegion> _regions;
        private byte[] _memDump;
        private const int MemCommit = 0x1000;
        private const int PageNoaccess = 0x01;
        private const int PageGuard = 0x100;

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="process"> </param>
        /// <param name="pointers"> </param>
        public SigFinder(Process process, List<Pointers> pointers)
        {
            _process = process;
            LoadOffsets(pointers);
        }

        /// <summary>
        /// </summary>
        /// <param name="pointers"></param>
        private void LoadOffsets(List<Pointers> pointers)
        {
            if (_process == null)
            {
                return;
            }
            LoadRegions();
            Locations = new Dictionary<string, uint>();
            if (pointers.Any())
            {
                foreach (var pointer in pointers)
                {
                    Locations.Add(pointer.Key, (uint)(FindByteString(pointer.Value) + pointer.Offset));
                }
            }
            else
            {
                Locations.Add("CHATLOG", (uint)FindByteString("81000000??????00????????????????????0000????0000????0000????????????????08000000??00000014000000") + 16);
            }
            //Locations.Add("CHATLOG", (uint)FindByteString("40000000060000000000000000010212020203"));
            _memDump = null;
        }

        /// <summary>
        /// </summary>
        private void LoadRegions()
        {
            try
            {
                _regions = new List<MemRegion>();
                var info = new UnsafeNativeMethods.MemoryBasicInformation();
                uint address = 0;
                while (UnsafeNativeMethods.VirtualQueryEx(_process.Handle, address, out info, Marshal.SizeOf(info)) != 0 && address < 0xFFFFFFFF && (address + info.RegionSize) > address)
                {
                    if (!IsSystemModule(info.BaseAddress) && info.State == MemCommit && (info.Protect & PageGuard) == 0 && (info.Protect & PageNoaccess) == 0)
                    {
                        var region = new MemRegion {Start = info.BaseAddress, Length = info.RegionSize};
                        _regions.Add(region);
                    }
                    address = (uint) info.BaseAddress + (uint) info.RegionSize;
                }
                var i = 0;
                while (i < _regions.Count - 1)
                {
                    if (_regions[i].Length > 512*1024)
                    {
                        var region = new MemRegion {Start = _regions[i].Start + 512*1024, Length = 512*1024};
                        _regions.Add(region);
                        region = _regions[i];
                        region.Length = (512*1024);
                        _regions[i] = region;
                    }
                    i++;
                }
            }
            catch (Exception ex)
            {
                Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="address"> </param>
        /// <returns> </returns>
        private bool IsSystemModule(int address)
        {
            var processModule = MemoryHandler.GetModuleByAddress(_process, address);
            if (processModule != null)
            {
                if (processModule.ModuleName.Contains(".dll"))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="prefix"> </param>
        /// <param name="suffix"> </param>
        /// <returns> </returns>
        private long FindMemLoc(string prefix, string suffix)
        {
            if (_process == null)
            {
                throw new Exception("Process value is null.");
            }
            try
            {
                var pointerStart = prefix.Replace("-", String.Empty);
                var address = FindByteString(String.Format("{0}-([0-9|A-F][0-9|A-F])-([0-9|A-F][0-9|A-F])-([0-9|A-F][0-9|A-F])-([0-9|A-F][0-9|A-F])-{1}", prefix, suffix));
                var temp = (address - _process.MainModule.BaseAddress.ToInt32()) + (pointerStart.Length/2);
                return BitConverter.ToInt32(_memDump, (int) temp);
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="offset"> </param>
        /// <param name="pattern"> </param>
        /// <param name="mask"> </param>
        /// <returns> </returns>
        private bool MaskCheck(int offset, byte[] pattern, string mask)
        {
            try
            {
                if (offset + pattern.Length > _memDump.Length)
                {
                    return false;
                }
                for (var x = 0; x < pattern.Length; x++)
                {
                    if (mask[x] == '?')
                    {
                        continue;
                    }
                    if ((mask[x] == 'x') && (pattern[x] != _memDump[offset + x]))
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
                return false;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="search"> </param>
        /// <returns> </returns>
        private long FindByteString(string search)
        {
            if (_process == null)
            {
                throw new Exception("Process value is null.");
            }
            search = search.Replace("([0-9|A-F][0-9|A-F])", "??").Replace("-", String.Empty);
            var mask = String.Empty;
            var pattern = new byte[(search.Length/2)];
            for (var x = 0; x < pattern.Length; x++)
            {
                if (search.Substring(x*2, 2).Contains("??"))
                {
                    mask += "?";
                    pattern[x] = 0xFF;
                }
                else
                {
                    mask += "x";
                    pattern[x] = Byte.Parse(search.Substring(x*2, 2), NumberStyles.HexNumber);
                }
            }
            try
            {
                for (var i = 0; i < _regions.Count; i++)
                {
                    _memDump = new MemoryHandler(_process, (uint) _regions[i].Start).GetByteArray((int) _regions[i].Length);
                    for (var x = 0; x < _memDump.Length; x++)
                    {
                        if (MaskCheck(x, pattern, mask))
                        {
                            return ((int) _regions[i].Start + x);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
                return -1;
            }
            return -1;
        }

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion
    }
}