// FFXIVAPP.Client
// LootWorkerDelegate.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Helpers.SocketIO;
using FFXIVAPP.Client.Models;
using FFXIVAPP.Client.ViewModels;
using FFXIVAPP.Client.Views;
using FFXIVAPP.Common.Helpers;

#endregion

namespace FFXIVAPP.Client.Delegates
{
    internal static class LootWorkerDelegate
    {
        #region Declarations

        public static readonly IList<LootEntry> LootList = new List<LootEntry>();
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
                LootList.Add(lootEntry);
                DispatcherHelper.Invoke(delegate
                {
                    AboutView.View.TotalLootLabel.Content = String.Format("Total Loot: {0}, Submitted: {1}", LootList.Count, UploadHelper.ChunksProcessed * UploadHelper.ChunkSize);
                });
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
                if (UploadHelper.Processing)
                {
                    return;
                }
                try
                {
                    UploadHelper.Processing = true;
                    //UploadHelper.EmitUpload("import_loot", new List<LootEntry>(LootList.ToList().Skip(chunksProcessed * chunkSize)));
                    UploadHelper.PostUpload("loot", new List<LootEntry>(LootList.ToList().Skip(chunksProcessed * chunkSize)));
                }
                catch (Exception ex)
                {
                }
            }, saveToDictionary);
        }
    }
}
