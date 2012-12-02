// FFXIVAPP.IPluginInterface
// IPlugin.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

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
        void OnNewLine(out bool success, params object[] entry);
    }
}