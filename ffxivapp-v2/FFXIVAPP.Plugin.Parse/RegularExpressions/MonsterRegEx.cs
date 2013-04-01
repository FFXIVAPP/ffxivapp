// FFXIVAPP.Plugin.Parse
// MonsterRegEx.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System.Text.RegularExpressions;
using FFXIVAPP.Common.RegularExpressions;

#endregion

namespace FFXIVAPP.Plugin.Parse.RegularExpressions
{
    internal static class MonsterRegEx
    {
        public static readonly Regex DamageEn = new Regex(@"^( ⇒ )?((?<parry>Parried)! )?((?<block>Blocked)! )?((?<crit>Critical)! )?(?<target>You|.+) takes? (?<amount>\d+) (\((?<givetake>\+|-)(?<modifier>\d+)%\) )?damage\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex DamageAutoEn = new Regex(@"^( ⇒ )?((?<parry>Parried)! )?((?<block>Blocked)! )?((?<crit>Critical)! )?((T|t)he )?(?<source>|.+) hits? (?<target>you|.+) for (?<amount>\d+) (\((?<givetake>\+|-)(?<modifier>\d+)%\) )?damage\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex FailedEn = new Regex(@"^( ⇒ )?The attack misses( (?<target>you|.+))?\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex FailedAutoEn = new Regex(@"^((T|t)he )?(?<source>|.+) misses (?<target>you|.+)\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex ActionsEn = new Regex(@"^((T|t)he )?(?<source>|.+) (use|cast)s? (?<action>.+)\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex ItemsEn = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex CureEn = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex BeneficialGainEn = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex BeneficialLoseEn = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex DetrimentalGainEn = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex DetrimentalLoseEn = new Regex(@"^\.$", SharedRegEx.DefaultOptions);
    }
}
