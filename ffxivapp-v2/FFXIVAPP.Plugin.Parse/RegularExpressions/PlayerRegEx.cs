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
        //ENGLISH REGULAR EXPRESSIONS
        public static Regex DamageEn = new Regex(@"^( ⇒ )?((?<parry>Parried)! )?((?<block>Blocked)! )?((?<crit>Critical)! )?((T|t)he )?(?<target>.+) takes? (?<amount>\d+) (\((?<modifier>.\d+)%\) )?damage\.$", SharedRegEx.DefaultOptions);

        public static Regex DamageAutoEn = new Regex(@"^( ⇒ )?((?<parry>Parried)! )?((?<block>Blocked)! )?((?<crit>Critical)! )?(?<source>You|.+) hits? ((T|t)he )?(?<target>.+) for (?<amount>\d+) (\((?<modifier>.\d+)%\) )?damage\.$", SharedRegEx.DefaultOptions);

        public static Regex FailedEn = new Regex(@"^( ⇒ )?The attack miss(es)?( ((T|t)he )?(?<target>.+))?\.$", SharedRegEx.DefaultOptions);

        public static Regex FailedAutoEn = new Regex(@"^(?<source>You|.+) miss(es)? ((T|t)he )?(?<target>.+)\.$", SharedRegEx.DefaultOptions);

        public static Regex ActionsEn = new Regex(@"^(?<source>You|.+) (use|cast)s? (?<action>.+)\.$", SharedRegEx.DefaultOptions);

        public static Regex ItemsEn = new Regex(@"^(?<source>You|.+) uses? (an? |the )?(?<item>.+)\.$", SharedRegEx.DefaultOptions);

        public static Regex CureEn = new Regex(@"( ⇒ )?(?<crit>Critical! )?((T|t)he )?(?<target>You|.+) recovers? (?<amount>\d+) (\((?<modifier>.\d+)%\) )?(?<type>\w+)\.$", SharedRegEx.DefaultOptions);

        public static Regex BeneficialGainEn = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex BeneficialLoseEn = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex DetrimentalGainEn = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex DetrimentalLoseEn = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex ObtainsEn = new Regex(@"^(?<source>You|.+) obtains? (an? |the )?(?<item>.+)\.$", SharedRegEx.DefaultOptions);

        public static Regex DefeatsEn = new Regex(@"^(?<source>You|.+) defeats? ((T|t)he )?(?<target>.+)\.$", SharedRegEx.DefaultOptions);

        //FRENCH REGULAR EXPRESSIONS
        public static Regex DamageFr = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex DamageAutoFr = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex FailedFr = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex FailedAutoFr = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex ActionsFr = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex ItemsFr = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex CureFr = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex BeneficialGainFr = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex BeneficialLoseFr = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex DetrimentalGainFr = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex DetrimentalLoseFr = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex ObtainsFr = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex DefeatsFr = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        //JAPANESE REGULAR EXPRESSIONS
        public static Regex DamageJa = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex DamageAutoJa = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex FailedJa = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex FailedAutoJa = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex ActionsJa = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex ItemsJa = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex CureJa = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex BeneficialGainJa = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex BeneficialLoseJa = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex DetrimentalGainJa = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex DetrimentalLoseJa = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex ObtainsJa = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex DefeatsJa = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        //GERMAN REGULAR EXPRESSIONS
        public static Regex DamageDe = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex DamageAutoDe = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex FailedDe = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex FailedAutoDe = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex ActionsDe = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex ItemsDe = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex CureDe = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex BeneficialGainDe = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex BeneficialLoseDe = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex DetrimentalGainDe = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex DetrimentalLoseDe = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex ObtainsDe = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex DefeatsDe = new Regex(@"^\.$", SharedRegEx.DefaultOptions);
    }
}
