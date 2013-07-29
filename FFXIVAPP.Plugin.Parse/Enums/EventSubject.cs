// FFXIVAPP.Plugin.Parse
// EventSubject.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

namespace FFXIVAPP.Plugin.Parse.Enums
{
    public enum EventSubject
    {
        Unknown = 0x00000000,
        You = 0x00000080,
        Party = 0x00000100,
        Other = 0x00000200,
        NPC = 0x00000400,
        Engaged = 0x00000800,
        UnEngaged = 0x00001000
    }
}
