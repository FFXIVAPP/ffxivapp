// FFXIVAPP.Client
// KillEntry.cs
// 
// © 2013 Ryan Wilson

using FFXIVAPP.Common.Core.Memory;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Models
{
    [DoNotObfuscate]
    public class KillEntry
    {
        #region Auto Properties

        public Coordinate Coordinate { get; set; }
        public uint MapIndex { get; set; }
        public uint ModelID { get; set; }

        #endregion

        public KillEntry()
        {
            Coordinate = new Coordinate();
            MapIndex = 0;
            ModelID = 0;
        }

        public bool IsValid()
        {
            return MapIndex > 0 && ModelID > 0;
        }
    }
}
