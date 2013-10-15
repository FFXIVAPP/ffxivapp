// FFXIVAPP.Client
// GoogleTranslate.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections;
using System.Net;
using System.Web;
using FFXIVAPP.Client.Plugins.Log.Views;
using FFXIVAPP.Client.Properties;
using HtmlAgilityPack;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Utilities
{
    [DoNotObfuscate]
    internal static class GoogleTranslate
    {
        public static readonly Hashtable Offsets = GetLanguage();
        private static string _baseUrl = "http://translate.google.ca/translate_t?hl=&ie=UTF-8&text=";
        private static HttpWebRequest _httpWReq;
        private static HttpWebResponse _httpWResp;

        /// <summary>
        /// </summary>
        /// <param name="line"> </param>
        /// <param name="jp"> </param>
        public static void RetreiveLang(string line, bool jp)
        {
            var timeStampColor = Settings.Default.TimeStampColor.ToString();
            var player = line.Substring(0, line.IndexOf(":", StringComparison.Ordinal)) + ": ";
            var tmpMessage = line.Substring(line.IndexOf(":", StringComparison.Ordinal) + 1);
            var result = Translate(tmpMessage, jp);
            if (result.Length <= 0 || line == result)
            {
                return;
            }
            Common.Constants.FD.AppendFlow(player, "", result, new[]
            {
                timeStampColor, "#EAFF00"
            }, MainView.View.TranslatedFD._FDR);
            if (SettingsProviders.Log.Settings.Default.SendToEcho)
            {
                //Plugin.PHost.Commands(Plugin.PName, new[]
                //{
                //    "/echo *** " + player + result
                //});
            }
        }

        #region Translation

        /// <summary>
        /// </summary>
        /// <param name="line"> </param>
        /// <param name="jp"> </param>
        /// <returns> </returns>
        private static string Translate(string line, bool jp)
        {
            var tempString = "";
            var jpOnly = SettingsProviders.Log.Settings.Default.TranslateJPOnly;
            var outLang = Offsets[SettingsProviders.Log.Settings.Default.TranslateTo].ToString();
            if (jpOnly)
            {
                if (jp)
                {
                    tempString = TranslateText(line, "ja", outLang, true);
                }
            }
            else
            {
                if (jp)
                {
                    tempString = TranslateText(line, "ja", outLang, true);
                    return tempString;
                }
                tempString = TranslateText(line, "en", outLang, false);
            }
            return HttpUtility.HtmlDecode(tempString);
        }

        /// <summary>
        /// </summary>
        /// <param name="textToTranslate"> </param>
        /// <param name="inLang"> </param>
        /// <param name="outLang"> </param>
        /// <param name="jpOnly"> </param>
        /// <returns> </returns>
        public static string TranslateText(string textToTranslate, string inLang, string outLang, bool jpOnly)
        {
            var result = "";
            try
            {
                if (jpOnly)
                {
                    var url = String.Format("{0}{1}&sl={2}&tl={3}#", _baseUrl, textToTranslate, inLang, outLang);
                    _httpWReq = (HttpWebRequest) WebRequest.Create(url);
                }
                else
                {
                    var url = String.Format("{0}{1}&sl=auto&tl={2}#auto|{2}|{1}", _baseUrl, textToTranslate, outLang);
                    _httpWReq = (HttpWebRequest) WebRequest.Create(url);
                }
                _httpWReq.UserAgent = "Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_3; en-US) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.375.70 Safari/533.4";
                _httpWResp = (HttpWebResponse) _httpWReq.GetResponse();
                var stream = _httpWResp.GetResponseStream();
                if (_httpWResp.StatusCode != HttpStatusCode.OK || stream == null)
                {
                }
                else
                {
                    var doc = new HtmlDocument();
                    doc.Load(stream, true);
                    var translated = doc.DocumentNode.SelectSingleNode("//span[@id='result_box']");
                    if (translated != null)
                    {
                        result = translated.InnerText;
                    }
                    if (SettingsProviders.Log.Settings.Default.SendRomanization)
                    {
                        var roman = doc.DocumentNode.SelectSingleNode("//div[@id='res-translit']");
                        if (roman != null)
                        {
                            result = roman.InnerText;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = "";
            }
            return HttpUtility.HtmlDecode(result);
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        private static Hashtable GetLanguage()
        {
            var offsets = new Hashtable
            {
                {
                    "Albanian", "sq"
                },
                {
                    "Arabic", "ar"
                },
                {
                    "Bulgarian", "bg"
                },
                {
                    "Catalan", "ca"
                },
                {
                    "Chinese (Simplified)", "zh-CN"
                },
                {
                    "Chinese (Traditional)", "zh-TW"
                },
                {
                    "Croatian", "hr"
                },
                {
                    "Czech", "cs"
                },
                {
                    "Danish", "da"
                },
                {
                    "Dutch", "nl"
                },
                {
                    "English", "en"
                },
                {
                    "Estonian", "et"
                },
                {
                    "Filipino", "tl"
                },
                {
                    "Finnish", "fi"
                },
                {
                    "French", "fr"
                },
                {
                    "Galician", "gl"
                },
                {
                    "German", "de"
                },
                {
                    "Greek", "el"
                },
                {
                    "Hebrew", "iw"
                },
                {
                    "Hindi", "hi"
                },
                {
                    "Hungarian", "hu"
                },
                {
                    "Indonesian", "id"
                },
                {
                    "Italian", "it"
                },
                {
                    "Japanese", "ja"
                },
                {
                    "Korean", "ko"
                },
                {
                    "Latvian", "lv"
                },
                {
                    "Lithuanian", "lt"
                },
                {
                    "Maltese", "mt"
                },
                {
                    "Norwegian", "no"
                },
                {
                    "Polish", "pl"
                },
                {
                    "Portuguese", "pt"
                },
                {
                    "Romanian", "ro"
                },
                {
                    "Russian", "ru"
                },
                {
                    "Serbian", "sr"
                },
                {
                    "Slovak", "sk"
                },
                {
                    "Slovenian", "sl"
                },
                {
                    "Spanish", "es"
                },
                {
                    "Swedish", "sv"
                },
                {
                    "Thai", "th"
                },
                {
                    "Turkish", "tr"
                },
                {
                    "Ukrainian", "uk"
                },
                {
                    "Vietnamese", "vi"
                }
            };
            return offsets;
        }

        #endregion
    }
}
