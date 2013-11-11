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
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Memory
{
    [DoNotObfuscateType]
    public class MemoryHandler : INotifyPropertyChanged
    {
        #region Property Bindings

        private Process _process;
        private IntPtr _processHandle;

        public Process Process
        {
            get { return _process; }
            set
            {
                _process = value;
                RaisePropertyChanged();
            }
        }

        public IntPtr ProcessHandle
        {
            get { return _processHandle; }
            set
            {
                _processHandle = value;
                RaisePropertyChanged();
            }
        }

        public static MemoryHandler Instance { get; set; }
        public SigScanner SigScanner { get; set; }

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
        public MemoryHandler(Process process)
        {
            Process = process;
            try
            {
                ProcessHandle = UnsafeNativeMethods.OpenProcess(UnsafeNativeMethods.ProcessAccessFlags.All, false, (uint) process.Id);
            }
            catch (Exception ex)
            {
                ProcessHandle = process.Handle;
            }
        }

        ~MemoryHandler()
        {
            UnsafeNativeMethods.CloseHandle(Instance.ProcessHandle);
        }

        /// <summary>
        /// </summary>
        /// <param name="target"> </param>
        /// <param name="data"> </param>
        /// <returns> </returns>
        private static bool Poke(uint target, byte[] data)
        {
            var byteWritten = new IntPtr(0);
            return UnsafeNativeMethods.WriteProcessMemory(Instance.ProcessHandle, new IntPtr(target), data, new UIntPtr((UInt32) data.Length), ref byteWritten);
        }

        /// <summary>
        /// </summary>
        /// <param name="address"> </param>
        /// <param name="buffer"> </param>
        /// <returns> </returns>
        private static bool Peek(uint address, byte[] buffer)
        {
            var target = new IntPtr(address);
            int lpNumberOfBytesRead;
            return UnsafeNativeMethods.ReadProcessMemory(Instance.ProcessHandle, target, buffer, buffer.Length, out lpNumberOfBytesRead);
        }

        /// <summary>
        /// </summary>
        /// <param name="address"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public byte GetByte(uint address, uint offset = 0)
        {
            var data = new byte[1];
            Peek(address + offset, data);
            return data[0];
        }

        /// <summary>
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public byte[] GetByteArray(uint address, int length)
        {
            var data = new byte[length];
            Peek(address, data);
            return data;
        }

        /// <summary>
        /// </summary>
        /// <param name="address"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public double GetDouble(uint address, uint offset = 0)
        {
            var value = new byte[8];
            Peek(address + offset, value);
            return BitConverter.ToDouble(value, 0);
        }

        /// <summary>
        /// </summary>
        /// <param name="address"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public float GetFloat(uint address, uint offset = 0)
        {
            var value = new byte[4];
            Peek(address + offset, value);
            return BitConverter.ToSingle(value, 0);
        }

        /// <summary>
        /// </summary>
        /// <param name="address"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public short GetInt16(uint address, uint offset = 0)
        {
            var value = new byte[2];
            Peek(address + offset, value);
            return BitConverter.ToInt16(value, 0);
        }

        /// <summary>
        /// </summary>
        /// <param name="address"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public int GetInt32(uint address, uint offset = 0)
        {
            var value = new byte[4];
            Peek(address + offset, value);
            return BitConverter.ToInt32(value, 0);
        }

        /// <summary>
        /// </summary>
        /// <param name="address"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public long GetInt64(uint address, uint offset = 0)
        {
            var value = new byte[8];
            Peek(address + offset, value);
            return BitConverter.ToInt64(value, 0);
        }

        /// <summary>
        /// </summary>
        /// <param name="address"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public string GetString(uint address, uint offset = 0, int size = 256)
        {
            var bytes = new byte[size];
            Peek(address + offset, bytes);
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
        /// <param name="address"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public int GetProgram(uint address, uint offset = 0)
        {
            var value = new byte[30];
            Peek(address + offset, value);
            return BitConverter.ToInt32(value, 0);
        }

        /// <summary>
        /// </summary>
        /// <param name="address"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public UInt32 GetUInt32(uint address, uint offset = 0)
        {
            var value = new byte[4];
            Peek(address + offset, value);
            return BitConverter.ToUInt32(value, 0);
        }

        /// <summary>
        /// </summary>
        /// <param name="address"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public UInt16 GetUInt16(uint address, uint offset = 0)
        {
            var value = new byte[4];
            Peek(address + offset, value);
            return BitConverter.ToUInt16(value, 0);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="address"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public T GetStructure<T>(uint address, int offset = 0)
        {
            int lpNumberOfBytesRead;
            var buffer = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof (T)));
            UnsafeNativeMethods.ReadProcessMemory(Process.Handle, new IntPtr(address) + offset, buffer, Marshal.SizeOf(typeof(T)), out lpNumberOfBytesRead);
            var retValue = (T) Marshal.PtrToStructure(buffer, typeof (T));
            Marshal.FreeCoTaskMem(buffer);
            return retValue;
        }

        /// <summary>
        /// </summary>
        /// <param name="address"></param>
        /// <param name="val"></param>
        /// <param name="offset"></param>
        public void Reset(uint address, int val, uint offset = 0)
        {
            var data = BitConverter.GetBytes(val);
            Poke(address + offset, data);
        }

        /// <summary>
        /// </summary>
        /// <param name="address"></param>
        /// <param name="val"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public bool SetByte(uint address, byte val, uint offset = 0)
        {
            var data = BitConverter.GetBytes(val);
            return Poke(address + offset, data);
        }

        /// <summary>
        /// </summary>
        /// <param name="address"></param>
        /// <param name="val"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public bool SetByteArray(uint address, byte[] val, uint offset = 0)
        {
            return Poke(address + offset, val);
        }

        /// <summary>
        /// </summary>
        /// <param name="address"></param>
        /// <param name="val"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public bool SetDouble(uint address, double val, uint offset = 0)
        {
            var data = BitConverter.GetBytes(val);
            return Poke(address + offset, data);
        }

        /// <summary>
        /// </summary>
        /// <param name="address"></param>
        /// <param name="val"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public bool SetFloat(uint address, float val, uint offset = 0)
        {
            var data = BitConverter.GetBytes(val);
            return Poke(address + offset, data);
        }

        /// <summary>
        /// </summary>
        /// <param name="address"></param>
        /// <param name="val"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public bool SetInt16(uint address, short val, uint offset = 0)
        {
            var data = BitConverter.GetBytes(val);
            return Poke(address + offset, data);
        }

        /// <summary>
        /// </summary>
        /// <param name="address"></param>
        /// <param name="val"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public bool SetInt32(uint address, int val, uint offset = 0)
        {
            var data = BitConverter.GetBytes(val);
            return Poke(address + offset, data);
        }

        /// <summary>
        /// </summary>
        /// <param name="address"></param>
        /// <param name="val"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public bool SetInt64(uint address, long val, uint offset = 0)
        {
            var data = BitConverter.GetBytes(val);
            return Poke(address + offset, data);
        }

        /// <summary>
        /// </summary>
        /// <param name="address"></param>
        /// <param name="val"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public bool SetUInt16(uint address, UInt16 val, uint offset = 0)
        {
            var data = BitConverter.GetBytes(val);
            return Poke(address + offset, data);
        }

        /// <summary>
        /// </summary>
        /// <param name="address"></param>
        /// <param name="val"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public bool SetUInt32(uint address, UInt32 val, uint offset = 0)
        {
            var data = BitConverter.GetBytes(val);
            return Poke(address + offset, data);
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
