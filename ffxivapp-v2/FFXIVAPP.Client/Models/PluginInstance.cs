// FFXIVAPP.Client
// PluginInstance.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

using FFXIVAPP.IPluginInterface;

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