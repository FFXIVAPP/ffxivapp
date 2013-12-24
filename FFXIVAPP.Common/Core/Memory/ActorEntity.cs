// FFXIVAPP.Common
// ActorEntity.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using FFXIVAPP.Common.Helpers;

namespace FFXIVAPP.Common.Core.Memory
{
    public class ActorEntity : IActorEntity
    {
        private string _name;
        private List<StatusEntry> _statusEntries;

        public Enums.Actor.Type ActorType
        {
            get
            {
                switch (Type)
                {
                    case 0x1:
                        return Enums.Actor.Type.PC;
                    case 0x2:
                        return Enums.Actor.Type.Monster;
                    case 0x3:
                        return Enums.Actor.Type.NPC;
                    case 0x5:
                        return Enums.Actor.Type.Aetheryte;
                    case 0x6:
                        return Enums.Actor.Type.Gathering;
                    case 0x9:
                        return Enums.Actor.Type.Minion;
                    default:
                        return Enums.Actor.Type.Unknown;
                }
            }
        }

        public Enums.Actor.TargetType ActorTargetType
        {
            get
            {
                switch (TargetType)
                {
                    case 0x1:
                        return Enums.Actor.TargetType.Own;
                    case 0x2:
                        return Enums.Actor.TargetType.True;
                    case 0x4:
                        return Enums.Actor.TargetType.False;
                    default:
                        return Enums.Actor.TargetType.Unknown;
                }
            }
        }

        public Enums.Actor.Status ActorStatus
        {
            get
            {
                switch (Status)
                {
                    case 0x1:
                        return Enums.Actor.Status.Idle;
                    case 0x2:
                        return Enums.Actor.Status.Dead;
                    case 0x3:
                        return Enums.Actor.Status.Sitting;
                    case 0x4:
                        return Enums.Actor.Status.Mounted;
                    case 0x5:
                        return Enums.Actor.Status.Crafting;
                    case 0x6:
                        return Enums.Actor.Status.Gathering;
                    case 0x7:
                        return Enums.Actor.Status.Melding;
                    case 0x8:
                        return Enums.Actor.Status.SMachine;
                    default:
                        return Enums.Actor.Status.Unknown;
                }
            }
        }

        public Enums.Actor.Icon ActorIcon
        {
            get
            {
                switch (Icon)
                {
                    case 0x0:
                        return Enums.Actor.Icon.None;
                    case 0x1:
                        return Enums.Actor.Icon.Yoshida;
                    case 0x2:
                        return Enums.Actor.Icon.GM;
                    case 0x3:
                        return Enums.Actor.Icon.SGM;
                    case 0x4:
                        return Enums.Actor.Icon.Clover;
                    case 0x5:
                        return Enums.Actor.Icon.DC;
                    case 0x6:
                        return Enums.Actor.Icon.Smiley;
                    case 0x9:
                        return Enums.Actor.Icon.RedCross;
                    case 0xA:
                        return Enums.Actor.Icon.GreyDC;
                    case 0xB:
                        return Enums.Actor.Icon.Processing;
                    case 0xC:
                        return Enums.Actor.Icon.Busy;
                    case 0xD:
                        return Enums.Actor.Icon.Duty;
                    case 0xE:
                        return Enums.Actor.Icon.ProcessingYellow;
                    case 0xF:
                        return Enums.Actor.Icon.ProcessingGrey;
                    case 0x10:
                        return Enums.Actor.Icon.Cutscene;
                    case 0x12:
                        return Enums.Actor.Icon.Away;
                    case 0x13:
                        return Enums.Actor.Icon.Sitting;
                    case 0x14:
                        return Enums.Actor.Icon.WrenchYellow;
                    case 0x15:
                        return Enums.Actor.Icon.Wrench;
                    case 0x16:
                        return Enums.Actor.Icon.Dice;
                    case 0x17:
                        return Enums.Actor.Icon.ProcessingGreen;
                    case 0x18:
                        return Enums.Actor.Icon.DutyFinder;
                    case 0x19:
                        return Enums.Actor.Icon.Recruiting;
                    case 0x1A:
                        return Enums.Actor.Icon.AllianceLeader;
                    case 0x1B:
                        return Enums.Actor.Icon.AllianceBlueLeader;
                    case 0x1C:
                        return Enums.Actor.Icon.AllianceBlue;
                    case 0x1D:
                        return Enums.Actor.Icon.PartyLeader;
                    case 0x1E:
                        return Enums.Actor.Icon.PartyMember;
                    case 0x1F:
                        return Enums.Actor.Icon.Sprout;
                    case 0x20:
                        return Enums.Actor.Icon.Gil;
                    default:
                        return Enums.Actor.Icon.Unknown;
                }
            }
        }

