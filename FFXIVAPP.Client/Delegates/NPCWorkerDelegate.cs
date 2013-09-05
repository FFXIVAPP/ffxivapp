// FFXIVAPP.Client
// NPCWorkerDelegate.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using FFXIVAPP.Client.Memory;
using FFXIVAPP.Common.Helpers;
using Newtonsoft.Json;

#endregion

namespace FFXIVAPP.Client.Delegates
{
    internal static class NPCWorkerDelegate
    {
        public static readonly Dictionary<string, NPCEntry> MonsterDatabase = new Dictionary<string, NPCEntry>();

        /// <summary>
        /// </summary>
        public static void OnNewNPC(List<NPCEntry> npcEntry)
        {
            // TODO: do stuff with the array
            Func<bool> saveToDictionary = delegate
            {
                foreach (var entry in npcEntry.Where(n => n.NPCType == NPCType.Monster))
                {
                    var key = String.Format("{0} [{1}]", entry.Name.ToLower(), entry.MapIndex);
                    try
                    {
                        if (!MonsterDatabase.ContainsKey(key))
                        {
                            MonsterDatabase.Add(key, entry);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show(ex.Message);
                    }
                }
                return true;
            };
            saveToDictionary.BeginInvoke(null, null);
        }
    }
}
