// FFXIVAPP.IPluginInterface
// IPlugin.cs
// 
// © 2013 Ryan Wilson

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
        string FriendlyName { get; }
        string Name { get; }
        string Icon { get; }
        string Description { get; }
        string Copyright { get; }
        string Version { get; }
        string Notice { get; }
        Exception Trace { get; }
        void Initialize();
        void Dispose(bool isUpdating = false);
        TabItem CreateTab();
        UserControl CreateControl();
        void OnNewLine(out bool success, params object[] entry);
        void SetConstants(ConstantsType type, object data);
    }
}
