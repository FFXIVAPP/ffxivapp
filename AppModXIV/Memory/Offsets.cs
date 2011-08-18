using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;

namespace AppModXIV.Memory
{
    public class Offsets
    {
        private string[] systemModules = new string[] { "physxcore.dll", "nxcooking.dll", "physxloader.dll", "physxextensions.dll", "cudart.dll", "openal32.dll", "vorbisfile.dll", "ogg.dll", "vorbis.dll", "vorbisenc.dll", "vorbisfile.dll", "binkw32.dll", "iconv.dll", "gameoverlayrenderer.dll", "mss32.dll", "dbghelp.dll", "umbra.dll", "unrar.dll", "cehook.dll", "allochook.dll", "dbghelp.dll", "d3dx9_24.dll", "d3dx9_25.dll", "d3dx9_26.dll", "d3dx9_27.dll", "d3dx9_28.dll", "d3dx9_29.dll", "d3dx9_30.dll", "d3dx9_31.dll", "d3dx9_32.dll", "d3dx9_33.dll", "d3dx9_34.dll", "d3dx9_35.dll", "d3dx9_36.dll", "d3dx9_37.dll", "d3dx9_38.dll", "d3dx9_39.dll", "d3dx9_40.dll", "d3dx9_41.dll", "d3dx9_42.dll", "d3dx9_43.dll", "d3dx9_44.dll", "d3dx9_45.dll", "d3dx10_33.dll", "d3dx10_34.dll", "d3dx10_35.dll", "d3dx10_36.dll", "d3dx10_37.dll", "d3dx10_38.dll", "d3dx10_39.dll", "d3dx10_40.dll", "d3dx10_41.dll", "d3dx10_42.dll", "d3dx10_43.dll", "d3dx10_44.dll", "d3dx10_45.dll", "d3d9.dll", "ffdshow.ax", "MP4Splitter.dll", "FLVSplitter.ax", "IMGDLL.dll", "ijl10.dll", "zlib1.dll", "steam_api.dll", "Steamclient.dll", "SteamAPIUpdater.dll", "UpdateDLLWrapper.dll", "gdf.dll", "gpudatabase.dll", "Lead3DEngine.dll", "Lead3DOffline.dll", "LeadD3DRender.dll", "LeadD3DRenderR.dll", "LeadD3DXenonR.dll", "systemdetection.dll", "DFA.dll", "EFLCGDF.dll", "FirewallInstallHelper.dll", "GameuxInstallHelper.dll", "LiveTokenHelper.dll", "paul.dll", "dsound.dll", "eax.dll", "D3DCompiler_42.dll", "Sims3GDF.dll", "TSLHost.dll", "EAWebkit.dll", "patchw32.dll", "VistaShellSupport.dll", "bs.dll", "dvm.dll", "dvm_bf.dll", "dvm_Bootstrapper.dll", "GDFBinary.dll", "pcre3.dll", "QtCore4.dll", "QtGui4.dll", "QtNetwork4.dll", "winui.dll", "rld.dll", "skidrow.dll", "fmodex.dll", "fmod_event.dll", "dishp.dll", "GDFDLL.dll", "nvtt.dll", "wrap_oal.dll", "saAuditMT.dll", "saAudit2005MT.dll", "IntelLaptopGamingXP.dll", "IntelLaptopGamingVista.dll", "PathEngine.dll", "cufft.dll" };

        public Dictionary<string, uint> Locations { get; set; }
        private Process _proc;
        private byte[] _memDump;
        private MemoryHandler _memory;

        public static string Base;

        private const int mem_image = 0x1000000;
        private const int mem_free = 0x40000;
        private const int mem_private = 0x20000;

        private const int mem_commit = 0x1000;

        private const int page_noaccess = 0x01;
        private const int page_guard = 0x100;

        private struct MemRegion
        {
            public long start;
            public long length;
        }

        private List<MemRegion> Regions;

        public Offsets(Process proc)
        {
            _proc = proc;
            _memory = new MemoryHandler(proc, 0);

            Base = Convert.ToString(_proc.MainModule.BaseAddress);

            LoadOffsets();
        }

        private void LoadOffsets()
        {
            if (_proc != null)
            {
                //DumpMemory();
                LoadRegions(0, (uint)_proc.PagedMemorySize64);

                Locations = new Dictionary<string, uint>();
                //Locations.Add("TARGETINFO", new MemoryHandler(_proc, (uint)FindByteString("000000004016D8000000000001000000") + 32).GetUInt32()); //3642A24
                //Locations.Add("NPCMAPPOINTER", new MemoryHandler(_proc, (uint)FindByteString("0000803F0000803F000040C000004040") + 608).GetUInt32());
                // Locations.Add("NAMELOC", (uint)FindByteString("015C636C69656E745C737177745C636F6D6D6F6E5C64656661756C742E77696E33322E677465780001004F445F636F6E6669675F646F7771") + 513);
                //Locations.Add("CRAFTPOINTER", (uint)FindByteString("507265766965774D6F7573655269676874427574746F6E5570") + 11479);
                Locations.Add("CHATPOINTER", (uint)FindByteString("81000000445FF0") + 20);
                _memDump = null;
            }

        }


