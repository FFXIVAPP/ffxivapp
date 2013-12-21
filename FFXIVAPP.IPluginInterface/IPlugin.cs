// FFXIVAPP.IPluginInterface
// IPlugin.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;

namespace FFXIVAPP.IPluginInterface
{
    [InheritedExport(typeof (IPlugin))]
    public interface IPlugin
    {
        MessageBoxResult PopupResult { get; set; }
        Dictionary<string, string> Locale { get; set; }
        string FriendlyName { get; }
        string Name { get; }
        string Icon { get; }
        string Description { get; }
        string Copyright { get; }
        string Version { get; }
        string Notice { get; }
        Exception Trace { get; }
        void Initialize(IPluginHost pluginHost);
        void Dispose(bool isUpdating = false);
        TabItem CreateTab();
    }
}
