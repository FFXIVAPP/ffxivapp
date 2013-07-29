using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private Decimal updatedValue = 0;
        protected void OnStatChanged(object src, StatChangedEvent e)
        {
            if (e.SourceStat.Name != "Total") return;
            gotEvent = true;
            updatedValue = Convert.ToDecimal(e.NewValue);
        }

        void describe_DamageMonitor()
        {
            var dm = new DamageMonitor();
            dm.OnStatChanged += OnStatChanged;
            context["given a matching attack event"] = () =>
                                                           {
    //                                                           before = () => gotEvent = false;
  //              EventParser.Instance.ParseAndPublish(0x0050, "Critical! Your attack hits the something for 12345 points of damage.");
//               it["should generate a dps event"] = () => gotEvent.should_be_true();
//                it["should get the new value in the event"] = () => updatedValue.should_be(12345);
            };
            context["when one of a monitor's stats changes"] = () =>
                                                                   {
                                                                       before = () =>
                                                                                    {
                                                                                        gotEvent = false;
                                                                                        dm.PropertyChanged +=
                                                                                            (object src,
                                                                                             PropertyChangedEventArgs e)
                                                                                            =>
                                                                                                {
                                                                                                    if (
                                                                                                        e.PropertyName ==
                                                                                                        "Total")
                                                                                                        gotEvent = true;
                                                                                                };
                                                                                        dm.Stats.GetStat("Total").Value
                                                                                            = 20;
                                                                                    };
                                                                       it["should send a property changed notification"]
                                                                           = () => gotEvent.should_be_true();
                                                                   };
        }
    }
}
