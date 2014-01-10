// FFXIVAPP.Client
// DamageEntry.cs
// 
// © 2013 Ryan Wilson

using FFXIVAPP.Common.Core.Memory;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Utilities
{
    [DoNotObfuscate]
    public class DamageEntry
    {
        #region Memory Array Items

        public int Code { get; set; }
        public int SequenceID { get; set; }
        public int SkillID { get; set; }
        public uint SourceID { get; set; }
        public byte Type { get; set; }
        public int Amount { get; set; }

        #endregion

        public ActorEntity NPCEntry { get; set; }

        public bool IsCritical
        {
            get { return Type == 5; }
        }
    }
}
