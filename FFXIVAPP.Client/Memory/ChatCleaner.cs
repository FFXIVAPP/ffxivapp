// FFXIVAPP.Client
// ChatCleaner.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.RegularExpressions;
using FFXIVAPP.Common.Utilities;
using NLog;

#endregion

namespace FFXIVAPP.Client.Memory
{
    internal class ChatCleaner : INotifyPropertyChanged
    {
        #region Property Bindings

        private static bool _colorFound;
        private string _result;

        private bool ColorFound
        {
            get { return _colorFound; }
            set
            {
                _colorFound = value;
                RaisePropertyChanged();
            }
        }

        public string Result
        {
            get { return _result; }
            private set
            {
                _result = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Declarations

        private static readonly Regex Checks = new Regex(@"^00(20|21|23|27|28|46|47|48|49|5C)$", SharedRegEx.DefaultOptions);

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="line"></param>
        public ChatCleaner(string line)
        {
            Result = ProcessName(line);
        }

        /// <summary>
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="ci"></param>
        /// <param name="jp"></param>
        public ChatCleaner(byte[] bytes, CultureInfo ci, out bool jp)
        {
            Result = ProcessFullLine(bytes, ci, out jp)
                .Trim();
        }

        /// <summary>
        /// </summary>
        /// <param name="bytes"> </param>
        /// <param name="ci"> </param>
        /// <param name="jp"> </param>
        /// <returns> </returns>
        private string ProcessFullLine(byte[] bytes, CultureInfo ci, out bool jp)
        {
            jp = false;
            var line = HttpUtility.HtmlDecode(Encoding.UTF8.GetString(bytes.ToArray()))
                                  .Replace("  ", " ");
            try
            {
                var autoTranslateList = new List<byte>();
                var newList = new List<byte>();
                //var check = Encoding.UTF8.GetString(bytes.Take(4)
                //                                         .ToArray());
                for (var x = 0; x < bytes.Count(); x++)
                {
                    if (bytes[x] == 2)
                    {
                        var byteString = String.Format("{0}{1}{2}{3}", bytes[x], bytes[x + 1], bytes[x + 2], bytes[x + 3]);
                        switch (byteString)
                        {
                            case "22913":
                            case "21613":
                            case "22213":
                                x += 4;
                                break;
                        }
                    }
                    switch (bytes[x])
                    {
                        case 2:
                            //2 46 5 7 242 2 210 3
                            //2 29 1 3
                            var length = bytes[x + 2];
                            if (length > 1)
                            {
                                x = x + 3;
                                autoTranslateList.Add(Convert.ToByte('['));
                                while (bytes[x] != 3)
                                {
                                    autoTranslateList.AddRange(Encoding.UTF8.GetBytes(bytes[x].ToString("X2")));
                                    x++;
                                }
                                autoTranslateList.Add(Convert.ToByte(']'));
                                string aCheckStr;
                                var checkedAt = autoTranslateList.GetRange(1, autoTranslateList.Count - 1)
                                                                 .ToArray();
                                if (!Constants.AutoTranslate.TryGetValue(Encoding.UTF8.GetString(checkedAt), out aCheckStr))
                                {
                                    aCheckStr = "";
                                }
                                var atbyte = (!String.IsNullOrWhiteSpace(aCheckStr)) ? Encoding.UTF8.GetBytes(aCheckStr) : autoTranslateList.ToArray();
                                newList.AddRange(atbyte);
                                autoTranslateList.Clear();
                            }
                            else
                            {
                                x = x + 4;
                                newList.Add(32);
                                newList.Add(bytes[x]);
                            }
                            break;
                        default:
                            if (bytes[x] > 127)
                            {
                                jp = true;
                            }
                            newList.Add(bytes[x]);
                            break;
                    }
                }
                var jpc = (ci.TwoLetterISOLanguageName == "ja");
                var cleaned = jpc ? HttpUtility.HtmlDecode(Encoding.UTF8.GetString(bytes.ToArray()))
                                               .Replace("  ", " ") : HttpUtility.HtmlDecode(Encoding.UTF8.GetString(newList.ToArray()))
                                                                                .Replace("  ", " ");
                autoTranslateList.Clear();
                newList.Clear();
                cleaned = Regex.Replace(cleaned, @"", "⇒");
                cleaned = Regex.Replace(cleaned, @"", "[HQ]");
                cleaned = Regex.Replace(cleaned, @"\[01010101\]", "");
                cleaned = Regex.Replace(cleaned, @"\[CF010101\]", "");
                cleaned = Regex.Replace(cleaned, @"\[..FF\w{6}\]|\[EC\]", "");
                line = cleaned;
            }
            catch (Exception ex)
            {
                //Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
            }
            return line;
        }

        /// <summary>
        /// </summary>
        /// <param name="cleaned"></param>
        /// <returns></returns>
        private string ProcessName(string cleaned)
        {
            var line = cleaned;
            try
            {
                // cleanup name if using other settings
                var playerRegEx = new Regex(@"(?<full>\[[A-Z0-9]{10}(?<first>[A-Z0-9]{3,})20(?<last>[A-Z0-9]{3,})\](?<short>[\w']+\.? [\w']+\.?)\[[A-Z0-9]{12}\])", SharedRegEx.DefaultOptions);
                var playerMatch = playerRegEx.Match(line);
                if (playerMatch.Success)
                {
                    var fullName = playerMatch.Groups[1].Value;
                    var firstName = StringHelper.HexToString(playerMatch.Groups[2].Value);
                    var lastName = StringHelper.HexToString(playerMatch.Groups[3].Value);
                    var player = String.Format("{0} {1}", firstName, lastName);
                    // remove double placement
                    cleaned = line.Replace(String.Format("{0}:{1}", fullName, fullName), "•name•");
                    // remove single placement
                    cleaned = cleaned.Replace(fullName, "•name•");
                    switch (Regex.IsMatch(cleaned, @"^([Vv]ous|[Dd]u|[Yy]ou)"))
                    {
                        case true:
                            cleaned = cleaned.Substring(1)
                                             .Replace("•name•", "");
                            break;
                        case false:
                            cleaned = cleaned.Replace("•name•", player);
                            break;
                    }
                }
                cleaned = Regex.Replace(cleaned, @"[\r\n]+", "");
                cleaned = Regex.Replace(cleaned, @"[\x00-\x1F]+", "");
                line = cleaned;
            }
            catch (Exception ex)
            {
                //Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
            }
            return line;
        }

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion
    }
}
