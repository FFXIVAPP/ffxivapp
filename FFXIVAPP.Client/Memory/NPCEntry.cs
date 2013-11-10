// FFXIVAPP.Client
// NPCEntry.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Memory
{
    [DoNotObfuscate]
    public class NPCEntry
    {
        private List<StatusEntry> _statusList;

        #region Property Backings

        public uint MapIndex { get; set; }
        public string Name { get; set; }
        public uint ID { get; set; }
        public uint NPCID1 { get; set; }
        public uint NPCID2 { get; set; }
        public int Type { get; set; }

        public NPCType NPCType
        {
            get
            {
                switch (Type)
                {
                    case 1:
                        return NPCType.PC;
                    case 2:
                        return NPCType.Monster;
                    case 3:
                        return NPCType.NPC;
                    case 6:
                        return NPCType.Gathering;
                }
                return NPCType.PC;
            }
        }

        public Coordinate Coordinate { get; set; }
        public float Heading { get; set; }
        public uint Fate { get; set; }
        public uint ModelID { get; set; }
        public byte Icon { get; set; }
        public byte Claimed { get; set; }
        public int TargetID { get; set; }
        public byte Level { get; set; }
        public int HPCurrent { get; set; }
        public int HPMax { get; set; }

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

        public int MPCurrent { get; set; }
        public int MPMax { get; set; }

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

        public int TPCurrent { get; set; }
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

        public int GPCurrent { get; set; }
        public int GPMax { get; set; }

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

        public int CPCurrent { get; set; }
        public int CPMax { get; set; }

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

        public List<StatusEntry> StatusList
        {
            get { return _statusList ?? (_statusList = new List<StatusEntry>()); }
            set
            {
                if (_statusList == null)
                {
                    _statusList = new List<StatusEntry>();
                }
                _statusList = value;
            }
        }

        public bool IsFate
        {
            get { return Fate == 0x801AFFFF && NPCType == NPCType.Monster; }
        }

        public bool IsClaimed
        {
            get { return Claimed == 1; }
        }

        public bool IsValid
        {
            get
            {
                if (Coordinate.X > 5000 || Coordinate.X < -5000)
                {
                    return false;
                }
                if (Coordinate.Y > 5000 || Coordinate.Y < -5000)
                {
                    return false;
                }
                if (Coordinate.Z > 5000 || Coordinate.Z < -5000)
                {
                    return false;
                }
                switch (NPCType)
                {
                    case NPCType.NPC:
                        return !String.IsNullOrEmpty(Name) && ID != 0 && (NPCID1 != 0 || NPCID2 != 0);
                    default:
                        return !String.IsNullOrEmpty(Name) && ID != 0;
                }
            }
        }

        #endregion

        public NPCEntry()
        {
            Name = "";
            StatusList = new List<StatusEntry>();
        }

        #region Declarations

        #endregion
    }
}
