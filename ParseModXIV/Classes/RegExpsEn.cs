// ParseModXIV
// RegExpsEn.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System.Text.RegularExpressions;

namespace ParseModXIV.Classes
{
    public static class RegExpsEn
    {
        private const RegexOptions DefaultOptions = RegexOptions.Compiled | RegexOptions.ExplicitCapture;

        public static readonly Regex Damage = new Regex(@"(?!.+échou\.)^(?<crit>Coup critique! )?(?<whoHit>\w+\s\w+) util\w+ (une? )?((?<ability>.+(?= sur)) (?<didHit>sur) ((l\w+ |(a|à)(\w+)? )?(l\')?(?<mob>.+(?= \()) \((?<direction>.+(?=\)))\) (?<didHit>et) inflige (?<amount>\d+).+$|(l\w+ |a\w+ )?(l\')?(?<mob>.+(?= et)) et inflige (?<amount>\d+).+\.$)|(?<ability>.+(?= et inflige)) (?<didHit>et) inflige (?<amount>\d+).+dégâts ((a|à)(\w+)?)? (l\w+ )?(l\')?(?<mob>.+(?=\.$))\.$)|((?<whoHit>\w+\s\w+) util\w+ (une? )?(?<ability>.+(?= sur)) (?<didHit>sur) (l\w+ |(a|à)(\w+)? )?(l\')?(?<mob>.+(?= mais)) (?<didHit>mais) échoue!$)", DefaultOptions);

        public static readonly Regex Additional = new Regex(@"^A.+effect: (?<amount>\d+) p.+of.+dealt.$", DefaultOptions);

        public static readonly Regex DamageToPlayer = new Regex(@"(?!.+misses(\.)? )^(?<crit>Critical! )?(The )?(?<whoHit>[\w\s]+(?='s))('s) (?<ability>.+(?= hits)) (?<didHit>hits) (?<player>you(?!.+'s.+for)|\w+\s\w+) (from the (?<direction>\w+) )?for (?<amount>\d+).+\.$|(The )?(?<whoHit>[\w\s]+(?='s))('s) (?<ability>.+(?= misses)) (?<didHit>misses) (?<player>you(?!.+'s.+from)|\w+\s\w+)( from the (?<direction>\w+)\.$|\.$)", DefaultOptions);

        public static readonly Regex Counter = new Regex(@"^(?<counter>Counter! )?((?<whoHit>Your?(?!.+'s.+hits)|\w+\s\w+)(['s]{1,2})?) hit(s)? (the )?(?<mob>(.+(?= for))) for (?<amount>\d+) points of damage.$", DefaultOptions);

        //public static readonly Regex Food = new Regex(
        //    @"^(?<whoEat>You|\w+\s\w+) eat(s)? a(n)?(?<food>.+(?=\.)).$"
        //    , DefaultOptions);

        public static readonly Regex Recovers = new Regex(@"^(?<whoDid>Your?|\w+\s\w+) (recovers?) (?<amount>\d+) (?<type>\w+) from (?<ability>\w+[\s\w+]{0,})\.$", DefaultOptions);

        public static readonly Regex Engaged = new Regex(@"^(?:The )?(?<whoEngaged>[^\p{P}]+)('s (?<isGroup>group))?(?=( is| has been)).+engaged\.(.*)?$", DefaultOptions);

        public static readonly Regex Defeated = new Regex(@"^(?<whoDefeated>\w+(\s\w+)?) defeat(s)? (?:the )?(?<whatDefeated>.+)('s (?<isGroup>group))?\.$", DefaultOptions);

        public static readonly Regex Obtains = new Regex(@"((?!.+\d+ of \d+))((?!.+Total))^(?<whoGet>You|All|\w+\s\w+) (party members.+)?obtain (the )?(a( )?(n )?)?(?<item>(\d+(,)?\d+)|.+(?=\.))( )?(?<gil>gil)?.$|^(?<whoGet>Your|\w+\s\w+)('s)?.+contains (a(n)?)?( )?(?<item>.+(?=\.)).$", DefaultOptions);

        //public static readonly Regex Loot = new Regex(
        //    @"^(?<whoGet>Your|\w+\s\w+)('s)?.+contains (a(n)?)?( )?(?<item>.+(?=\.)).$"
        //    , DefaultOptions);

        public static readonly Regex Loses = new Regex(@"((?<monster>.+)) use(s)? (?<ability>[\w\s]+)? on (?<player>(\w+\s\w+)).+((?<loseGain>loses|gains) (?<amount>\d+) (?<type>\w+))\.", DefaultOptions);

        //public static readonly Regex UseOnMob = new Regex(
        //    @"((?<whoHit>You?|\w+\s\w+)(['s]{1,2})?) use(s)? (?<ability>[\w\s]+)? on (the )?(?<usedOn>(\w+([\s\w+'s]{1,})?)).+((?<loseGain>loses|gains) (?<amount>\d+) (?<type>\w+))?."
        //    , DefaultOptions);

        public static readonly Regex UseOnParty = new Regex(@"((?<whoDid>You?|\w+\s\w+)) use(s)? (((?<ability>.+(?= on)) on (?<castOn>\w+\s\w+)).+(?<recLoss>recover(s)?|lose(s)?) (?<amount>\d+) (?<type>\w+)\.)?(((?<ability>\w+[\s\w+]{0,})\.)(?! ))?(((?<ability>\w+[\s\w+]{0,})\.) (?<castOn>You) (?<recLoss>recover(s)?|lose(s)?) (?<amount>\d+) (?<type>\w+)\.)?", DefaultOptions);

