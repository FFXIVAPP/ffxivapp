// FFXIVAPP.Client
// JsonPostHelper.cs
// 
// © 2013 Ryan Wilson

using System;
using System.IO;
using System.Net;
using FFXIVAPP.Common.Utilities;
using NLog;

namespace FFXIVAPP.Client.Helpers
{
    public static class JsonPostHelper
    {
        /// <summary>
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        public static void Post(string url, string postData)
        {
            var httpWebRequest = (HttpWebRequest) WebRequest.Create(url);
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "POST";
            try
            {
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(postData);
                    streamWriter.Flush();
                    streamWriter.Close();
                    var httpResponse = (HttpWebResponse) httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
            }
        }
    }
}
