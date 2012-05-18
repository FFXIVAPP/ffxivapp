// ParseModXIV
// RegExpsEn.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System.Text.RegularExpressions;

namespace ParseModXIV.Classes
{
    public static class RegExpsJa
    {
        private const RegexOptions DefaultOptions = RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant;

        public static readonly Regex Damage = new Regex(@"^(?<whoHit>\w+\s\w+)[\u306F](?<mob>.+)[\u306B][\u300C][\u30B5](?<ability>.+)[\u21D2].+(?<amount>\d+)[\u30C0].+[\u3002]$", DefaultOptions);
        
        public static readonly Regex Additional = new Regex(@"^追加効果.+(?<amount>\d+)ダメージ。$", DefaultOptions);
    }
}