        private void LoadRegions(uint startAddress, uint stopAddress)
        {
            try
            {

                this.Regions = new List<MemRegion>();
                UnsafeNativeMethods.MEMORY_BASIC_INFORMATION info = new UnsafeNativeMethods.MEMORY_BASIC_INFORMATION();

                uint address = 0;
                while (UnsafeNativeMethods.VirtualQueryEx(_proc.Handle, address, out info, Marshal.SizeOf(info)) != 0 && address < 0xFFFFFFFF && (address + info.RegionSize) > address)
                {
                    if (!isSystemModule(info.BaseAddress) && info.State == mem_commit && (info.Protect & page_guard) == 0 && (info.Protect & page_noaccess) == 0)
                    {
                        MemRegion region = new MemRegion { start = info.BaseAddress, length = info.RegionSize };
                        Regions.Add(region);
                    }
                    address = (uint)info.BaseAddress + (uint)info.RegionSize;
                }

                int i = 0;
                while (i < Regions.Count - 1)
                {
                    if (Regions[i].length > 512 * 1024)
                    {
                        MemRegion region = new MemRegion { start = Regions[i].start + 512 * 1024, length = 512 * 1024 };
                        Regions.Add(region);
                        region = Regions[i];
                        region.length = (long)(512 * 1024);
                        Regions[i] = region;
                    }
                    i++;
                }
            }
            catch (Exception e)
            {
                //if (Constants.LogErrors == 1)
                //{
                //    ErrorLogging.LogError(e.Message + e.StackTrace + e.InnerException);
                //}
            }
        }

        private bool isSystemModule(int address)
        {
            ProcessModule pm = MemoryHandler.GetModuleByAddress(_proc, address);
            if (pm != null)
            {
                foreach (string mod in systemModules)
                {
                    if (mod == pm.ModuleName)
                        return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Scan type 1.
        /// 
        /// Returns the pointer found between the prefix and suffix.
        /// </summary>
        /// <param name="strPrefix"></param>
        /// <param name="strSuffix"></param>
        /// <returns></returns>
        private long FindMemLoc(string strPrefix, string strSuffix)
        {
            // Validate PlayOnline Running
            if (_proc == null)
                throw new Exception("Process value is null.");

            try
            {
                string strPointerStart = strPrefix.Replace("-", String.Empty);
                long lngAddress = FindByteString(String.Format("{0}-([0-9|A-F][0-9|A-F])-([0-9|A-F][0-9|A-F])-([0-9|A-F][0-9|A-F])-([0-9|A-F][0-9|A-F])-{1}", strPrefix, strSuffix));

                long lngTemp = (lngAddress - _proc.MainModule.BaseAddress.ToInt32()) + (strPointerStart.Length / 2);

                return BitConverter.ToInt32(_memDump, (int)lngTemp);

                //return Int32.Parse(lngAddress + (strPointerStart.Length / 2), NumberStyles.HexNumber);
            }
            catch { return -1; }
        }

        private bool MaskCheck(int nOffset, byte[] btPattern, string strMask)
        {
            try
            {
                if (nOffset + btPattern.Length > _memDump.Length)
                    return false;

                for (int x = 0; x < btPattern.Length; x++)
                {
                    if (strMask[x] == '?')
                        continue;
                    if ((strMask[x] == 'x') && (btPattern[x] != _memDump[nOffset + x]))
                        return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private long FindByteString(string search)
        {
            // Validate PlayOnline Running
            if (_proc == null)
                throw new Exception("Process value is null.");

            // Build New Search String [Replace Regex Wildcards, Replace Hyphens]
            string strSearch = search.Replace("([0-9|A-F][0-9|A-F])", "??").Replace("-", String.Empty);

            // Build Search Mask and Pattern
            string strMask = String.Empty;
            byte[] btPattern = new byte[(strSearch.Length / 2)];
            for (int x = 0; x < btPattern.Length; x++)
            {
                if (strSearch.Substring(x * 2, 2).Contains("??"))
                {
                    strMask += "?";
                    btPattern[x] = 0xFF;
                }
                else
                {
                    strMask += "x";
                    btPattern[x] = Byte.Parse(strSearch.Substring(x * 2, 2), NumberStyles.HexNumber);
                }
            }
            try
            {
                // Scan For Pattern In Memory Dump
                for (int i = 0; i < Regions.Count; i++)
                {
                    _memDump = new MemoryHandler(_proc, (uint)Regions[i].start).GetByteArray((int)Regions[i].length);
                    for (int x = 0; x < _memDump.Length; x++)
                    {
                        if (MaskCheck(x, btPattern, strMask))
                            return ((int)Regions[i].start + x);
                    }
                }
            }
            catch (Exception ex)
            {
                //if (Constants.LogErrors == 1)
                //{
                //    ErrorLogging.LogError(ex.Message + ex.StackTrace + ex.InnerException);
                //}
            }
            return -1;
        }

    }
}
