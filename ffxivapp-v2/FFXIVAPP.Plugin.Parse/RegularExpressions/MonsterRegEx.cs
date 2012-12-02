// FFXIVAPP.Plugin.Parse
// MonsterRegEx.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

using System.Text.RegularExpressions;
using FFXIVAPP.Common.RegularExpressions;

namespace FFXIVAPP.Plugin.Parse.RegularExpressions
{
    internal static class MonsterRegEx
    {
        public static readonly Regex ActionEn = new Regex(@"(?!.+misses(\.)? )^(?<crit>Critical! )?(The )?(?<source>[\w\s']+(?='s))('s) (?<action>.+(?= hits)) (?<hit>hits) (?<target>you(?!.+'s.+for)|\w+\s\w+) (from the (?<direction>\w+) )?for (?<amount>\d+).+\.$|(The )?(?<source>[\w\s]+(?='s))('s) (?<action>.+(?= misses)) (?<hit>misses) (?<target>you(?!.+'s.+from)|\w+\s\w+)( from the (?<direction>\w+)\.$|\.$)", SharedRegEx.DefaultOptions);

        public static readonly Regex ActionFr = new Regex(@"(?!.+échou\.)(?!.+résiste.+)^(?<crit>Coup critique! )?(L[aes]{0,2} |([LEAD]'))?(?<source>.+(?= util)) util\w+ (une? )?((?<action>.+(?= sur)) (?<hit>sur) (?<target>\w+\s\w+) (\((?<direction>.+(?=\)))\) et inflige (?<amount>\d+).+$|et inflige (?<amount>\d+).+$))", SharedRegEx.DefaultOptions);

        public static readonly Regex ActionJa = new Regex(@"^(?<source>.+)は(?<target>.+)(?<hit>に)((?<direction>.+)?(から)?「(?<action>.+)」|「(?<action>.+)」)　⇒　((?<crit>クリティカル！)　(?<amount>\d+)ダメージ。$|(?<amount>\d+)ダメージ。$)", SharedRegEx.DefaultOptions);

        public static readonly Regex ActionDe = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex UsedEn = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex UsedFr = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex UsedJa = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex UsedDe = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex AdditionalEn = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex AdditionalFr = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex AdditionalJa = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex AdditionalDe = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex CounterEn = new Regex(@"^(?<counter>Counter! )(?<source>.+)'s (?<action>[\w\s]+) (?<hit>hits?)( the)? (?<target>.+(?= for)) for (?<amount>\d+) points of damage\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex CounterFr = new Regex(@"^(L[aes]{0,2} |([LEAD]'))?(?<source>.+) (?<counter>contre) (?<target>.+(?= et)) et.+inflige (?<amount>\d+).+$", SharedRegEx.DefaultOptions);

        public static readonly Regex CounterJa = new Regex(@"^(?<source>.+)は(?<target>.+)の.+(?<counter>カウンター。)　⇒　(?<amount>\d+)ダメージを与えた。$", SharedRegEx.DefaultOptions);

        public static readonly Regex CounterDe = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex BlockEn = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex BlockFr = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex BlockJa = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex BlockDe = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex ParryEn = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex ParryFr = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex ParryJa = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex ParryDe = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex ResistEn = new Regex(@"^((?<target>.+(?= partially))|(?<target>.+(?= resists))) (?<partial>partially )?(?<resist>resists) (the )?(?<source>.+('s)?(?='s))('s)? ((?!.+from)(?<action>\w+[\s\w+]{1,})(, taking (?<amount>\d+).+$|\.$)|(?<action>.+(?= from)) from the (?<direction>\w+)(, taking (?<amount>\d+).+$|\.$))", SharedRegEx.DefaultOptions);

        public static readonly Regex ResistFr = new Regex(@"^(L[aes]{0,2} |([LEAD]'))?(?<source>.+) utilisez?( une?)? (?<action>.+(?= sur)) sur (?<target>.+(?= mais)) mais(.+(?<resist>résiste).+moitié\..+subit (?<amount>\d+) points? de dégâts \(mitigés\)\.|.+(?<resist>résiste)\..+complètement!)$", SharedRegEx.DefaultOptions);

        public static readonly Regex ResistJa = new Regex(@"^(?<target>.+)は(?<source>.+)に「(?<action>.+)」　⇒　.+は(?<amount>\d+).+(?<resist>半減).+。$", SharedRegEx.DefaultOptions);

        public static readonly Regex ResistDe = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex EvadeEn = new Regex(@"^((?<target>.+(?= partially))|(?<target>.+(?= evades))) (?<partial>partially )?(?<evade>evades) (the )?(?<source>.+('s)?(?='s))('s)? ((?!.+from)(?<action>\w+[\s\w+]{1,})(, taking (?<amount>\d+).+$|\.$)|(?<action>.+(?= from)) from the (?<direction>\w+)(, taking (?<amount>\d+).+$|\.$))", SharedRegEx.DefaultOptions);

        public static readonly Regex EvadeFr = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex EvadeJa = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex EvadeDe = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        //public static readonly Regex ResEvaEn = new Regex(@"^(The )?((?<whoevaded>(.+(?= partially)))|(?<whoevaded>(.+(?= resists)))|(?<whoevaded>.+(?= evades))) ((?<partial>partially) ((?<resist>resists)|(?<evade>evades)) |((?<resist>resists)|(?<evade>evades)) )(((the )?(?<source>.+('s)?(?='s))('s)? )|(?<source>your|\w+\s\w+)( )?('s )?)(?<action>\w+[\s\w+]{1,})(,|\.)?( taking (?<amount>\d+) point(s)? of damage\.)?", SharedRegEx.DefaultOptions);

        //public static readonly Regex ResEvaFr = new Regex(@"^(?<source>\w+\s\w+|([Ll]\w?'? ?)?.+) util\w+ (une? )?(?<action>.+(?= sur)) (?<hit>sur) (l\w?'? ?)?(?<whoevaded>.+(?= mais)).+cel.+ci (?<resist>résiste) à moitié\..+subit (?<amount>\d+).+$", SharedRegEx.DefaultOptions);

        //public static readonly Regex ResEvaJa = new Regex(@"^(?<whoevaded>.+)は(?<target>.+)(?<hit>に)「(?<action>.+)」　⇒　.+は(?<amount>\d+).+(?<resist>半減).+。$", SharedRegEx.DefaultOptions);
    }
}