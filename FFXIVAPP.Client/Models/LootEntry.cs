// FFXIVAPP.Client
// LootEntry.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using FFXIVAPP.Client.Memory;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Models
{
    [DoNotObfuscate]
    public class LootEntry
    {
        #region Auto Properties

        public Coordinate Coordinate { get; set; }
        public string ItemName { get; set; }
        public uint MapIndex { get; set; }
        public uint ModelID { get; set; }

        #endregion

        public LootEntry(string itemName = "")
        {
            Coordinate = new Coordinate();
            ItemName = itemName;
            MapIndex = 0;
            ModelID = 0;
        }

        public bool IsValid()
        {
            return !String.IsNullOrWhiteSpace(ItemName) && MapIndex > 0;
        }
    }
}
