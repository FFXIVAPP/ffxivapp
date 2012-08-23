// FFXIVAPP
// Player.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System.Text.RegularExpressions;

namespace FFXIVAPP.Classes.RegExs
{
    public class Player
    {
        public static Regex ActionEn = new Regex(@"((?!.+misses\.)^(?<crit>Critical! )?(?<source>Your|\w+\s\w+)('s)? (?<action>.+(?= hits)) (?<hit>hits) (the )?((?!.+from)((?<target>.+)'s (?<part>.+)|(?<target>.+)) for |((?<target>.+)'s (?<part>.+)|(?<target>.+)) from the (?<direction>\w+) for )(?<amount>\d+) points? of damage\.$)|(^(?<source>Your|\w+\s\w+)('s)? (?<action>.+(?= misses)) (?<miss>misses) (the )?((?!.+from)((?<target>.+)'s (?<part>.+)|(?<target>.+))\.$|((?<target>.+)'s (?<part>.+)|(?<target>.+)) from the (?<direction>\w+)\.$))", Shared.DefaultOptions);

        public static Regex ActionFr = new Regex(@"(?!.+échou\.)^(?<crit>Coup critique! )?(?<source>.+) utilisez?( une?)? ((?<action>.+(?= sur)) (?<hit>sur) (([Ll]\w |([LEAD' ]+))?(l\')?(?<target>.+(?= \()) \((?<direction>.+(?=\)))\) (?<hit>et) inflige (?<amount>\d+).+$|([Ll]\w |([LEAD' ]+))?(?<target>.+(?= et)) et inflige (?<amount>\d+).+\.$)|(?<action>.+(?= et inflige)) (?<hit>et) inflige (?<amount>\d+).+dégâts ((a|à)(\w+)?)? ([Ll]\w |([LEAD' ]+))?(?<target>.+(?=\.$))\.$)|((?<source>\w+\s\w+) util\w+ (une? )?(?<action>.+(?= sur)) (?<hit>sur) ([Ll]\w |([LEAD' ]+))?((?<target>.+(?= \()) \((?<direction>.+(?=\)))\)|(?<target>.+)) (?<miss>mais) échoue!$)", Shared.DefaultOptions);

        public static Regex ActionJa = new Regex(@"^(?<source>.+)は(?<target>.+)(?<hit>に)((?<direction>.+)?(から)?「(?<action>.+)」|「(?<action>.+)」)　⇒　((?<crit>クリティカル！)　(?<amount>\d+)ダメージ。$|(?<amount>\d+)ダメージ。$)", Shared.DefaultOptions);

        public static Regex ActionDe = new Regex(@"^\.$", Shared.DefaultOptions);

        public static Regex UsedEn = new Regex(@"((?<source>You|\w+\s\w+)) use(s)? (((?<action>.+(?= on)) on (?<target>\w+\s\w+)).+(?<recloss>recover(s)?|lose(s)?) (?<amount>\d+) (?<type>\w+)\.)?(((?<action>\w+[\s\w+]{0,})\.)(?! ))?(((?<action>\w+[\s\w+]{0,})\.) (?<target>You) (?<recloss>recover(s)?|lose(s)?) (?<amount>\d+) (?<type>\w+)\.)?", Shared.DefaultOptions);

        public static Regex UsedFr = new Regex(@"^(?<source>\w+\s\w+) util\w+ (?<action>.+(?= sur)) sur (?<target>\w+\s\w+)\. ?\w+\s\w+\s?(?<recloss>récupère?|lose(s)?) (?<amount>\d+) (?<type>\w+)\.$", Shared.DefaultOptions);

        public static Regex UsedJa = new Regex(@"^(?<source>.+)は(?<target>.+)に「(?<action>.+)」　⇒　.+は(?<type>\w\w)を(?<amount>\d+)回復した。$", Shared.DefaultOptions);

        public static Regex UsedDe = new Regex(@"^\.$", Shared.DefaultOptions);

        public static Regex AdditionalEn = new Regex(@"^A.+effect: (?<amount>\d+) p.+of.+dealt.$", Shared.DefaultOptions);

        public static Regex AdditionalFr = new Regex(@"^Effet.+: (?<amount>\d+) p.+dégâts.$", Shared.DefaultOptions);

        public static Regex AdditionalJa = new Regex(@"^追加効果.+(?<amount>\d+)ダメージ。$", Shared.DefaultOptions);

        public static Regex AdditionalDe = new Regex(@"^\.$", Shared.DefaultOptions);

        public static Regex CounterEn = new Regex(@"^(?<counter>Counter! )(?<source>(.+(?:'s) |Your? ))(?<action>[\w\s]+) (?<hit>hits?)( the)? (?<target>.+(?= for)) for (?<amount>\d+) points of damage\.$", Shared.DefaultOptions);

        public static Regex CounterFr = new Regex(@"^(?<source>\w+\s\w+) (?<counter>contre) (l\w?'? ?)?(?<target>.+(?= et)) et.+inflige (?<amount>\d+).+$", Shared.DefaultOptions);

