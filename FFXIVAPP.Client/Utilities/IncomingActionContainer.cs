// FFXIVAPP.Client
// IncomingActionContainer.cs
// 
// © 2013 Ryan Wilson

using System.Collections.Generic;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Utilities
{
    [DoNotObfuscate]
    public class IncomingActionContainer
    {
        public IncomingActionContainer(uint targetID)
        {
            TargetID = targetID;
            IncomingActionEntries = new List<IncomingActionEntry>();
        }

        public uint TargetID { get; set; }
        public List<IncomingActionEntry> IncomingActionEntries { get; set; }
    }
}
