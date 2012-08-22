// FFXIVAPP
// MemoryHandler.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using NLog;

namespace FFXIVAPP.Classes.Memory
{
    public class MemoryHandler
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// </summary>
        public struct MemoryBlock
        {
            public long Start;
            public long Length;
        }

        private Process Process { get; set; }
        public uint Address { private get; set; }

        /// <summary>
        /// </summary>
        /// <param name="proc"> </param>
        /// <param name="address"> </param>
        public MemoryHandler(Process proc, uint address)
        {
            Process = proc;
            Address = address;
        }

        /// <summary>
        /// </summary>
        /// <param name="proc"> </param>
        /// <param name="target"> </param>
        /// <param name="data"> </param>
        /// <returns> </returns>
        private static bool Poke(Process proc, uint target, byte[] data)
        {
            var byteWritten = new IntPtr(0);
            return UnsafeNativeMethods.WriteProcessMemory(proc.Handle, new IntPtr(target), data, new UIntPtr((UInt32) data.Length), ref byteWritten);
        }

        /// <summary>
        /// </summary>
        /// <param name="proc"> </param>
        /// <param name="address"> </param>
        /// <param name="buffer"> </param>
        /// <returns> </returns>
        private static bool Peek(Process proc, uint address, byte[] buffer)
        {
            var target = new IntPtr(address);
            return proc != null && UnsafeNativeMethods.ReadProcessMemory(proc.Handle, target, buffer, buffer.Length, 0);
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public static Process[] GetProcesses()
        {
            var result = Process.GetProcesses();
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="id"> </param>
        /// <returns> </returns>
        public static Process GetProcessById(int id)
        {
            try
            {
                var result = Process.GetProcessById(id);
                return result;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <returns> </returns>
        public static Process GetProcessByName(string name)
        {
            var processes = Process.GetProcessesByName(name);
            if (processes.Length <= 0)
            {
                return null;
            }
            var result = processes[0];
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="proc"> </param>
        /// <returns> </returns>
        private static IEnumerable<ProcessModule> GetModules(Process proc)
        {
            try
            {
                var modules = proc.Modules;
                var result = new ProcessModule[modules.Count];
                for (var i = 0; i < modules.Count; i++)
                {
                    result[i] = modules[i];
                }
                return result;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="proc"> </param>
        /// <param name="name"> </param>
        /// <returns> </returns>
        public static ProcessModule GetModuleByName(Process proc, String name)
        {
            ProcessModule result = null;
            try
            {
                var modules = GetModules(proc);
                foreach (var module in modules.Where(module => module.ModuleName.IndexOf(name, StringComparison.Ordinal) > -1))
                {
                    result = module;
                    break;
                }
            }
            catch
            {
                return null;
            }
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="proc"> </param>
        /// <param name="address"> </param>
        /// <returns> </returns>
        public static ProcessModule GetModuleByAddress(Process proc, Int32 address)
        {
            try
            {
                var modules = GetModules(proc);
                return (from module in modules let baseAddress = module.BaseAddress.ToInt32() where (baseAddress <= address) && (baseAddress + module.ModuleMemorySize >= address) select module).FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="proc"> </param>
        /// <param name="file"> </param>
        /// <returns> </returns>
        private static int GetModuleBaseAddress(Process proc, string file)
        {
            var modCol = proc.Modules;
            foreach (var procMod in modCol.Cast<ProcessModule>().Where(procMod => procMod.FileName == file))
            {
                return procMod.BaseAddress.ToInt32();
            }
            return -1;
        }

        /// <summary>
        /// </summary>
        /// <param name="proc"> </param>
        /// <param name="file"> </param>
        /// <returns> </returns>
        private static int GetModuleEndAddress(Process proc, string file)
        {
            var modCol = proc.Modules;
            foreach (var procMod in modCol.Cast<ProcessModule>().Where(procMod => procMod.FileName == file))
            {
                return procMod.BaseAddress.ToInt32() + procMod.ModuleMemorySize;
            }
            return -1;
        }

        /// <summary>
        /// </summary>
        /// <param name="proc"> </param>
        /// <returns> </returns>
        public static MemoryBlock GetProcessMemoryBlock(Process proc)
        {
            var counter = new UnsafeNativeMethods.ProcessMemoryCounters();
            UnsafeNativeMethods.GetProcessMemoryInfo(proc.Handle, out counter, Marshal.SizeOf(counter));
            var block = new MemoryBlock {Start = proc.MainModule.BaseAddress.ToInt64(), Length = counter.PagefileUsage};
            return block;
        }

        /// <summary>
        /// </summary>
        /// <param name="intLength"> </param>
        /// <returns> </returns>
        public string GetOperationCode(int intLength)
        {
            var buffer1 = GetByteArray(intLength);
            return BitConverter.ToString(buffer1);
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public byte GetByte()
        {
            var data = new byte[1];
            Peek(Process, Address, data);
            return data[0];
        }

        /// <summary>
        /// </summary>
        /// <param name="offset"> </param>
        /// <returns> </returns>
        public byte GetByte(uint offset)
        {
            var data = new byte[1];
            Peek(Process, Address + offset, data);
            return data[0];
        }

        /// <summary>
        /// </summary>
        /// <param name="num"> </param>
        /// <returns> </returns>
        public byte[] GetByteArray(int num)
        {
            var data = new byte[num];
            Peek(Process, Address, data);
            return data;
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public double GetDouble()
        {
            var value = new byte[8];
            Peek(Process, Address, value);
            return BitConverter.ToDouble(value, 0);
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public float GetFloat()
        {
            var value = new byte[4];
            Peek(Process, Address, value);
            return BitConverter.ToSingle(value, 0);
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public short GetInt16()
        {
            var value = new byte[2];
            Peek(Process, Address, value);
            return BitConverter.ToInt16(value, 0);
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public int GetInt32()
        {
            var value = new byte[4];
            Peek(Process, Address, value);
            return BitConverter.ToInt32(value, 0);
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public long GetInt64()
        {
            var value = new byte[8];
            Peek(Process, Address, value);
            return BitConverter.ToInt64(value, 0);
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public string GetString()
        {
            var bytes = new byte[24];
            Peek(Process, Address, bytes);
            return Encoding.ASCII.GetString(bytes);
        }

        /// <summary>
        /// </summary>
        /// <param name="maxSize"> </param>
        /// <returns> </returns>
        public string GetString(int maxSize)
        {
            var bytes = new byte[maxSize];
            Peek(Process, Address, bytes);
            var realSize = 0;
            for (var i = 0; i < maxSize; i++)
            {
                if (bytes[i] != 0)
                {
                    continue;
                }
                realSize = i;
                break;
            }
            Array.Resize(ref bytes, realSize);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// </summary>
        /// <param name="size"> </param>
        /// <returns> </returns>
        public string GetStringBySize(int size)
        {
            var bytes = new byte[size];
            Peek(Process, Address, bytes);
            return Encoding.ASCII.GetString(bytes);
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public int GetProgram()
        {
            var value = new byte[30];
            Peek(Process, Address, value);
            return BitConverter.ToInt32(value, 0);
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public UInt32 GetUInt32()
        {
            var value = new byte[4];
            Peek(Process, Address, value);
            return BitConverter.ToUInt32(value, 0);
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public UInt16 GetUInt16()
        {
            var value = new byte[4];
            Peek(Process, Address, value);
            return BitConverter.ToUInt16(value, 0);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <returns> </returns>
        public T GetStructure<T>()
        {
            var lpBytesWritten = 0;
            var buffer = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof (T)));
            UnsafeNativeMethods.ReadProcessMemory(Process.Handle, new IntPtr(Address), buffer, Marshal.SizeOf(typeof (T)), ref lpBytesWritten);
            var retValue = (T) Marshal.PtrToStructure(buffer, typeof (T));
            Marshal.FreeCoTaskMem(buffer);
            return retValue;
        }

        /// <summary>
        /// </summary>
        /// <param name="val"> </param>
        public void Reset(int val)
        {
            var data = BitConverter.GetBytes(val);
            Poke(Process, Address, data);
        }

        /// <summary>
        /// </summary>
        /// <param name="val"> </param>
        /// <returns> </returns>
        public bool SetByte(byte val)
        {
            var data = BitConverter.GetBytes(val);
            return Poke(Process, Address, data);
        }

        /// <summary>
        /// </summary>
        /// <param name="val"> </param>
        /// <returns> </returns>
        public bool SetByteArray(byte[] val)
        {
            return Poke(Process, Address, val);
        }

        /// <summary>
        /// </summary>
        /// <param name="val"> </param>
        /// <returns> </returns>
        public bool SetDouble(double val)
        {
            var data = BitConverter.GetBytes(val);
            return Poke(Process, Address, data);
        }

        /// <summary>
        /// </summary>
        /// <param name="val"> </param>
        /// <returns> </returns>
        public bool SetFloat(float val)
        {
            var data = BitConverter.GetBytes(val);
            return Poke(Process, Address, data);
        }

        /// <summary>
        /// </summary>
        /// <param name="val"> </param>
        /// <returns> </returns>
        public bool SetInt16(short val)
        {
            var data = BitConverter.GetBytes(val);
            return Poke(Process, Address, data);
        }

        /// <summary>
        /// </summary>
        /// <param name="val"> </param>
        /// <returns> </returns>
        public bool SetInt32(int val)
        {
            var data = BitConverter.GetBytes(val);
            return Poke(Process, Address, data);
        }

        /// <summary>
        /// </summary>
        /// <param name="val"> </param>
        /// <returns> </returns>
        public bool SetInt64(long val)
        {
            var data = BitConverter.GetBytes(val);
            return Poke(Process, Address, data);
        }

        /// <summary>
        /// </summary>
        /// <param name="val"> </param>
        /// <returns> </returns>
        public bool SetUInt16(UInt16 val)
        {
            var data = BitConverter.GetBytes(val);
            return Poke(Process, Address, data);
        }

        /// <summary>
        /// </summary>
        /// <param name="val"> </param>
        /// <returns> </returns>
        public bool SetUInt32(UInt32 val)
        {
            var data = BitConverter.GetBytes(val);
            return Poke(Process, Address, data);
        }

        /// <summary>
        /// </summary>
        /// <param name="bytes"> </param>
        /// <returns> </returns>
        public static string GetStringFromByteArray(byte[] bytes)
        {
            var u8 = new UTF8Encoding();
            var text1 = u8.GetString(bytes);
            var startIndex = text1.IndexOf(Convert.ToChar(0));
            if ((startIndex != -1))
            {
                text1 = text1.Remove(startIndex, (text1.Length - startIndex));
            }
            return text1;
        }

        /// <summary>
        /// </summary>
        /// <param name="byAr"> </param>
        /// <returns> </returns>
        public static char[] ByteArToCharAr(byte[] byAr)
        {
            var chAr = new char[byAr.Length];
            for (var x = 0; x < byAr.Length; x++)
            {
                chAr[x] = Convert.ToChar(byAr[x]);
            }
            return chAr;
        }

        /// <summary>
        /// </summary>
        /// <param name="chAr"> </param>
        /// <returns> </returns>
        public static byte[] CharArToByteAr(char[] chAr)
        {
            var byAr = new byte[chAr.Length];
            for (var x = 0; x < chAr.Length; x++)
            {
                byAr[x] = Convert.ToByte(chAr[x]);
            }
            return byAr;
        }
    }
}