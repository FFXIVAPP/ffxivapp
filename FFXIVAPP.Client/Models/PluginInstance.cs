// FFXIVAPP.Client
// PluginInstance.cs
// 
// © 2013 Ryan Wilson

using FFXIVAPP.IPluginInterface;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Models
{
    [DoNotObfuscate]
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
