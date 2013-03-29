// FFXIVAPP.Plugin.Parse
// PlayerRegEx.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System.Text.RegularExpressions;
using FFXIVAPP.Common.RegularExpressions;

#endregion

namespace FFXIVAPP.Plugin.Parse.RegularExpressions
{
    internal static class PlayerRegEx
    {
        public static readonly Regex DamageEn = new Regex(@"^( ⇒ )?(?<crit>Critical! )?((T|t)he )?(?<target>.+) takes? (?<amount>\d+) (\((?<givetake>\+|-)(?<modifier>\d+)%\) )?damage\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex DamageAutoEn = new Regex(@"^(?<crit>Critical! )?(?<source>You|.+) hits? ((T|t)he )?(?<target>.+) for (?<amount>\d+) (\((?<givetake>\+|-)(?<modifier>\d+)%\) )?damage\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex FailedEn = new Regex(@"^( ⇒ )?The attack miss(es)? ((T|t)he )?(?<target>.+)\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex FailedAutoEn = new Regex(@"^(?<source>You|.+) miss(es)? ((T|t)he )?(?<target>.+)\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex ActionsEn = new Regex(@"^(?<source>You|.+) (use|cast)s? (?<action>.+)\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex ItemsEn = new Regex(@"^(?<source>You|.+) uses? an? (?<item>.+)\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex CureEn = new Regex(@"( ⇒ )?(?<crit>Critical! )?((T|t)he )?(?<target>You|.+) recovers? (?<amount>\d+) (?<type>\w+)\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex BeneficialGainEn = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex BeneficialLoseEn = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex DetrimentalGainEn = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex DetrimentalLoseEn = new Regex(@"^\.$", SharedRegEx.DefaultOptions);
    }
}
