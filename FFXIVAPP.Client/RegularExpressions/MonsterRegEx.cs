// FFXIVAPP.Client
// MonsterRegEx.cs
// 
// Copyright © 2007 - 2014 Ryan Wilson - All Rights Reserved
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions are met: 
// 
//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright 
//    notice, this list of conditions and the following disclaimer in the 
//    documentation and/or other materials provided with the distribution. 
//  * Neither the name of SyndicatedLife nor the names of its contributors may 
//    be used to endorse or promote products derived from this software 
//    without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE. 

using System.Text.RegularExpressions;
using FFXIVAPP.Common.RegularExpressions;

namespace FFXIVAPP.Client.RegularExpressions
{
    internal static class MonsterRegEx
    {
        //ENGLISH REGULAR EXPRESSIONS
        public static Regex DamageEn = new Regex(@"^ ⇒ (?<block>Blocked! )?(?<parry>Parried! )?(?<crit>Critical! )?(?<target>You|.+) takes? (?<amount>\d+) ?(\((?<modifier>.\d+)%\) )?damage\.$", SharedRegEx.DefaultOptions);

        public static Regex DamageAutoEn = new Regex(@"^(?! ⇒)(?<block>Blocked! )?(?<parry>Parried! )?(?<crit>Critical! )?((T|t)he )?(?<source>.+) hits? (?<target>you|.+) for (?<amount>\d+) ?(\((?<modifier>.\d+)%\) )?damage\.$", SharedRegEx.DefaultOptions);

        public static Regex ResistEn = new Regex(@"^(?<resist>(Full|Partial|Half) resist! )((T|t)he )?(?<source>.+) takes? ((?<amount>\d+) ?(\((?<modifier>.\d+)%\) )?|no )damage\.$", SharedRegEx.DefaultOptions);

        public static Regex FailedEn = new Regex(@"^ ⇒ The attack misses( (?<target>you|.+))?\.$", SharedRegEx.DefaultOptions);

        public static Regex FailedAutoEn = new Regex(@"^(?! ⇒)((T|t)he )?(?<source>.+) misses (?<target>you|.+)\.$", SharedRegEx.DefaultOptions);

        public static Regex ActionsEn = new Regex(@"^((T|t)he )?(?<source>.+) (use|cast)s? (?<action>.+)\.$", SharedRegEx.DefaultOptions);

        public static Regex ItemsEn = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex CureEn = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex BeneficialGainEn = new Regex(@"^( ⇒ )?((T|t)he )?(?<target>.+) gains? the effect of (?<status>.+)\.$", SharedRegEx.DefaultOptions);

        public static Regex BeneficialLoseEn = new Regex(@"^( ⇒ )?((T|t)he )?(?<target>.+) loses? the effect of (?<status>.+)\.$", SharedRegEx.DefaultOptions);

        public static Regex DetrimentalGainEn = new Regex(@"^( ⇒ )?((T|t)he )?(?<target>.+) suffers? the effect of (?<status>.+)\.$", SharedRegEx.DefaultOptions);

        public static Regex DetrimentalLoseEn = new Regex(@"^( ⇒ )?((T|t)he )?(?<target>.+) recovers? from the effect of (?<status>.+)\.$", SharedRegEx.DefaultOptions);

        //FRENCH REGULAR EXPRESSIONS
        public static Regex DamageFr = new Regex(@"^ ⇒ (?<parry>Parade ?! )?(?<block>Blocage ?! )?(?<crit>Critique ?! )?(?<target>Vous|.+) subi(t|ssez?)? (?<amount>\d+) ?(\((?<modifier>.\d+)%\) )?points? de dégâts?\.$", SharedRegEx.DefaultOptions);

        public static Regex DamageAutoFr = new Regex(@"^(?! ⇒)(?<parry>Parade ?! )?(?<block>Blocage ?! )?(?<crit>Critique ?! )?(L[aes] |[LEAD]')?(?<source>.+) ((?<target>Vous|.+) infligez?|infligez? à (?<target>vous|.+)) (?<amount>\d+) ?(\((?<modifier>.\d+)%\) )?points? de dégâts?\.$", SharedRegEx.DefaultOptions);

        public static Regex ResistFr = new Regex(@"^ ⇒ (?<resist>(Résistance totale|Résistance partielle|Semi résistance)! )(?<target>Vous|.+) (subi(t|ssez?)? (?<amount>\d+) ?(\((?<modifier>.\d+)%\) )?points? de |ne subi(t|ssez?)? aucun )dégâts?\.$", SharedRegEx.DefaultOptions);

        public static Regex FailedFr = new Regex(@"^ ⇒ L'attaque ((?<target>vous) manque|manquez? (?<target>.+))\.$", SharedRegEx.DefaultOptions);

        public static Regex FailedAutoFr = new Regex(@"^(?! ⇒)(L[aes] |[LEAD]')?(?<source>.+) ((?<target>vous) manque|manquez? (?<target>.+))\.$", SharedRegEx.DefaultOptions);

        public static Regex ActionsFr = new Regex(@"^(L[aes] |[LEAD]')?(?<source>.+) (utilise|lance)z? (?<action>.+)\.$", SharedRegEx.DefaultOptions);

        public static Regex ItemsFr = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex CureFr = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex BeneficialGainFr = new Regex(@"^( ⇒ )?(L[aes] |[LEAD]')?(?<target>.+) bénéficiez? de l'effet (?<status>.+)\.$", SharedRegEx.DefaultOptions);

        public static Regex BeneficialLoseFr = new Regex(@"^( ⇒ )?(L[aes] |[LEAD]')?(?<target>.+) perd(ez?)? l'effet (?<status>.+)\.$", SharedRegEx.DefaultOptions);

