// FFXIVAPP.Client
// Structures.cs
// 
// © 2013 Ryan Wilson

using System.Runtime.InteropServices;
using FFXIVAPP.Common.Core.Memory.Enums;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Memory
{
    [DoNotObfuscate]
    public static class Structures
    {
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
        public struct NPCEntry
        {
            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0x0)] //0
            public uint GatheringType;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0x74)] //116
            public uint ID;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0x78)] //120
            public uint NPCID1;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0x80)] //128
            public uint NPCID2;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(0x8A)] //138
            public Actor.Type Type;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(0x8C)] //140
            public Actor.TargetType TargetType;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(0x8D)] //141
            public byte Distance;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(0x8E)] //142
            public byte GatheringStatus;

            [MarshalAs(UnmanagedType.R4)]
            [FieldOffset(0xA0)] //160
            public float X;

            [MarshalAs(UnmanagedType.R4)]
            [FieldOffset(0xA4)] //164
            public float Z;

            [MarshalAs(UnmanagedType.R4)]
            [FieldOffset(0xA8)] //168
            public float Y;

            [MarshalAs(UnmanagedType.R4)]
            [FieldOffset(0xB0)] //176
            public float Heading;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0xE4)] //228
            public uint Fate;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(0x11C)] //284
            public byte GatheringInvisible;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0x184)] //388
            public uint ModelID;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(0x188)] //392
            public Actor.ActionStatus ActionStatus;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(0x193)] //393
            public bool IsGM;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(0x194)] //394
            public Actor.Icon Icon;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(0x196)] //405
            public Actor.Status Status;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0xA78)] //416
            public int TargetID;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(0x1698)] //5784
            public Actor.Job Job;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(0x1699)] //5785
            public byte Level;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(0x169A)] //5786
            public byte GrandCompany;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(0x169B)] //5787
            public byte GrandCompanyRank;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(0x169E)] //5790
            public byte Title;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0x16A0)] //5792
            public int HPCurrent;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0x16A4)] //5796
            public int HPMax;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0x16A8)] //5800
            public int MPCurrent;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0x16AC)] //5804
            public int MPMax;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0x16B0)] //5808
            public short TPCurrent;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0x16B2)] //5810
            public short GPCurrent;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0x16B4)] //5812
            public short GPMax;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0x16B6)] //5814
            public short CPCurrent;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0x16B8)] //5816
            public short CPMax;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(0x2E58)] //11864
            public byte Race;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(0x2E59)] //11865
            public Actor.Sex Sex;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(0x2E72)] //11890
            public byte Agro;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
            [FieldOffset(0x2FF8)] //12280
            public Status[] Statuses;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(0x3170)]
            public bool IsCasting;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0x3174)]
            public short CastingID;

            [MarshalAs(UnmanagedType.R4)]
            [FieldOffset(0x31A4)]
            public float CastingProgress;

            [MarshalAs(UnmanagedType.R4)]
            [FieldOffset(0x31A8)]
            public float CastingTime;
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
        public struct PlayerInfo
        {
            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(0x64)] //100
            public byte JobID;

            #region Job Levels

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(0x66)] //102
            public byte PGL;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(0x68)] //104
            public byte GLD;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(0x6A)] //106
            public byte MRD;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(0x6C)] //108
            public byte ARC;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(0x6E)] //110
            public byte LNC;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(0x70)] //112
            public byte THM;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(0x72)] //114
            public byte CNJ;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(0x74)] //116
            public byte CPT;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(0x76)] //118
            public byte BSM;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(0x78)] //120
            public byte ARM;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(0x7A)] //122
            public byte GSM;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(0x7C)] //124
            public byte LTW;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(0x7E)] //126
            public byte WVR;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(0x80)] //128
            public byte ALC;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(0x82)] //130
            public byte CUL;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(0x84)] //132
            public byte MIN;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(0x86)] //134
            public byte BOT;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(0x88)] //136
            public byte FSH;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(0x8A)] //138
            public byte ACN;

            #endregion

            #region Job Exp In Level

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0x8C)] //140
            public int PGL_CurrentEXP;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0x90)] //144
            public int GLD_CurrentEXP;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0x94)] //148
            public int MRD_CurrentEXP;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0x98)] //152
            public int ARC_CurrentEXP;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0x9C)] //156
            public int LNC_CurrentEXP;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0xA0)] //160
            public int THM_CurrentEXP;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0xA4)] //164
            public int CNJ_CurrentEXP;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0xA8)] //168
            public int ACN_CurrentEXP;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0xAC)] //172
            public int BSM_CurrentEXP;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0xB0)] //176
            public int CPT_CurrentEXP;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0xB4)] //180
            public int GSM_CurrentEXP;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0xB8)] //184
            public int ARM_CurrentEXP;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0xBC)] //188
            public int WVR_CurrentEXP;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0xC0)] //192
            public int LTW_CurrentEXP;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0xC4)] //196
            public int CUL_CurrentEXP;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0xC8)] //200
            public int MIN_CurrentEXP;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0xCC)] //204
            public int BOT_CurrentEXP;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0xD0)] //208
            public int FSH_CurrentEXP;

            #endregion

            #region Base Stats

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0xE4)] //228
            public short BaseStrength;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0xE8)] //232
            public short BaseDexterity;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0xEC)] //236
            public short BaseVitality;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0xF0)] //240
            public short BaseIntelligence;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0xF4)] //244
            public short BaseMind;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0xF8)] //248
            public short BasePiety;

            #endregion

            #region Stats (base+gear+bonus)

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0x100)] //256
            public short Strength;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0x104)] //260
            public short Dexterity;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0x108)] //264
            public short Vitality;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0x10C)] //268
            public short Intelligence;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0x110)] //272
            public short Mind;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0x114)] //276
            public short Piety;

            #endregion

            #region Basic infos

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0x118)] //280
            public int HPMax;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0x11C)] //284
            public int MPMax;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0x120)] //288
            public int TPMax;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0x124)] //292
            public int GPMax;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0x128)] //296
            public int CPMax;

            #endregion

            #region Defensive stats

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0x148)] //328
            public short Parry;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0x14A)] //330
            public short Defense;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0x158)] //344
            public short Evasion;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0x15C)] //348
            public short MagicDefense;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0x170)] //368
            public short SlashingResistance;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0x174)] //372
            public short PiercingResistance;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0x178)] //376
            public short BluntResistance;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0x190)] //400
            public short FireResistance;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0x194)] //404
            public short IceResistance;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0x198)] //408
            public short WindResistance;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0x19C)] //412
            public short EarthResistance;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0x1A0)] //416
            public short LightningResistance;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0x1A4)] //420
            public short WaterResistance;

            #endregion

            #region Offensive stats

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0x14C)] //332
            public short AttackPower;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0x154)] //340
            public short Accuracy;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0x168)] //360
            public short CriticalHitRate;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0x180)] //384
            public short AttackMagicPotency;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0x184)] //388
            public short HealingMagicPotency;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0x1AC)] //428
            public short Determination;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0x1B0)] //432
            public short SkillSpeed;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0x1B4)] //436
            public short SpellSpeed;

            #endregion

            #region DoH/DoL stats

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0x214)] //532
            public short Craftmanship;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0x218)] //536
            public short Control;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0x21C)] //540
            public short Gathering;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(0x220)] //544
            public short Perception;

            #endregion
        };

        [StructLayout(LayoutKind.Explicit, Pack = 1)]
        public struct Status
        {
            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(0x0)] //0
            public byte StatusID;

            [MarshalAs(UnmanagedType.R4)]
            [FieldOffset(0x4)] //4
            public float Duration;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0x8)] //8
            public uint CasterID;
        };

        [StructLayout(LayoutKind.Explicit, Pack = 1)]
        public struct Target
        {
            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0x0)] //0
            public uint CurrentTarget;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0x18)] //24
            public uint MouseOverTarget;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0x40)] //64
            public uint FocusTarget;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0x4C)] //76
            public uint PreviousTarget;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0x60)] //96
            public uint CurrentTargetID;
        }
    }
}
