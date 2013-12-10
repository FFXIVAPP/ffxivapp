// FFXIVAPP.Client
// LootWorkerDelegate.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using System.Linq;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Models;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Client.ViewModels;
using Newtonsoft.Json;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Delegates
{
    [DoNotObfuscate]
    internal static class LootWorkerDelegate
    {
        #region Declarations

        public static readonly IList<LootEntry> LootEntries = new List<LootEntry>();

        private static readonly UploadHelper UploadHelper = new UploadHelper(5);

        #endregion

        /// <summary>
        /// </summary>
        public static void OnNewLoot(LootEntry lootEntry)
        {
            Func<bool> saveToDictionary = delegate
            {
                if (!lootEntry.IsValid())
                {
                    return false;
                }
                if (HttpPostHelper.IsValidJson(JsonConvert.SerializeObject(lootEntry)))
                {
                    LootEntries.Add(lootEntry);
                }
                XIVDBViewModel.Instance.LootSeen++;
                return true;
            };
            saveToDictionary.BeginInvoke(delegate
            {
                var chunkSize = UploadHelper.ChunkSize;
                var chunksProcessed = UploadHelper.ChunksProcessed;
                if (LootEntries.Count <= (chunkSize * (chunksProcessed + 1)))
                {
                    return;
                }
                ProcessUploads();
            }, saveToDictionary);
        }

        /// <summary>
        /// </summary>
        public static void ProcessUploads()
        {
            if (UploadHelper.Processing || !Settings.Default.AllowXIVDBIntegration || !Constants.IsOpen || !XIVDBViewModel.Instance.LootUploadEnabled)
            {
                return;
            }
            var chunkSize = UploadHelper.ChunkSize;
            var chunksProcessed = UploadHelper.ChunksProcessed;
            try
            {
                UploadHelper.Processing = true;
                UploadHelper.PostUpload("loot", new List<LootEntry>(LootEntries.ToList()
                                                                               .Skip(chunksProcessed * chunkSize)));
                XIVDBViewModel.Instance.LootProcessed++;
            }
            catch (Exception ex)
            {
                UploadHelper.Processing = false;
            }
        }
    }
}
