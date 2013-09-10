// FFXIVAPP.Client
// NPCWorkerDelegate.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using FFXIVAPP.Client.Helpers.SocketIO;
using FFXIVAPP.Client.Memory;

#endregion

namespace FFXIVAPP.Client.Delegates
{
    internal static class NPCWorkerDelegate
    {
        #region Declarations

        public static readonly List<NPCEntry> NPCList = new List<NPCEntry>();

        #endregion

        /// <summary>
        /// </summary>
        public static void OnNewNPC(List<NPCEntry> npcEntry)
        {
            if (!npcEntry.Any())
            {
                return;
            }
            Func<bool> saveToDictionary = delegate
            {
                foreach (var entry in npcEntry.Where(entry => NPCList.All(m => m.NPCID != entry.NPCID)))
                {
                    NPCList.Add(entry);
                }
                return true;
            };
            saveToDictionary.BeginInvoke(delegate
            {
                const int chunkSize = NPCEntryHelper.ChunkSize;
                var chunksProcessed = NPCEntryHelper.ChunksProcessed;
                if (NPCList.Count <= (chunkSize * (chunksProcessed + 1)))
                {
                    return;
                }
                if (!NPCEntryHelper.Processing)
                {
                    NPCEntryHelper.ProcessUpload(new List<NPCEntry>(NPCList.Skip(chunksProcessed * chunkSize)));
                }
            }, saveToDictionary);
        }
    }
}
