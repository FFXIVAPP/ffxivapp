// FFXIVAPP.Client
// XmlLogHelper.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FFXIVAPP.Client.Delegates;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.Models;
using FFXIVAPP.Common.Utilities;
using Newtonsoft.Json;
using NLog;

#endregion

namespace FFXIVAPP.Client.Helpers
{
    public static class XmlLogHelper
    {
        public static bool SaveCurrentLog(bool isTemporary = true)
        {
            ChatWorkerDelegate.IsPaused = true;
            if (NPCWorkerDelegate.MonsterList.Any())
            {
                var savedMonsterDatabase = DateTime.Now.ToString("yyyy_MM_dd_HH.mm.ss_") + "MonsterList.json";
                File.WriteAllText(AppViewModel.Instance.LogsPath + savedMonsterDatabase, JsonConvert.SerializeObject(NPCWorkerDelegate.MonsterList));
            }
            if (AppViewModel.Instance.ChatHistory.Any())
            {
                try
                {
                    var savedLogName = DateTime.Now.ToString("yyyy_MM_dd_HH.mm.ss_") + "ChatHistory.xml";
                    var savedLog = ResourceHelper.XDocResource(Common.Constants.AppPack + "Defaults/ChatHistory.xml");
                    foreach (var entry in AppViewModel.Instance.ChatHistory)
                    {
                        var xCode = entry.Code;
                        var xBytes = entry.Bytes.Aggregate("", (current, bytes) => current + (bytes + " "))
                                          .Trim();
                        //var xCombined = entry.Combined;
                        //var xJP = entry.JP.ToString();
                        var xLine = entry.Line;
                        //var xRaw = entry.Raw;
                        var xTimeStamp = entry.TimeStamp.ToString("[HH:mm:ss]");
                        var keyPairList = new List<XValuePair>();
                        keyPairList.Add(new XValuePair
                        {
                            Key = "Bytes",
                            Value = xBytes
                        });
                        //keyPairList.Add(new XValuePair {Key = "Combined", Value = xCombined});
                        //keyPairList.Add(new XValuePair {Key = "JP", Value = xJP});
                        keyPairList.Add(new XValuePair
                        {
                            Key = "Line",
                            Value = xLine
                        });
                        //keyPairList.Add(new XValuePair {Key = "Raw", Value = xRaw});
                        keyPairList.Add(new XValuePair
                        {
                            Key = "TimeStamp",
                            Value = xTimeStamp
                        });
                        XmlHelper.SaveXmlNode(savedLog, "History", "Entry", xCode, keyPairList);
                    }
                    savedLog.Save(AppViewModel.Instance.LogsPath + savedLogName);
                }
                catch (Exception ex)
                {
                    Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
                }
            }
            if (isTemporary)
            {
                AppViewModel.Instance.ChatHistory.Clear();
                ChatWorkerDelegate.IsPaused = false;
            }
            return true;
        }
    }
}
