// FFXIVAPP.Client
// DamageTracker.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Utilities
{
    [DoNotObfuscate]
    public static class CombatTracker
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

        public static void EnsureHistoryItem(IEnumerable<IncomingActionEntry> damageTaken)
        {
            lock (History)
            {
                try
                {
                    foreach (var damageEntry in damageTaken)
                    {
                        var damageContainer = History.FirstOrDefault(h => h.TargetID == damageEntry.TargetID) ?? new DamageContainer(damageEntry.TargetID);
                        if (!SequenceHistory.Contains(damageEntry.SequenceID))
                        {
                            SequenceHistory.Add(damageEntry.SequenceID);
                            damageContainer.DamageEntries.Add(damageEntry);
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

        public static List<IncomingActionEntry> GetHistoryItemsBySourceID(uint id)
        {
            lock (History)
            {
                var list = new List<IncomingActionEntry>();
                foreach (var damageContainer in History)
                {
                    list.AddRange(damageContainer.DamageEntries.Where(damageEntry => damageEntry.SourceID == id));
                }
                return list;
            }
        }

        public static List<IncomingActionEntry> GetHistoryItemsByTargetID(uint id)
        {
            lock (History)
            {
                var list = new List<IncomingActionEntry>();
                foreach (var damageContainer in History)
                {
                    list.AddRange(damageContainer.DamageEntries.Where(damageEntry => damageEntry.TargetID == id));
                }
                return list;
            }
        }

        #endregion

        private static readonly IList<DamageContainer> History = new List<DamageContainer>();
    }
}
