// FFXIVAPP.Client
// NPCWorkerDelegate.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using System.Linq;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Client.ViewModels;
using FFXIVAPP.Common.Core.Memory;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Delegates
{
    [DoNotObfuscate]
    internal static class NPCWorkerDelegate
    {
        #region Declarations

        public static IList<ActorEntity> NPCEntries = new List<ActorEntity>();

        public static readonly IList<ActorEntity> UniqueNPCEntries = new List<ActorEntity>();

        public static readonly UploadHelper UploadHelper = new UploadHelper(50);

        #endregion

        /// <summary>
        /// </summary>
        public static void ProcessUploads()
        {
            if (UploadHelper.Processing || !Settings.Default.AllowXIVDBIntegration || !Constants.IsOpen || !XIVDBViewModel.Instance.NPCUploadEnabled)
            {
                return;
            }
            var chunkSize = UploadHelper.ChunkSize;
            var chunksProcessed = UploadHelper.ChunksProcessed;
            try
            {
                UploadHelper.Processing = true;
                UploadHelper.PostUpload("npc", new List<ActorEntity>(UniqueNPCEntries.ToList()
                                                                                     .Skip(chunksProcessed * chunkSize)));
                XIVDBViewModel.Instance.NPCProcessed++;
            }
            catch (Exception ex)
            {
                UploadHelper.Processing = false;
            }
        }
    }
}
