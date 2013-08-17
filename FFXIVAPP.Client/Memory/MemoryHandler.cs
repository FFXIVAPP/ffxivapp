// FFXIVAPP.Client
// MemoryHandler.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

#endregion

namespace FFXIVAPP.Client.Memory
{
    internal class MemoryHandler : INotifyPropertyChanged
    {
        #region Property Bindings

        private uint _address;
        private Process _process;

        private Process Process
        {
            get { return _process; }
            set
            {
                _process = value;
                RaisePropertyChanged();
            }
        }

        public uint Address
        {
            private get { return _address; }
            set
            {
                _address = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Private Structs

        public struct MemoryBlock
        {
            public long Length;
            public long Start;
        }

        #endregion

        #region Declarations

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="process"> </param>
        /// <param name="address"> </param>
        public MemoryHandler(Process process, uint address)
        {
            Process = process;
            Address = address;
        }

        /// <summary>
        /// </summary>
        /// <param name="process"> </param>
        /// <param name="target"> </param>
        /// <param name="data"> </param>
        /// <returns> </returns>
        private static bool Poke(Process process, uint target, byte[] data)
        {
            var byteWritten = new IntPtr(0);
            return UnsafeNativeMethods.WriteProcessMemory(process.Handle, new IntPtr(target), data, new UIntPtr((UInt32) data.Length), ref byteWritten);
        }

        /// <summary>
        /// </summary>
        /// <param name="process"> </param>
        /// <param name="address"> </param>
        /// <param name="buffer"> </param>
        /// <returns> </returns>
        private static bool Peek(Process process, uint address, byte[] buffer)
        {
            var target = new IntPtr(address);
            return process != null && UnsafeNativeMethods.ReadProcessMemory(process.Handle, target, buffer, buffer.Length, 0);
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
        /// <param name="name"> </param>
        /// <returns> </returns>
        public static Process[] GetProcessesByName(string name)
        {
            var result = Process.GetProcessesByName(name);
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="process"> </param>
        /// <returns> </returns>
        private static IEnumerable<ProcessModule> GetModules(Process process)
        {
            try
            {
                var modules = process.Modules;
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
        /// <param name="process"> </param>
        /// <param name="name"> </param>
        /// <returns> </returns>
        public static ProcessModule GetModuleByName(Process process, string name)
        {
            ProcessModule result = null;
            try
            {
                var modules = GetModules(process);
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
        /// <param name="process"> </param>
        /// <param name="address"> </param>
        /// <returns> </returns>
        public static ProcessModule GetModuleByAddress(Process process, Int32 address)
        {
            try
            {
                var modules = GetModules(process);
                return (from module in modules let baseAddress = module.BaseAddress.ToInt32() where (baseAddress <= address) && (baseAddress + module.ModuleMemorySize >= address) select module).FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="process"> </param>
        /// <param name="file"> </param>
        /// <returns> </returns>
        private static int GetModuleBaseAddress(Process process, string file)
        {
            var modCol = process.Modules;
            foreach (var procMod in modCol.Cast<ProcessModule>()
                                          .Where(procMod => procMod.FileName == file))
            {
                return procMod.BaseAddress.ToInt32();
            }
            return -1;
        }

        /// <summary>
        /// </summary>
        /// <param name="process"> </param>
        /// <param name="file"> </param>
        /// <returns> </returns>
        private static int GetModuleEndAddress(Process process, string file)
        {
            var modCol = process.Modules;
            foreach (var procMod in modCol.Cast<ProcessModule>()
                                          .Where(procMod => procMod.FileName == file))
            {
                return procMod.BaseAddress.ToInt32() + procMod.ModuleMemorySize;
            }
            return -1;
        }

        /// <summary>
        /// </summary>
        /// <param name="process"> </param>
        /// <returns> </returns>
        public static MemoryBlock GetProcessMemoryBlock(Process process)
        {
            var counter = new UnsafeNativeMethods.ProcessMemoryCounters();
            UnsafeNativeMethods.GetProcessMemoryInfo(process.Handle, out counter, Marshal.SizeOf(counter));
            var block = new MemoryBlock
            {
                Start = process.MainModule.BaseAddress.ToInt64(),
                Length = counter.PagefileUsage
            };
            return block;
        }

        /// <summary>
        /// </summary>
        /// <param name="length"> </param>
        /// <returns> </returns>
        public string GetOperationCode(int length)
        {
            var buffer = GetByteArray(length);
            return BitConverter.ToString(buffer);
        }

        /// <summary>
        /// </summary>
        /// <param name="offset"> </param>
        /// <returns> </returns>
        public byte GetByte(uint offset = 0)
        {
            var data = new byte[1];
            Peek(Process, Address + offset, data);
            return data[0];
        }

        /// <summary>
        /// </summary>
        /// <param name="length"> </param>
        /// <returns> </returns>
        public byte[] GetByteArray(int length)
        {
            var data = new byte[length];
            Peek(Process, Address, data);
            return data;
        }

        /// <summary>
        /// </summary>
        /// <param name="offset"> </param>
        /// <returns> </returns>
        public double GetDouble(uint offset = 0)
        {
            var value = new byte[8];
            Peek(Process, Address + offset, value);
            return BitConverter.ToDouble(value, 0);
        }

        /// <summary>
        /// </summary>
        /// <param name="offset"> </param>
        /// <returns> </returns>
        public float GetFloat(uint offset = 0)
        {
            var value = new byte[4];
            Peek(Process, Address + offset, value);
            return BitConverter.ToSingle(value, 0);
        }

        /// <summary>
        /// </summary>
        /// <param name="offset"> </param>
        /// <returns> </returns>
        public short GetInt16(uint offset = 0)
        {
            var value = new byte[2];
            Peek(Process, Address + offset, value);
            return BitConverter.ToInt16(value, 0);
        }

        /// <summary>
        /// </summary>
        /// <param name="offset"> </param>
        /// <returns> </returns>
        public int GetInt32(uint offset = 0)
        {
            var value = new byte[4];
            Peek(Process, Address + offset, value);
            return BitConverter.ToInt32(value, 0);
        }

        /// <summary>
        /// </summary>
        /// <param name="offset"> </param>
        /// <returns> </returns>
        public long GetInt64(uint offset = 0)
        {
            var value = new byte[8];
            Peek(Process, Address + offset, value);
            return BitConverter.ToInt64(value, 0);
        }

        /// <summary>
        /// </summary>
        /// <param name="offset"> </param>
        /// <param name="size"> </param>
        /// <returns> </returns>
        public string GetString(uint offset = 0, int size = 24)
        {
            var bytes = new byte[size];
            Peek(Process, Address + offset, bytes);
            var realSize = 0;
            for (var i = 0; i < size; i++)
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
        /// <param name="offset"> </param>
        /// <returns> </returns>
        public int GetProgram(uint offset = 0)
        {
            var value = new byte[30];
            Peek(Process, Address + offset, value);
            return BitConverter.ToInt32(value, 0);
        }

        /// <summary>
        /// </summary>
        /// <param name="offset"> </param>
        /// <returns> </returns>
        public UInt32 GetUInt32(uint offset = 0)
        {
            var value = new byte[4];
            Peek(Process, Address + offset, value);
            return BitConverter.ToUInt32(value, 0);
        }

        /// <summary>
        /// </summary>
        /// <param name="offset"> </param>
        /// <returns> </returns>
        public UInt16 GetUInt16(uint offset = 0)
        {
            var value = new byte[4];
            Peek(Process, Address + offset, value);
            return BitConverter.ToUInt16(value, 0);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="offset"> </param>
        /// <returns> </returns>
        public T GetStructure<T>(int offset = 0)
        {
            var lpBytesWritten = 0;
            var buffer = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof (T)));
            UnsafeNativeMethods.ReadProcessMemory(Process.Handle, new IntPtr(Address) + offset, buffer, Marshal.SizeOf(typeof (T)), ref lpBytesWritten);
            var retValue = (T) Marshal.PtrToStructure(buffer, typeof (T));
            Marshal.FreeCoTaskMem(buffer);
            return retValue;
        }

        /// <summary>
        /// </summary>
        /// <param name="val"> </param>
        /// <param name="offset"> </param>
        public void Reset(int val, uint offset = 0)
        {
            var data = BitConverter.GetBytes(val);
            Poke(Process, Address + offset, data);
        }

        /// <summary>
        /// </summary>
        /// <param name="val"> </param>
        /// <returns> </returns>
        public bool SetByte(byte val, uint offset = 0)
        {
            var data = BitConverter.GetBytes(val);
            return Poke(Process, Address + offset, data);
        }

        /// <summary>
        /// </summary>
        /// <param name="val"> </param>
        /// <param name="offset"> </param>
        /// <returns> </returns>
        public bool SetByteArray(byte[] val, uint offset = 0)
        {
            return Poke(Process, Address + offset, val);
        }

        /// <summary>
        /// </summary>
        /// <param name="val"> </param>
        /// <param name="offset"> </param>
        /// <returns> </returns>
        public bool SetDouble(double val, uint offset = 0)
        {
            var data = BitConverter.GetBytes(val);
            return Poke(Process, Address + offset, data);
        }

        /// <summary>
        /// </summary>
        /// <param name="val"> </param>
        /// <param name="offset"> </param>
        /// <returns> </returns>
        public bool SetFloat(float val, uint offset = 0)
        {
            var data = BitConverter.GetBytes(val);
            return Poke(Process, Address + offset, data);
        }

        /// <summary>
        /// </summary>
        /// <param name="val"> </param>
        /// <param name="offset"> </param>
        /// <returns> </returns>
        public bool SetInt16(short val, uint offset = 0)
        {
            var data = BitConverter.GetBytes(val);
            return Poke(Process, Address + offset, data);
        }

        /// <summary>
        /// </summary>
        /// <param name="val"> </param>
        /// <param name="offset"> </param>
        /// <returns> </returns>
        public bool SetInt32(int val, uint offset = 0)
        {
            var data = BitConverter.GetBytes(val);
            return Poke(Process, Address + offset, data);
        }

        /// <summary>
        /// </summary>
        /// <param name="val"> </param>
        /// <param name="offset"> </param>
        /// <returns> </returns>
        public bool SetInt64(long val, uint offset = 0)
        {
            var data = BitConverter.GetBytes(val);
            return Poke(Process, Address + offset, data);
        }

        /// <summary>
        /// </summary>
        /// <param name="val"> </param>
        /// <param name="offset"> </param>
        /// <returns> </returns>
        public bool SetUInt16(UInt16 val, uint offset = 0)
        {
            var data = BitConverter.GetBytes(val);
            return Poke(Process, Address + offset, data);
        }

        /// <summary>
        /// </summary>
        /// <param name="val"> </param>
        /// <param name="offset"> </param>
        /// <returns> </returns>
        public bool SetUInt32(UInt32 val, uint offset = 0)
        {
            var data = BitConverter.GetBytes(val);
            return Poke(Process, Address + offset, data);
        }

        /// <summary>
        /// </summary>
        /// <param name="bytes"> </param>
        /// <returns> </returns>
        public static string GetStringFromByteArray(byte[] bytes)
        {
            var u8 = new UTF8Encoding();
            var text = u8.GetString(bytes);
            var startIndex = text.IndexOf(Convert.ToChar(0));
            if ((startIndex != -1))
            {
                text = text.Remove(startIndex, (text.Length - startIndex));
            }
            return text;
        }

        /// <summary>
        /// </summary>
        /// <param name="byteArray"> </param>
        /// <returns> </returns>
        public static char[] ByteArrayToCharArray(byte[] byteArray)
        {
            var charArray = new char[byteArray.Length];
            for (var x = 0; x < byteArray.Length; x++)
            {
                charArray[x] = Convert.ToChar(byteArray[x]);
            }
            return charArray;
        }

        /// <summary>
        /// </summary>
        /// <param name="charArray"> </param>
        /// <returns> </returns>
        public static byte[] CharArrayToByteArray(char[] charArray)
        {
            var byteArray = new byte[charArray.Length];
            for (var x = 0; x < charArray.Length; x++)
            {
                byteArray[x] = Convert.ToByte(charArray[x]);
            }
            return byteArray;
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
