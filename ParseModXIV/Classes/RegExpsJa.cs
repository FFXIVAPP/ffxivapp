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

        public static readonly Regex Damage = new Regex(@"^(?<whoHit>.+)は(?<mob>.+)(?<didHit>に)((?<direction>.+)から「(?<ability>.+)」|「(?<ability>.+)」)　⇒　((?<crit>クリティカル！)　(?<amount>\d+)ダメージを与えた。$|(?<amount>\d+)ダメージを与えた。$)", DefaultOptions);

        public static readonly Regex Additional = new Regex(@"^追加効果.+(?<amount>\d+)ダメージ。$", DefaultOptions);

        public static readonly Regex DamageToPlayer = new Regex(@"^(?<whoHit>.+)は(?<player>.+)(?<didHit>に)((?<direction>.+)から「(?<ability>.+)」|「(?<ability>.+)」)　⇒　((?<crit>クリティカル！)　(?<amount>\d+)ダメージを与えた。$|(?<amount>\d+)ダメージを与えた。$)", DefaultOptions);

        public static readonly Regex Counter = new Regex(@"^placeholder$", DefaultOptions);

        //public static readonly Regex Food = new Regex(@"", DefaultOptions);

        //public static readonly Regex Recovers = new Regex(@"", DefaultOptions);

        //public static readonly Regex Engaged = new Regex(@"", DefaultOptions);

        public static readonly Regex Defeated = new Regex(@"^(?<whoDefeated>.+)は(?<whatDefeated>.+)を倒した。$", DefaultOptions);

        public static readonly Regex Obtains = new Regex(@"^(?<item>\d,\d+)ギルを得た。|「(?<item>.+)を入手した。$|^「(?<item>.+)が(?<who>.+)の戦利品目録に加わった。$", DefaultOptions);

        //public static readonly Regex Loot = new Regex(@"", DefaultOptions);

        public static readonly Regex Loses = new Regex(@"((?<monster>.+)) use(s)? (?<ability>[\w\s]+)? on (?<player>(\w+\s\w+)).+((?<loseGain>loses|gains) (?<amount>\d+) (?<type>\w+))\.", DefaultOptions);

        //public static readonly Regex UseOnMob = new Regex(@"", DefaultOptions);

        public static readonly Regex UseOnParty = new Regex(@"^(?<whoDid>.+)は(?<castOn>.+)に「(?<ability>.+)」　⇒　.+は(?<type>\w\w)を(?<amount>\d+)回復した。$", DefaultOptions);

        public static readonly Regex ResistsOrEvades = new Regex(@"^(?<whoEvaded>.+)は(?<mob>.+)(?<didHit>に)「(?<ability>.+)」　⇒　.+は(?<amount>\d+).+(?<resist>半減).+。$", DefaultOptions);

        //public static readonly Regex PlayerResistsOrEvades = new Regex(@"", DefaultOptions);

        //public static readonly Regex Parries = new Regex(@"", DefaultOptions);

        public static readonly Regex Blocks = new Regex(@"^placeholder$", DefaultOptions);

        //public static readonly Regex GrantsEffect = new Regex(@"", DefaultOptions);

        public static readonly Regex JoinParty = new Regex(@"^(?<whoJoined>.+)がパーティに参加しました。$", DefaultOptions);

        public static readonly Regex DisbandParty = new Regex(@"^パーティを解散しました。$", DefaultOptions);

        public static readonly Regex LeaveParty = new Regex(@"^(?<whoLeft>.+)はパーティから離脱しました。$", DefaultOptions);
    }
}