        public Enums.Actor.Claimed ActorClaimed
        {
            get
            {
                switch (Claimed)
                {
                    case 0x1:
                        return Enums.Actor.Claimed.Claimed;
                    case 0x2:
                        return Enums.Actor.Claimed.Idle;
                    case 0x5:
                        return Enums.Actor.Claimed.Crafting;
                    default:
                        return Enums.Actor.Claimed.Unknown;
                }
            }
        }

        public Enums.Actor.Job ActorJob
        {
            get
            {
                switch (Job)
                {
                    case 0x1:
                        return Enums.Actor.Job.GLD;
                    case 0x2:
                        return Enums.Actor.Job.PGL;
                    case 0x3:
                        return Enums.Actor.Job.MRD;
                    case 0x4:
                        return Enums.Actor.Job.LNC;
                    case 0x5:
                        return Enums.Actor.Job.ARC;
                    case 0x6:
                        return Enums.Actor.Job.CNJ;
                    case 0x7:
                        return Enums.Actor.Job.THM;
                    case 0x8:
                        return Enums.Actor.Job.CPT;
                    case 0x9:
                        return Enums.Actor.Job.BSM;
                    case 0xA:
                        return Enums.Actor.Job.ARC;
                    case 0xB:
                        return Enums.Actor.Job.GSM;
                    case 0xC:
                        return Enums.Actor.Job.LTW;
                    case 0xD:
                        return Enums.Actor.Job.WVR;
                    case 0xE:
                        return Enums.Actor.Job.ALC;
                    case 0xF:
                        return Enums.Actor.Job.CUL;
                    case 0x10:
                        return Enums.Actor.Job.MIN;
                    case 0x11:
                        return Enums.Actor.Job.BOT;
                    case 0x12:
                        return Enums.Actor.Job.FSH;
                    case 0x13:
                        return Enums.Actor.Job.PLD;
                    case 0x14:
                        return Enums.Actor.Job.MNK;
                    case 0x15:
                        return Enums.Actor.Job.WAR;
                    case 0x16:
                        return Enums.Actor.Job.DRG;
                    case 0x17:
                        return Enums.Actor.Job.BRD;
                    case 0x18:
                        return Enums.Actor.Job.WHM;
                    case 0x19:
                        return Enums.Actor.Job.BLM;
                    case 0x1A:
                        return Enums.Actor.Job.ACN;
                    case 0x1B:
                        return Enums.Actor.Job.SMN;
                    case 0x1C:
                        return Enums.Actor.Job.SCH;
                    case 0x1D:
                        return Enums.Actor.Job.Chocobo;
                    case 0x1E:
                        return Enums.Actor.Job.Pet;
                    default:
                        return Enums.Actor.Job.Unknown;
                }
            }
        }

        public decimal HPPercent
        {
            get
            {
                try
                {
                    return (decimal) (Convert.ToDouble(HPCurrent) / Convert.ToDouble(HPMax));
                }
                catch
                {
                    return 0;
                }
            }
        }

        public string HPString
        {
            get { return String.Format("{0}/{1} [{2:P2}]", HPCurrent, HPMax, HPPercent); }
        }

        public decimal MPPercent
        {
            get
            {
                try
                {
                    return (decimal) (Convert.ToDouble(MPCurrent) / Convert.ToDouble(MPMax));
                }
                catch
                {
                    return 0;
                }
            }
        }

        public string MPString
        {
            get { return String.Format("{0}/{1} [{2:P2}]", MPCurrent, MPMax, MPPercent); }
        }

        public int TPMax { get; set; }

        public decimal TPPercent
        {
            get
            {
                try
                {
                    return (decimal) (Convert.ToDouble(TPCurrent) / Convert.ToDouble(TPMax));
                }
                catch
                {
                    return 0;
                }
            }
        }

        public string TPString
        {
            get { return String.Format("{0}/{1} [{2:P2}]", TPCurrent, TPMax, TPPercent); }
        }

