// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SavedlLogsHelper.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   SavedlLogsHelper.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.Helpers {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;

    using FFXIVAPP.Client.SettingsProviders.Application;
    using FFXIVAPP.Common.Helpers;
    using FFXIVAPP.Common.Models;
    using FFXIVAPP.Common.Utilities;

    using NLog;

    using Sharlayan.Core;

    internal static class SavedlLogsHelper {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static bool SaveCurrentLog(bool isTemporary = true) {
            if (!isTemporary) { }

            if (AppViewModel.Instance.ChatHistory.Any()) {
                try {
                    // setup common save logs info
                    var savedTextLogName = $"{DateTime.Now:yyyy_MM_dd_HH.mm.ss}.txt";
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
                    var savedLogName = $"{DateTime.Now:yyyy_MM_dd_HH.mm.ss}_ChatHistory.xml";
                    XDocument savedLog = ResourceHelper.XDocResource(Constants.AppPack + "Defaults/ChatHistory.xml");
                    foreach (ChatLogItem entry in AppViewModel.Instance.ChatHistory) {
                        // process text logging
                        try {
                            if (Settings.Default.SaveLog) {
                                switch (entry.Code) {
                                    case "000A":
                                        savedSayBuilder.AppendLine($"{entry.TimeStamp} {entry.Line}");
                                        break;
                                    case "000B":
                                        savedShoutBuilder.AppendLine($"{entry.TimeStamp} {entry.Line}");
                                        break;
                                    case "000E":
                                        savedPartyBuilder.AppendLine($"{entry.TimeStamp} {entry.Line}");
                                        break;
                                    case "000C":
                                    case "000D":
                                        savedTellBuilder.AppendLine($"{entry.TimeStamp} {entry.Line}");
                                        break;
                                    case "0010":
                                        savedLS1Builder.AppendLine($"{entry.TimeStamp} {entry.Line}");
                                        break;
                                    case "0011":
                                        savedLS2Builder.AppendLine($"{entry.TimeStamp} {entry.Line}");
                                        break;
                                    case "0012":
                                        savedLS3Builder.AppendLine($"{entry.TimeStamp} {entry.Line}");
                                        break;
                                    case "0013":
                                        savedLS4Builder.AppendLine($"{entry.TimeStamp} {entry.Line}");
                                        break;
                                    case "0014":
                                        savedLS5Builder.AppendLine($"{entry.TimeStamp} {entry.Line}");
                                        break;
                                    case "0015":
                                        savedLS6Builder.AppendLine($"{entry.TimeStamp} {entry.Line}");
                                        break;
                                    case "0016":
                                        savedLS7Builder.AppendLine($"{entry.TimeStamp} {entry.Line}");
                                        break;
                                    case "0017":
                                        savedLS8Builder.AppendLine($"{entry.TimeStamp} {entry.Line}");
                                        break;
                                    case "0018":
                                        savedFCBuilder.AppendLine($"{entry.TimeStamp} {entry.Line}");
                                        break;
                                    case "001E":
                                        savedYellBuilder.AppendLine($"{entry.TimeStamp} {entry.Line}");
                                        break;
                                }
                            }
                        }
                        catch (Exception ex) {
                            Logging.Log(Logger, new LogItem(ex, true));
                        }

                        // process xml log
                        try {
                            var xCode = entry.Code;
                            var xBytes = entry.Bytes.Aggregate(string.Empty, (current, bytes) => current + (bytes + " ")).Trim();

                            // var xCombined = entry.Combined;
                            // var xJP = entry.JP.ToString();
                            var xLine = entry.Line;

                            // var xRaw = entry.Raw;
                            var xTimeStamp = entry.TimeStamp.ToString("[HH:mm:ss]");
                            List<XValuePair> keyPairList = new List<XValuePair>();
                            keyPairList.Add(
                                new XValuePair {
                                    Key = "Bytes",
                                    Value = xBytes
                                });

                            // keyPairList.Add(new XValuePair {Key = "Combined", Value = xCombined});
                            // keyPairList.Add(new XValuePair {Key = "JP", Value = xJP});
                            keyPairList.Add(
                                new XValuePair {
                                    Key = "Line",
                                    Value = xLine
                                });

                            // keyPairList.Add(new XValuePair {Key = "Raw", Value = xRaw});
                            keyPairList.Add(
                                new XValuePair {
                                    Key = "TimeStamp",
                                    Value = xTimeStamp
                                });
                            XmlHelper.SaveXmlNode(savedLog, "History", "Entry", xCode, keyPairList);
                        }
                        catch (Exception ex) {
                            Logging.Log(Logger, new LogItem(ex, true));
                        }
                    }

                    // save text logs
                    if (Settings.Default.SaveLog) {
                        try {
                            if (savedSayBuilder.Length > 0) {
                                var path = Path.Combine(AppViewModel.Instance.LogsPath, "Say", savedTextLogName);
                                File.WriteAllText(path, savedSayBuilder.ToString());
                            }

                            if (savedShoutBuilder.Length > 0) {
                                var path = Path.Combine(AppViewModel.Instance.LogsPath, "Shout", savedTextLogName);
                                File.WriteAllText(path, savedShoutBuilder.ToString());
                            }

                            if (savedPartyBuilder.Length > 0) {
                                var path = Path.Combine(AppViewModel.Instance.LogsPath, "Party", savedTextLogName);
                                File.WriteAllText(path, savedPartyBuilder.ToString());
                            }

                            if (savedTellBuilder.Length > 0) {
                                var path = Path.Combine(AppViewModel.Instance.LogsPath, "Tell", savedTextLogName);
                                File.WriteAllText(path, savedTellBuilder.ToString());
                            }

                            if (savedLS1Builder.Length > 0) {
                                var path = Path.Combine(AppViewModel.Instance.LogsPath, "LS1", savedTextLogName);
                                File.WriteAllText(path, savedLS1Builder.ToString());
                            }

                            if (savedLS2Builder.Length > 0) {
                                var path = Path.Combine(AppViewModel.Instance.LogsPath, "LS2", savedTextLogName);
                                File.WriteAllText(path, savedLS2Builder.ToString());
                            }

                            if (savedLS3Builder.Length > 0) {
                                var path = Path.Combine(AppViewModel.Instance.LogsPath, "LS3", savedTextLogName);
                                File.WriteAllText(path, savedLS3Builder.ToString());
                            }

                            if (savedLS4Builder.Length > 0) {
                                var path = Path.Combine(AppViewModel.Instance.LogsPath, "LS4", savedTextLogName);
                                File.WriteAllText(path, savedLS4Builder.ToString());
                            }

                            if (savedLS5Builder.Length > 0) {
                                var path = Path.Combine(AppViewModel.Instance.LogsPath, "LS5", savedTextLogName);
                                File.WriteAllText(path, savedLS5Builder.ToString());
                            }

                            if (savedLS6Builder.Length > 0) {
                                var path = Path.Combine(AppViewModel.Instance.LogsPath, "LS6", savedTextLogName);
                                File.WriteAllText(path, savedLS6Builder.ToString());
                            }

                            if (savedLS7Builder.Length > 0) {
                                var path = Path.Combine(AppViewModel.Instance.LogsPath, "LS7", savedTextLogName);
                                File.WriteAllText(path, savedLS7Builder.ToString());
                            }

                            if (savedLS8Builder.Length > 0) {
                                var path = Path.Combine(AppViewModel.Instance.LogsPath, "LS8", savedTextLogName);
                                File.WriteAllText(path, savedLS8Builder.ToString());
                            }

                            if (savedFCBuilder.Length > 0) {
                                var path = Path.Combine(AppViewModel.Instance.LogsPath, "FC", savedTextLogName);
                                File.WriteAllText(path, savedFCBuilder.ToString());
                            }

                            if (savedYellBuilder.Length > 0) {
                                var path = Path.Combine(AppViewModel.Instance.LogsPath, "Yell", savedTextLogName);
                                File.WriteAllText(path, savedYellBuilder.ToString());
                            }
                        }
                        catch (Exception ex) {
                            Logging.Log(Logger, new LogItem(ex, true));
                        }
                    }

                    // save xml log
                    try {
                        savedLog.Save(Path.Combine(AppViewModel.Instance.LogsPath, savedLogName));
                    }
                    catch (Exception ex) {
                        Logging.Log(Logger, new LogItem(ex, true));
                    }
                }
                catch (Exception ex) {
                    Logging.Log(Logger, new LogItem(ex, true));
                }
            }

            if (!isTemporary) {
                return true;
            }

            AppViewModel.Instance.ChatHistory.Clear();
            return true;
        }
    }
}