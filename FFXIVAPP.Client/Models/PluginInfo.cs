// FFXIVAPP.Client
// PluginInfo.cs
// 
// © 2013 Ryan Wilson

using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Models
{
    [DoNotObfuscate]
    public class PluginInfo
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public string Copyright { get; set; }
        public string Version { get; set; }
    }
}
