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

#endregion

namespace FFXIVAPP.Client.Delegates
{
    internal static class NPCWorkerDelegate
    {
        #region Declarations

        public static readonly IList<NPCEntry> NPCList = new List<NPCEntry>();
        private static readonly UploadHelper UploadHelper = new UploadHelper(50);

        #endregion

        /// <summary>
        /// </summary>
        public static void OnNewNPC(List<NPCEntry> npcEntries)
        {
            Func<bool> saveToDictionary = delegate
            {
                try
                {
                    var enumerable = NPCList.ToList();
                    foreach (var npcEntry in npcEntries)
                    {
                        var exists = enumerable.FirstOrDefault(n => n.NPCID == npcEntry.NPCID);
                        if (exists != null)
                        {
                            continue;
                        }
                        if (HttpPostHelper.IsValidJson(JsonConvert.SerializeObject(npcEntry)))
                        {
                            NPCList.Add(npcEntry);
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
                if (UploadHelper.Processing || !Settings.Default.AllowXIVDBIntegration)
                {
                    return;
                }
                var chunkSize = UploadHelper.ChunkSize;
                var chunksProcessed = UploadHelper.ChunksProcessed;
                if (NPCList.Count <= (chunkSize * (chunksProcessed + 1)))
                {
                    return;
                }
                try
                {
                    UploadHelper.Processing = true;
                    UploadHelper.PostUpload("npc", new List<NPCEntry>(NPCList.ToList()
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
                UploadHelper.PostUpload("npc", new List<NPCEntry>(NPCList.ToList()
                                                                         .Skip(chunksProcessed * chunkSize)));
            }
            catch (Exception ex)
            {
            }
        }
    }
}
