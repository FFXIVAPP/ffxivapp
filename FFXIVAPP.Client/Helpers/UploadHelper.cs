// FFXIVAPP.Client
// UploadHelper.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using System.Web;
using FFXIVAPP.Client.Memory;
using FFXIVAPP.Client.Models;
using Newtonsoft.Json;

namespace FFXIVAPP.Client.Helpers
{
    public class UploadHelper
    {
        #region Auto-Properties

        public int ChunkSize { get; set; }
        public int ChunksProcessed { get; set; }
        public bool Processing { get; set; }
        public Dictionary<string, object> ImportData { get; set; }

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="chunkSize"></param>
        public UploadHelper(int chunkSize = 200)
        {
            ChunkSize = chunkSize;
            ChunksProcessed = 0;
            Processing = false;
            ImportData = new Dictionary<string, object>();
        }

        /// <summary>
        /// </summary>
        /// <param name="postKey"></param>
        /// <param name="entries"></param>
        public void PostUpload(string postKey, object entries)
        {
            ImportData.Clear();
            object data;
            switch (postKey)
            {
                case "npc":
                    data = entries as IList<NPCEntry>;
                    break;
                case "mob":
                    data = entries as IList<NPCEntry>;
                    break;
                case "kill":
                    data = entries as IList<KillEntry>;
                    break;
                case "loot":
                    data = entries as IList<LootEntry>;
                    break;
                default:
                    data = entries;
                    break;
            }
            ImportData.Add("data", data);
            ImportData.Add("type", postKey);
            ImportData.Add("version", AppViewModel.Instance.CurrentVersion);
            try
            {
                var jsonData = JsonConvert.SerializeObject(ImportData);
                var postData = String.Format("jobj={0}", HttpUtility.UrlEncode(jsonData));
                var jsonResult = HttpPostHelper.Post("http://db.xivdev.com/modules/dataimporter/data_importer.php", HttpPostHelper.PostType.Form, postData);
                var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResult)["result"];
                switch (result)
                {
                    case "success":
                        ChunksProcessed++;
                        break;
                    case "error":
                        ChunksProcessed++;
                        break;
                    default:
                        ChunksProcessed++;
                        break;
                }
                Processing = false;
            }
            catch (Exception ex)
            {
                ChunksProcessed++;
                Processing = false;
            }
        }
    }
}
