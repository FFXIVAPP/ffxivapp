// FFXIVAPP.Client
// MonsterWorkerDelegate.cs
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
    public static class MonsterWorkerDelegate
    {
        #region Collection Access & Modification

        public static void AddNPCEntity(ActorEntity entity)
        {
            lock (_npcEntities)
            {
                _npcEntities.Add(entity);
            }
        }

        public static void ReplaceNPCEntities(IEnumerable<ActorEntity> entities)
        {
            lock (_npcEntities)
            {
                _npcEntities = new List<ActorEntity>(entities);
            }
        }

        public static IList<ActorEntity> GetNPCEntities()
        {
            lock (_npcEntities)
            {
                return new List<ActorEntity>(_npcEntities);
            }
        }

        public static void AddUniqueNPCEntity(ActorEntity entity)
        {
            lock (_uniqueNPCEntities)
            {
                _uniqueNPCEntities.Add(entity);
            }
        }

        public static void ReplaceUniqueNPCEntities(IEnumerable<ActorEntity> entities)
        {
            lock (_uniqueNPCEntities)
            {
                _uniqueNPCEntities = new List<ActorEntity>(entities);
            }
        }

        public static IList<ActorEntity> GetUniqueNPCEntities()
        {
            lock (_uniqueNPCEntities)
            {
                return new List<ActorEntity>(_uniqueNPCEntities);
            }
        }

        #endregion

        #region Declarations

        private static IList<ActorEntity> _npcEntities = new List<ActorEntity>();

        private static IList<ActorEntity> _uniqueNPCEntities = new List<ActorEntity>();

        public static readonly UploadHelper UploadHelper = new UploadHelper(100);

        #endregion

        /// <summary>
        /// </summary>
        public static void ProcessUploads()
        {
            if (UploadHelper.Processing || !Settings.Default.AllowXIVDBIntegration || !Constants.IsOpen || !XIVDBViewModel.Instance.MonsterUploadEnabled)
            {
                return;
            }
            var chunkSize = UploadHelper.ChunkSize;
            var chunksProcessed = UploadHelper.ChunksProcessed;
            try
            {
                UploadHelper.Processing = true;
                UploadHelper.PostUpload("mob", new List<ActorEntity>(GetUniqueNPCEntities()
                    .Skip(chunksProcessed * chunkSize)));
                XIVDBViewModel.Instance.MonsterProcessed++;
            }
            catch (Exception ex)
            {
                UploadHelper.Processing = false;
            }
        }
    }
}
