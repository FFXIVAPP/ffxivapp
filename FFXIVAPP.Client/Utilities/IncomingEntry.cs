// FFXIVAPP.Client
// IncomingEntry.cs
// 
// © 2013 Ryan Wilson

using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Common.Core.Memory.Enums;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Utilities
{
    [DoNotObfuscate]
    public class IncomingEntry
    {
        #region Memory Array Items

        public int Amount { get; set; }
        public int ComboAmount { get; set; }
        public uint ID { get; set; }
        public int SkillID { get; set; }
        public int Type1 { get; set; }
        public int Type2 { get; set; }
        public int UNK1 { get; set; }
        public int UNK2 { get; set; }
        public int UNK3 { get; set; }
        public int UNK4 { get; set; }

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
