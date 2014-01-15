// FFXIVAPP.Client
// OutGoingEntry.cs
// 
// © 2013 Ryan Wilson

using System.Collections.Generic;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Common.Core.Memory.Enums;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Utilities
{
    [DoNotObfuscate]
    public class OutGoingEntry
    {
        public OutGoingEntry()
        {
            TargetIDs = new List<uint>();
        }

        #region Memory Array Items

        public int Counter { get; set; }
        public int SkillID { get; set; }
        public int SequenceID { get; set; }
        public List<uint> TargetIDs { get; set; }

        #endregion

        public string SkillName
        {
            get { return ConstantsHelper.GetActionNameByID(SkillID); }
        }

        public uint SourceID { get; set; }
        public string SourceName { get; set; }
        public Actor.Type SourceType { get; set; }
    }
}
