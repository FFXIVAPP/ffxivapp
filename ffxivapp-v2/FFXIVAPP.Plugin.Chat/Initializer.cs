// FFXIVAPP.Plugin.Chat
// Initializer.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Xml.Linq;
using FFXIVAPP.Plugin.Chat.Properties;

#endregion

namespace FFXIVAPP.Plugin.Chat
{
    internal static class Initializer
    {
        #region Declarations

        #endregion

        /// <summary>
        /// </summary>
        public static void LoadSettings()
        {
            if (Constants.XSettings != null)
            {
                foreach (var xElement in Constants.XSettings.Descendants()
                                                  .Elements("Setting"))
                {
                    var xKey = (string) xElement.Attribute("Key");
                    var xValue = (string) xElement.Element("Value");
                    if (String.IsNullOrWhiteSpace(xKey) || String.IsNullOrWhiteSpace(xValue))
                    {
                        return;
                    }
                    Settings.SetValue(xKey, xValue);
                    if (!Constants.Settings.Contains(xKey))
                    {
                        Constants.Settings.Add(xKey);
                    }
                }
            }
        }
    }
}
