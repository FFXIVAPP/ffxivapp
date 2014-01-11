// FFXIVAPP.Client
// ActionInfo.cs
// 
// © 2013 Ryan Wilson

using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Models
{
    [DoNotObfuscate]
    public class ActionInfo
    {
        public string JA { get; set; }
        public string EN { get; set; }
        public string FR { get; set; }
        public string DE { get; set; }
        public string JA_HelpLabel { get; set; }
        public string EN_HelpLabel { get; set; }
        public string FR_HelpLabel { get; set; }
        public string DE_HelpLabel { get; set; }
    }
}
