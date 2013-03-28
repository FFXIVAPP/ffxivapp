// FFXIVAPP.Plugin.Parse
// EventDirection.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

namespace FFXIVAPP.Plugin.Parse.Enums
{
    public enum EventDirection
    {
        Unknown = 0x000000,
        Self = 0x000001,
        You = 0x000002,
        Party = 0x000004,
        Other = 0x000008,
        NPC = 0x000010,
        Engaged = 0x000020,
        UnEngaged = 0x000040
    }
}
