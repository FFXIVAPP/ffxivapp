// FFXIVAPP
// AutomaticUpdates.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Net;
using HtmlAgilityPack;

#endregion

namespace FFXIVAPP.Classes
{
    public class AutomaticUpdates
    {
        public string CurrentVersion;
        private string _latestVersion;

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public bool CheckUpdates()
        {
            try
            {
                var req = (HttpWebRequest) WebRequest.Create("http://ffxiv-app.com/appv/?q=FFXIVAPP-GA");
                req.UserAgent = "Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_3; en-US) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.375.70 Safari/533.4";
                var response = (HttpWebResponse) req.GetResponse();
                var s = response.GetResponseStream();
                if (response.StatusCode != HttpStatusCode.OK || s == null) {}
                else
                {
                    var doc = new HtmlDocument();
                    doc.Load(s, true);
                    _latestVersion = doc.DocumentNode.SelectSingleNode("//span[@id='version']").InnerText;
                    _latestVersion = (String.IsNullOrWhiteSpace(_latestVersion)) ? "1.0.0.0" : _latestVersion;
                    s.Close();
                    response.Close();
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            if (!String.IsNullOrEmpty(_latestVersion))
            {
                var lver = _latestVersion.Split('.');
                var cver = CurrentVersion.Split('.');
                int lmajor, lminor, lbuild, lrevision;
                int cmajor, cminor, cbuild, crevision;
                try
                {
                    lmajor = Int32.Parse(lver[0]);
                    lminor = Int32.Parse(lver[1]);
                    lbuild = Int32.Parse(lver[2]);
                    lrevision = Int32.Parse(lver[3]);
                    cmajor = Int32.Parse(cver[0]);
                    cminor = Int32.Parse(cver[1]);
                    cbuild = Int32.Parse(cver[2]);
                    crevision = Int32.Parse(cver[3]);
                }
                catch
                {
                    return false;
                }
                if (lmajor <= cmajor)
                {
                    if (lminor <= cminor)
                    {
                        if (lbuild == cbuild)
                        {
                            return lrevision > crevision;
                        }
                        return lbuild > cbuild;
                    }
                    return true;
                }
                return true;
            }
            return false;
        }
    }
}
