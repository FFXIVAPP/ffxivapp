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
        Damage = 0x001000,
        Failed = 0x002000,
        Actions = 0x004000,
        Items = 0x008000,
        Cure = 0x010000,
        Benficial = 0x020000,
        Detrimental = 0x040000,
        Chat = 0x080000
    }
}
