// FFXIVAPP.Client
// LootWorkerDelegate.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using FFXIVAPP.Client.Helpers.SocketIO;
using FFXIVAPP.Client.Models;
using FFXIVAPP.Client.ViewModels;
using FFXIVAPP.Common.Helpers;

#endregion

namespace FFXIVAPP.Client.Delegates
{
    internal static class KillWorkerDelegate
    {
        #region Declarations

        public static readonly List<KillEntry> KillList = new List<KillEntry>();
        private static readonly UploadHelper UploadHelper = new UploadHelper("import_kill", 10);

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
                if (!UploadHelper.Processing)
                {
                    UploadHelper.ProcessUpload(new List<KillEntry>(KillList.Skip(chunksProcessed * chunkSize)));
                }
            }, saveToDictionary);
        }
    }
}
