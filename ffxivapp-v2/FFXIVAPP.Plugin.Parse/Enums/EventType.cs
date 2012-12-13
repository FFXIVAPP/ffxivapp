// FFXIVAPP.Plugin.Parse
// EventType.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

namespace FFXIVAPP.Plugin.Parse.Enums
{
    public enum EventType
    {
        Unknown = 0x0000,
        Attack = 0x0040,
        Heal = 0x0080,
        Buff = 0x0100,
        Debuff = 0x0200,
        SkillPoints = 0x0400,
        Crafting = 0x0800,
        Gathering = 0x1000,
        Chat = 0x2000,
        Notice = 0x4000
    }
}
