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
    internal static class LootWorkerDelegate
    {
        #region Declarations

        public static readonly List<LootEntry> LootList = new List<LootEntry>();
        private static readonly UploadHelper UploadHelper = new UploadHelper("import_loot", 10);

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
                LootList.Add(lootEntry);
                return true;
            };
            saveToDictionary.BeginInvoke(delegate
            {
                var chunkSize = UploadHelper.ChunkSize;
                var chunksProcessed = UploadHelper.ChunksProcessed;
                if (LootList.Count <= (chunkSize * (chunksProcessed + 1)))
                {
                    return;
                }
                if (!UploadHelper.Processing)
                {
                    UploadHelper.ProcessUpload(new List<LootEntry>(LootList.Skip(chunksProcessed * chunkSize)));
                }
            }, saveToDictionary);
        }
    }
}
