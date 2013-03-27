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
        Damage = 0x000100,
        Failed = 0x000200,
        Actions = 0x000400,
        Items = 0x000800,
        Cure = 0x001000,
        Benficial = 0x002000,
        Detrimental = 0x004000,
        Chat = 0x008000
    }
}
