// FFXIVAPP
// Monster.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System.Text.RegularExpressions;

namespace FFXIVAPP.Classes.RegExs
{
    public class Monster
    {
        public static Regex ActionEn = new Regex(@"(?!.+misses(\.)? )^(?<crit>Critical! )?(The )?(?<source>[\w\s']+(?='s))('s) (?<action>.+(?= hits)) (?<hit>hits) (?<target>you(?!.+'s.+for)|\w+\s\w+) (from the (?<direction>\w+) )?for (?<amount>\d+).+\.$|(The )?(?<source>[\w\s]+(?='s))('s) (?<action>.+(?= misses)) (?<hit>misses) (?<target>you(?!.+'s.+from)|\w+\s\w+)( from the (?<direction>\w+)\.$|\.$)", Shared.DefaultOptions);

        public static Regex ActionFr = new Regex(@"(?!.+échou\.)(?!.+résiste.+)^(?<crit>Coup critique! )?(L\w |([LEAD' ]+))?(?<source>.+(?= util)) util\w+ (une? )?((?<action>.+(?= sur)) (?<hit>sur) (?<target>\w+\s\w+) (\((?<direction>.+(?=\)))\) et inflige (?<amount>\d+).+$|et inflige (?<amount>\d+).+$))", Shared.DefaultOptions);

        public static Regex ActionJa = new Regex(@"^(?<source>.+)は(?<target>.+)(?<hit>に)((?<direction>.+)?(から)?「(?<action>.+)」|「(?<action>.+)」)　⇒　((?<crit>クリティカル！)　(?<amount>\d+)ダメージ。$|(?<amount>\d+)ダメージ。$)", Shared.DefaultOptions);

        public static Regex ActionDe = new Regex(@"^\.$", Shared.DefaultOptions);

        public static Regex UsedEn = new Regex(@"^\.$", Shared.DefaultOptions);

        public static Regex UsedFr = new Regex(@"^\.$", Shared.DefaultOptions);

        public static Regex UsedJa = new Regex(@"^\.$", Shared.DefaultOptions);

        public static Regex UsedDe = new Regex(@"^\.$", Shared.DefaultOptions);

        public static Regex AdditionalEn = new Regex(@"^\.$", Shared.DefaultOptions);

        public static Regex AdditionalFr = new Regex(@"^\.$", Shared.DefaultOptions);

        public static Regex AdditionalJa = new Regex(@"^\.$", Shared.DefaultOptions);

        public static Regex AdditionalDe = new Regex(@"^\.$", Shared.DefaultOptions);

        public static Regex CounterEn = new Regex(@"^(?<counter>Counter! )(?<source>.+)'s (?<action>[\w\s]+) (?<hit>hits?)( the)? (?<target>.+(?= for)) for (?<amount>\d+) points of damage\.$", Shared.DefaultOptions);

        public static Regex CounterFr = new Regex(@"^(L\w |([LEAD' ]+))?(?<source>.+) (?<counter>contre) (?<target>.+(?= et)) et.+inflige (?<amount>\d+).+$", Shared.DefaultOptions);

        public static Regex CounterJa = new Regex(@"^(?<source>.+)は(?<target>.+)の.+(?<counter>カウンター。)　⇒　(?<amount>\d+)ダメージを与えた。$", Shared.DefaultOptions);

        public static Regex CounterDe = new Regex(@"^\.$", Shared.DefaultOptions);

        public static Regex BlockEn = new Regex(@"^\.$", Shared.DefaultOptions);

        public static Regex BlockFr = new Regex(@"^\.$", Shared.DefaultOptions);

        public static Regex BlockJa = new Regex(@"^\.$", Shared.DefaultOptions);

        public static Regex BlockDe = new Regex(@"^\.$", Shared.DefaultOptions);

        public static Regex ParryEn = new Regex(@"^\.$", Shared.DefaultOptions);

        public static Regex ParryFr = new Regex(@"^\.$", Shared.DefaultOptions);

        public static Regex ParryJa = new Regex(@"^\.$", Shared.DefaultOptions);

        public static Regex ParryDe = new Regex(@"^\.$", Shared.DefaultOptions);

        public static Regex ResistEn = new Regex(@"^((?<target>.+(?= partially))|(?<target>.+(?= resists))) (?<partial>partially )?(?<resist>resists) (the )?(?<source>.+('s)?(?='s))('s)? ((?!.+from)(?<action>\w+[\s\w+]{1,})(, taking (?<amount>\d+).+$|\.$)|(?<action>.+(?= from)) from the (?<direction>\w+)(, taking (?<amount>\d+).+$|\.$))", Shared.DefaultOptions);

        public static Regex ResistFr = new Regex(@"^(L\w |([LEAD' ]+))?(?<source>.+) utilisez?( une?)? (?<action>.+(?= sur)) sur ([Ll]\w |([LEAD' ]+))?(?<target>.+(?= mais)) mais(.+(?<resist>résiste).+moitié\..+subit (?<amount>\d+) points? de dégâts \(mitigés\)\.|.+(?<resist>résiste)\..+complètement!)$", Shared.DefaultOptions);

        public static Regex ResistJa = new Regex(@"^(?<target>.+)は(?<source>.+)に「(?<action>.+)」　⇒　.+は(?<amount>\d+).+(?<resist>半減).+。$", Shared.DefaultOptions);

        public static Regex ResistDe = new Regex(@"^\.$", Shared.DefaultOptions);

        public static Regex EvadeEn = new Regex(@"^((?<target>.+(?= partially))|(?<target>.+(?= evades))) (?<partial>partially )?(?<evade>evades) (the )?(?<source>.+('s)?(?='s))('s)? ((?!.+from)(?<action>\w+[\s\w+]{1,})(, taking (?<amount>\d+).+$|\.$)|(?<action>.+(?= from)) from the (?<direction>\w+)(, taking (?<amount>\d+).+$|\.$))", Shared.DefaultOptions);

        public static Regex EvadeFr = new Regex(@"^\.$", Shared.DefaultOptions);

        public static Regex EvadeJa = new Regex(@"^\.$", Shared.DefaultOptions);

        public static Regex EvadeDe = new Regex(@"^\.$", Shared.DefaultOptions);

        //public static Regex ResEvaEn = new Regex(@"^(The )?((?<whoevaded>(.+(?= partially)))|(?<whoevaded>(.+(?= resists)))|(?<whoevaded>.+(?= evades))) ((?<partial>partially) ((?<resist>resists)|(?<evade>evades)) |((?<resist>resists)|(?<evade>evades)) )(((the )?(?<source>.+('s)?(?='s))('s)? )|(?<source>your|\w+\s\w+)( )?('s )?)(?<action>\w+[\s\w+]{1,})(,|\.)?( taking (?<amount>\d+) point(s)? of damage\.)?", Shared.DefaultOptions);

        //public static Regex ResEvaFr = new Regex(@"^(?<source>\w+\s\w+|(L\w?'? ?)?.+) util\w+ (une? )?(?<action>.+(?= sur)) (?<hit>sur) (l\w?'? ?)?(?<whoevaded>.+(?= mais)).+cel.+ci (?<resist>résiste) à moitié\..+subit (?<amount>\d+).+$", Shared.DefaultOptions);

        //public static Regex ResEvaJa = new Regex(@"^(?<whoevaded>.+)は(?<target>.+)(?<hit>に)「(?<action>.+)」　⇒　.+は(?<amount>\d+).+(?<resist>半減).+。$", Shared.DefaultOptions);
    }
}