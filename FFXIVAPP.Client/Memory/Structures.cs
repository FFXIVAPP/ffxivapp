// FFXIVAPP.Client
// Structures.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.Runtime.InteropServices;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Memory
{
    [DoNotObfuscate]
    public static class Structures
    {
        [StructLayout(LayoutKind.Explicit)]
        internal struct ChatLog
        {
            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0)]
            public uint LineCount;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(32)]
            public uint OffsetArrayStart;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(36)]
            public uint OffsetArrayPos;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(40)]
            public uint OffsetArrayEnd;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(48)]
            public uint LogStart;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(52)]
            public uint LogNext;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(56)]
            public uint LogEnd;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct NPCEntry
        {
            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(116)]
            public uint ID;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(120)]
            public uint NPCID1;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(128)]
            public uint NPCID2;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(138)]
            public byte Type;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(140)]
            public byte CurrentTarget;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(141)]
            public byte Distance;

            [MarshalAs(UnmanagedType.R4)]
            [FieldOffset(160)]
            public float X;

            [MarshalAs(UnmanagedType.R4)]
            [FieldOffset(164)]
            public float Z;

            [MarshalAs(UnmanagedType.R4)]
            [FieldOffset(168)]
            public float Y;

            [MarshalAs(UnmanagedType.R4)]
            [FieldOffset(176)]
            public float Heading;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(228)]
            public uint Fate;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(388)]
            public uint ModelID;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(392)]
            public byte CurrentStatus;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(393)]
            public bool IsGM;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(394)]
            public byte Icon;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(405)]
            public byte Claimed;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(416)]
            public int TargetID;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(5769)]
            public byte Level;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(5770)]
            public byte GrandCompany;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(5771)]
            public byte GrandCompanyRank;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(5774)]
            public byte Title;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(5776)]
            public int HPCurrent;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(5780)]
            public int HPMax;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(5784)]
            public int MPCurrent;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(5788)]
            public int MPMax;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(5792)]
            public short TPCurrent;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(5794)]
            public short GPCurrent;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(5796)]
            public short GPMax;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(5798)]
            public short CPCurrent;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(5800)]
            public short CPMax;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(11704)]
            public byte Race;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(11705)]
            public byte Sex;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            [FieldOffset(12104)]
            public Status[] Statuses;
        }

        [StructLayout(LayoutKind.Explicit, Pack = 1)]
        public struct Status
        {
            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(0)]
            public byte StatusID;

            [MarshalAs(UnmanagedType.R4)]
            [FieldOffset(4)]
            public float Duration;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(8)]
            public uint CasterID;
        };
    }
}
