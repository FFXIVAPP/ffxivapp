// FFXIVAPP.Client
// MemoryHandler.cs
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
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using NLog;

namespace FFXIVAPP.Client.Memory
{
    public class MemoryHandler : INotifyPropertyChanged
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Property Bindings

        private static MemoryHandler _instance;
        private Process _process;
        private IntPtr _processHandle;
        private SigScanner _sigScanner;

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

        public static MemoryHandler Instance
        {
            get { return _instance ?? (_instance = new MemoryHandler(null)); }
            set { _instance = value; }
        }

        public SigScanner SigScanner
        {
            get { return _sigScanner ?? (_sigScanner = new SigScanner()); }
            set
            {
                if (_sigScanner == null)
                {
                    _sigScanner = new SigScanner();
                }
                _sigScanner = value;
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
        public MemoryHandler(Process process)
        {
            if (process != null)
            {
                SetProcess(process);
            }
        }

        ~MemoryHandler()
        {
            try
            {
                UnsafeNativeMethods.CloseHandle(Instance.ProcessHandle);
            }
            catch (Exception ex)
            {
            }
        }

        public void SetProcess(Process process)
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
            Constants.ProcessHandle = ProcessHandle;
            SigScanner.Locations.Clear();
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

        public string GetStringFromBytes(byte[] source, int offset = 0, int size = 256)
        {
            var bytes = new byte[size];
            Array.Copy(source, offset, bytes, 0, size);
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

        public int ReadInt32(IntPtr address, int offset = 0)
        {
            var value = new byte[4];
            Peek((uint) (address + offset), value);
            return BitConverter.ToInt32(value, 0);
        }

        public IntPtr ReadPointer(IntPtr address, int offset = 0)
        {
            var value = new byte[4];
            Peek((uint) (address + offset), value);
            return IntPtr.Add(IntPtr.Zero, BitConverter.ToInt32(value, 0));
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
            UnsafeNativeMethods.ReadProcessMemory(Process.Handle, new IntPtr(address) + offset, buffer, Marshal.SizeOf(typeof (T)), out lpNumberOfBytesRead);
            var retValue = (T) Marshal.PtrToStructure(buffer, typeof (T));
            Marshal.FreeCoTaskMem(buffer);
            return retValue;
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public T GetStructureFromBytes<T>(byte[] source) where T : struct
        {
            unsafe
            {
                fixed (byte* p = &source[0])
                {
                    return (T) Marshal.PtrToStructure(new IntPtr(p), typeof (T));
                }
            }
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
