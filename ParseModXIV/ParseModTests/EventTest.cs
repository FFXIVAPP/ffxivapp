using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSpec;

using ParseModXIV.Classes;

namespace ParseModTests
{
    public class describe_Event : nspec
    {
        void describe_IsUnknown() {
            context["given an empty event"] = () => {
                before = () => ev = new Event();
                it["should return true"] = () => ev.IsUnknown.should_be_true();
                context["with a non-null EventCode"] = () =>
                {
                    before = () =>
                    {
                        var ec = new EventCode("unknown event", 0x1234, null);
                        ev = new Event(ec);
                    };
                    it["should return true"] = () => ev.IsUnknown.should_be_true();
                };
            };
            context["given a known event"] = () =>
            {
                before = () =>
                {
                    var ec = new EventCode("known event", 0x0020, new EventGroup { Name = "Known Event Group", Direction = EventDirection.By, Subject = EventSubject.You, Type = EventType.SkillPoints });
                    ev = new Event(ec);
                };
                it["should return false"] = () => ev.IsUnknown.should_be_false();
            };
        }

        void describe_filtering()
        {
            context["given an event filter"] = () => {
                context["when it contains all types"] = () =>
                {
                    before = () => filter = (UInt16)EventParser.TYPE_MASK;
                    context["and a direction"] = () =>
                    {
                        before = () => filter |= (UInt16)EventDirection.By;
                        context["and a subject"] = () => {
                            before = () => filter |= (UInt16)EventSubject.You;
                            it["should match an event with that subject"] = () => evt.MatchesFilter(filter).should_be_true();
                            it["should not match an event with a different subject"] = () => evt2.MatchesFilter(filter).should_be_false();
                        };
                    };
                    context["and multiple subjects"] = () =>
                    {
                        before = () =>
                        {
                            filter |= (UInt16)EventSubject.You;
                            filter |= (UInt16)EventSubject.Enemy;
                        };
                        context["and multiple directions"] = () =>
                        {
                            before = () =>
                            {
                                filter |= (UInt16)EventDirection.By;
                                filter |= (UInt16)EventDirection.On;
                            };
                            it["should match an event with either subject"] = () => (evt.MatchesFilter(filter) && evt2.MatchesFilter(filter)).should_be_true();
                            it["should match an event with either direction"] = () => (evt.MatchesFilter(filter) && evt2.MatchesFilter(filter)).should_be_true();
                        };
                    };
                };
                context["when it contains everything"] = () =>
                {
                    before = () => filter = EventParser.ALL_EVENTS;
                    it["should match anything"] = () => evt.MatchesFilter(filter).should_be_true();
                };
            };
        }
 
        private UInt16 filter = 0x0000;
        private Event evt = new Event(new EventCode("Testing Event", 0x1234, new EventGroup { Direction = EventDirection.By, Type = EventType.Attack, Subject = EventSubject.You }));
        private Event evt2 = new Event(new EventCode("Testing Event2", 0x5678, new EventGroup { Direction = EventDirection.On, Type = EventType.Buff, Subject = EventSubject.Enemy }));

        private Event ev;
    }
}
