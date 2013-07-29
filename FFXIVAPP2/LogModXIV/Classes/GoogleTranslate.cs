// LogModXIV
// GoogleTranslate.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using AppModXIV.Classes;
using LogModXIV.View;

namespace LogModXIV.Classes
{
    internal static class GoogleTranslate
    {
        public static readonly Hashtable Offsets = GetLanguage();
        private static HttpWebRequest _httpWReq;
        private static HttpWebResponse _httpWResp;
        private static Encoding _resEncoding;
        private static string _tempTranString;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rMessage"></param>
        /// <param name="jpOnly"></param>
        /// <param name="jp"></param>
        public static void RetreiveLang(string rMessage, Boolean jpOnly, Boolean jp)
        {
            var player = rMessage.Substring(0, rMessage.IndexOf(":", StringComparison.Ordinal)) + ": ";
            var tmpMessage = rMessage.Substring(rMessage.IndexOf(":", StringComparison.Ordinal) + 1);
            var mResults = Translate(tmpMessage, jpOnly, jp);
            if (mResults.Length > 0 && rMessage != mResults)
            {
                ChatWorkerDelegate.AppendFlow(player, mResults, "#eaff00", MainTabControlView.View.Translated_FDR);
                if (Settings.Default.Gui_TranslateToEcho)
                {
                    var asc = Encoding.ASCII;
                    KeyHelper.SendNotify(asc.GetBytes("/echo *** " + player + mResults));
                }
            }
        }

        #region " TRANSLATION "

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="jpOnly"></param>
        /// <param name="jp"></param>
        /// <returns></returns>
        private static string Translate(string message, Boolean jpOnly, Boolean jp)
        {
            _tempTranString = "";
            if (jpOnly)
            {
                if (jp)
                {
                    _tempTranString = TranslateText(message, "ja", Offsets[MainMenuView.View.Gui_TranslateTo.Text].ToString(), Settings.Default.Gui_TranslateJPOnly, false);
                    return _tempTranString;
                }
            }
            else
            {
                if (jp)
                {
                    _tempTranString = TranslateText(message, "ja", Offsets[MainMenuView.View.Gui_TranslateTo.Text].ToString(), true, false);
                    return _tempTranString;
                }
                _tempTranString = TranslateText(message, "en", Offsets[MainMenuView.View.Gui_TranslateTo.Text].ToString(), Settings.Default.Gui_TranslateJPOnly, false);
                return _tempTranString;
            }
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textToTranslate"></param>
        /// <param name="lngInput"></param>
        /// <param name="lngOutput"></param>
        /// <param name="jpOnly"></param>
        /// <param name="romaji"></param>
        /// <returns></returns>
        public static string TranslateText(string textToTranslate, string lngInput, string lngOutput, Boolean jpOnly, Boolean romaji)
        {
            string result;
            var roman = string.Empty;
            try
            {
                var url = String.Format("http://translate.google.ca/translate_t?hl=&ie=UTF-8&text={0}&sl={1}&tl={2}#", textToTranslate, lngInput, lngOutput);
                var bgl = string.Format("http://translate.google.ca/translate_t?hl=&ie=UTF-8&text={0}&sl=auto&tl={1}#auto|{1}|{0}", textToTranslate, lngOutput);
                if (jpOnly)
                {
                    _httpWReq = (HttpWebRequest) WebRequest.Create(url);
                }
                else
                {
                    _httpWReq = (HttpWebRequest) WebRequest.Create(bgl);
                }
                _httpWResp = (HttpWebResponse) _httpWReq.GetResponse();
                _resEncoding = Encoding.GetEncoding(_httpWResp.CharacterSet);
                var sr = new StreamReader(_httpWResp.GetResponseStream(), _resEncoding);
                var textResponse = sr.ReadToEnd();
                sr.Close();
                sr.Dispose();
                _httpWResp.Close();
                var source = textResponse;
                const string startStr = "id=result_box";
                const string endStr = "</span></span>";
                var i = source.IndexOf(startStr, StringComparison.Ordinal) + 20;
                result = source.Substring(i);
                var f = result.IndexOf(endStr, StringComparison.Ordinal);
                var f2 = f + 28;
                if (result.Substring(f, f2).Contains("<div id="))
                {
                    const string endString = "</div><div id=gt-res-dict";
                    var y = result.IndexOf("translit", StringComparison.Ordinal);

                    var x = result.Substring(y).IndexOf(endString, StringComparison.Ordinal);
                    roman = result.Substring(y, x);
                    roman = SMid(roman, roman.IndexOf(">", StringComparison.Ordinal) + 1);
                }
                result = result.Substring(0, f);
                result = result.Replace("</span>", "•");
                result = result.Replace("><span", "•");
                var newResult = result.Split('•');
                result = "";
                for (i = 0; i <= newResult.Length - 1; i++)
                {
                    if (string.IsNullOrEmpty(newResult[i]))
                    {
                        continue;
                    }
                    if (newResult[i].Contains(">"))
                    {
                        var tmpResult = newResult[i];
                        var tmpLength = newResult[i].IndexOf(">", StringComparison.Ordinal) + 1;
                        result += tmpResult.Substring(tmpLength);
                    }
                }
                _httpWReq = null;
                _resEncoding = null;
                _httpWResp = null;
            }
            catch
            {
                result = null;
                roman = null;
            }
            result = romaji ? HttpUtility.HtmlDecode(!string.IsNullOrEmpty(roman) ? roman : result) : HttpUtility.HtmlDecode(result);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static Hashtable GetLanguage()
        {
            var offsets = new Hashtable {{"Albanian", "sq"}, {"Arabic", "ar"}, {"Bulgarian", "bg"}, {"Catalan", "ca"}, {"Chinese (Simplified)", "zh-CN"}, {"Chinese (Traditional)", "zh-TW"}, {"Croatian", "hr"}, {"Czech", "cs"}, {"Danish", "da"}, {"Dutch", "nl"}, {"English", "en"}, {"Estonian", "et"}, {"Filipino", "tl"}, {"Finnish", "fi"}, {"French", "fr"}, {"Galician", "gl"}, {"German", "de"}, {"Greek", "el"}, {"Hebrew", "iw"}, {"Hindi", "hi"}, {"Hungarian", "hu"}, {"Indonesian", "id"}, {"Italian", "it"}, {"Japanese", "ja"}, {"Korean", "ko"}, {"Latvian", "lv"}, {"Lithuanian", "lt"}, {"Maltese", "mt"}, {"Norwegian", "no"}, {"Polish", "pl"}, {"Portuguese", "pt"}, {"Romanian", "ro"}, {"Russian", "ru"}, {"Serbian", "sr"}, {"Slovak", "sk"}, {"Slovenian", "sl"}, {"Spanish", "es"}, {"Swedish", "sv"}, {"Thai", "th"}, {"Turkish", "tr"}, {"Ukrainian", "uk"}, {"Vietnamese", "vi"}};
            return offsets;
        }

        #endregion

        #region " STRING CONTROLS "

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        private static string SMid(string param, int startIndex)
        {
            return param.Substring(startIndex);
        }

        #endregion
    }
}