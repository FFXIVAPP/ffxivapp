// FFXIVAPP.Client
// WindowsMessageEvents.cs
// 
// © 2013 Ryan Wilson

using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Models
{
    [DoNotObfuscate]
    public static class WindowsMessageEvents
    {
        public const int KeyDown = 0x100;
        public const int KeyUp = 0x101;
        public const int Char = 0x102;
        public const int UniChar = 0x109;
        public const int Paste = 0x302;
        public const int SetText = 0x0C;
    }
}
