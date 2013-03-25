// FFXIVAPP.Plugin.Parse
// EventSubject.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

namespace FFXIVAPP.Plugin.Parse.Enums
{
    public enum EventSubject
    {
        Unknown = 0x0000,
        You = 0x0004,
        Party = 0x0008,
        Other = 0x00010,
        Engaged = 0x0020,
        UnEngaged = 0x0040
    }
}
