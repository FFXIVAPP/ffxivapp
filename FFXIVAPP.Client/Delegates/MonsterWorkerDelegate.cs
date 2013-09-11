    // FFXIVAPP.Client
// NPCWorkerDelegate.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using FFXIVAPP.Client.Helpers.SocketIO;
using FFXIVAPP.Client.Memory;
using FFXIVAPP.Client.ViewModels;
using FFXIVAPP.Common.Helpers;

#endregion

namespace FFXIVAPP.Client.Delegates
{
    internal static class MonsterWorkerDelegate
    {
        #region Property Backings

        private static List<uint> _pets;

        public static List<uint> Pets
        {
            get
            {
                return _pets ?? (_pets = new List<uint>
                {
                    1398,
                    1399,
                    1400,
                    1401,
                    1402,
                    1403,
                    1404,
                    2095
                });
            }
        }

        #endregion

        #region Declarations

        public static NPCEntry CurrentUser;
        public static readonly List<NPCEntry> NPCList = new List<NPCEntry>();
        private static readonly UploadHelper UploadHelper = new UploadHelper("import_mob");

        #endregion

        /// <summary>
        /// </summary>
        public static void OnNewNPC(NPCEntry npcEntry)
        {
            if (CurrentUser == null)
            {
                CurrentUser = npcEntry;
                return;
            }
            Func<bool> saveToDictionary = delegate
            {
                var current = NPCList.Any() ? NPCList.ToList() : new List<NPCEntry>();
                if (current.Any(n => n.ID == npcEntry.ID) || Pets.Contains(npcEntry.ModelID) || npcEntry.NPCType != NPCType.Monster)
                {
                    return false;
                }
                NPCList.Add(npcEntry);
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
                if (!UploadHelper.Processing)
                {
                    UploadHelper.ProcessUpload(new List<NPCEntry>(NPCList.Skip(chunksProcessed * chunkSize)));
                }
            }, saveToDictionary);
        }
    }
}
