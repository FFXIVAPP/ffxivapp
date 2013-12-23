// FFXIVAPP.Client
// KillWorkerDelegate.cs
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
    public static class KillWorkerDelegate
    {
        #region Collection Access & Modification

        public static void AddKillEntry(KillEntry entry)
        {
            lock (_killEntries)
            {
                _killEntries.Add(entry);
            }
        }

        public static void ReplaceKillEntries(IEnumerable<KillEntry> entries)
        {
            lock (_killEntries)
            {
                _killEntries = new List<KillEntry>(entries);
            }
        }

        public static IList<KillEntry> GetKillEntries()
        {
            lock (_killEntries)
            {
                return new List<KillEntry>(_killEntries);
            }
        }

        #endregion

        #region Declarations

        private static IList<KillEntry> _killEntries = new List<KillEntry>();

        private static readonly UploadHelper UploadHelper = new UploadHelper(5);

        #endregion

        /// <summary>
        /// </summary>
        public static void OnNewKill(KillEntry killEntry)
        {
            Func<bool> saveToDictionary = delegate
            {
                if (!killEntry.IsValid())
                {
                    return false;
                }
                if (HttpPostHelper.IsValidJson(JsonConvert.SerializeObject(killEntry)))
                {
                    AddKillEntry(killEntry);
                }
                XIVDBViewModel.Instance.KillSeen++;
                return true;
            };
            saveToDictionary.BeginInvoke(delegate
            {
                var chunkSize = UploadHelper.ChunkSize;
                var chunksProcessed = UploadHelper.ChunksProcessed;
                if (GetKillEntries()
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
            if (UploadHelper.Processing || !Settings.Default.AllowXIVDBIntegration || !Constants.IsOpen || !XIVDBViewModel.Instance.KillUploadEnabled)
            {
                return;
            }
            var chunkSize = UploadHelper.ChunkSize;
            var chunksProcessed = UploadHelper.ChunksProcessed;
            try
            {
                UploadHelper.Processing = true;
                UploadHelper.PostUpload("kill", new List<KillEntry>(GetKillEntries()
                    .Skip(chunksProcessed * chunkSize)));
                XIVDBViewModel.Instance.KillProcessed++;
            }
            catch (Exception ex)
            {
                UploadHelper.Processing = false;
            }
        }
    }
}