        public static readonly Regex ResistsOrEvades = new Regex(@"^(The )?((?<whoEvaded>(.+(?= partially)))|(?<whoEvaded>(.+(?= resists)))|(?<whoEvaded>.+(?= evades))) ((?<resist>partially(?= resists))|(?<resist>resists)|(?<resist>evades)) (resists )?(?<whoHit>your|\w+\s\w+)( )?('s )?(?<ability>\w+[\s\w+]{1,})(,|\.)?( taking (?<amount>\d+) points of damage\.)?", DefaultOptions);

        //public static readonly Regex PlayerResistsOrEvades = new Regex(
        //    @"(?<player>You|\w+\s\w+(?= (partially|resist|evade))) (((?<resist>partially) resist(s)? (the )?(?<mob>.+(?='s))('s)? (?<ability>.+(?=,)), taking (?<amount>\d+).+\.$)|(?<resist>\w+) (the )?(?<mob>.+(?='s))'s (?<ability>.+(?=\.)\.$))"
        //    , DefaultOptions);

        //public static readonly Regex Parries = new Regex(
        //    @"(?<player>You|\w+\s\w+(?= (partially|parry|parries))) (((?<parry>partially)) parr(y|ies) (the )?(?<mob>.+(?='s))('s)? ((?<ability>.+(?= from)) from the (?<direction>\w+), taking (?<amount>\d+).+\.$|(?<ability>.+(?=,)), taking (?<amount>\d+).+\.$)|(?<parry>parr(y|ies)) (the )?(?<mob>.+(?='s))('s)? ((?<ability>.+(?= from)) from the (?<direction>\w+), taking (?<amount>\d+).+\.$|(?<ability>.+(?=,)), taking (?<amount>\d+).+\.$)|(?<parry>parry) (?<mob>.+(?='s))('s)? (?<ability>.+(?=\.))\.$)"
        //    , DefaultOptions);

        public static readonly Regex Blocks = new Regex(@"(?<player>You|\w+\s\w+(?= (partially|block|blocks))) (((?<block>partially)) block(s)? (the )?(?<whoHit>.+(?='s))('s)? ((?<ability>.+(?= from)) from the (?<direction>\w+), taking (?<amount>\d+).+\.$|(?<ability>.+(?=,)), taking (?<amount>\d+).+\.$)|(?<block>block(s)?) (the )?(?<whoHit>.+(?='s))('s)? ((?<ability>.+(?= from)) from the (?<direction>\w+), taking (?<amount>\d+).+\.$|(?<ability>.+(?=,)), taking (?<amount>\d+).+\.$)|(?<block>block(s)?) (?<whoHit>.+(?='s))('s)? ((?<ability>.+(?= from)) from the (?<direction>\w+)|(?<ability>.+(?=\.)\.$)))", DefaultOptions);

        //public static readonly Regex GrantsEffect = new Regex(
        //    @"^(?<whoDid>Your|\w+\s\w+)('s)? (?<ability>.+(?= grants)) grants (?<whoGot>you|\w+\s\w+) the effect of (?<effect>.+(?=\.))\.$"
        //    , DefaultOptions);

        public static readonly Regex JoinParty = new Regex(@"^(?<whoJoined>\w+\s\w+)\s+joins? the party", DefaultOptions);

        public static readonly Regex DisbandParty = new Regex(@"the party disbands", DefaultOptions);

        public static readonly Regex LeaveParty = new Regex(@"(?<whoLeft>you|\w+\s\w+)\s+(leaves?|(is|are) ousted from) the party", DefaultOptions);

        //public static readonly Regex Incap = new Regex(
        //    @"^(?<mob>.+(?='s))'s (?<bodypart>.+(?= is)) is incapacitated.$"
        //    , DefaultOptions);

        //public static readonly Regex InflictOnMob = new Regex(
        //    @"^(?<whoDid>Your|\w+\s\w+)('s)? (?<ability>.+(?= inflicts)) inflicts (?<mob>.+(?= with)) w.+effect of((?!.+Regimen) (?<effect>.+(?=\.)).$| Regimen of (?<regimen>.+(?=\.)).$)"
        //    , DefaultOptions);

        //public static readonly Regex Experience = new Regex(
        //    @"(?<whoGet>You)[\s\w]{0,}\s(?<amount>\d+).+\.$"
        //    , DefaultOptions);

        //public static readonly Regex NoLongerEffectsMob = new Regex(
        //    @"(The )?((?!.+is no)(?<mob>.+(?= no)).+suffers.+effect of (?!.+Regimen)(?<effect>.+(?=\.))\.$|(?<mob>.+(?= is)).+longer (?<effect>.+(?=\.))\.$)"
        //    , DefaultOptions);

        //public static readonly Regex NoLongerEffectsPlayer = new Regex(
        //    @"^(?<player>You|\w+\s\w+) (are.+longer (under.+effect of (?<effect>.+(?=\.))\.$|(?<effect>.+(?=\.))\.$)|no longer suffer(s)?.the.+effect of (?<effect>.+(?=\.))\.$|is.+longer (under.+effect of (?<effect>.+(?=\.))\.$|(?<effect>.+(?=\.))\.$))"
        //    , DefaultOptions);

        //public static readonly Regex Absorbs = new Regex(
        //    @"^(The )?(?<mob>.+(?= absorbs)).absorbs.(?<amount>\d+) (?<type>\w+) from (?<player>.+(?=\.))\.$"
        //    , DefaultOptions);
    }
}