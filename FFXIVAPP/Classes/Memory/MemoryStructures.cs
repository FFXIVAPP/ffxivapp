// FFXIVAPP
// MemoryStructures.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System.Runtime.InteropServices;

namespace FFXIVAPP.Classes.Memory
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ChatPointers
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Unk1;
        public uint LineCount1;
        public uint LineCount2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
        public byte[] Unk2;
        public uint OffsetArrayStart;
        public uint OffsetArrayPos;
        public uint OffsetArrayEnd;
        public uint Unk3;
        public uint LogStart;
        public uint LogNext;
        public uint LogEnd;
        public uint Unk4;
        public uint pOldChat2Unk1;
        public uint pOldChat2Unk2;
        public uint pOldChat2Unk3;
        //public readonly uint LineCount;
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        //public readonly byte[] Unk1;
        //public readonly uint UnknownPointer1;
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        //public readonly byte[] Unk2;
        //public readonly uint OffsetArrayStart;
        //public readonly uint OffsetArrayPos;
        //public readonly uint OffsetArrayStop;
        //public readonly uint Unk3;
        //public readonly uint LogStart;
        //public readonly uint LogNext;
        //public readonly uint LogEnd;
        //public readonly uint Unk4;
        //public readonly uint UnknownPointer4;
        //public readonly uint UnknownPointer5;
        //public readonly uint UnknownPointer6;
        //public readonly uint UnknownPointer7;
    }
}