        public static Regex CounterJa = new Regex(@"^(?<source>.+)は(?<target>.+)の.+(?<counter>カウンター。)　⇒　(?<amount>\d+)ダメージを与えた。$", Shared.DefaultOptions);

        public static Regex CounterDe = new Regex(@"^\.$", Shared.DefaultOptions);

        public static Regex BlockEn = new Regex(@"^(?<target>(You|\w+\s\w+)) (?<partial>partially )?(?<block>block(s)?) ((t|T)he )?(?<source>.+(?='s))('s)? ((?<action>.+(?= from)) from the (?<direction>\w+)(, taking (?<amount>\d+).+$|\.$)|(?<action>.+), taking (?<amount>\d+).+$|(?<action>.+)\.$)", Shared.DefaultOptions);

        public static Regex BlockFr = new Regex(@"^([Ll]\w |([LEAD' ]+))?(?<source>.+) utilisez?( une?)? (?<action>.+) sur (?<target>\w+\s\w+)( \((?<direction>.+)\))? mais.+bouclier\.(.+par.+(?<block>coup)!$|.+(?<block>subit) (?<amount>\d+) points? de d.+\.$)", Shared.DefaultOptions);

        public static Regex BlockJa = new Regex(@"^\.$", Shared.DefaultOptions);

        public static Regex BlockDe = new Regex(@"^\.$", Shared.DefaultOptions);

        public static Regex ParryEn = new Regex(@"^(?<target>(You|\w+\s\w+)) (?<partial>partially )?(?<parry>parr(y|ies)?) ((t|T)he )?(?<source>.+(?='s))('s)? ((?<action>.+(?= from)) from the (?<direction>\w+)(, taking (?<amount>\d+).+$|\.$)|(?<action>.+), taking (?<amount>\d+).+$|(?<action>.+)\.$)", Shared.DefaultOptions);

        public static Regex ParryFr = new Regex(@"^([Ll]\w |([LEAD' ]+))?(?<source>[\w\s'-]+) utilise( une?)? (?<action>[\w\s'-]+) sur (?<target>\w+\s\w+)( \((?<direction>.+)\))? mais.+arrive à (?<parry>parer)\.(\w+ \w+ (?<partial>ne subit aucun).+!|\w+ \w+ subit (?<amount>\d+) points?.+\(mit.+\)\.)$", Shared.DefaultOptions);

        public static Regex ParryJa = new Regex(@"^\.$", Shared.DefaultOptions);

        public static Regex ParryDe = new Regex(@"^\.$", Shared.DefaultOptions);

        public static Regex ResistEn = new Regex(@"^(The )?((?<target>.+(?= partially))|(?<target>.+(?= resists))) (?<partial>partially )?(?<resist>resists) (?<source>your|\w+\s\w+)(?:'s)? ((?!.+from)(?<action>\w+[\s\w+]{1,})(, taking (?<amount>\d+).+$|\.$)|(?<action>.+(?= from)) from the (?<direction>\w+)(, taking (?<amount>\d+).+$|\.$))", Shared.DefaultOptions);

        public static Regex ResistFr = new Regex(@"^(?<source>.+) utilisez?( une?)? (?<action>.+(?= sur)) sur ([Ll]\w |([LEAD' ]+))?(?<target>.+(?= mais)) mais(.+(?<resist>résiste).+moitié\..+subit (?<amount>\d+) points? de dégâts \(mitigés\)\.|.+(?<resist>résiste)\..+complètement!)$", Shared.DefaultOptions);

        public static Regex ResistJa = new Regex(@"^(?<target>.+)は(?<source>.+)に「(?<action>.+)」　⇒　.+は(?<amount>\d+).+(?<resist>半減).+。$", Shared.DefaultOptions);

        public static Regex ResistDe = new Regex(@"^\.$", Shared.DefaultOptions);

        public static Regex EvadeEn = new Regex(@"^(The )?((?<target>.+(?= partially))|(?<target>.+(?= evades))) (?<partial>partially )?(?<evade>evades) (?<source>your|\w+\s\w+)(?:'s)? ((?!.+from)(?<action>\w+[\s\w+]{1,})(, taking (?<amount>\d+).+$|\.$)|(?<action>.+(?= from)) from the (?<direction>\w+)(, taking (?<amount>\d+).+$|\.$))", Shared.DefaultOptions);

        public static Regex EvadeFr = new Regex(@"^\.$", Shared.DefaultOptions);

        public static Regex EvadeJa = new Regex(@"^\.$", Shared.DefaultOptions);

        public static Regex EvadeDe = new Regex(@"^\.$", Shared.DefaultOptions);

        public static Regex DefeatsEn = new Regex(@"^(?<source>\w+(\s\w+)?) defeat(s)? (?:the )?(?<target>.+)('s (?<group>group))?\.$", Shared.DefaultOptions);

