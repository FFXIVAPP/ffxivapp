using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace AppModXIV.Memory
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ChatPointers
    {
        public uint LineCount;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] Unk1;
        public uint UnknownPointer1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] unk2;
        public uint OffsetArrayStart;
        public uint OffsetArrayStop;
        public uint UnknownPointer2;
        public uint Padding1;
        public uint LogStart;
        public uint LogNextEntry;
        public uint UnknownPointer3;
        public uint Padding2;
        public uint UnknownPointer4;
        public uint UnknownPointer5;
        public uint UnknownPointer6;
        public uint UnknownPointer7;
    }
}
