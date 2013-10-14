// FFXIVAPP.Client
// KillWorkerDelegate.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Models;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Client.ViewModels;
using Newtonsoft.Json;

#endregion

namespace FFXIVAPP.Client.Delegates
{
    internal static class KillWorkerDelegate
    {
        #region Declarations

        public static readonly IList<KillEntry> KillEntries = new List<KillEntry>();

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
                    KillEntries.Add(killEntry);
                }
                XIVDBViewModel.Instance.KillSeen++;
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
                if (KillEntries.Count <= (chunkSize * (chunksProcessed + 1)))
                {
                    return;
                }
                try
                {
                    UploadHelper.Processing = true;
                    UploadHelper.PostUpload("kill", new List<KillEntry>(KillEntries.ToList()
                                                                                   .Skip(chunksProcessed * chunkSize)));
                    XIVDBViewModel.Instance.KillProcessed++;
                }
                catch (Exception ex)
                {
                    UploadHelper.Processing = false;
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
                UploadHelper.PostUpload("kill", new List<KillEntry>(KillEntries.ToList()
                                                                               .Skip(chunksProcessed * chunkSize)));
            }
            catch (Exception ex)
            {
            }
        }
    }
}
