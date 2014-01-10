// FFXIVAPP.Client
// DamageEntry.cs
// 
// © 2013 Ryan Wilson

using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Utilities
{
    [DoNotObfuscate]
    public class DamageEntry
    {
        public int Code { get; set; }
        public int SequenceID { get; set; }
        public int SkillID { get; set; }
        public uint SourceID { get; set; }
        public uint TargetID { get; set; }
        public int Damage { get; set; }
        public bool IsCritical { get; set; }
    }
}
