// FFXIVAPP.Client
// MonsterWorkerDelegate.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Memory;
using FFXIVAPP.Client.Views;
using FFXIVAPP.Common.Helpers;
using Newtonsoft.Json;

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
        public static readonly IList<NPCEntry> NPCList = new List<NPCEntry>();
        public static readonly IList<NPCEntry> PlayerList = new List<NPCEntry>();
        private static readonly UploadHelper UploadHelper = new UploadHelper(100);

        #endregion

        /// <summary>
        /// </summary>
        public static void OnNewNPC(List<NPCEntry> npcEntries)
        {
            if (!npcEntries.Any())
            {
                return;
            }
            CurrentUser = CurrentUser ?? npcEntries.First();
            Func<bool> saveToDictionary = delegate
            {
                try
                {
                    var monsters = npcEntries.Where(n => n.NPCType == NPCType.Monster && !Pets.Contains(n.ModelID));
                    var enumerable = NPCList.ToList();
                    foreach (var npcEntry in monsters)
                    {
                        var exists = enumerable.FirstOrDefault(n => n.ID == npcEntry.ID);
                        if (exists != null)
                        {
                            continue;
                        }
                        if (HttpPostHelper.IsValidJson(JsonConvert.SerializeObject(npcEntry)))
                        {
                            NPCList.Add(npcEntry);
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                try
                {
                    var players = npcEntries.Where(n => n.NPCType == NPCType.PC);
                    var enumerable = PlayerList.ToList();
                    foreach (var npcEntry in players)
                    {
                        var exists = enumerable.FirstOrDefault(n => String.Equals(n.Name, npcEntry.Name, StringComparison.CurrentCultureIgnoreCase));
                        if (exists == null)
                        {
                            PlayerList.Add(npcEntry);
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                DispatcherHelper.Invoke(delegate
                {
                    AboutView.View.TotalPlayerLabel.Content = String.Format("Total Players: {0}", PlayerList.Count);
                    AboutView.View.TotalMobLabel.Content = String.Format("Total Mob: {0}, Submitted: {1}", NPCList.Count, UploadHelper.ChunksProcessed * UploadHelper.ChunkSize);
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
                    UploadHelper.PostUpload("mob", new List<NPCEntry>(NPCList.ToList()
                                                                             .Skip(chunksProcessed * chunkSize)));
                }
                catch (Exception ex)
                {
                    UploadHelper.Processing = false;
                }
            }, saveToDictionary);
        }
    }
}
