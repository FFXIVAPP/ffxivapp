// FFXIVAPP
// Offsets.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.
// 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;

namespace FFXIVAPP.Classes.Memory
{
    public class Offsets
    {
        //***
        //commented out as xiv doesn't use dll's in main memory structure, will remove all .dll's from search
        //***
        //private string[] _systemModules = new[] {"physxcore.dll", "nxcooking.dll", "physxloader.dll", "physxextensions.dll", "cudart.dll", "openal32.dll", "vorbisfile.dll", "ogg.dll", "vorbis.dll", "vorbisenc.dll", "vorbisfile.dll", "binkw32.dll", "iconv.dll", "gameoverlayrenderer.dll", "mss32.dll", "dbghelp.dll", "umbra.dll", "unrar.dll", "cehook.dll", "allochook.dll", "dbghelp.dll", "d3dx9_24.dll", "d3dx9_25.dll", "d3dx9_26.dll", "d3dx9_27.dll", "d3dx9_28.dll", "d3dx9_29.dll", "d3dx9_30.dll", "d3dx9_31.dll", "d3dx9_32.dll", "d3dx9_33.dll", "d3dx9_34.dll", "d3dx9_35.dll", "d3dx9_36.dll", "d3dx9_37.dll", "d3dx9_38.dll", "d3dx9_39.dll", "d3dx9_40.dll", "d3dx9_41.dll", "d3dx9_42.dll", "d3dx9_43.dll", "d3dx9_44.dll", "d3dx9_45.dll", "d3dx10_33.dll", "d3dx10_34.dll", "d3dx10_35.dll", "d3dx10_36.dll", "d3dx10_37.dll", "d3dx10_38.dll", "d3dx10_39.dll", "d3dx10_40.dll", "d3dx10_41.dll", "d3dx10_42.dll", "d3dx10_43.dll", "d3dx10_44.dll", "d3dx10_45.dll", "d3d9.dll", "ffdshow.ax", "MP4Splitter.dll", "FLVSplitter.ax", "IMGDLL.dll", "ijl10.dll", "zlib1.dll", "steam_api.dll", "Steamclient.dll", "SteamAPIUpdater.dll", "UpdateDLLWrapper.dll", "gdf.dll", "gpudatabase.dll", "Lead3DEngine.dll", "Lead3DOffline.dll", "LeadD3DRender.dll", "LeadD3DRenderR.dll", "LeadD3DXenonR.dll", "systemdetection.dll", "DFA.dll", "EFLCGDF.dll", "FirewallInstallHelper.dll", "GameuxInstallHelper.dll", "LiveTokenHelper.dll", "paul.dll", "dsound.dll", "eax.dll", "D3DCompiler_42.dll", "Sims3GDF.dll", "TSLHost.dll", "EAWebkit.dll", "patchw32.dll", "VistaShellSupport.dll", "bs.dll", "dvm.dll", "dvm_bf.dll", "dvm_Bootstrapper.dll", "GDFBinary.dll", "pcre3.dll", "QtCore4.dll", "QtGui4.dll", "QtNetwork4.dll", "winui.dll", "rld.dll", "skidrow.dll", "fmodex.dll", "fmod_event.dll", "dishp.dll", "GDFDLL.dll", "nvtt.dll", "wrap_oal.dll", "saAuditMT.dll", "saAudit2005MT.dll", "IntelLaptopGamingXP.dll", "IntelLaptopGamingVista.dll", "PathEngine.dll", "cufft.dll"};

        public Dictionary<string, uint> Locations { get; private set; }
        private readonly Process _proc;
        private byte[] _memDump;
        private const int MemCommit = 0x1000;
        private const int PageNoaccess = 0x01;
        private const int PageGuard = 0x100;

        private struct MemRegion
        {
            public long Start;
            public long Length;
        }

        private List<MemRegion> _regions;

        /// <summary>
        /// </summary>
        /// <param name="proc"> </param>
        public Offsets(Process proc)
        {
            _proc = proc;
            LoadOffsets();
        }

