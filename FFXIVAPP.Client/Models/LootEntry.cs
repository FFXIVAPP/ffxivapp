// FFXIVAPP.Client
// LootEntry.cs
// 
// © 2013 Ryan Wilson

using System;

namespace FFXIVAPP.Client.Models
{
    public class LootEntry
    {
        #region Auto Properties

        public string ItemName { get; set; }
        public ushort MapIndex { get; set; }
        public uint ModelID { get; set; }

        #endregion

        public LootEntry(string itemName = "", ushort mapIndex = 0, uint modelID = 0)
        {
            ItemName = itemName;
            MapIndex = mapIndex;
            ModelID = modelID;
        }

        public bool IsValid()
        {
            return !String.IsNullOrWhiteSpace(ItemName) && MapIndex > 0;
        }
    }
}
