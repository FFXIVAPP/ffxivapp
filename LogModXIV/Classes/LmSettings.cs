// Project: LogModXIV
// File: LmSettings.cs
// 
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using AppModXIV.Classes;

namespace LogModXIV.Classes
{
    public static class LmSettings
    {
        private const String DefaultSettingsPath = "./Resources/Settings_Log.xml";
        private static readonly string[] RSettings = { "Tab", "Color" };
        public static readonly Dictionary<string, string> XColor = new Dictionary<string, string>();
        public static readonly Dictionary<string, string> XTab = new Dictionary<string, string>();
        private static XDocument _settingsXml;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        private static void LoadFromXml(String path)
        {
            _settingsXml = XDocument.Load(path);
            foreach (var t in RSettings)
            {
                LoadSettingsXml(t);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void LoadFromXml()
        {
            LoadFromXml(DefaultSettingsPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setting"></param>
        private static void LoadSettingsXml(string setting)
        {
            var items = from item in _settingsXml.Descendants(setting) select new XValuePairs { Key = (string) item.Attribute("Key"), Value = (string) item.Attribute("Value") };
            foreach (var item in items)
            {
                switch (setting)
                {
                    case "Tab":
                        if (item.Key != null && item.Value != null)
                        {
                            XTab.Add(item.Key, item.Value);
                        }
                        break;
                    case "Color":
                        if (item.Key != null && item.Value != null)
                        {
                            XColor.Add(item.Key, item.Value);
                        }
                        break;
                }
            }
        }
    }
}