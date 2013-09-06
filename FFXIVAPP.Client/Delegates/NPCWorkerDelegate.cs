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
        public static readonly Dictionary<string, NPCEntry> MonsterDatabase = new Dictionary<string, NPCEntry>();

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
                foreach (var entry in npcEntry)
                {
                    var key = String.Format("{0}-{1} [{2}]", entry.Name.ToLower(), entry.ID, entry.MapIndex);
                    try
                    {
                        if (!MonsterDatabase.ContainsKey(key))
                        {
                            MonsterDatabase.Add(key, entry);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return true;
            };
            saveToDictionary.BeginInvoke(null, null);
        }
    }
}
