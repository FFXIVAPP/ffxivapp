// FFXIVAPP.Client
// DamageTracker.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using System.Linq;
using FFXIVAPP.Common.Utilities;
using Newtonsoft.Json;
using NLog;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Utilities
{
    [DoNotObfuscate]
    public static class DamageTracker
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region History Setters & Getters

        private static readonly List<int> SequenceHistory = new List<int>();

        public static List<DamageContainer> GetHistoryItems()
        {
            lock (History)
            {
                return new List<DamageContainer>(History);
            }
        }

        public static void EnsureHistoryItem(IEnumerable<DamageEntry> damageTaken)
        {
            lock (History)
            {
                try
                {
                    foreach (var damageEntry in damageTaken)
                    {
                        var damageContainer = History.FirstOrDefault(h => h.NPCEntry.ID == damageEntry.NPCEntry.ID) ?? new DamageContainer(damageEntry.NPCEntry);
                        if (!SequenceHistory.Contains(damageEntry.SequenceID))
                        {
                            SequenceHistory.Add(damageEntry.SequenceID);
                            damageContainer.DamageEntries.Add(damageEntry);
                            Logging.Log(Logger, JsonConvert.SerializeObject(damageEntry));
                        }
                        if (SequenceHistory.Count > 30)
                        {
                            SequenceHistory.RemoveAt(0);
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static List<DamageEntry> GetHistoryItemsBySourceID(uint id)
        {
            lock (History)
            {
                var list = new List<DamageEntry>();
                foreach (var damageContainer in History)
                {
                    list.AddRange(damageContainer.DamageEntries.Where(damageEntry => damageEntry.SourceID == id));
                }
                return list;
            }
        }

        public static List<DamageEntry> GetHistoryItemsByTargetID(uint id)
        {
            lock (History)
            {
                var list = new List<DamageEntry>();
                foreach (var damageContainer in History)
                {
                    list.AddRange(damageContainer.DamageEntries.Where(damageEntry => damageEntry.NPCEntry.ID == id));
                }
                return list;
            }
        }

        #endregion

        private static readonly IList<DamageContainer> History = new List<DamageContainer>();
    }
}
