// FFXIVAPP.IPluginInterface
// IPlugin.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

#endregion

namespace FFXIVAPP.IPluginInterface
{
    /// <summary>
    /// </summary>
    public interface IPlugin
    {
        MessageBoxResult PopupResult { get; set; }
        IPluginHost Host { get; set; }
        Dictionary<string, string> Locale { get; set; }
        string Name { get; }
        string Icon { get; }
        string Description { get; }
        string Copyright { get; }
        string Version { get; }
        string Notice { get; }
        Exception Trace { get; }
        void Initialize();
        void Dispose();
        TabItem CreateTab();
        UserControl CreateControl();
        void OnNewLine(out bool success, params object[] entry);
        void SetConstants(ConstantsType type, object data);
    }
}