        /// <summary>
        /// </summary>
        private void LoadOffsets()
        {
            if (_proc == null)
            {
                return;
            }
            LoadRegions();
            Locations = new Dictionary<string, uint>();
            Locations.Add("CHAT_POINTER", (uint) FindByteString("81000000??????00????????????????????0000????0000????0000????????????????08000000??00000014000000") + 16);
            //Locations.Add("STAT_INFO", (uint)FindByteString("000000000000000000000000000000803F0000803E000000000000") + 27);
            //Locations.Add("WIDGET_POINTER", (uint)FindByteString("0000000057696E646F775F4D61696E4D656E7557696467657400000000") + 4);
            //Locations.Add("NAME", (uint)FindByteString("0000??????000000??????00000000000000803F0000000000000000180000000E00000000000000000000000000803F0000") + 50);
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
                while (UnsafeNativeMethods.VirtualQueryEx(_proc.Handle, address, out info, Marshal.SizeOf(info)) != 0 && address < 0xFFFFFFFF && (address + info.RegionSize) > address)
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
            catch
            {
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="address"> </param>
        /// <returns> </returns>
        private bool IsSystemModule(int address)
        {
            var pm = MemoryHandler.GetModuleByAddress(_proc, address);
            if (pm != null)
            {
                if (pm.ModuleName.Contains(".dll"))
                {
                    return true;
                }
                //return _systemModules.Any(mod => mod == pm.ModuleName);
            }
            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="strPrefix"> </param>
        /// <param name="strSuffix"> </param>
        /// <returns> </returns>
        private long FindMemLoc(string strPrefix, string strSuffix)
        {
            if (_proc == null)
            {
                throw new Exception("Process value is null.");
            }
            try
            {
                var strPointerStart = strPrefix.Replace("-", String.Empty);
                var lngAddress = FindByteString(String.Format("{0}-([0-9|A-F][0-9|A-F])-([0-9|A-F][0-9|A-F])-([0-9|A-F][0-9|A-F])-([0-9|A-F][0-9|A-F])-{1}", strPrefix, strSuffix));
                var lngTemp = (lngAddress - _proc.MainModule.BaseAddress.ToInt32()) + (strPointerStart.Length/2);
                return BitConverter.ToInt32(_memDump, (int) lngTemp);
            }
            catch
            {
                return -1;
            }
        }

        private bool MaskCheck(int nOffset, byte[] btPattern, string strMask)
        {
            try
            {
                if (nOffset + btPattern.Length > _memDump.Length)
                {
                    return false;
                }
                for (var x = 0; x < btPattern.Length; x++)
                {
                    if (strMask[x] == '?')
                    {
                        continue;
                    }
                    if ((strMask[x] == 'x') && (btPattern[x] != _memDump[nOffset + x]))
                    {
                        return false;
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="search"> </param>
        /// <returns> </returns>
        private long FindByteString(string search)
        {
            if (_proc == null)
            {
                throw new Exception("Process value is null.");
            }
            var strSearch = search.Replace("([0-9|A-F][0-9|A-F])", "??").Replace("-", String.Empty);
            var strMask = String.Empty;
            var btPattern = new byte[(strSearch.Length/2)];
            for (var x = 0; x < btPattern.Length; x++)
            {
                if (strSearch.Substring(x*2, 2).Contains("??"))
                {
                    strMask += "?";
                    btPattern[x] = 0xFF;
                }
                else
                {
                    strMask += "x";
                    btPattern[x] = Byte.Parse(strSearch.Substring(x*2, 2), NumberStyles.HexNumber);
                }
            }
            try
            {
                for (var i = 0; i < _regions.Count; i++)
                {
                    _memDump = new MemoryHandler(_proc, (uint) _regions[i].Start).GetByteArray((int) _regions[i].Length);
                    for (var x = 0; x < _memDump.Length; x++)
                    {
                        if (MaskCheck(x, btPattern, strMask))
                        {
                            return ((int) _regions[i].Start + x);
                        }
                    }
                }
            }
            catch
            {
            }
            return -1;
        }
    }
}