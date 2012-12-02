// FFXIVAPP
// GoogleTranslate.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

using System;
using System.Collections;
using System.Net;
using System.Text;
using FFXIVAPP.Classes.Helpers;
using FFXIVAPP.Properties;
using FFXIVAPP.Views;
using HtmlAgilityPack;

namespace FFXIVAPP.Classes
{
    internal static class GoogleTranslate
    {
        public static readonly Hashtable Offsets = GetLanguage();
        private static HttpWebRequest _httpWReq;
        private static HttpWebResponse _httpWResp;
        private static string _tempTranString;
        private static FlowDocHelper FD = new FlowDocHelper();
        private const string BaseURL = "http://translate.google.ca/translate_t?hl=&ie=UTF-8&text=";

        /// <summary>
        /// </summary>
        /// <param name="rMessage"> </param>
        /// <param name="mJp"> </param>
        public static void RetreiveLang(string rMessage, Boolean mJp)
        {
            var player = rMessage.Substring(0, rMessage.IndexOf(":", StringComparison.Ordinal)) + ": ";
            var tmpMessage = rMessage.Substring(rMessage.IndexOf(":", StringComparison.Ordinal) + 1);
            var mResults = Translate(tmpMessage, mJp);
            if (mResults.Length > 0 && rMessage != mResults)
            {
                FD.AppendFlow(player, mResults, "#EAFF00", MainWindow.View.LogView.Translated._FDR);
                if (Settings.Default.TranslateToEcho)
                {
                    var asc = Encoding.GetEncoding("utf-16");
                    KeyHelper.KeyPress(Keys.Escape);
                    KeyHelper.SendNotify(asc.GetBytes("/echo *** " + player + mResults));
                }
            }
        }

        #region Translation

        /// <summary>
        /// </summary>
        /// <param name="message"> </param>
        /// <param name="mJp"> </param>
        /// <returns> </returns>
        private static string Translate(string message, Boolean mJp)
        {
            _tempTranString = "";
            var jpOnly = Settings.Default.TranslateJPOnly;
            var o = Offsets[SettingsV.View.Log.TranslateTo.Text].ToString();
            if (jpOnly)
            {
                if (mJp)
                {
                    _tempTranString = TranslateText(message, "ja", o, true);
                    return _tempTranString;
                }
            }
            else
            {
                if (mJp)
                {
                    _tempTranString = TranslateText(message, "ja", o, true);
                    return _tempTranString;
                }
                _tempTranString = TranslateText(message, "en", o, false);
                return _tempTranString;
            }
            return "";
        }

        /// <summary>
        /// </summary>
        /// <param name="textToTranslate"> </param>
        /// <param name="lngInput"> </param>
        /// <param name="lngOutput"> </param>
        /// <param name="jpOnly"> </param>
        /// <returns> </returns>
        public static string TranslateText(string textToTranslate, string lngInput, string lngOutput, Boolean jpOnly)
        {
            var result = "";
            try
            {
                var url = String.Format("{0}{1}&sl={2}&tl={3}#", BaseURL, textToTranslate, lngInput, lngOutput);
                var bgl = string.Format("{0}{1}&sl=auto&tl={2}#auto|{2}|{1}", BaseURL, textToTranslate, lngOutput);
                if (jpOnly)
                {
                    _httpWReq = (HttpWebRequest) WebRequest.Create(url);
                }
                else
                {
                    _httpWReq = (HttpWebRequest) WebRequest.Create(bgl);
                }
                _httpWReq.UserAgent = "Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_3; en-US) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.375.70 Safari/533.4";
                _httpWResp = (HttpWebResponse) _httpWReq.GetResponse();
                var s = _httpWResp.GetResponseStream();
                if (_httpWResp.StatusCode != HttpStatusCode.OK || s == null)
                {
                }
                else
                {
                    var doc = new HtmlDocument();
                    doc.Load(s, true);
                    var translated = doc.DocumentNode.SelectSingleNode("//span[@id='result_box']");
                    if (translated != null)
                    {
                        result = translated.InnerText;
                    }
                    if (Settings.Default.Romanization)
                    {
                        var roman = doc.DocumentNode.SelectSingleNode("//div[@id='res-translit']");
                        if (roman != null)
                        {
                            result = roman.InnerText;
                        }
                    }
                }
            }
            catch
            {
                result = "";
            }
            return result;
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        private static Hashtable GetLanguage()
        {
            var offsets = new Hashtable {{"Albanian", "sq"}, {"Arabic", "ar"}, {"Bulgarian", "bg"}, {"Catalan", "ca"}, {"Chinese (Simplified)", "zh-CN"}, {"Chinese (Traditional)", "zh-TW"}, {"Croatian", "hr"}, {"Czech", "cs"}, {"Danish", "da"}, {"Dutch", "nl"}, {"English", "en"}, {"Estonian", "et"}, {"Filipino", "tl"}, {"Finnish", "fi"}, {"French", "fr"}, {"Galician", "gl"}, {"German", "de"}, {"Greek", "el"}, {"Hebrew", "iw"}, {"Hindi", "hi"}, {"Hungarian", "hu"}, {"Indonesian", "id"}, {"Italian", "it"}, {"Japanese", "ja"}, {"Korean", "ko"}, {"Latvian", "lv"}, {"Lithuanian", "lt"}, {"Maltese", "mt"}, {"Norwegian", "no"}, {"Polish", "pl"}, {"Portuguese", "pt"}, {"Romanian", "ro"}, {"Russian", "ru"}, {"Serbian", "sr"}, {"Slovak", "sk"}, {"Slovenian", "sl"}, {"Spanish", "es"}, {"Swedish", "sv"}, {"Thai", "th"}, {"Turkish", "tr"}, {"Ukrainian", "uk"}, {"Vietnamese", "vi"}};
            return offsets;
        }

        #endregion
    }
}