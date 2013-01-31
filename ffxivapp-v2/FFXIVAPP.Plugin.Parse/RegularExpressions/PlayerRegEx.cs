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
        public static readonly Regex ActionEn = new Regex(@"((?!.+misses\.)^(?<crit>Critical! )?(?<source>Your|\w+\s\w+)('s)? (?<action>.+(?= hits)) (?<hit>hits) (the )?((?!.+from)((?<target>.+)'s (?<part>.+)|(?<target>.+)) for |((?<target>.+)'s (?<part>.+)|(?<target>.+)) from the (?<direction>\w+) for )(?<amount>\d+) points? of damage\.$)|(^(?<source>Your|\w+\s\w+)('s)? (?<action>.+(?= misses)) (?<miss>misses) (the )?((?!.+from)((?<target>.+)'s (?<part>.+)|(?<target>.+))\.$|((?<target>.+)'s (?<part>.+)|(?<target>.+)) from the (?<direction>\w+)\.$))", SharedRegEx.DefaultOptions);

        public static readonly Regex ActionFr = new Regex(@"(?!.+échou\.)^(?<crit>Coup critique! )?(?<source>.+) utilisez?( une?)? ((?<action>.+(?= sur)) (?<hit>sur) ((([lead]')|(l[aes]{0,2} )?)?(?<target>.+(?= \()) \((?<direction>.+(?=\)))\) (?<hit>et) inflige (?<amount>\d+).+$|(([lead]')|(l[aes]{0,2} )?)?(?<target>.+(?= et)) et inflige (?<amount>\d+).+\.$)|(?<action>.+(?= et inflige)) (?<hit>et) inflige (?<amount>\d+).+dégâts ((a|à)(\w+)? )?(([lead]')|(l[aes]{0,2} )?)?(?<target>.+(?=\.$))\.$)|((?<source>\w+\s\w+) util\w+ (une? )?(?<action>.+(?= sur)) (?<hit>sur) (([lead]')|(l[aes]{0,2} )?)?((?<target>.+(?= \()) \((?<direction>.+(?=\)))\)|(?<target>.+)) (?<miss>mais) échoue!$)", SharedRegEx.DefaultOptions);

        public static readonly Regex ActionJa = new Regex(@"^(?<source>.+)は(?<target>.+)(?<hit>に)((?<direction>.+)?(から)?「(?<action>.+)」|「(?<action>.+)」)　⇒　((?<crit>クリティカル！)　(?<amount>\d+)ダメージ。$|(?<amount>\d+)ダメージ。$)", SharedRegEx.DefaultOptions);

        public static readonly Regex ActionDe = new Regex(@"^(?<source>Du|\w+ \w+) fügs?t( dem)? (?<target>.+(?= mit)) (?<hit>mit) (?<action>.+) (?<amount>\d+) Punkte Schaden zu\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex UsedEn = new Regex(@"((?<source>You|\w+\s\w+)) use(s)? (((?<action>.+(?= on)) on (?<target>\w+\s\w+)).+(?<recloss>recover(s)?|lose(s)?) (?<amount>\d+) (?<type>\w+)\.)?(((?<action>\w+[\s\w+]{0,})\.)(?! ))?(((?<action>\w+[\s\w+]{0,})\.) (?<target>You) (?<recloss>recover(s)?|lose(s)?) (?<amount>\d+) (?<type>\w+)\.)?", SharedRegEx.DefaultOptions);

        public static readonly Regex UsedFr = new Regex(@"^(?<source>\w+\s\w+) util\w+ (?<action>.+(?= sur)) sur (?<target>\w+\s\w+)\. ?\w+\s\w+\s?(?<recloss>récupère?|lose(s)?) (?<amount>\d+) (?<type>\w+)\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex UsedJa = new Regex(@"^(?<source>.+)は(?<target>.+)に「(?<action>.+)」　⇒　.+は(?<type>\w\w)を(?<amount>\d+)回復した。$", SharedRegEx.DefaultOptions);

        public static readonly Regex UsedDe = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex AdditionalEn = new Regex(@"^A.+effect: (?<amount>\d+) p.+of.+dealt.$", SharedRegEx.DefaultOptions);

        public static readonly Regex AdditionalFr = new Regex(@"^Effet.+: (?<amount>\d+) p.+dégâts.$", SharedRegEx.DefaultOptions);

        public static readonly Regex AdditionalJa = new Regex(@"^追加効果.+(?<amount>\d+)ダメージ。$", SharedRegEx.DefaultOptions);

        public static readonly Regex AdditionalDe = new Regex(@"^Der Zusatzefeckt verursacht (?<amount>\d+) Punkte? Schaden\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex CounterEn = new Regex(@"^(?<counter>Counter! )(?<source>(.+(?:'s) |Your? ))(?<action>[\w\s]+) (?<hit>hits?)( the)? (?<target>.+(?= for)) for (?<amount>\d+) points of damage\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex CounterFr = new Regex(@"^(?<source>\w+\s\w+) (?<counter>contre) (l\w?'? ?)?(?<target>.+(?= et)) et.+inflige (?<amount>\d+).+$", SharedRegEx.DefaultOptions);

        public static readonly Regex CounterJa = new Regex(@"^(?<source>.+)は(?<target>.+)の.+(?<counter>カウンター。)　⇒　(?<amount>\d+)ダメージを与えた。$", SharedRegEx.DefaultOptions);

        public static readonly Regex CounterDe = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex BlockEn = new Regex(@"^(?<target>(You|\w+\s\w+)) (?<partial>partially )?(?<block>block(s)?) ((t|T)he )?(?<source>.+(?='s))('s)? ((?<action>.+(?= from)) from the (?<direction>\w+)(, taking (?<amount>\d+).+$|\.$)|(?<action>.+), taking (?<amount>\d+).+$|(?<action>.+)\.$)", SharedRegEx.DefaultOptions);

        public static readonly Regex BlockFr = new Regex(@"^(L[aes]{0,2} |([LEAD]'))?(?<source>.+) utilisez?( une?)? (?<action>.+) sur (?<target>\w+\s\w+)( \((?<direction>.+)\))? mais.+bouclier\.(.+par.+(?<block>coup)!$|.+(?<block>subit) (?<amount>\d+) points? de d.+\.$)", SharedRegEx.DefaultOptions);

        public static readonly Regex BlockJa = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex BlockDe = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex ParryEn = new Regex(@"^(?<target>(You|\w+\s\w+)) (?<partial>partially )?(?<parry>parr(y|ies)?) ((t|T)he )?(?<source>.+(?='s))('s)? ((?<action>.+(?= from)) from the (?<direction>\w+)(, taking (?<amount>\d+).+$|\.$)|(?<action>.+), taking (?<amount>\d+).+$|(?<action>.+)\.$)", SharedRegEx.DefaultOptions);

        public static readonly Regex ParryFr = new Regex(@"^(L[aes]{0,2} |([LEAD]'))?(?<source>[\w\s'-]+) utilise( une?)? (?<action>[\w\s'-]+) sur (?<target>\w+\s\w+)( \((?<direction>.+)\))? mais.+arrive à (?<parry>parer)\.(\w+ \w+ (?<partial>ne subit aucun).+!|\w+ \w+ subit (?<amount>\d+) points?.+\(mit.+\)\.)$", SharedRegEx.DefaultOptions);

        public static readonly Regex ParryJa = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex ParryDe = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex ResistEn = new Regex(@"^(The )?((?<target>.+(?= partially))|(?<target>.+(?= resists))) (?<partial>partially )?(?<resist>resists) (?<source>your|\w+\s\w+)(?:'s)? ((?!.+from)(?<action>\w+[\s\w+]{1,})(, taking (?<amount>\d+).+$|\.$)|(?<action>.+(?= from)) from the (?<direction>\w+)(, taking (?<amount>\d+).+$|\.$))", SharedRegEx.DefaultOptions);

        public static readonly Regex ResistFr = new Regex(@"^(?<source>.+) utilisez?( une?)? (?<action>.+(?= sur)) sur (l[aes]{0,2} |([lead]'))?(?<target>.+(?= mais)) mais(.+(?<resist>résiste).+moitié\..+subit (?<amount>\d+) points? de dégâts \(mitigés\)\.|.+(?<resist>résiste)\..+complètement!)$", SharedRegEx.DefaultOptions);

        public static readonly Regex ResistJa = new Regex(@"^(?<target>.+)は(?<source>.+)に「(?<action>.+)」　⇒　.+は(?<amount>\d+).+(?<resist>半減).+。$", SharedRegEx.DefaultOptions);

        public static readonly Regex ResistDe = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex EvadeEn = new Regex(@"^(The )?((?<target>.+(?= partially))|(?<target>.+(?= evades))) (?<partial>partially )?(?<evade>evades) (?<source>your|\w+\s\w+)(?:'s)? ((?!.+from)(?<action>\w+[\s\w+]{1,})(, taking (?<amount>\d+).+$|\.$)|(?<action>.+(?= from)) from the (?<direction>\w+)(, taking (?<amount>\d+).+$|\.$))", SharedRegEx.DefaultOptions);

        public static readonly Regex EvadeFr = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex EvadeJa = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex EvadeDe = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex DefeatsEn = new Regex(@"^(?<source>\w+(\s\w+)?) defeat(s)? (?:the )?(?<target>.+)('s (?<group>group))?\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex DefeatsFr = new Regex(@"^(?<source>\w+(\s\w+)?) a vaincu (l\w?'? ?)?(?<target>.+)\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex DefeatsJa = new Regex(@"^(?<source>.+)は(?<target>.+)を倒した。$", SharedRegEx.DefaultOptions);

        public static readonly Regex DefeatsDe = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex ObtainsEn = new Regex(@"((?!.+\d+ of \d+))((?!.+Total))^(?<whoGet>You|All|\w+\s\w+) (party members.+)?obtain (the )?(a( )?(n )?)?(?<item>(\d+(,)?\d+)|.+(?=\.))( )?(?<gil>gil)?.$|^(?<whoGet>Your|\w+\s\w+)('s)?.+contains (a(n)?)?( )?(?<item>.+(?=\.)).$", SharedRegEx.DefaultOptions);

        public static readonly Regex ObtainsFr = new Regex(@"((?!.+s'ajoute)^(?<whoGet>Vous|Tous) (obt\w+ (une? )?(?<item>.+(?=\.))|les \w+\s\w+ (?<item>.+(?=\.)))|(?!.+obt)(?<item>.+(?= s'a)) .+de (?<whoGet>.+(?=\.))\.)", SharedRegEx.DefaultOptions);

        public static readonly Regex ObtainsJa = new Regex(@"^(?<item>\d,\d+)ギルを得た。|「(?<item>.+)を入手した。$|^「(?<item>.+)が(?<who>.+)の戦利品目録に加わった。$", SharedRegEx.DefaultOptions);

        public static readonly Regex ObtainsDe = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex JoinEn = new Regex(@"^(?<who>\w+\s\w+)\s+joins the party", SharedRegEx.DefaultOptions);

        public static readonly Regex JoinFr = new Regex(@"^(?<who>Vous|\w+\s\w+) rejoint l'équipe.$", SharedRegEx.DefaultOptions);

        public static readonly Regex JoinJa = new Regex(@"^(?<who>.+)がパーティに参加しました。$", SharedRegEx.DefaultOptions);

        public static readonly Regex JoinDe = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex DisbandEn = new Regex(@"the party disbands", SharedRegEx.DefaultOptions);

        public static readonly Regex DisbandFr = new Regex(@"L'équipe est dissoute.", SharedRegEx.DefaultOptions);

        public static readonly Regex DisbandJa = new Regex(@"^パーティを解散しました。$", SharedRegEx.DefaultOptions);

        public static readonly Regex DisbandDe = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex LeftEn = new Regex(@"(?<who>you|\w+\s\w+)\s+(leaves|(is|are) ousted from) the party", SharedRegEx.DefaultOptions);

        public static readonly Regex LeftFr = new Regex(@"^(?<who>.+)はパーティから離脱しました。$", SharedRegEx.DefaultOptions);

        public static readonly Regex LeftJa = new Regex(@"^(?<who>Vous|\w+\s\w+) quitte l'équipe.$", SharedRegEx.DefaultOptions);

        public static readonly Regex LeftDe = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex MultiFlagEn = new Regex(@"^(?<source>You|\w+\s\w+) use(s)? (?<action>.+) on ((t|T)he )?(((?<target>.+)(?:'s)|(?<target>.+)) from the (?<direction>.+)\.$|(?<target>.+(?='s))('s)?\.$|(?<target>.+)\.$)", SharedRegEx.DefaultOptions);

        public static readonly Regex MultiFlagFr = new Regex(@"(?!.+et)^(?<source>.+) utilisez? (?<action>.+(?= sur)) sur ((a|à)(\w+)? )?(([lead]')|(l[aes]{0,2} )?)?((?!.+\()(?<target>.+)|(?<target>.+(?= \()) \((?<direction>[\w\s]+)\))\. ?$", SharedRegEx.DefaultOptions);

        public static readonly Regex MultiFlagJa = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex MultiFlagDe = new Regex(@"^(?<source>.+) triffs?t d\w+ (?<target>.+(?= mit)) mit ((?<action>.+(?= von)) von (?<direction>.+)|(?<action>.+))\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex MultiEn = new Regex(@"(?!.+uses)^(?<crit>Critical! )?(The )?((?<target>.+)('s) takes (?<amount>\d+).+\.$|(?<target>.+(?= takes)) takes (?<amount>\d+).+\.$)", SharedRegEx.DefaultOptions);

        public static readonly Regex MultiFr = new Regex(@"(?!.+util)^(?<crit>Coup critique! )?(((L[aes]{0,2} |(a|à)(\w+)? )?(L\')?(?<target>.+))) subit (?<amount>\d+) points?.+\. ?$", SharedRegEx.DefaultOptions);

        public static readonly Regex MultiJa = new Regex(@"^\.$", SharedRegEx.DefaultOptions);

        public static readonly Regex MultiDe = new Regex(@"^　⇒　((?<crit>Kritischer Treffer!) (?<amount>\d+) |(?<amount>\d+) )Punkte? Schaden\.$", SharedRegEx.DefaultOptions);
    }
}
