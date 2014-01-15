// FFXIVAPP.Client
// IncomingContainer.cs
// 
// © 2013 Ryan Wilson

using System.Collections.Generic;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Utilities
{
    [DoNotObfuscate]
    public class IncomingContainer
    {
        public IncomingContainer(uint targetID)
        {
            TargetID = targetID;
            IncomingEntries = new List<IncomingEntry>();
        }

        public uint TargetID { get; set; }
        public List<IncomingEntry> IncomingEntries { get; set; }
    }
}
