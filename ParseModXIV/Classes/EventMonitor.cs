using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParseModXIV.Stats;

namespace ParseModXIV.Classes
{
    public class EventMonitor : StatGroup
    {
        public String name { get; set; }
        protected DateTime lastEventReceived { get; set; }
        public UInt16 Filter { get; set; }

        public EventMonitor(String name) : base(name) {
            this.name = name;
            doInit();
            EventParser.Instance.OnLogEvent += FilterEvent;
        }

        private void doInit()
        {
            InitStats();
        }

        // Call base.InitStats() last in derived classes after you add all your stats
        protected virtual void InitStats()
        {
            foreach(var stat in Stats)
            {
                stat.OnValueChanged += DoStatChanged;
            }
        }

        protected void FilterEvent(object src, Event e)
        {
            if (e.MatchesFilter(Filter))
            {
                lastEventReceived = e.Timestamp;
                HandleEvent(e);
            }
        }

        protected virtual void HandleEvent(Event e)
        {
        }

        // HOOK INTO THIS EVENT IN THE GUI OR ELSEWHERE IF YOU WANT TO GET NOTIFIED WHENEVER A STAT IS UPDATED!
        public event EventHandler<StatChangedEvent> OnStatChanged;   

        protected virtual void DoStatChanged(object src, StatChangedEvent e)
        {
            var onStatChange = OnStatChanged;
            if(onStatChange != null) OnStatChanged(this, e);
        }
    }
}
