using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSpec;
using Moq;
using ParseModXIV.Classes;

namespace ParseModTests
{
    class TestMonitor : EventMonitor
    {
        public Boolean gotEvent = false;

        public TestMonitor()
            : base("Test Monitor")
        {
            Filter = EventParser.ALL_EVENTS;
        }

        protected override void HandleEvent(Event e)
        {
            gotEvent = true;
        }
    }
    public class EventPublishingTest : nspec
    {
        void given_a_parsed_event()
        {
            TestMonitor testMonitor = new TestMonitor();
            context["when there is an EventMonitor that listens for it"] = () =>
            {
                EventParser.Instance.ParseAndPublish(0x0020, "this is a line");
                it["should receive the event"] = () => testMonitor.gotEvent.should_be_true();
            };
        }
    }
}