        public static Regex DefeatsFr = new Regex(@"^(?<source>\w+(\s\w+)?) a vaincu (l\w?'? ?)?(?<target>.+)\.$", Shared.DefaultOptions);

        public static Regex DefeatsJa = new Regex(@"^(?<source>.+)は(?<target>.+)を倒した。$", Shared.DefaultOptions);

        public static Regex DefeatsDe = new Regex(@"^\.$", Shared.DefaultOptions);

        public static Regex ObtainsEn = new Regex(@"((?!.+\d+ of \d+))((?!.+Total))^(?<whoGet>You|All|\w+\s\w+) (party members.+)?obtain (the )?(a( )?(n )?)?(?<item>(\d+(,)?\d+)|.+(?=\.))( )?(?<gil>gil)?.$|^(?<whoGet>Your|\w+\s\w+)('s)?.+contains (a(n)?)?( )?(?<item>.+(?=\.)).$", Shared.DefaultOptions);

        public static Regex ObtainsFr = new Regex(@"((?!.+s'ajoute)^(?<whoGet>Vous|Tous) (obt\w+ (une? )?(?<item>.+(?=\.))|les \w+\s\w+ (?<item>.+(?=\.)))|(?!.+obt)(?<item>.+(?= s'a)) .+de (?<whoGet>.+(?=\.))\.)", Shared.DefaultOptions);

        public static Regex ObtainsJa = new Regex(@"^(?<item>\d,\d+)ギルを得た。|「(?<item>.+)を入手した。$|^「(?<item>.+)が(?<who>.+)の戦利品目録に加わった。$", Shared.DefaultOptions);

        public static Regex ObtainsDe = new Regex(@"^\.$", Shared.DefaultOptions);

        public static Regex JoinEn = new Regex(@"^(?<who>\w+\s\w+)\s+joins the party", Shared.DefaultOptions);

        public static Regex JoinFr = new Regex(@"^(?<who>Vous|\w+\s\w+) rejoint l'équipe.$", Shared.DefaultOptions);

        public static Regex JoinJa = new Regex(@"^(?<who>.+)がパーティに参加しました。$", Shared.DefaultOptions);

        public static Regex JoinDe = new Regex(@"^\.$", Shared.DefaultOptions);

        public static Regex DisbandEn = new Regex(@"the party disbands", Shared.DefaultOptions);

        public static Regex DisbandFr = new Regex(@"L'équipe est dissoute.", Shared.DefaultOptions);

        public static Regex DisbandJa = new Regex(@"^パーティを解散しました。$", Shared.DefaultOptions);

        public static Regex DisbandDe = new Regex(@"^\.$", Shared.DefaultOptions);

        public static Regex LeftEn = new Regex(@"(?<who>you|\w+\s\w+)\s+(leaves|(is|are) ousted from) the party", Shared.DefaultOptions);

        public static Regex LeftFr = new Regex(@"^(?<who>.+)はパーティから離脱しました。$", Shared.DefaultOptions);

        public static Regex LeftJa = new Regex(@"^(?<who>Vous|\w+\s\w+) quitte l'équipe.$", Shared.DefaultOptions);

        public static Regex LeftDe = new Regex(@"^\.$", Shared.DefaultOptions);

        public static Regex MultiFlagEn = new Regex(@"^(?<source>You|\w+\s\w+) use(s)? (?<action>.+) on ((t|T)he )?(((?<target>.+)(?:'s)|(?<target>.+)) from the (?<direction>.+)\.$|(?<target>.+(?='s))('s)?\.$|(?<target>.+)\.$)", Shared.DefaultOptions);

        public static Regex MultiFlagFr = new Regex(@"(?!.+et)^(?<source>.+) utilisez? (?<action>.+(?= sur)) sur (l\w+|(a|à)(\w+)?)?(l\')? ?((?!.+\()(?<target>.+)|(?<target>.+(?= \()) \((?<direction>[\w\s]+)\))\. ?$", Shared.DefaultOptions);

        public static Regex MultiFlagJa = new Regex(@"^\.$", Shared.DefaultOptions);

        public static Regex MultiFlagDe = new Regex(@"^\.$", Shared.DefaultOptions);

        public static Regex MultiEn = new Regex(@"(?!.+uses)^(?<crit>Critical! )?(The )?((?<target>.+)('s) takes (?<amount>\d+).+\.$|(?<target>.+(?= takes)) takes (?<amount>\d+).+\.$)", Shared.DefaultOptions);

        public static Regex MultiFr = new Regex(@"(?!.+util)^(?<crit>Coup critique! )?((([Ll]\w+ |(a|à)(\w+)? )?(L\')?(?<target>.+))) subit (?<amount>\d+) points?.+\. ?$", Shared.DefaultOptions);

        public static Regex MultiJa = new Regex(@"^\.$", Shared.DefaultOptions);

        public static Regex MultiDe = new Regex(@"^\.$", Shared.DefaultOptions);
    }
}