// FFXIVAPP.Common
// ActorEntity.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using FFXIVAPP.Common.Core.Memory.Enums;
using FFXIVAPP.Common.Core.Memory.Interfaces;
using FFXIVAPP.Common.Helpers;

namespace FFXIVAPP.Common.Core.Memory
{
    public class ActorEntity : IActorEntity
    {
        private string _name;
        private List<StatusEntry> _statusEntries;

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

        public bool IsFate
        {
            get { return Fate == 0x801AFFFF && Type == Actor.Type.Monster; }
        }

        public bool IsClaimed
        {
            get { return Status == Actor.Status.Claimed; }
        }

        public bool IsValid
        {
            get
            {
                switch (Type)
                {
                    case Actor.Type.NPC:
                        return !String.IsNullOrEmpty(Name) && ID != 0 && (NPCID1 != 0 || NPCID2 != 0);
                    default:
                        return !String.IsNullOrEmpty(Name) && ID != 0;
                }
            }
        }

        public Coordinate Coordinate { get; set; }

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
        public Actor.Type Type { get; set; }
        public Actor.TargetType TargetType { get; set; }
        public byte Distance { get; set; }
        public double X { get; set; }
        public double Z { get; set; }
        public double Y { get; set; }
        public float Heading { get; set; }
        public byte GatheringInvisible { get; set; }
        public uint Fate { get; set; }
        public uint ModelID { get; set; }
        public Actor.ActionStatus ActionStatus { get; set; }
        public bool IsGM { get; set; }
        public Actor.Icon Icon { get; set; }
        public Actor.Status Status { get; set; }
        public int TargetID { get; set; }
        public Actor.Job Job { get; set; }
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
        public Actor.Sex Sex { get; set; }

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

        public float GetDistanceTo(ActorEntity compare)
        {
            var distanceX = (float) Math.Abs(X - compare.X);
            var distanceY = (float) Math.Abs(Y - compare.Y);
            var distanceZ = (float) Math.Abs(Z - compare.Z);
            return (float) Math.Sqrt((distanceX * distanceX) + (distanceY * distanceY) + (distanceZ * distanceZ));
        }
    }
}