        public static Regex DetrimentalGainFr = new Regex(@"^( ⇒ )?(L[aes] |[LEAD]')?(?<target>.+) subi(t|ssez?) l'effet (?<status>.+)\.$", SharedRegEx.DefaultOptions);

        public static Regex DetrimentalLoseFr = new Regex(@"^( ⇒ )?(L[aes] |[LEAD]')?(?<target>.+) ne subi(t|ssez?) plus l'effet (?<status>.+)\.$", SharedRegEx.DefaultOptions);

        //JAPANESE REGULAR EXPRESSIONS
        public static Regex DamageJa = new Regex(@"^ ⇒ (?<crit>クリティカル！ )?(?<target>.+)((に|は)、?)(?<block>ブロックした！ )?(?<parry>受け流した！ )?(?<amount>\d+) ?(\((?<modifier>.\d+)%\) ?)?ダメージ。$", SharedRegEx.DefaultOptions);

        public static Regex DamageAutoJa = new Regex(@"^(?! ⇒)(?<source>.+)の攻撃( ⇒ )?(?<crit>クリティカル！ )?(?<target>.+)((に|は)、?)(?<block>ブロックした！ )?(?<parry>受け流した！ )?(?<amount>\d+) ?(\((?<modifier>.\d+)%\) ?)?ダメージ。$", SharedRegEx.DefaultOptions);

        public static Regex ResistJa = new Regex(@"^ ⇒ (?<target>.+)((に|は)、?)((?<resist>レジストした！)|(?<resist>クォータレジストした！ )?(?<resist>ハーフレジストした！ )?(?<amount>\d+) ?(\((?<modifier>.\d+)%\) ?)?ダメージ。)$", SharedRegEx.DefaultOptions);

        public static Regex FailedJa = new Regex(@"^ ⇒ (?<target>.+)((に|は)、?)ミス！$", SharedRegEx.DefaultOptions);

        public static Regex FailedAutoJa = new Regex(@"^(?<source>.+)の攻撃( ⇒ )?(?<target>.+)((に|は)、?)ミス！$", SharedRegEx.DefaultOptions);

        public static Regex ActionsJa = new Regex(@"^(?<source>.+)の「(?<action>.+)」$", SharedRegEx.DefaultOptions);

        public static Regex ItemsJa = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex CureJa = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex BeneficialGainJa = new Regex(@"^( ⇒ )?(?<target>.+)((に|は)、?)「(?<status>.+)」の効果。$", SharedRegEx.DefaultOptions);

        public static Regex BeneficialLoseJa = new Regex(@"^( ⇒ )?(?<target>.+)((に|は)、?)「(?<status>.+)」が切れた。$", SharedRegEx.DefaultOptions);

        public static Regex DetrimentalGainJa = new Regex(@"^( ⇒ )?(?<target>.+)((に|は)、?)「(?<status>.+)」の効果。$", SharedRegEx.DefaultOptions);

        public static Regex DetrimentalLoseJa = new Regex(@"^( ⇒ )?(?<target>.+)((に|は)、?)「(?<status>.+)」が切れた。$", SharedRegEx.DefaultOptions);

        //GERMAN REGULAR EXPRESSIONS
        public static Regex DamageDe = new Regex(@"^ ⇒ (?<block>Geblockt! ?)?(?<parry>Pariert! ?)?(?<crit>Kritischer Treffer! ?)?(?<target>dich|.+)( erleides?t (nur )?|, aber der Schaden wird auf )(?<amount>\d+) ?(\((?<modifier>.\d+)%\) )?Punkte? (Schaden|reduziert)\.$", SharedRegEx.DefaultOptions);

        public static Regex DamageAutoDe = new Regex(@"^(?! ⇒)(?<block>Geblockt! ?)?(?<parry>Pariert! ?)?(?<crit>Kritischer Treffer! ?)?(D(u|einer|(i|e)r|ich|as|ie|en) )?(?<source>.+) triffs?t (?<target>dich|.+)( und verursachs?t |, aber der Schaden wird auf )(?<amount>\d+) ?(\((?<modifier>.\d+)%\) )?Punkte? (Schaden|reduziert)\.$", SharedRegEx.DefaultOptions);

        public static Regex ResistDe = new Regex(@"^(?! ⇒)(?<resist>(Vollkommen widerstanden|Teilweise widerstanden|Halb widerstanden!)! )(D(u|einer|(i|e)r|ich|as|ie|en) )?(?<target>.+) erleides?t (?<amount>\d+) ?(\((?<modifier>.\d+)%\) )?Punkte? Schaden\.$", SharedRegEx.DefaultOptions);

        public static Regex FailedDe = new Regex(@"^ ⇒ Die Attacke verfehlt? (?<target>dich|.+)\.$", SharedRegEx.DefaultOptions);

        public static Regex FailedAutoDe = new Regex(@"^(?! ⇒)(D(u|einer|(i|e)r|ich|as|ie|en) )?(?<source>.+) verfehls?t (?<target>dich|.+)\.$", SharedRegEx.DefaultOptions);

        public static Regex ActionsDe = new Regex(@"^(D(u|einer|(i|e)r|ich|as|ie|en) )?(?<source>.+) (setzt (?<action>.+) ein|wirks?t (?<action>.+))\.$", SharedRegEx.DefaultOptions);

        public static Regex ItemsDe = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex CureDe = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex BeneficialGainDe = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex BeneficialLoseDe = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex DetrimentalGainDe = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static Regex DetrimentalLoseDe = new Regex(@"^\.$", SharedRegEx.DefaultOptions);
    }
}
