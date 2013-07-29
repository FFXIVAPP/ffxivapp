// ParseModXIV
// RegExpsFr.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System.Text.RegularExpressions;

namespace ParseModXIV.Classes
{
    public static class RegExpsFr
    {
        private const RegexOptions DefaultOptions = RegexOptions.Compiled | RegexOptions.ExplicitCapture;

        public static readonly Regex Damage = new Regex(@"(?!.+échou\.)^(?<crit>Coup critique! )?(?<whoHit>\w+\s\w+) util\w+ (une? )?((?<ability>.+(?= sur)) (?<didHit>sur) ((l\w+ |(a|à)(\w+)? )?(l\')?(?<mob>.+(?= \()) \((?<direction>.+(?=\)))\) (?<didHit>et) inflige (?<amount>\d+).+$|(l\w+ |a\w+ )?(l\')?(?<mob>.+(?= et)) et inflige (?<amount>\d+).+\.$)|(?<ability>.+(?= et inflige)) (?<didHit>et) inflige (?<amount>\d+).+dégâts ((a|à)(\w+)?)? (l\w+ )?(l\')?(?<mob>.+(?=\.$))\.$)|((?<whoHit>\w+\s\w+) util\w+ (une? )?(?<ability>.+(?= sur)) (?<didHit>sur) (l\w+ |(a|à)(\w+)? )?(l\')?((?<mob>.+(?= \()) \((?<direction>.+(?=\)))\)|(?<mob>.+)) (?<didHit>mais) échoue!$)", DefaultOptions);

        public static readonly Regex Additional = new Regex(@"^Effet.+: (?<amount>\d+) p.+dégâts.$", DefaultOptions);

        public static readonly Regex DamageToPlayer = new Regex(@"(?!.+échou\.)(?!.+résiste.+)^(?<crit>Coup critique! )?(L\w?'? ?)?(?<whoHit>.+(?= util)) util\w+ (une? )?((?<ability>.+(?= sur)) (?<didHit>sur) (?<player>\w+\s\w+) (\((?<direction>.+(?=\)))\) et inflige (?<amount>\d+).+$|et inflige (?<amount>\d+).+$))", DefaultOptions);

        public static readonly Regex Counter = new Regex(@"^(?<whoHit>\w+\s\w+) contre (l\w?'? ?)?(?<mob>.+(?= et)) et lui inflige (?<amount>\d+).+$", DefaultOptions);


        public static readonly Regex Defeated = new Regex(@"^(?<whoDefeated>\w+(\s\w+)?) a vaincu (l\w?'? ?)?(?<whatDefeated>.+)\.$", DefaultOptions);

        public static readonly Regex Obtains = new Regex(@"((?!.+s'ajoute)^(?<whoGet>Vous|Tous) (obt\w+ (une? )?(?<item>.+(?=\.))|les \w+\s\w+ (?<item>.+(?=\.)))|(?!.+obt)(?<item>.+(?= s'a)) .+de (?<whoGet>.+(?=\.))\.)", DefaultOptions);

        public static readonly Regex UseOnParty = new Regex(@"^(?<whoDid>\w+\s\w+) util\w+ (?<ability>.+(?= sur)) sur (?<castOn>\w+\s\w+)\. ?\w+\s\w+\s?(?<recLoss>récupère?|lose(s)?) (?<amount>\d+) (?<type>\w+)\.$", DefaultOptions);

        public static readonly Regex Resists = new Regex(@"^(?<whoHit>\w+\s\w+|(L\w?'? ?)?.+) util\w+ (une? )?(?<ability>.+(?= sur)) (?<didHit>sur) (l\w?'? ?)?(?<whoEvaded>.+(?= mais)).+cel.+ci (?<resist>résiste) à moitié\..+subit (?<amount>\d+).+$", DefaultOptions);

        public static readonly Regex Blocks = new Regex(@"^(l|L)?\w+ (?<whoHit>.+) util.+un\w+ (?<ability>.+) sur (?<player>\w+\s\w+).+mais.+bouclier\.(.+par.+(?<block>coup)!$|.+(?<block>subit) (?<amount>\d+) points de d.+\.$)", DefaultOptions);

        public static readonly Regex JoinParty = new Regex(@"^(?<whoJoined>Vous|\w+\s\w+) rejoint l'équipe.$", DefaultOptions);

        public static readonly Regex DisbandParty = new Regex(@"L'équipe est dissoute.", DefaultOptions);

        public static readonly Regex LeaveParty = new Regex(@"^(?<whoLeft>Vous|\w+\s\w+) quitte l'équipe.$", DefaultOptions);
    }
}