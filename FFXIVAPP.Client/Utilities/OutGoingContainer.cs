// FFXIVAPP.Client
// OutGoingContainer.cs
// 
// © 2013 Ryan Wilson

using System.Collections.Generic;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Utilities
{
    [DoNotObfuscate]
    public class OutGoingContainer
    {
        public OutGoingContainer(uint sourceID)
        {
            SourceID = sourceID;
            OutGoingEntries = new List<OutGoingEntry>();
        }

        public uint SourceID { get; set; }
        public List<OutGoingEntry> OutGoingEntries { get; set; }
    }
}
