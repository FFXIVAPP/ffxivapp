// FFXIVAPP.Plugin.Parse
// EventType.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

namespace FFXIVAPP.Plugin.Parse.Enums
{
    public enum EventType
    {
        Unknown = 0x0000,
        Damage = 0x080,
        Failed = 0x00100,
        Actions = 0x0200,
        Items = 0x0400,
        Cure = 0x0800,
        Benficial = 0x1000,
        Detrimental = 0x2000,
        Chat = 0x4000
    }
}
