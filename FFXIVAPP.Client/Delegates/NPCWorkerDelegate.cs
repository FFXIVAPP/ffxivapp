// FFXIVAPP.Client
// NPCWorkerDelegate.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using FFXIVAPP.Client.Memory;

#endregion

namespace FFXIVAPP.Client.Delegates
{
    internal static class NPCWorkerDelegate
    {
        public static NPCEntry CurrentUser;
        public static readonly List<NPCEntry> MonsterList = new List<NPCEntry>();

        /// <summary>
        /// </summary>
        public static void OnNewNPC(List<NPCEntry> npcEntry)
        {
            if (!npcEntry.Any())
            {
                return;
            }
            CurrentUser = npcEntry.First();
            Func<bool> saveToDictionary = delegate
            {
                foreach (var entry in npcEntry.Where(e => e.NPCType == NPCType.Monster)
                                              .Where(entry => MonsterList.All(m => m.ID != entry.ID)))
                {
                    MonsterList.Add(entry);
                }
                return true;
            };
            saveToDictionary.BeginInvoke(null, saveToDictionary);
        }
    }
}
