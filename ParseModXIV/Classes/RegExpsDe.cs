// ParseModXIV
// RegExpsJa.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System.Text.RegularExpressions;

namespace ParseModXIV.Classes
{
    public static class RegExpsDe
    {
        private const RegexOptions DefaultOptions = RegexOptions.Compiled | RegexOptions.ExplicitCapture;

        public static readonly Regex Damage = new Regex(@"^placeholder$", DefaultOptions);

        public static readonly Regex Additional = new Regex(@"^placeholder$", DefaultOptions);

        public static readonly Regex DamageToPlayer = new Regex(@"^placeholder$", DefaultOptions);

        public static readonly Regex Counter = new Regex(@"^placeholder$", DefaultOptions);

        //public static readonly Regex Food = new Regex(@"", DefaultOptions);

        //public static readonly Regex Recovers = new Regex(@"", DefaultOptions);

        //public static readonly Regex Engaged = new Regex(@"", DefaultOptions);

        public static readonly Regex Defeated = new Regex(@"^placeholder$", DefaultOptions);

        public static readonly Regex Obtains = new Regex(@"^placeholder$", DefaultOptions);

        //public static readonly Regex Loot = new Regex(@"", DefaultOptions);

        public static readonly Regex Loses = new Regex(@"^placeholder$", DefaultOptions);

        //public static readonly Regex UseOnMob = new Regex(@"", DefaultOptions);

        public static readonly Regex UseOnParty = new Regex(@"^placeholder$", DefaultOptions);

        public static readonly Regex Resists = new Regex(@"^placeholder$", DefaultOptions);

        //public static readonly Regex PlayerResistsOrEvades = new Regex(@"", DefaultOptions);

        //public static readonly Regex Parries = new Regex(@"", DefaultOptions);

        public static readonly Regex Blocks = new Regex(@"^placeholder$", DefaultOptions);

        //public static readonly Regex GrantsEffect = new Regex(@"", DefaultOptions);

        public static readonly Regex JoinParty = new Regex(@"^placeholder$", DefaultOptions);

        public static readonly Regex DisbandParty = new Regex(@"^placeholder$", DefaultOptions);

        public static readonly Regex LeaveParty = new Regex(@"^placeholder$", DefaultOptions);
    }
}