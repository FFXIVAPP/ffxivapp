// FFXIVAPP
// Monster.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System.Text.RegularExpressions;

namespace FFXIVAPP.Classes.RegExs
{
    public static class Monster
    {
        public static readonly Regex ActionEn = new Regex(@"(?!.+misses(\.)? )^(?<crit>Critical! )?(The )?(?<source>[\w\s']+(?='s))('s) (?<action>.+(?= hits)) (?<hit>hits) (?<target>you(?!.+'s.+for)|\w+\s\w+) (from the (?<direction>\w+) )?for (?<amount>\d+).+\.$|(The )?(?<source>[\w\s]+(?='s))('s) (?<action>.+(?= misses)) (?<hit>misses) (?<target>you(?!.+'s.+from)|\w+\s\w+)( from the (?<direction>\w+)\.$|\.$)", Shared.DefaultOptions);

        public static readonly Regex ActionFr = new Regex(@"(?!.+échou\.)(?!.+résiste.+)^(?<crit>Coup critique! )?(L\w?'? ?)?(?<source>.+(?= util)) util\w+ (une? )?((?<action>.+(?= sur)) (?<hit>sur) (?<target>\w+\s\w+) (\((?<direction>.+(?=\)))\) et inflige (?<amount>\d+).+$|et inflige (?<amount>\d+).+$))", Shared.DefaultOptions);

        public static readonly Regex ActionJa = new Regex(@"^(?<source>.+)は(?<target>.+)(?<hit>に)((?<direction>.+)?(から)?「(?<action>.+)」|「(?<action>.+)」)　⇒　((?<crit>クリティカル！)　(?<amount>\d+)ダメージ。$|(?<amount>\d+)ダメージ。$)", Shared.DefaultOptions);

        public static readonly Regex ActionDe = new Regex(@"^\.$", Shared.DefaultOptions);

        public static readonly Regex ResistEn = new Regex(@"^((?<target>.+(?= partially))|(?<target>.+(?= resists))) (?<partial>partially )?(?<resist>resists) (the )?(?<source>.+('s)?(?='s))('s)? ((?!.+from)(?<action>\w+[\s\w+]{1,})(, taking (?<amount>\d+).+$|\.$)|(?<action>.+(?= from)) from the (?<direction>\w+)(, taking (?<amount>\d+).+$|\.$))", Shared.DefaultOptions);

        public static readonly Regex ResistFr = new Regex(@"^(?<source>\w+\s\w+|(L\w?'? ?)?.+) util\w+ (une? )?(?<action>.+(?= sur)) sur (l\w?'? ?)?(?<target>.+(?= mais)).+cel.+ci (?<resist>résiste) à moitié\..+subit (?<amount>\d+).+$", Shared.DefaultOptions);

        public static readonly Regex ResistJa = new Regex(@"^(?<target>.+)は(?<source>.+)に「(?<action>.+)」　⇒　.+は(?<amount>\d+).+(?<resist>半減).+。$", Shared.DefaultOptions);

        public static readonly Regex ResistDe = new Regex(@"^\.$", Shared.DefaultOptions);

        public static readonly Regex EvadeEn = new Regex(@"^((?<target>.+(?= partially))|(?<target>.+(?= evades))) (?<partial>partially )?(?<evade>evades) (the )?(?<source>.+('s)?(?='s))('s)? ((?!.+from)(?<action>\w+[\s\w+]{1,})(, taking (?<amount>\d+).+$|\.$)|(?<action>.+(?= from)) from the (?<direction>\w+)(, taking (?<amount>\d+).+$|\.$))", Shared.DefaultOptions);

        public static readonly Regex EvadeFr = new Regex(@"^\.$", Shared.DefaultOptions);

        public static readonly Regex EvadeJa = new Regex(@"^\.$", Shared.DefaultOptions);

        public static readonly Regex EvadeDe = new Regex(@"^\.$", Shared.DefaultOptions);

        //public static readonly Regex ResEvaEn = new Regex(@"^(The )?((?<whoevaded>(.+(?= partially)))|(?<whoevaded>(.+(?= resists)))|(?<whoevaded>.+(?= evades))) ((?<partial>partially) ((?<resist>resists)|(?<evade>evades)) |((?<resist>resists)|(?<evade>evades)) )(((the )?(?<source>.+('s)?(?='s))('s)? )|(?<source>your|\w+\s\w+)( )?('s )?)(?<action>\w+[\s\w+]{1,})(,|\.)?( taking (?<amount>\d+) point(s)? of damage\.)?", Shared.DefaultOptions);

        //public static readonly Regex ResEvaFr = new Regex(@"^(?<source>\w+\s\w+|(L\w?'? ?)?.+) util\w+ (une? )?(?<action>.+(?= sur)) (?<hit>sur) (l\w?'? ?)?(?<whoevaded>.+(?= mais)).+cel.+ci (?<resist>résiste) à moitié\..+subit (?<amount>\d+).+$", Shared.DefaultOptions);

        //public static readonly Regex ResEvaJa = new Regex(@"^(?<whoevaded>.+)は(?<target>.+)(?<hit>に)「(?<action>.+)」　⇒　.+は(?<amount>\d+).+(?<resist>半減).+。$", Shared.DefaultOptions);
    }
}