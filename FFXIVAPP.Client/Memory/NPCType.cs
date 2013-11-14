// FFXIVAPP.Client
// NPCType.cs
// 
// © 2013 Ryan Wilson

#region Usings

using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Memory
{
    [DoNotObfuscate]
    public enum NPCType
    {
        NPC,
        PC,
        Monster,
        Gathering
    }
}
