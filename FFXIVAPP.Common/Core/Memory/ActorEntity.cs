// FFXIVAPP.Common
// ActorEntity.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using FFXIVAPP.Common.Helpers;

#endregion

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
                    case 1:
                        return Enums.Actor.Type.PC;
                    case 2:
                        return Enums.Actor.Type.Monster;
                    case 3:
                        return Enums.Actor.Type.NPC;
                    case 6:
                        return Enums.Actor.Type.Gathering;
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
                    case 1:
                        return Enums.Actor.TargetType.Own;
                    case 2:
                        return Enums.Actor.TargetType.True;
                    case 4:
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
                    case 1:
                        return Enums.Actor.Status.Idle;
                    case 2:
                        return Enums.Actor.Status.Dead;
                    case 3:
                        return Enums.Actor.Status.Sitting;
                    case 4:
                        return Enums.Actor.Status.Mounted;
                    case 5:
                        return Enums.Actor.Status.Crafting;
                    case 6:
                        return Enums.Actor.Status.Gathering;
                    case 7:
                        return Enums.Actor.Status.Melding;
                    case 8:
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
                    case 0:
                        return Enums.Actor.Icon.None;
                    case 1:
                        return Enums.Actor.Icon.Yoshida;
                    case 2:
                        return Enums.Actor.Icon.GM;
                    case 3:
                        return Enums.Actor.Icon.SGM;
                    case 4:
                        return Enums.Actor.Icon.Clover;
                    case 5:
                        return Enums.Actor.Icon.DC;
                    case 6:
                        return Enums.Actor.Icon.Smiley;
                    case 9:
                        return Enums.Actor.Icon.Red_Cross;
                    case 10:
                        return Enums.Actor.Icon.Grey_DC;
                    case 11:
                        return Enums.Actor.Icon.Processing;
                    case 12:
                        return Enums.Actor.Icon.Busy;
                    case 13:
                        return Enums.Actor.Icon.Duty;
                    case 14:
                        return Enums.Actor.Icon.Processing_Yellow;
                    case 15:
                        return Enums.Actor.Icon.Processing_Grey;
                    case 16:
                        return Enums.Actor.Icon.Cutscene;
                    case 18:
                        return Enums.Actor.Icon.Chocobo;
                    case 19:
                        return Enums.Actor.Icon.Sitting;
                    case 20:
                        return Enums.Actor.Icon.Wrench_Yellow;
                    case 21:
                        return Enums.Actor.Icon.Wrench;
                    case 22:
                        return Enums.Actor.Icon.Dice;
                    case 23:
                        return Enums.Actor.Icon.Processing_Green;
                    case 24:
                        return Enums.Actor.Icon.Sword;
                    case 25:
                        return Enums.Actor.Icon.DutyFinder;
                    case 26:
                        return Enums.Actor.Icon.Alliance_Leader;
                    case 27:
                        return Enums.Actor.Icon.Alliance_Blue_Leader;
                    case 28:
                        return Enums.Actor.Icon.Alliance_Blue;
                    case 32:
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
                    case 1:
                        return Enums.Actor.Claimed.Claimed;
                    case 2:
                        return Enums.Actor.Claimed.UnClaimed;
                    default:
                        return Enums.Actor.Claimed.Unknown;
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
                    return Convert.ToByte(Math.Ceiling((Convert.ToDouble(CPCurrent) / Convert.ToDouble(CPMax))));
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
                    case 0:
                        return Enums.Actor.Sex.Male;
                    case 1:
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

        public string Name
        {
            get { return _name; }
            set { _name = StringHelper.TitleCase(value); }
        }

        public uint ID { get; set; }
        public uint NPCID1 { get; set; }
        public uint NPCID2 { get; set; }
        public byte Type { get; set; }

        public byte TargetType { get; set; }

        public byte Distance { get; set; }
        public float X { get; set; }
        public float Z { get; set; }
        public float Y { get; set; }
        public float Heading { get; set; }
        public uint Fate { get; set; }
        public uint ModelID { get; set; }
        public byte Status { get; set; }

        public bool IsGM { get; set; }
        public byte Icon { get; set; }

        public byte Claimed { get; set; }

        public int TargetID { get; set; }
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
    }
}
