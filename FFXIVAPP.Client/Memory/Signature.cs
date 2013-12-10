// FFXIVAPP.Client
// Signature.cs
// 
// © 2013 Ryan Wilson

using System.Text.RegularExpressions;
using SmartAssembly.Attributes;

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
