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
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Client.ViewModels;
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

        // FULL LIST
        public static IList<NPCEntry> NPCEntries = new List<NPCEntry>();

        public static NPCEntry CurrentUser;

        //UNIQUE LISTS
        public static readonly IList<NPCEntry> UniqueNPCEntries = new List<NPCEntry>();
        public static readonly IList<NPCEntry> UniquePlayerEntries = new List<NPCEntry>();

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
            NPCEntries = npcEntries;
            CurrentUser = npcEntries.First();
            Func<bool> saveToDictionary = delegate
            {
                try
                {
                    var monsters = npcEntries.Where(n => n.NPCType == NPCType.Monster && !Pets.Contains(n.ModelID));
                    var enumerable = UniqueNPCEntries.ToList();
                    foreach (var npcEntry in monsters)
                    {
                        var exists = enumerable.FirstOrDefault(n => n.ID == npcEntry.ID);
                        if (exists != null)
                        {
                            continue;
                        }
                        if (HttpPostHelper.IsValidJson(JsonConvert.SerializeObject(npcEntry)))
                        {
                            UniqueNPCEntries.Add(npcEntry);
                        }
                        XIVDBViewModel.Instance.MobSeen++;
                    }
                }
                catch (Exception ex)
                {
                }
                try
                {
                    var players = npcEntries.Where(n => n.NPCType == NPCType.PC);
                    var enumerable = UniquePlayerEntries.ToList();
                    foreach (var npcEntry in players)
                    {
                        var exists = enumerable.FirstOrDefault(n => String.Equals(n.Name, npcEntry.Name, StringComparison.CurrentCultureIgnoreCase));
                        if (exists != null)
                        {
                            continue;
                        }
                        UniquePlayerEntries.Add(npcEntry);
                        XIVDBViewModel.Instance.PlayerSeen++;
                    }
                }
                catch (Exception ex)
                {
                }
                return true;
            };
            saveToDictionary.BeginInvoke(delegate
            {
                if (UploadHelper.Processing || !Settings.Default.AllowXIVDBIntegration)
                {
                    return;
                }
                var chunkSize = UploadHelper.ChunkSize;
                var chunksProcessed = UploadHelper.ChunksProcessed;
                if (UniqueNPCEntries.Count <= (chunkSize * (chunksProcessed + 1)))
                {
                    return;
                }
                try
                {
                    UploadHelper.Processing = true;
                    UploadHelper.PostUpload("mob", new List<NPCEntry>(UniqueNPCEntries.ToList()
                                                                                      .Skip(chunksProcessed * chunkSize)));
                    XIVDBViewModel.Instance.MobProcessed++;
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
                UploadHelper.PostUpload("mob", new List<NPCEntry>(UniqueNPCEntries.ToList()
                                                                                  .Skip(chunksProcessed * chunkSize)));
            }
            catch (Exception ex)
            {
            }
        }
    }
}
