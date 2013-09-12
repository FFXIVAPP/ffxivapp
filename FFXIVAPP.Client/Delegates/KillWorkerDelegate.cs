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
using FFXIVAPP.Client.Views;
using FFXIVAPP.Common.Helpers;

#endregion

namespace FFXIVAPP.Client.Delegates
{
    internal static class KillWorkerDelegate
    {
        #region Declarations

        public static readonly IList<KillEntry> KillList = new List<KillEntry>();
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
                KillList.Add(killEntry);
                DispatcherHelper.Invoke(delegate { AboutView.View.TotalKillLabel.Content = String.Format("Total Kill: {0}, Submitted: {1}", KillList.Count, UploadHelper.ChunksProcessed * UploadHelper.ChunkSize); });
                return true;
            };
            saveToDictionary.BeginInvoke(delegate
            {
                var chunkSize = UploadHelper.ChunkSize;
                var chunksProcessed = UploadHelper.ChunksProcessed;
                if (KillList.Count <= (chunkSize * (chunksProcessed + 1)))
                {
                    return;
                }
                if (UploadHelper.Processing)
                {
                    return;
                }
                try
                {
                    UploadHelper.Processing = true;
                    UploadHelper.PostUpload("kill", new List<KillEntry>(KillList.ToList()
                                                                                .Skip(chunksProcessed * chunkSize)));
                }
                catch (Exception ex)
                {
                }
            }, saveToDictionary);
        }
    }
}
