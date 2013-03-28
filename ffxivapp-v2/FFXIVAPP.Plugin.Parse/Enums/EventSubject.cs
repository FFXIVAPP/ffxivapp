// FFXIVAPP.Plugin.Parse
// EventSubject.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

namespace FFXIVAPP.Plugin.Parse.Enums
{
    public enum EventSubject
    {
        Unknown = 0x000000,
        You = 0x000080,
        Party = 0x000100,
        Other = 0x000200,
        NPC = 0x000400,
        Engaged = 0x000800,
        UnEngaged = 0x001000
    }
}
