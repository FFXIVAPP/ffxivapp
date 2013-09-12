// FFXIVAPP.Client
// UploadHelper.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using FFXIVAPP.Client.Memory;
using FFXIVAPP.Client.Models;
using FFXIVAPP.Client.Properties;
using Newtonsoft.Json;

namespace FFXIVAPP.Client.Helpers
{
    public class UploadHelper
    {
        #region Auto-Properties

        public int ChunkSize { get; set; }
        public int ChunksProcessed { get; set; }
        public bool Processing { get; set; }

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="chunkSize"></param>
        public UploadHelper(int chunkSize = 200)
        {
            ChunkSize = chunkSize;
            ChunksProcessed = 0;
            Processing = false;
        }

        /// <summary>
        /// </summary>
        /// <param name="postKey"></param>
        /// <param name="entries"></param>
        public void PostUpload(string postKey, object entries)
        {
            if (Settings.Default.AllowXIVDBIntegration)
            {
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
                var dataImport = new Dictionary<string, object>
                {
                    {
                        "data", data
                    },
                    {
                        "type", postKey
                    }
                };
                try
                {
                    var jsonSettings = new JsonSerializerSettings();
                    jsonSettings.StringEscapeHandling = StringEscapeHandling.EscapeNonAscii;
                    jsonSettings.Formatting = Formatting.None;
                    var postData = String.Format("jobj={0}", JsonConvert.SerializeObject(dataImport, jsonSettings));
                    var jsonResult = HttpPostHelper.Post("http://db.xivdev.com/modules/dataimporter/data_importer.php", HttpPostHelper.PostType.Form, postData);
                    var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResult)["result"];
                    switch (result)
                    {
                        case "success":
                            ChunksProcessed++;
                            break;
                        case "error":
                            break;
                    }
                    Processing = false;
                }
                catch (Exception ex)
                {
                    Processing = false;
                }
            }
        }
    }
}
