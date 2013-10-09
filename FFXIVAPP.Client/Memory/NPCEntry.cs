// FFXIVAPP.Client
// NPCEntry.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using System.Windows.Documents;

namespace FFXIVAPP.Client.Memory
{
    public class NPCEntry
    {
        private List<StatusEntry> _statusList;

        #region Property Backings

        public uint MapIndex { get; set; }
        public string Name { get; set; }
        public uint ID { get; set; }
        public uint NPCID { get; set; }
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
        public int HPCurrent { get; set; }
        public int HPMax { get; set; }

        public byte HPPercent
        {
            get
            {
                try
                {
                    return Convert.ToByte(Math.Ceiling((Convert.ToDouble(HPCurrent) / Convert.ToDouble(HPMax)) * 100.0));
                }
                catch
                {
                    return 0;
                }
            }
        }

        public int MPCurrent { get; set; }
        public int MPMax { get; set; }

        public byte MPPercent
        {
            get
            {
                try
                {
                    return Convert.ToByte(Math.Ceiling((Convert.ToDouble(MPCurrent) / Convert.ToDouble(MPMax)) * 100.0));
                }
                catch
                {
                    return 0;
                }
            }
        }

        public int TPCurrent { get; set; }
        public int TPMax { get; set; }

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
                        return !String.IsNullOrEmpty(Name) && ID != 0 && NPCID != 0;
                    default:
                        return !String.IsNullOrEmpty(Name) && ID != 0;
                }
            }
        }

        #endregion

        #region Declarations

        #endregion
    }
}
