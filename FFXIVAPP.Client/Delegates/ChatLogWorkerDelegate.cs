// FFXIVAPP.Client
// ChatLogWorkerDelegate.cs
// 
// © 2013 Ryan Wilson

#region Usings

using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Delegates
{
    [DoNotObfuscate]
    internal static class ChatLogWorkerDelegate
    {
        public static bool IsPaused { get; set; }
    }
}
