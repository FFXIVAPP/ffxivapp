// Project: AppModXIV
// File: MemoryStructures.cs
// 
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System.Runtime.InteropServices;

namespace AppModXIV.Memory
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ChatPointers
    {
        public readonly uint LineCount;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        private readonly byte[] Unk1;

        private readonly uint UnknownPointer1;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        private readonly byte[] unk2;

        public readonly uint OffsetArrayStart;
        public readonly uint OffsetArrayStop;
        private readonly uint UnknownPointer2;
        private readonly uint Padding1;
        public readonly uint LogStart;
        public readonly uint LogNextEntry;
        private readonly uint UnknownPointer3;
        private readonly uint Padding2;
        private readonly uint UnknownPointer4;
        private readonly uint UnknownPointer5;
        private readonly uint UnknownPointer6;
        private readonly uint UnknownPointer7;
    }
}