using System;
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace AppModXIV.Memory
{
    public class MemoryHandler
    {
        public struct MemoryBlock
        {
            public long Start;
            public long Length;
        }


        public Process process { get; set; }
        public uint address { get; set; }



        public MemoryHandler(Process proc, uint address)
        {
            this.process = proc;
            this.address = address;
        }



        /// <summary>
        /// Provides access to API.ReadProcessMemory in a way that is digestable to a normal person.
        /// data is a returned by referance. Data is a buffer byte array.
        /// Depends on API.ReadProcessMemory
        /// </summary>
        /// <param name="proc"></param>
        /// <param name="Address"></param>
        /// <param name="Buffer"></param>
        /// <returns>bool</returns>
        public static bool Poke(Process proc, uint target, byte[] data)
        {
            IntPtr byteWritten = new IntPtr(0);
            return UnsafeNativeMethods.WriteProcessMemory(proc.Handle, new IntPtr(target), data, new UIntPtr((UInt32)data.Length), ref byteWritten);
        }

        /// <summary>
        /// Allows access to API.WriteProcessMemory in a digestable manner. Data is passed in as a buffer.
        /// Depends on API.WriteProcessMemory
        /// </summary>
        /// <param name="proc"></param>
        /// <param name="Address"></param>
        /// <param name="Buffer"></param>
        /// <returns>bool</returns>
        public static bool Peek(Process proc, uint Address, byte[] Buffer)
        {
            IntPtr Target = new IntPtr(Address);
            if (proc != null)
            {
                return UnsafeNativeMethods.ReadProcessMemory(proc.Handle, Target, Buffer, Buffer.Length, 0);
            }
            else
            {
                return false;
            }
        }



        /// <summary>
        /// Uses Process.GetProcesses to retrieve a process Array
        /// </summary>
        /// <returns>Process Array</returns>
        public static Process[] GetProcesses()
        {
            Process[] Result = Process.GetProcesses();
            return Result;
        }

        /// <summary>
        /// Uses Process.GetProcessById to return a process by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>Process</returns>
        public static Process GetProcessByID(int ID)
        {
            try
            {
                Process Result = Process.GetProcessById(ID);
                return Result;
            }
            catch { return null; }
        }

        /// <summary>
        /// Uses Process.GetProcessesByName to locate a process by name. 
        /// This only returns the first process found.
        /// </summary>
        /// <param name="Name"></param>
        /// <returns>Process</returns>
        public static Process GetProcessByName(string Name)
        {
            Process[] Processes = Process.GetProcessesByName(Name);
            Process Result = null;
            if (Processes.Length > 0)
            {
                Result = Processes[0];
                return Result;
            }
            else return null;
        }

        /// <summary>
        /// Uses Process.Modules to convert the ProcessModuleCollection to a ProcessModule Array
        /// </summary>
        /// <param name="Proc"></param>
        /// <returns>ProcessModule Array</returns>
        public static ProcessModule[] GetModules(Process Proc)
        {
            try
            {
                ProcessModuleCollection Modules = Proc.Modules;
                ProcessModule[] Result = new ProcessModule[Modules.Count];

                for (int i = 0; i < Modules.Count; i++)
                {
                    Result[i] = Modules[i];
                }

                return Result;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Allows the user to find a ProcessModule in a process. this returns only the first instance.
        /// Depends on getModules
        /// </summary>
        /// <param name="Proc"></param>
        /// <param name="Name"></param>
        /// <returns>ProcessModule</returns>
        public static ProcessModule GetModuleByName(Process Proc, String Name)
        {
            ProcessModule Result = null;
            try
            {
                ProcessModule[] Modules = GetModules(Proc);
                for (int i = 0; i < Modules.Length; i++)
                {
                    ProcessModule Module = Modules[i];
                    if (Module.ModuleName.IndexOf(Name) > -1)
                    {
                        Result = Module;
                        break;
                    }
                }
            }
            catch
            {
                return null;
            }
            return Result;
        }

        /// <summary>
        /// Allows the user to find out what module an address is in.
        /// Depends on getModules
        /// </summary>
        /// <param name="Proc"></param>
        /// <param name="Address">Int32 Address</param>
        /// <returns>ProcessModule</returns>
        public static ProcessModule GetModuleByAddress(Process Proc, Int32 Address)
        {
            try
            {
                ProcessModule[] Modules = GetModules(Proc);
                ProcessModule Result = null;
                for (int i = 0; i < Modules.Length; i++)
                {
                    ProcessModule Module = Modules[i];
                    Int32 BaseAddress = Module.BaseAddress.ToInt32();
                    if ((BaseAddress <= Address) && (BaseAddress + Module.ModuleMemorySize >= Address))
                    {
                        Result = Module;
                        break;
                    }
                }
                return Result;
            }
            catch { return null; }

        }

        /// <summary>
        /// Gets the base address of a module.
        /// </summary>
        /// <param name="Proc"></param>
        /// <param name="File"></param>
        /// <returns></returns>
        private static int GetModuleBaseAddress(Process Proc, string File)
        {
            ProcessModuleCollection ModCol = Proc.Modules;
            foreach (ProcessModule ProcMod in ModCol)
            {
                if (ProcMod.FileName == File)
                {
                    return ProcMod.BaseAddress.ToInt32();
                }
            }
            return -1;
        }

        private static int GetModuleEndAddress(Process Proc, string File)
        {
            ProcessModuleCollection ModCol = Proc.Modules;
            foreach (ProcessModule ProcMod in ModCol)
            {
                if (ProcMod.FileName == File)
                {
                    return ProcMod.BaseAddress.ToInt32() + ProcMod.ModuleMemorySize;
                }
            }
            return -1;
        }

        public static MemoryBlock GetProcessMemoryBlock(Process Proc)
        {
            UnsafeNativeMethods.PROCESS_MEMORY_COUNTERS counter = new UnsafeNativeMethods.PROCESS_MEMORY_COUNTERS();
            UnsafeNativeMethods.GetProcessMemoryInfo(Proc.Handle, out counter, Marshal.SizeOf(counter));
            MemoryBlock block = new MemoryBlock();
            block.Start = Proc.MainModule.BaseAddress.ToInt64();
            block.Length = counter.PagefileUsage;
            return block;
        }

        /// <summary>
        /// Gets the operation code in memory
        /// </summary>
        /// <param name="intLength"></param>
        /// <returns></returns>
        public string GetOperationCode(int intLength)
        {
            byte[] buffer1 = this.GetByteArray(intLength);
            return BitConverter.ToString(buffer1);
        }



        /// <summary>
        /// Gets Byte in Memory
        /// </summary>
        /// <returns></returns>
        public byte GetByte()
        {
            byte[] data = new byte[1];
            Peek(this.process, this.address, data);
            return data[0];
        }

        /// <summary>
        /// Gets Byte in Memory By Offset
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public byte GetByte(uint offset)
        {
            byte[] data = new byte[1];
            Peek(this.process, this.address + offset, data);
            return data[0];
        }

        /// <summary>
        /// Gets a Byte Array in Memory
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public byte[] GetByteArray(int num)
        {
            byte[] data = new byte[num];
            Peek(this.process, this.address, data);
            return data;
        }

        /// <summary>
        /// Gets a Double in Memory
        /// </summary>
        /// <returns></returns>
        public double GetDouble()
        {
            byte[] value = new byte[8];
            Peek(this.process, this.address, value);
            return BitConverter.ToDouble(value, 0);
        }

        /// <summary>
        /// Gets a Float in Memory
        /// </summary>
        /// <returns></returns>
        public float GetFloat()
        {
            byte[] value = new byte[4];
            Peek(this.process, this.address, value);
            return BitConverter.ToSingle(value, 0);
        }

        /// <summary>
        /// Gets a Short Integer (Int16) in Memory
        /// </summary>
        /// <returns></returns>
        public short GetInt16()
        {
            byte[] value = new byte[2];
            Peek(this.process, this.address, value);
            return BitConverter.ToInt16(value, 0);
        }

        /// <summary>
        /// Gets an Integer in Memory
        /// </summary>
        /// <returns></returns>
        public int GetInt32()
        {
            byte[] value = new byte[4];
            Peek(this.process, this.address, value);
            return BitConverter.ToInt32(value, 0);
        }

        /// <summary>
        /// Gets a Long (Int64) in Memory
        /// </summary>
        /// <returns></returns>
        public long GetInt64()
        {
            byte[] value = new byte[8];
            Peek(this.process, this.address, value);
            return BitConverter.ToInt64(value, 0);
        }

        /// <summary>
        /// Gets a String from Memory
        /// </summary>
        /// <returns></returns>
        public string GetString()
        {
            byte[] bytes = new byte[24];
            Peek(this.process, this.address, bytes);
            return Encoding.ASCII.GetString(bytes);
        }

        /// <summary>
        /// Gets a string from Memory based on the max length specified
        /// </summary>
        /// <param name="maxSize">The maximum size for the string</param>
        /// <returns></returns>
        public string GetString(int maxSize)
        {
            byte[] bytes = new byte[maxSize];
            Peek(process, address, bytes);
            int realSize = 0;
            for (int i = 0; i < maxSize; i++)
            {
                if (bytes[i] == 0)
                {
                    realSize = i;
                    break;
                }
            }
            Array.Resize<byte>(ref bytes, realSize);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// Gets a String from Memory
        /// </summary>
        /// <returns></returns>
        public string GetStringBySize(int size)
        {
            byte[] bytes = new byte[size];
            Peek(this.process, this.address, bytes);
            return Encoding.ASCII.GetString(bytes);
        }

        /// <summary>
        /// Gets something from Memory.
        /// </summary>
        /// <returns></returns>
        public int GetProgram()
        {
            byte[] value = new byte[30];
            Peek(this.process, this.address, value);
            return BitConverter.ToInt32(value, 0);
        }



        /// <summary>
        /// Gets an Unsigned Integer (UInt32)
        /// </summary>
        /// <returns></returns>
        public UInt32 GetUInt32()
        {
            byte[] value = new byte[4];
            Peek(this.process, this.address, value);
            return BitConverter.ToUInt32(value, 0);
        }

        /// <summary>
        /// Gets an Unsigned Short (UInt16)
        /// </summary>
        /// <returns></returns>
        public UInt16 GetUInt16()
        {
            byte[] value = new byte[4];
            Peek(this.process, this.address, value);
            return BitConverter.ToUInt16(value, 0);
        }

        public T GetStructure<T>()
        {
            int lpBytesWritten = 0;
            IntPtr buffer = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(T)));
            IntPtr pol = UnsafeNativeMethods.OpenProcess(UnsafeNativeMethods.PROCESS_ALL_ACCESS, false, this.process.Id);
            UnsafeNativeMethods.ReadProcessMemory(this.process.Handle, new IntPtr(this.address), buffer, Marshal.SizeOf(typeof(T)), ref lpBytesWritten);
            T retValue = (T)Marshal.PtrToStructure(buffer, typeof(T));
            Marshal.FreeCoTaskMem(buffer);
            return retValue;
        }


        /// <summary>
        /// Resets somthing.
        /// </summary>
        /// <param name="val"></param>
        public void Reset(int val)
        {
            byte[] data = BitConverter.GetBytes(val);
            Poke(this.process, this.address, data);
        }

        /// <summary>
        /// Sets a Byte in memory
        /// </summary>
        /// <param name="val"></param>
        public void SetByte(byte val)
        {
            Boolean byt;
            byte[] data = BitConverter.GetBytes(val);
            byt = Poke(this.process, this.address, data);
        }

        /// <summary>
        /// Sets a Byte array in Memory
        /// </summary>
        /// <param name="val"></param>
        public void SetByteArray(byte[] val)
        {
            Poke(this.process, this.address, val);
        }

        /// <summary>
        /// Sets a Double in Memory
        /// </summary>
        /// <param name="val"></param>
        public void SetDouble(double val)
        {
            byte[] data = BitConverter.GetBytes(val);
            Poke(this.process, this.address, data);
        }

        /// <summary>
        /// Sets a Float in Memory
        /// </summary>
        /// <param name="val"></param>
        public void SetFloat(float val)
        {
            byte[] data = BitConverter.GetBytes(val);
            Poke(this.process, this.address, data);
        }

        /// <summary>
        /// Set a Short Integer (Int16) in Memory
        /// </summary>
        /// <returns></returns>
        public void SetInt16(short val)
        {
            byte[] data = BitConverter.GetBytes(val);
            Poke(this.process, this.address, data);
        }

        /// <summary>
        /// Sets an Integer in Memory
        /// </summary>
        /// <param name="val"></param>
        public void SetInt32(int val)
        {
            byte[] data = BitConverter.GetBytes(val);
            Poke(this.process, this.address, data);
        }

        /// <summary>
        /// Sets a Long in Memory (Int64)
        /// </summary>
        /// <param name="val"></param>
        public void SetInt64(long val)
        {
            byte[] data = BitConverter.GetBytes(val);
            Poke(this.process, this.address, data);
        }

        /// <summary>
        /// Sets an Unsigned Short in Memory (UInt16)
        /// </summary>
        /// <param name="val"></param>
        public void SetUInt16(UInt16 val)
        {
            byte[] data = BitConverter.GetBytes(val);
            Poke(this.process, this.address, data);
        }

        /// <summary>
        /// Sets an Unsigned Integer in Memory (UInt32)
        /// </summary>
        /// <param name="val"></param>
        public void SetUInt32(UInt32 val)
        {
            byte[] data = BitConverter.GetBytes(val);
            Poke(this.process, this.address, data);
        }

        /// <summary>
        /// Gets a string from a ByteArray
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string GetStringFromByteArray(byte[] bytes)
        {
            UTF8Encoding u8 = new UTF8Encoding();
            string text1 = u8.GetString(bytes);
            int startIndex = text1.IndexOf(Convert.ToChar(0));
            if ((!(startIndex == -1)))
            {
                text1 = text1.Remove(startIndex, (text1.Length - startIndex));
            }
            return text1;
        }

        public static char[] ByteArToCharAr(byte[] byAr)
        {
            char[] chAr = new char[byAr.Length];
            for (int x = 0; x < byAr.Length; x++)
            {
                chAr[x] = Convert.ToChar(byAr[x]);
            }
            return chAr;
        }

        public static byte[] CharArToByteAr(char[] chAr)
        {
            byte[] byAr = new byte[chAr.Length];
            for (int x = 0; x < chAr.Length; x++)
            {
                byAr[x] = Convert.ToByte(chAr[x]);
            }
            return byAr;
        }
    }
}
