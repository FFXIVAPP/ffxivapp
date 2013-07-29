// FFXIVAPP.Client
// ConstantsHelper.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using FFXIVAPP.Client.Models;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.IPluginInterface;
using NLog;

#endregion

namespace FFXIVAPP.Client.Helpers
{
    public static class ConstantsHelper
    {
        public static void UpdatePluginConstants(ConstantsType type, object data)
        {
            foreach (PluginInstance pluginInstance in App.Plugins.Loaded)
            {
                pluginInstance.Instance.SetConstants(type, data);
                Logging.Log(LogManager.GetCurrentClassLogger(), String.Format("ConstantsUpdated {0}:\n{1}", type, data));
            }
        }
    }
}
