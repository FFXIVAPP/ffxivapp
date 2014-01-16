// FFXIVAPP.Client
// XOverTimeAction.cs
// 
// © 2013 Ryan Wilson

using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Models.Parse
{
    [DoNotObfuscate]
    public class XOverTimeAction
    {
        public int ActionPotency { get; set; }
        public int ActionOverTimePotency { get; set; }
        public int Duration { get; set; }
        public bool HasNoInitialResult { get; set; }
    }
}
