    // FFXIVAPP.Client
// Structures.cs
// 
// © 2013 Ryan Wilson

using System.Runtime.InteropServices;
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
            [FieldOffset(5785)]
            public byte Level;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(5786)]
            public byte GrandCompany;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(5787)]
            public byte GrandCompanyRank;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(5790)]
            public byte Title;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(5792)]
            public int HPCurrent;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(5796)]
            public int HPMax;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(5800)]
            public int MPCurrent;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(5804)]
            public int MPMax;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(5808)]
            public short TPCurrent;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(5810)]
            public short GPCurrent;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(5812)]
            public short GPMax;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(5814)]
            public short CPCurrent;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(5816)]
            public short CPMax;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(11704)]
            public byte Race;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(11705)]
            public byte Sex;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
            [FieldOffset(12112)]
            public Status[] Statuses;
        }

        [StructLayout(LayoutKind.Explicit, Pack = 1)]
        public struct PlayerInfo
        {
            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(100)]
            public byte JobID;

            #region Job Levels

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(102)]
            public byte PGL;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(104)]
            public byte GLD;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(106)]
            public byte MRD;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(108)]
            public byte ARC;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(110)]
            public byte LNC;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(112)]
            public byte THM;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(114)]
            public byte CNJ;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(116)]
            public byte CPT;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(118)]
            public byte BSM;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(120)]
            public byte ARM;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(122)]
            public byte GSM;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(124)]
            public byte LTW;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(126)]
            public byte WVR;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(128)]
            public byte ALC;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(130)]
            public byte CUL;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(132)]
            public byte MIN;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(134)]
            public byte BOT;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(136)]
            public byte FSH;

            [MarshalAs(UnmanagedType.I1)]
            [FieldOffset(138)]
            public byte ACN;

            #endregion

            #region Job Exp In Level

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(140)]
            public int PGL_CurrentEXP;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(144)]
            public int GLD_CurrentEXP;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(148)]
            public int MRD_CurrentEXP;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(152)]
            public int ARC_CurrentEXP;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(156)]
            public int LNC_CurrentEXP;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(160)]
            public int THM_CurrentEXP;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(164)]
            public int CNJ_CurrentEXP;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(168)]
            public int ACN_CurrentEXP;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(172)]
            public int BSM_CurrentEXP;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(176)]
            public int CPT_CurrentEXP;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(180)]
            public int GSM_CurrentEXP;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(184)]
            public int ARM_CurrentEXP;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(188)]
            public int WVR_CurrentEXP;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(192)]
            public int LTW_CurrentEXP;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(196)]
            public int CUL_CurrentEXP;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(200)]
            public int MIN_CurrentEXP;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(204)]
            public int BOT_CurrentEXP;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(208)]
            public int FSH_CurrentEXP;

            #endregion

            #region Base Stats

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(228)]
            public short BaseStrength;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(232)]
            public short BaseDexterity;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(236)]
            public short BaseVitality;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(240)]
            public short BaseIntelligence;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(244)]
            public short BaseMind;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(248)]
            public short BasePiety;

            #endregion

            #region Stats (base+gear+bonus)

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(256)]
            public short Strength;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(260)]
            public short Dexterity;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(264)]
            public short Vitality;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(268)]
            public short Intelligence;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(272)]
            public short Mind;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(276)]
            public short Piety;

            #endregion

            #region Basic infos

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(280)]
            public int HPMax;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(284)]
            public int MPMax;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(288)]
            public int TPMax;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(292)]
            public int GPMax;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(296)]
            public int CPMax;

            #endregion

            #region Defensive stats

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(328)]
            public short Parry;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(330)]
            public short Defense;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(344)]
            public short Evasion;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(348)]
            public short MagicDefense;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(368)]
            public short SlashingResistance;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(372)]
            public short PiercingResistance;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(376)]
            public short BluntResistance;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(400)]
            public short FireResistance;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(404)]
            public short IceResistance;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(408)]
            public short WindResistance;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(412)]
            public short EarthResistance;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(416)]
            public short LightningResistance;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(420)]
            public short WaterResistance;

            #endregion

            #region Offensive stats

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(332)]
            public short AttackPower;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(340)]
            public short Accuracy;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(360)]
            public short CriticalHitRate;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(384)]
            public short AttackMagicPotency;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(388)]
            public short HealingMagicPotency;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(428)]
            public short Determination;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(432)]
            public short SkillSpeed;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(436)]
            public short SpellSpeed;

            #endregion

            #region DoH/DoL stats

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(532)]
            public short Craftmanship;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(536)]
            public short Control;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(540)]
            public short Gathering;

            [MarshalAs(UnmanagedType.I2)]
            [FieldOffset(544)]
            public short Perception;

            #endregion
        };

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

        [StructLayout(LayoutKind.Explicit, Pack = 1)]
        public struct Target
        {
            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(0)]
            public uint CurrentTarget;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(24)]
            public uint MouseOverTarget;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(64)]
            public uint FocusTarget;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(76)]
            public uint PreviousTarget;

            [MarshalAs(UnmanagedType.I4)]
            [FieldOffset(96)]
            public uint CurrentTargetID;
        }
    }
}
