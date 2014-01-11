// FFXIVAPP.Client
// CombatEntry.cs
// 
// © 2013 Ryan Wilson

using System.Collections.Generic;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Utilities
{
    [DoNotObfuscate]
    public class CombatEntry
    {
        public List<IncomingEntry> IncomingEntries { get; set; }
        public List<IncomingActionEntry> IncomingActionEntries { get; set; }
    }
}
