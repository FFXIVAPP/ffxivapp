// FFXIVAPP.Client
// IncomingActionEntry.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Globalization;
using FFXIVAPP.Client.Properties;
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
            get
            {
                var key = SkillID.ToString(CultureInfo.InvariantCulture);
                try
                {
                    if (Constants.Actions.ContainsKey(key))
                    {
                        switch (Settings.Default.GameLanguage)
                        {
                            case "English":
                                return Constants.Actions[key].EN;
                            case "French":
                                return Constants.Actions[key].FR;
                            case "Japanese":
                                return Constants.Actions[key].JA;
                            case "German":
                                return Constants.Actions[key].DE;
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                return "UNKNOWN";
            }
        }

        public uint TargetID { get; set; }
        public string TargetName { get; set; }
        public Actor.Type TargetType { get; set; }
    }
}
