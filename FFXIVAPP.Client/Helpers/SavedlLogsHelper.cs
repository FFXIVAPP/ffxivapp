// FFXIVAPP.Client
// SavedlLogsHelper.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FFXIVAPP.Client.Delegates;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.Models;
using FFXIVAPP.Common.Utilities;
using NLog;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Helpers
{
    public static class SavedlLogsHelper
    {
        [DoNotObfuscate]
        public static bool SaveCurrentLog(bool isTemporary = true)
        {
            ChatLogWorkerDelegate.IsPaused = true;
            if (!isTemporary)
            {
            }
            if (AppViewModel.Instance.ChatHistory.Any())
            {
                try
                {
                    // setup common save logs info
                    var savedTextLogName = String.Format("{0}.txt", DateTime.Now.ToString("yyyy_MM_dd_HH.mm.ss"));
                    var savedSayBuilder = new StringBuilder();
                    var savedShoutBuilder = new StringBuilder();
                    var savedPartyBuilder = new StringBuilder();
                    var savedTellBuilder = new StringBuilder();
                    var savedLS1Builder = new StringBuilder();
                    var savedLS2Builder = new StringBuilder();
                    var savedLS3Builder = new StringBuilder();
                    var savedLS4Builder = new StringBuilder();
                    var savedLS5Builder = new StringBuilder();
                    var savedLS6Builder = new StringBuilder();
                    var savedLS7Builder = new StringBuilder();
                    var savedLS8Builder = new StringBuilder();
                    var savedFCBuilder = new StringBuilder();
                    var savedYellBuilder = new StringBuilder();
                    // setup full chatlog xml file
                    var savedLogName = String.Format("{0}_ChatHistory.xml", DateTime.Now.ToString("yyyy_MM_dd_HH.mm.ss"));
                    var savedLog = ResourceHelper.XDocResource(Common.Constants.AppPack + "Defaults/ChatHistory.xml");
                    foreach (var entry in AppViewModel.Instance.ChatHistory)
                    {
                        // process text logging
                        try
                        {
                            if (Settings.Default.SaveLog)
                            {
                                switch (entry.Code)
                                {
                                    case "000A":
                                        savedSayBuilder.AppendLine(string.Format("{0} {1}", entry.TimeStamp, entry.Line));
                                        break;
                                    case "000B":
                                        savedShoutBuilder.AppendLine(string.Format("{0} {1}", entry.TimeStamp, entry.Line));
                                        break;
                                    case "000E":
                                        savedPartyBuilder.AppendLine(string.Format("{0} {1}", entry.TimeStamp, entry.Line));
                                        break;
                                    case "000C":
                                    case "000D":
                                        savedTellBuilder.AppendLine(string.Format("{0} {1}", entry.TimeStamp, entry.Line));
                                        break;
                                    case "0010":
                                        savedLS1Builder.AppendLine(string.Format("{0} {1}", entry.TimeStamp, entry.Line));
                                        break;
                                    case "0011":
                                        savedLS2Builder.AppendLine(string.Format("{0} {1}", entry.TimeStamp, entry.Line));
                                        break;
                                    case "0012":
                                        savedLS3Builder.AppendLine(string.Format("{0} {1}", entry.TimeStamp, entry.Line));
                                        break;
                                    case "0013":
                                        savedLS4Builder.AppendLine(string.Format("{0} {1}", entry.TimeStamp, entry.Line));
                                        break;
                                    case "0014":
                                        savedLS5Builder.AppendLine(string.Format("{0} {1}", entry.TimeStamp, entry.Line));
                                        break;
                                    case "0015":
                                        savedLS6Builder.AppendLine(string.Format("{0} {1}", entry.TimeStamp, entry.Line));
                                        break;
                                    case "0016":
                                        savedLS7Builder.AppendLine(string.Format("{0} {1}", entry.TimeStamp, entry.Line));
                                        break;
                                    case "0017":
                                        savedLS8Builder.AppendLine(string.Format("{0} {1}", entry.TimeStamp, entry.Line));
                                        break;
                                    case "0018":
                                        savedFCBuilder.AppendLine(string.Format("{0} {1}", entry.TimeStamp, entry.Line));
                                        break;
                                    case "001E":
                                        savedYellBuilder.AppendLine(string.Format("{0} {1}", entry.TimeStamp, entry.Line));
                                        break;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        // process xml log
                        try
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
                        catch (Exception ex)
                        {
                        }
                    }
                    // save text logs
                    if (Settings.Default.SaveLog)
                    {
                        try
                        {
                            if (savedSayBuilder.Length > 0)
                            {
                                var path = String.Format("{0}Say\\{1}", AppViewModel.Instance.LogsPath, savedTextLogName);
                                File.WriteAllText(path, savedSayBuilder.ToString());
                            }
                            if (savedShoutBuilder.Length > 0)
                            {
                                var path = String.Format("{0}Shout\\{1}", AppViewModel.Instance.LogsPath, savedTextLogName);
                                File.WriteAllText(path, savedShoutBuilder.ToString());
                            }
                            if (savedPartyBuilder.Length > 0)
                            {
                                var path = String.Format("{0}Party\\{1}", AppViewModel.Instance.LogsPath, savedTextLogName);
                                File.WriteAllText(path, savedPartyBuilder.ToString());
                            }
                            if (savedTellBuilder.Length > 0)
                            {
                                var path = String.Format("{0}Tell\\{1}", AppViewModel.Instance.LogsPath, savedTextLogName);
                                File.WriteAllText(path, savedTellBuilder.ToString());
                            }
                            if (savedLS1Builder.Length > 0)
                            {
                                var path = String.Format("{0}LS1\\{1}", AppViewModel.Instance.LogsPath, savedTextLogName);
                                File.WriteAllText(path, savedLS1Builder.ToString());
                            }
                            if (savedLS2Builder.Length > 0)
                            {
                                var path = String.Format("{0}LS2\\{1}", AppViewModel.Instance.LogsPath, savedTextLogName);
                                File.WriteAllText(path, savedLS2Builder.ToString());
                            }
                            if (savedLS3Builder.Length > 0)
                            {
                                var path = String.Format("{0}LS3\\{1}", AppViewModel.Instance.LogsPath, savedTextLogName);
                                File.WriteAllText(path, savedLS3Builder.ToString());
                            }
                            if (savedLS4Builder.Length > 0)
                            {
                                var path = String.Format("{0}LS4\\{1}", AppViewModel.Instance.LogsPath, savedTextLogName);
                                File.WriteAllText(path, savedLS4Builder.ToString());
                            }
                            if (savedLS5Builder.Length > 0)
                            {
                                var path = String.Format("{0}LS5\\{1}", AppViewModel.Instance.LogsPath, savedTextLogName);
                                File.WriteAllText(path, savedLS5Builder.ToString());
                            }
                            if (savedLS6Builder.Length > 0)
                            {
                                var path = String.Format("{0}LS6\\{1}", AppViewModel.Instance.LogsPath, savedTextLogName);
                                File.WriteAllText(path, savedLS6Builder.ToString());
                            }
                            if (savedLS7Builder.Length > 0)
                            {
                                var path = String.Format("{0}LS7\\{1}", AppViewModel.Instance.LogsPath, savedTextLogName);
                                File.WriteAllText(path, savedLS7Builder.ToString());
                            }
                            if (savedLS8Builder.Length > 0)
                            {
                                var path = String.Format("{0}LS8\\{1}", AppViewModel.Instance.LogsPath, savedTextLogName);
                                File.WriteAllText(path, savedLS8Builder.ToString());
                            }
                            if (savedFCBuilder.Length > 0)
                            {
                                var path = String.Format("{0}FC\\{1}", AppViewModel.Instance.LogsPath, savedTextLogName);
                                File.WriteAllText(path, savedFCBuilder.ToString());
                            }
                            if (savedYellBuilder.Length > 0)
                            {
                                var path = String.Format("{0}Yell\\{1}", AppViewModel.Instance.LogsPath, savedTextLogName);
                                File.WriteAllText(path, savedYellBuilder.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    // save xml log
                    try
                    {
                        savedLog.Save(AppViewModel.Instance.LogsPath + savedLogName);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                catch (Exception ex)
                {
                    Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
                }
            }
            if (!isTemporary)
            {
                return true;
            }
            AppViewModel.Instance.ChatHistory.Clear();
            ChatLogWorkerDelegate.IsPaused = false;
            return true;
        }
    }
}
