// FFXIVAPP.Plugin.Parse
// DamageOverTimeAction.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

namespace FFXIVAPP.Plugin.Parse.Models
{
    public class DamageOverTimeAction
    {
        public string Name { get; set; }
        public int ActionPotency { get; set; }
        public int DoTPotency { get; set; }
        public int Duration { get; set; }
    }
}
