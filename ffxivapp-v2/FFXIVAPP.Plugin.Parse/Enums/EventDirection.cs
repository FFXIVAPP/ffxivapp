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
        Party = 0x000002,
        Other = 0x000004,
        NPC = 0x000008,
        Engaged = 0x000010,
        UnEngaged = 0x000020
    }
}
