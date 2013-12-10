// FFXIVAPP.Client
// ChatLogWorkerDelegate.cs
// 
// © 2013 Ryan Wilson

using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Delegates
{
    [DoNotObfuscate]
    internal static class ChatLogWorkerDelegate
    {
        public static bool IsPaused { get; set; }
    }
}
