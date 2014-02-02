// FFXIVAPP.Client
// PluginStatus.cs
// 
// © 2013 Ryan Wilson

using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Models
{
    [DoNotObfuscate]
    [DoNotObfuscateType]
    public enum PluginStatus
    {
        NotInstalled,
        Installed,
        UpdateAvailable
    }
}
