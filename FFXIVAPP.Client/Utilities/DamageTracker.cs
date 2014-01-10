// FFXIVAPP.Client
// DamageTracker.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using System.Linq;
using FFXIVAPP.Client.Memory;
using FFXIVAPP.Common.Core.Memory;
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

        private static List<int> SequenceHistory = new List<int>();

        public static List<DamageContainer> GetHistoryItems()
        {
            lock (History)
            {
                return new List<DamageContainer>(History);
            }
        }

        public static void EnsureHistoryItem(ActorEntity actorEntity, IEnumerable<Structures.DamageTaken> damageTaken)
        {
            lock (History)
            {
                try
                {
                    var damageContainer = History.FirstOrDefault(h => h.NPCEntry.ID == actorEntity.ID) ?? new DamageContainer(actorEntity);
                    foreach (var d in damageTaken)
                    {
                        var damageEntry = new DamageEntry
                        {
                            Code = d.Code,
                            Damage = d.Damage,
                            IsCritical = d.IsCritical == 5,
                            SequenceID = d.SequenceID,
                            SkillID = d.SkillID,
                            SourceID = d.SourceID,
                            TargetID = actorEntity.ID
                        };
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
                    list.AddRange(damageContainer.DamageEntries.Where(damageEntry => damageEntry.TargetID == id));
                }
                return list;
            }
        }

        #endregion

        private static IList<DamageContainer> History = new List<DamageContainer>();
    }
}
