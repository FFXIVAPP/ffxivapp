// FFXIVAPP.Client
// IncomingActionEntry.cs
// 
// © 2013 Ryan Wilson

using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Common.Core.Memory.Enums;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Utilities
{
    [DoNotObfuscate]
    public class IncomingActionEntry
    {
        #region Memory Array Items

        public int Code { get; set; }
        public int SequenceID { get; set; }
        public int SkillID { get; set; }
        public uint SourceID { get; set; }
        public byte Type { get; set; }
        public int Amount { get; set; }

        #endregion

        public string SkillName
        {
            get { return ConstantsHelper.GetActionNameByID(SkillID); }
        }

        public uint TargetID { get; set; }
        public string TargetName { get; set; }
        public Actor.Type TargetType { get; set; }
    }
}
