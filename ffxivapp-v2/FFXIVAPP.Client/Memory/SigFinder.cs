// FFXIVAPP.Client
// SigFinder.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using FFXIVAPP.Common.Utilities;
using NLog;

#endregion

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

        #region Constants

        private const int MemCommit = 0x1000;
        private const int PageNoAccess = 0x01;
        private const int PageReadwrite = 0x04;
        private const int PageWritecopy = 0x08;
        private const int PageExecuteReadwrite = 0x40;
        private const int PageExecuteWritecopy = 0x80;
        private const int PageGuard = 0x100;
        private const int Writable = PageReadwrite | PageWritecopy | PageExecuteReadwrite | PageExecuteWritecopy | PageGuard;

        #endregion

        #region Declarations

        private readonly Process _process;
        private byte[] _memDump;
        private List<UnsafeNativeMethods.MemoryBasicInformation> _regions;

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="process"> </param>
        /// <param name="signatures"> </param>
        public SigFinder(Process process, List<Signature> signatures)
        {
            _process = process;
            LoadOffsets(signatures);
        }

        /// <summary>
        /// </summary>
        /// <param name="signatures"> </param>
        private void LoadOffsets(List<Signature> signatures)
        {
            Func<bool> d = delegate
            {
                var sw = new Stopwatch();
                sw.Start();
                if (_process == null)
                {
                    return false;
                }
                LoadRegions();
                Locations = new Dictionary<string, uint>();
                if (signatures.Any())
                {
                    //batch search testing
                    FindByteStringBatchSearch(signatures);
                    
                    //handle pointers with old search
                    //foreach (var pointer in pointers)
                    //{
                    //    Locations.Add(pointer.Name, (uint) (FindByteString(pointer.Pattern) + pointer.Offset));
                    //}
                }
                _memDump = null;
                sw.Stop();
                Debug.Print("SigFinder Completion Time: " + sw.Elapsed.TotalSeconds.ToString(CultureInfo.InvariantCulture));
                return true;
            };
            d.BeginInvoke(null, null);
        }

        /// <summary>
        /// </summary>
        private void LoadRegions()
        {
            try
            {
                _regions = new List<UnsafeNativeMethods.MemoryBasicInformation>();
                var info = new UnsafeNativeMethods.MemoryBasicInformation();
                var address = 0;
                while (UnsafeNativeMethods.VirtualQueryEx(_process.Handle, (uint) address, out info, (uint) Marshal.SizeOf(info)) != 0 && address < 4294967295)
                {
                    if (!IsSystemModule(info.BaseAddress))
                    {
                        var result = UnsafeNativeMethods.VirtualQueryEx(_process.Handle, (uint) address, out info, (uint) Marshal.SizeOf(info));
                        if (0 == result)
                        {
                            break;
                        }
                        if (0 != (info.State & MemCommit) && 0 != (info.Protect & Writable) && 0 == (info.Protect & PageGuard))
                        {
                            _regions.Add(info);
                        }
                    }
                    address = info.BaseAddress + info.RegionSize;
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
                var temp = (address - _process.MainModule.BaseAddress.ToInt32()) + (pointerStart.Length / 2);
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
            search = search.Replace("([0-9|A-F][0-9|A-F])", "??")
                           .Replace("-", String.Empty);
            var mask = String.Empty;
            var pattern = new byte[(search.Length / 2)];
            for (var x = 0; x < pattern.Length; x++)
            {
                if (search.Substring(x * 2, 2)
                          .Contains("??"))
                {
                    mask += "?";
                    pattern[x] = 0xFF;
                }
                else
                {
                    mask += "x";
                    pattern[x] = Byte.Parse(search.Substring(x * 2, 2), NumberStyles.HexNumber);
                }
            }
            try
            {
                for (var i = 0; i < _regions.Count; i++)
                {
                    _memDump = new MemoryHandler(_process, (uint) _regions[i].BaseAddress).GetByteArray(_regions[i].RegionSize);
                    for (var x = 0; x < _memDump.Length; x++)
                    {
                        if (MaskCheck(x, pattern, mask))
                        {
                            return (_regions[i].BaseAddress + x);
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

        private void FindByteStringBatchSearch(IList<Signature> signatures)
        {
            foreach (var signature in signatures)
            {
                var search = signature.Value.Replace("([0-9|A-F][0-9|A-F])", "..")
                                      .Replace("??", "..");
                signature.RegularExpress = new Regex(search);
            }
            for (var i = 0; i < _regions.Count; i++)
            {
                _memDump = new MemoryHandler(_process, (uint) _regions[i].BaseAddress).GetByteArray(_regions[i].RegionSize);
                var pattern = BitConverter.ToString(_memDump);
                for (var k = signatures.Count - 1; k >= 0; k--)
                {
                    var match = signatures[k].RegularExpress.Match(pattern);
                    if (!match.Success)
                    {
                        continue;
                    }
                    var pointerAddress = ((match.Index / 3) + _regions[i].BaseAddress);
                    Locations.Add(signatures[k].Key, (uint) (pointerAddress + signatures[k].Offset));
                    signatures.RemoveAt(k);
                }
            }
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
