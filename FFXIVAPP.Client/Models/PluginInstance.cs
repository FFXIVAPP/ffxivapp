// FFXIVAPP.Client
// PluginInstance.cs
// 
// © 2013 Ryan Wilson

#region Usings

using FFXIVAPP.IPluginInterface;
using SmartAssembly.Attributes;

#endregion

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
