// FFXIVAPP.Client
// Signature.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.Text.RegularExpressions;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Memory
{
    [DoNotObfuscate]
    public class Signature
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public Regex RegularExpress { get; set; }
        public int Offset { get; set; }
    }
}
