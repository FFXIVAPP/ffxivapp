// AppModXIV
// AutomaticUpdates.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;

namespace AppModXIV.Classes
{
    public class AutomaticUpdates
    {
        public string CurrentVersion;
        private string _latestVersion;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dllName"></param>
        /// <param name="ver"></param>
        public bool CheckDlls(string dllName, string ver)
        {
            CurrentVersion = ver;
            if (dllName == "AppModXIV")
            {
                CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
            return CheckUpdates(dllName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool CheckUpdates(string filename)
        {
            var lver = new string[] {};
            var cver = new string[] {};
            try
            {
                var req = (HttpWebRequest) WebRequest.Create(string.Format("http://ffxiv-app.com/appv/?q={0}", filename));
                var response = (HttpWebResponse) req.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var cu = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8"));
                    _latestVersion = cu.ReadLine();
                    cu.Close();
                    response.Close();
                }
            }
            catch (WebException)
            {
                return false;
            }
            if (!string.IsNullOrEmpty(_latestVersion))
            {
                lver = _latestVersion.Split('.');
                cver = CurrentVersion.Split('.');
            }
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
    }
}