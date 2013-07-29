using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Threading;
using ParseModXIV.Classes;
using ParseModXIV.Stats;

namespace ParseModXIV.Monitors
{
    public class DamageMonitor : EventMonitor
    {
        private readonly Regex damageAmountRegex = new Regex(@"\A(?<crit>Critical! )?(?<whoHit>Your|\w+\s\w+)[^ ]* (?<ability>[\w\s]+) (?<didHit>hits|misses)\s(?<target>\w+\s\w+)('s)?( (?<bodypart>(from the left|right|rear|front)|\w+(\s\w+)?))?(\.| for (?<amount>\d+) points of damage)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly Regex name2Regex = new Regex(@"Your", RegexOptions.Compiled);
        protected readonly String[] statNames = {"Total", "C Total", "C Hits", "Hits", "Misses"};
        private readonly TotalStat partyDamage = new TotalStat("Overall Damage");
        protected readonly String[] linkedStats = { "Damage %", "Accuracy", "Min Damage", "Max Damage", "C Min Damage", "C Max Damage", "Critical %" };
         
        public DamageMonitor()
            : base("You")
        {
            Filter = ((UInt16)EventDirection.By | (UInt16)EventSubject.You | (UInt16)EventSubject.Party | (UInt16)EventType.Attack);
            EventParser.Instance.OnPartyChanged += (object src, PartyEventArgs e) => UpdatePartyList(e);
        }

        protected override void InitStats()
        {
            Stats.AddStats(NewStatList());
            base.InitStats();
        }

        protected void UpdatePartyList(PartyEventArgs e)
        {
            switch(e.EventType)
            {
                case PartyEventArgs.PartyEventType.Disband:
                    Clear();
                    break;
                case PartyEventArgs.PartyEventType.Join:
                    if(!HasGroup(e.Info))
                    {
                        var newGroup = new StatGroup(e.Info);
                        newGroup.Stats.AddStats(NewStatList());
                        AddGroup(newGroup);
                    }
                    break;
                case PartyEventArgs.PartyEventType.Leave:
                    if(EventParser.Instance.InParty)
                    {
                        if(HasGroup(e.Info)) Remove(GetGroup(e.Info));
                    }
                    else
                    {
                        Clear();
                    }
                    break;
            }
        }

        public Stat<Decimal>[] NewStatList()
        {
            var totalStat = new TotalStat("Total");
            partyDamage.AddDependency(totalStat);
            var critTotalStat = new TotalStat("C Total");
            var cHitStat = new TotalStat("C Hits");
            var hitStat = new TotalStat("Hits");
            var missStat = new TotalStat("Misses");
            var damagePctStat = new AccuracyStat("Damage %", totalStat, partyDamage);
            var accuracyStat = new AccuracyStat("Accuracy", hitStat, missStat);
            var minStat = new MinStat("Min Damage", totalStat);
            var maxStat = new MaxStat("Max Damage", totalStat);
            var cMinStat = new MinStat("C Min Damage", critTotalStat);
            var cMaxStat = new MaxStat("C Max Damage", critTotalStat);
            var critPctStat = new AccuracyStat("Critical %", cHitStat, hitStat);
            return new Stat<decimal>[] {totalStat, critTotalStat, cHitStat, hitStat, missStat, damagePctStat, accuracyStat, minStat, maxStat, cMinStat, cMaxStat, critPctStat};
        }

        protected override void HandleEvent(Event e)
        {
            Match mReg = damageAmountRegex.Match(e.RawLine);

            if (!mReg.Success) return;
            var didHit = Convert.ToString(mReg.Groups["didHit"].Value) == "hits";            
            var whoHit = Convert.ToString(mReg.Groups["whoHit"].Value);

            StatGroup target = null;
            if (whoHit == "Your") target = this;
            else
            {
                if(HasGroup(whoHit))
                {
                    target = GetGroup(whoHit);
                }/* else
                {
                    var stats = NewStatList();
                    target = new StatGroup(whoHit);
                    target.Stats.AddStats(stats);
                    Add(target);
                }*/
            }
            if (target == null) return;
            if(didHit)
            {
                var damage = Convert.ToDecimal(mReg.Groups["amount"].Value);
                var didCrit = Convert.ToString(mReg.Groups["crit"].Value) != String.Empty;
                target.Stats.GetStat("Total").Value += damage;
                target.Stats.GetStat("Hits").Value += 1;
                if (didCrit)
                {
                    target.Stats.GetStat("C Total").Value += damage;
                    target.Stats.GetStat("C Hits").Value += 1;
                }                
            }
            else
            {
                target.Stats.GetStat("Misses").Value += 1;
            }
        }
    }
}