        public decimal GPPercent
        {
            get
            {
                try
                {
                    return (decimal) (Convert.ToDouble(GPCurrent) / Convert.ToDouble(GPMax));
                }
                catch
                {
                    return 0;
                }
            }
        }

        public string GPString
        {
            get { return String.Format("{0}/{1} [{2:P2}]", GPCurrent, GPMax, GPPercent); }
        }

        public decimal CPPercent
        {
            get
            {
                try
                {
                    return (decimal) Math.Ceiling((Convert.ToDouble(CPCurrent) / Convert.ToDouble(CPMax)));
                }
                catch
                {
                    return 0;
                }
            }
        }

        public string CPString
        {
            get { return String.Format("{0}/{1} [{2:P2}]", CPCurrent, CPMax, CPPercent); }
        }

        public Enums.Actor.Sex ActorSex
        {
            get
            {
                switch (Sex)
                {
                    case 0x0:
                        return Enums.Actor.Sex.Male;
                    case 0x1:
                        return Enums.Actor.Sex.Female;
                    default:
                        return Enums.Actor.Sex.Unknown;
                }
            }
        }

        public bool IsFate
        {
            get { return Fate == 0x801AFFFF && ActorType == Enums.Actor.Type.Monster; }
        }

        public bool IsClaimed
        {
            get { return Claimed == 1; }
        }

        public bool IsValid
        {
            get
            {
                switch (ActorType)
                {
                    case Enums.Actor.Type.NPC:
                        return !String.IsNullOrEmpty(Name) && ID != 0 && (NPCID1 != 0 || NPCID2 != 0);
                    default:
                        return !String.IsNullOrEmpty(Name) && ID != 0;
                }
            }
        }

        public Coordinate Coordinate { get; set; }
        public byte GatheringStatus { get; set; }
        public uint MapIndex { get; set; }

        public string Name
        {
            get { return _name ?? ""; }
            set { _name = StringHelper.TitleCase(value); }
        }

        public uint ID { get; set; }
        public uint NPCID1 { get; set; }
        public uint NPCID2 { get; set; }
        public byte Type { get; set; }
        public byte TargetType { get; set; }
        public byte Distance { get; set; }
        public double X { get; set; }
        public double Z { get; set; }
        public double Y { get; set; }
        public float Heading { get; set; }
        public byte GatheringInvisible { get; set; }
        public uint Fate { get; set; }
        public uint ModelID { get; set; }
        public byte Status { get; set; }
        public bool IsGM { get; set; }
        public byte Icon { get; set; }
        public byte Claimed { get; set; }
        public int TargetID { get; set; }
        public byte Job { get; set; }
        public byte Level { get; set; }
        public byte GrandCompany { get; set; }
        public byte GrandCompanyRank { get; set; }
        public byte Title { get; set; }
        public int HPCurrent { get; set; }
        public int HPMax { get; set; }
        public int MPCurrent { get; set; }
        public int MPMax { get; set; }
        public int TPCurrent { get; set; }
        public short GPCurrent { get; set; }
        public short GPMax { get; set; }
        public short CPCurrent { get; set; }
        public short CPMax { get; set; }
        public byte Race { get; set; }
        public byte Sex { get; set; }

        public List<StatusEntry> StatusEntries
        {
            get { return _statusEntries ?? (_statusEntries = new List<StatusEntry>()); }
            set
            {
                if (_statusEntries == null)
                {
                    _statusEntries = new List<StatusEntry>();
                }
                _statusEntries = value;
            }
        }

        public bool IsCasting { get; set; }
        public short CastingID { get; set; }
        public float CastingProgress { get; set; }
        public float CastingTime { get; set; }

        public decimal CastingPercentage
        {
            get
            {
                try
                {
                    if (IsCasting && CastingTime > 0)
                    {
                        return (decimal) (CastingProgress / CastingTime);
                    }
                }
                catch (Exception ex)
                {
                }
                return 0;
            }
        }

        public float GetDistanceTo(ActorEntity compare)
        {
            var distanceX = (float) Math.Abs(X - compare.X);
            var distanceY = (float) Math.Abs(Y - compare.Y);
            var distanceZ = (float) Math.Abs(Z - compare.Z);
            return (float)Math.Sqrt((distanceX * distanceX) + (distanceY * distanceY) + (distanceZ * distanceZ));
        }
    }
}
