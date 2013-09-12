// FFXIVAPP.Client
// NPCWorkerDelegate.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Helpers.SocketIO;
using FFXIVAPP.Client.Memory;
using FFXIVAPP.Client.ViewModels;
using FFXIVAPP.Client.Views;
using FFXIVAPP.Common.Helpers;
using SocketIOClient.Messages;

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
                        var exists = enumerable.FirstOrDefault(n => n.ID == npcEntry.ID);
                        if (exists == null)
                        {
                            NPCList.Add(npcEntry);
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                DispatcherHelper.Invoke(delegate
                {
                    AboutView.View.TotalNPCLabel.Content = String.Format("Total NPC: {0}, Submitted: {1}", NPCList.Count, UploadHelper.ChunksProcessed * UploadHelper.ChunkSize);
                });
                return true;
            };
            saveToDictionary.BeginInvoke(delegate
            {
                var chunkSize = UploadHelper.ChunkSize;
                var chunksProcessed = UploadHelper.ChunksProcessed;
                if (NPCList.Count <= (chunkSize * (chunksProcessed + 1)))
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
                    //UploadHelper.EmitUpload("import_npc", new List<NPCEntry>(NPCList.ToList().Skip(chunksProcessed * chunkSize)));
                    UploadHelper.PostUpload("npc", new List<NPCEntry>(NPCList.ToList().Skip(chunksProcessed * chunkSize)));
                }
                catch (Exception ex)
                {
                }
            }, saveToDictionary);
        }
    }
}
