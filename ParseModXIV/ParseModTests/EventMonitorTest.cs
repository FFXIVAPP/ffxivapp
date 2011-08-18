using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSpec;
using ParseModXIV.Classes;
using ParseModXIV.Monitors;
using ParseModXIV.Stats;

namespace ParseModTests
{
    public class EventMonitorTest : nspec
    {
        private Boolean gotEvent = false;
        private Decimal updatedValue;
        protected void OnStatChanged(object src, StatChangedEvent e)
        {
            if (e.SourceStat.Name != "Total") return;
            gotEvent = true;
            updatedValue = Convert.ToDecimal(e.NewValue);
        }

        void describe_DamageMonitor()
        {
            DamageMonitor dm = new DamageMonitor();
            dm.OnStatChanged += OnStatChanged;
            context["given a matching attack event"] = () =>
            {
                EventParser.Instance.ParseAndPublish(0x0050, "Critical! Your attack hits the something for 12345 points of damage.");
                it["should generate a dps event"] = () => gotEvent.should_be_true();
                it["should get the new value in the event"] = () => updatedValue.should_be(12345);
            };
        }
    }
}
