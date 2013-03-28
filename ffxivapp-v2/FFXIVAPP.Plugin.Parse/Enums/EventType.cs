// FFXIVAPP.Plugin.Parse
// EventType.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

namespace FFXIVAPP.Plugin.Parse.Enums
{
    public enum EventType
    {
        Unknown = 0x000000,
        Damage = 0x002000,
        Failed = 0x004000,
        Actions = 0x008000,
        Items = 0x010000,
        Cure = 0x020000,
        Benficial = 0x040000,
        Detrimental = 0x080000,
        Chat = 0x100000
    }
}
