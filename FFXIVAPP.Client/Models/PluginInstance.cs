// FFXIVAPP.Client
// PluginInstance.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using FFXIVAPP.IPluginInterface;

#endregion

namespace FFXIVAPP.Client.Models
{
    internal class PluginInstance
    {
        public PluginInstance()
        {
            AssemblyPath = "";
        }

        public IPlugin Instance { get; set; }
        public string AssemblyPath { get; set; }
    }
}
