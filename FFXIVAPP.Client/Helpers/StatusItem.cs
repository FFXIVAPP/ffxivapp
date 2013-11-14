// FFXIVAPP.Client
// StatusItem.cs
// 
// © 2013 Ryan Wilson

#region Usings

using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Helpers
{
    [DoNotObfuscate]
    public class StatusItem
    {
        public StatusLocalization Name { get; set; }
        public bool CompanyAction { get; set; }
    }
}
