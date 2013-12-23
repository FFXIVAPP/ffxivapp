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
    public static class LootWorkerDelegate
    {
        #region Collection Access & Modification

        public static void AddLootEntry(LootEntry entry)
        {
            lock (_lootEntries)
            {
                _lootEntries.Add(entry);
            }
        }

        public static void ReplaceLootEntries(IEnumerable<LootEntry> entries)
        {
            lock (_lootEntries)
            {
                _lootEntries = new List<LootEntry>(entries);
            }
        }

        public static IList<LootEntry> GetLootEntries()
        {
            lock (_lootEntries)
            {
                return new List<LootEntry>(_lootEntries);
            }
        }

        #endregion

        #region Declarations

        private static IList<LootEntry> _lootEntries = new List<LootEntry>();

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
                    AddLootEntry(lootEntry);
                }
                XIVDBViewModel.Instance.LootSeen++;
                return true;
            };
            saveToDictionary.BeginInvoke(delegate
            {
                var chunkSize = UploadHelper.ChunkSize;
                var chunksProcessed = UploadHelper.ChunksProcessed;
                if (GetLootEntries()
                    .Count <= (chunkSize * (chunksProcessed + 1)))
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
                UploadHelper.PostUpload("loot", new List<LootEntry>(GetLootEntries()
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
