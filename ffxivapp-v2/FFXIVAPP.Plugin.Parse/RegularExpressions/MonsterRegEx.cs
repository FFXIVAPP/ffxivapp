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
        //ENGLISH REGULAR EXPRESSIONS
        public static Regex DamageEn = new Regex(@"^( ⇒ )?(?<parry>Parried! )?(?<block>Blocked! )?(?<crit>Critical! )?(?<target>You|.+) takes? (?<amount>\d+) (\((?<modifier>.\d+)%\) )?damage\.$", SharedRegEx.DefaultOptions);

        public static Regex DamageAutoEn = new Regex(@"^(?! ⇒)(?<parry>Parried! )?(?<block>Blocked! )?(?<crit>Critical! )?((T|t)he )?(?<source>|.+) hits? (?<target>you|.+) for (?<amount>\d+) (\((?<modifier>.\d+)%\) )?damage\.$", SharedRegEx.DefaultOptions);

        public static Regex FailedEn = new Regex(@"^( ⇒ )?The attack misses( (?<target>you|.+))?\.$", SharedRegEx.DefaultOptions);

        public static Regex FailedAutoEn = new Regex(@"^(?! ⇒)((T|t)he )?(?<source>.+) misses (?<target>you|.+)\.$", SharedRegEx.DefaultOptions);

        public static Regex ActionsEn = new Regex(@"^((T|t)he )?(?<source>|.+) (use|cast)s? (?<action>.+)\.$", SharedRegEx.DefaultOptions);

        public static Regex ItemsEn = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex CureEn = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex BeneficialGainEn = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex BeneficialLoseEn = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex DetrimentalGainEn = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex DetrimentalLoseEn = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        //FRENCH REGULAR EXPRESSIONS
        public static Regex DamageFr = new Regex(@"^( ⇒ )?(?<parry>Parade ?! )?(?<block>Blocage ?! )?(?<crit>Critique ?! )?(?<target>Vous|.+) subi(t|ssez?)? (?<amount>\d+) (\((?<modifier>.\d+)%\) )?points? de dégâts?\.$", SharedRegEx.DefaultOptions);

        public static Regex DamageAutoFr = new Regex(@"^(?! ⇒)(?<parry>Parade ?! )?(?<block>Blocage ?! )?(?<crit>Critique ?! )?(L[aes] |[LEAD]')?(?<source>.+) ((?<target>Vous|.+) infligez?|infligez? à (?<target>vous|.+)) (?<amount>\d+) (\((?<modifier>.\d+)%\) )?points? de dégâts?\.$", SharedRegEx.DefaultOptions);

        public static Regex FailedFr = new Regex(@"^( ⇒ )?L'attaque ((?<target>vous) manque|manquez? (?<target>.+))\.$", SharedRegEx.DefaultOptions);

        public static Regex FailedAutoFr = new Regex(@"^(?! ⇒)(L[aes] |[LEAD]')?(?<source>.+) ((?<target>vous) manque|manquez? (?<target>.+))\.$", SharedRegEx.DefaultOptions);

        public static Regex ActionsFr = new Regex(@"^(L[aes] |[LEAD]')?(?<source>.+) (utilise|lance)z? (?<action>.+)\.$", SharedRegEx.DefaultOptions);

        public static Regex ItemsFr = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex CureFr = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex BeneficialGainFr = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex BeneficialLoseFr = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex DetrimentalGainFr = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex DetrimentalLoseFr = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

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
    }
}
