using System;
using System.Text.RegularExpressions;
using ParseModXIV.Classes;
using ParseModXIV.Stats;

namespace ParseModXIV.Monitors
{
    public class DamageMonitor : EventMonitor
    {
        private readonly Regex critRegex = new Regex(@"Critical!", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly Regex missRegex = new Regex(@"misses", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly Regex damageAmountRegex = new Regex(@"for (\d+) points of damage", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly Regex name1Regex = new Regex(@"(^[a-zA-Z\s]*)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly Regex name2Regex = new Regex(@"Your", RegexOptions.Compiled);
        private CounterStat critStat, totalStat, missStat, hitStat;
                
        public DamageMonitor()
            : base("Party Total")
        {
            Filter = ((UInt16)EventDirection.By | (UInt16)EventSubject.You | (UInt16)EventSubject.Party | (UInt16)EventType.Attack);
            EventParser.Instance.OnPartyChanged += (object src, PartyEventArgs e) => UpdatePartyList(e);
        }

        protected override void InitStats()
        {
            critStat = new CounterStat("Criticals");
            missStat = new CounterStat("Misses");
            hitStat = new CounterStat("Hits");
            totalStat = new CounterStat("Total");
            Stats.AddStats(totalStat,
                     new AccuracyStat("Accuracy", hitStat, missStat),
                     new AccuracyStat("Critical %", critStat, hitStat),
                     critStat, missStat, hitStat,
                     new MaxStat("Max Damage", totalStat),
                     new MinStat("Min Damage", totalStat));
            base.InitStats();
        }

        protected void UpdatePartyList(PartyEventArgs e)
        {
            switch(e.EventType)
            {
                case PartyEventArgs.PartyEventType.Disband:
                    break;
                case PartyEventArgs.PartyEventType.Join:
                    break;
                case PartyEventArgs.PartyEventType.Leave:
                    break;
            }
        }

        protected override void HandleEvent(Event e)
        {
            Match mReg = damageAmountRegex.Match(e.RawLine);
            var name2Reg = name2Regex.Match(e.RawLine);
            var name1Reg = name1Regex.Match(e.RawLine);
            var missReg = missRegex.Match(e.RawLine);
            String cname = String.Empty;

            
            if (MainWindow.myWindow != null && name2Reg.Success)
            {
                cname = MainWindow.myWindow.guiYourFName.Text + " " + MainWindow.myWindow.guiYourLName.Text;
            }
            if (name1Reg.Success)
            {
                cname = name1Reg.Groups[1].Captures[0].Value;
            }

            if (mReg.Success)
            {
                var damage = mReg.Groups[1].Captures[0].Value;
                var critReg = critRegex.Match(e.RawLine);
                totalStat.Increment(Convert.ToDecimal(damage));
                hitStat.Increment();
                if (critReg.Success) critStat.Increment();
            }
            else if (missReg.Success) missStat.Increment();

        }
    }
}