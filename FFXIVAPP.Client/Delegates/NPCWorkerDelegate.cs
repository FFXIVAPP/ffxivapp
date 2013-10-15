// FFXIVAPP.Client
// NPCWorkerDelegate.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Memory;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Client.ViewModels;
using Newtonsoft.Json;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Delegates
{
    [DoNotObfuscate]
    internal static class NPCWorkerDelegate
    {
        #region Declarations

        public static IList<NPCEntry> NPCEntries = new List<NPCEntry>();

        public static readonly IList<NPCEntry> UniqueNPCEntries = new List<NPCEntry>();

        private static readonly UploadHelper UploadHelper = new UploadHelper(50);

        #endregion

        /// <summary>
        /// </summary>
        public static void OnNewNPC(List<NPCEntry> npcEntries)
        {
            if (!npcEntries.Any())
            {
                return;
            }
            NPCEntries = npcEntries;
            Func<bool> saveToDictionary = delegate
            {
                try
                {
                    var enumerable = UniqueNPCEntries.ToList();
                    foreach (var npcEntry in npcEntries)
                    {
                        var exists = enumerable.FirstOrDefault(n => n.NPCID == npcEntry.NPCID);
                        if (exists != null)
                        {
                            continue;
                        }
                        if (HttpPostHelper.IsValidJson(JsonConvert.SerializeObject(npcEntry)))
                        {
                            UniqueNPCEntries.Add(npcEntry);
                        }
                        XIVDBViewModel.Instance.NPCSeen++;
                    }
                }
                catch (Exception ex)
                {
                }
                return true;
            };
            saveToDictionary.BeginInvoke(delegate
            {
                if (UploadHelper.Processing || !Settings.Default.AllowXIVDBIntegration || !Constants.IsOpen)
                {
                    return;
                }
                var chunkSize = UploadHelper.ChunkSize;
                var chunksProcessed = UploadHelper.ChunksProcessed;
                if (UniqueNPCEntries.Count <= (chunkSize * (chunksProcessed + 1)))
                {
                    return;
                }
                try
                {
                    UploadHelper.Processing = true;
                    UploadHelper.PostUpload("npc", new List<NPCEntry>(UniqueNPCEntries.ToList()
                                                                                      .Skip(chunksProcessed * chunkSize)));
                    XIVDBViewModel.Instance.NPCProcessed++;
                }
                catch (Exception ex)
                {
                    UploadHelper.Processing = true;
                }
            }, saveToDictionary);
        }

        /// <summary>
        /// </summary>
        public static void ProcessRemaining()
        {
            var chunkSize = UploadHelper.ChunkSize;
            var chunksProcessed = UploadHelper.ChunksProcessed;
            try
            {
                UploadHelper.PostUpload("npc", new List<NPCEntry>(UniqueNPCEntries.ToList()
                                                                                  .Skip(chunksProcessed * chunkSize)));
            }
            catch (Exception ex)
            {
            }
        }
    }
}
