﻿// FFXIVAPP.Client
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
        public CombatEntry()
        {
            IncomingEntries = new List<IncomingEntry>();
            IncomingActionEntries = new List<IncomingActionEntry>();
        }

        public List<IncomingEntry> IncomingEntries { get; set; }
        public List<IncomingActionEntry> IncomingActionEntries { get; set; }
    }
}
