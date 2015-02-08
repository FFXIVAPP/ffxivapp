// FFXIVAPP.Client
// Structures.cs
// 
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
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

using System.Runtime.InteropServices;
using FFXIVAPP.Common.Core.Memory.Enums;

namespace FFXIVAPP.Client.Memory
{
    public static class Structures
    {
        [StructLayout(LayoutKind.Explicit)]
        internal struct CHARMAP
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1000)]
            [FieldOffset(0x0)] //0
            public Location[] Locations;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct ChatLog
        {
            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0x0)] //0
            public uint LineCount;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0x20)] //32
            public uint OffsetArrayStart;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0x24)] //36
            public uint OffsetArrayPos;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0x28)] //40
            public uint OffsetArrayEnd;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0x30)] //48
            public uint LogStart;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0x34)] //52
            public uint LogNext;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0x38)] //56
            public uint LogEnd;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct Location
        {
            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0x0)] //0
            public uint BaseAddress;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct NPCMAP
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            [FieldOffset(0x0)] //0
            public Location[] Locations;
        }

        [StructLayout(LayoutKind.Explicit, Pack = 1)]
        public struct PartyMember
        {
            [MarshalAs(UnmanagedType.R4)]
            [FieldOffset(0x0)]
            public float X;

            [MarshalAs(UnmanagedType.R4)]
            [FieldOffset(0x4)]
            public float Z;

            [MarshalAs(UnmanagedType.R4)]
            [FieldOffset(0x8)]
            public float Y;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0x10)]
            public uint ID;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(0x61)]
            public Actor.Job Job;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(0x63)]
            public byte Level;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0x68)]
            public int HPCurrent;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0x6C)]
            public int HPMax;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0x70)]
            public short MPCurrent;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0x72)]
            public short MPMax;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            [FieldOffset(0x80)]
            public Status[] Statuses;
        };

        [StructLayout(LayoutKind.Explicit, Pack = 1)]
        public struct Status
        {
            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0x0)] //0
            public short StatusID;

            [MarshalAs(UnmanagedType.R4)]
            [FieldOffset(0x4)] //4
            public float Duration;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0x8)] //8
            public uint CasterID;
        };
    }
}
