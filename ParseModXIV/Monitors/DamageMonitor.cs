using System;
using System.Text.RegularExpressions;
using ParseModXIV.Classes;
using ParseModXIV.Stats;

namespace ParseModXIV.Monitors
{
    class DamageMonitor : EventMonitor
    {
        private int damage = 0;
        private Boolean crit = false;
        private Boolean miss = false;
        private string cname;

        private readonly Regex critRegex = new Regex(@"Critical!", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly Regex missRegex = new Regex(@"misses", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly Regex damageAmountRegex = new Regex(@"for (\d+) points of damage", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly Regex name1Regex = new Regex(@"(^[a-zA-Z\s]*)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly Regex name2Regex = new Regex(@"Your", RegexOptions.Compiled);

        public int DAMAGE
        {
            get { return damage; }
            set { damage = value; }
        }

        public string NAME
        {
            get { return cname; }
            set { cname = value; }
        }

        public Boolean CRIT
        {
            get { return crit; }
            set { crit = value; }
        }

        public Boolean MISS
        {
            get { return miss; }
            set { miss = value; }
        }

        public event EventHandler<Event> OnDPSChange;

        public DamageMonitor()
            : base("DPS")
        {
            Filter = (UInt16)((UInt16)EventDirection.By | (UInt16)EventSubject.You | (UInt16)EventSubject.Party | (UInt16)EventType.Attack);
        }

        protected override void InitStats()
        {
            var critStat = new CounterStat("Criticals");
            var missStat = new CounterStat("Misses");
            var hitStat = new CounterStat("Hits");
            var totalStat = new CounterStat("Total", 10);
            AddStats(totalStat,
                     new AccuracyStat("Accuracy", hitStat, missStat),
                     new AccuracyStat("Critical %", critStat, hitStat),
                     critStat, missStat, hitStat,
                     new MaxStat("Max Damage", totalStat),
                     new MinStat("Min Damage", totalStat));
            base.InitStats();
        }

        protected override void HandleEvent(Event e)
        {
            Match mReg = damageAmountRegex.Match(e.RawLine);
            Match critReg = critRegex.Match(e.RawLine);
            Match missReg = missRegex.Match(e.RawLine);
            Match name1Reg = name1Regex.Match(e.RawLine);
            Match name2Reg = name2Regex.Match(e.RawLine);

            if (mReg.Success)
            {
                damage = Convert.ToInt32(mReg.Groups[1].Captures[0].Value);

                if (critReg.Success)
                {
                    crit = true;
                }
                else
                {
                    crit = false;
                }

                if (missReg.Success)
                {
                    miss = true;
                }
                else
                {
                    miss = false;
                }

                if (name2Reg.Success)
                {
                    cname = MainWindow.myWindow.guiYourFName.Text + " " + MainWindow.myWindow.guiYourLName.Text;
                }
                else if (name1Reg.Success)
                {
                    cname = name1Reg.Groups[1].Captures[0].Value;
                }

                EventHandler<Event> onTotal = OnDPSChange;
                if (onTotal != null)
                {
                    onTotal(this, e);
                }
            }
        }
    }
}