// FFXIVAPP.Client
// CombatTracker.cs
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
    public static class CombatTracker
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region IncomingHistory Setters & Getters

        private static readonly List<Tuple<uint, int, int>> IncomingSequenceHistory = new List<Tuple<uint, int, int>>();

        public static List<IncomingContainer> GetIncomingHistoryItems()
        {
            lock (IncomingHistory)
            {
                return new List<IncomingContainer>(IncomingHistory);
            }
        }

        public static void EnsureIncomingEntries(IEnumerable<IncomingEntry> incomingEntries)
        {
            lock (IncomingActionHistory)
            {
                try
                {
                    foreach (var entry in incomingEntries)
                    {
                        var container = IncomingHistory.FirstOrDefault(h => h.TargetID == entry.TargetID) ?? new IncomingContainer(entry.TargetID);
                        if (!IncomingSequenceHistory.Any(item => item.Item1 == entry.ID && item.Item2 == entry.SkillID && item.Item3 == entry.Amount))
                        {
                            IncomingSequenceHistory.Add(new Tuple<uint, int, int>(entry.ID, entry.SkillID, entry.Amount));
                            container.IncomingEntries.Add(entry);
                            Logging.Log(Logger, JsonConvert.SerializeObject(entry));
                        }
                        if (IncomingSequenceHistory.Count > 50)
                        {
                            IncomingSequenceHistory.RemoveAt(0);
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static List<IncomingActionEntry> GetIncomingHistoryItemsByTargetID(uint id)
        {
            lock (IncomingHistory)
            {
                var list = new List<IncomingActionEntry>();
                foreach (var incomingActionContainer in IncomingActionHistory)
                {
                    list.AddRange(incomingActionContainer.IncomingActionEntries.Where(entry => entry.TargetID == id));
                }
                return list;
            }
        }

        #endregion

        #region IncomingActionHistory Setters & Getters

        private static readonly List<int> IncomingActionSequenceHistory = new List<int>();

        public static List<IncomingActionContainer> GetIncomingActionHistoryItems()
        {
            lock (IncomingActionHistory)
            {
                return new List<IncomingActionContainer>(IncomingActionHistory);
            }
        }

        public static void EnsureIncomingActionEntries(IEnumerable<IncomingActionEntry> incomingActionEntries)
        {
            lock (IncomingActionHistory)
            {
                try
                {
                    foreach (var entry in incomingActionEntries)
                    {
                        var container = IncomingActionHistory.FirstOrDefault(h => h.TargetID == entry.TargetID) ?? new IncomingActionContainer(entry.TargetID);
                        if (!IncomingActionSequenceHistory.Contains(entry.SequenceID))
                        {
                            IncomingActionSequenceHistory.Add(entry.SequenceID);
                            container.IncomingActionEntries.Add(entry);
                            Logging.Log(Logger, JsonConvert.SerializeObject(entry));
                        }
                        if (IncomingActionSequenceHistory.Count > 50)
                        {
                            IncomingActionSequenceHistory.RemoveAt(0);
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static List<IncomingActionEntry> GetIncomingActionHistoryItemsBySourceID(uint id)
        {
            lock (IncomingActionHistory)
            {
                var list = new List<IncomingActionEntry>();
                foreach (var incomingActionContainer in IncomingActionHistory)
                {
                    list.AddRange(incomingActionContainer.IncomingActionEntries.Where(entry => entry.SourceID == id));
                }
                return list;
            }
        }

        public static List<IncomingActionEntry> GetIncomingActionHistoryItemsByTargetID(uint id)
        {
            lock (IncomingActionHistory)
            {
                var list = new List<IncomingActionEntry>();
                foreach (var incomingActionContainer in IncomingActionHistory)
                {
                    list.AddRange(incomingActionContainer.IncomingActionEntries.Where(entry => entry.TargetID == id));
                }
                return list;
            }
        }

        #endregion

        #region OutGoingHistory Setters & Getters

        private static readonly List<int> OutGoingSequenceHistory = new List<int>();

        public static List<OutGoingContainer> GetOutGoingHistoryItems()
        {
            lock (OutGoingHistory)
            {
                return new List<OutGoingContainer>(OutGoingHistory);
            }
        }

        public static void EnsureOutGoingEntries(OutGoingEntry outGoingEntry)
        {
            lock (OutGoingHistory)
            {
                try
                {
                    var container = OutGoingHistory.FirstOrDefault(h => h.SourceID == outGoingEntry.SourceID) ?? new OutGoingContainer(outGoingEntry.SourceID);
                    if (!OutGoingSequenceHistory.Contains(outGoingEntry.SequenceID))
                    {
                        OutGoingSequenceHistory.Add(outGoingEntry.SequenceID);
                        container.OutGoingEntries.Add(outGoingEntry);
                        Logging.Log(Logger, JsonConvert.SerializeObject(outGoingEntry));
                    }
                    if (OutGoingSequenceHistory.Count > 50)
                    {
                        OutGoingSequenceHistory.RemoveAt(0);
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static List<OutGoingEntry> GetOutGoingHistoryItemsBySourceID(uint id)
        {
            lock (OutGoingHistory)
            {
                var list = new List<OutGoingEntry>();
                foreach (var outGoingContainer in OutGoingHistory)
                {
                    list.AddRange(outGoingContainer.OutGoingEntries.Where(entry => entry.SourceID == id));
                }
                return list;
            }
        }

        #endregion

        private static readonly IList<IncomingContainer> IncomingHistory = new List<IncomingContainer>();

        private static readonly IList<IncomingActionContainer> IncomingActionHistory = new List<IncomingActionContainer>();

        private static readonly IList<OutGoingContainer> OutGoingHistory = new List<OutGoingContainer>();
    }
}
