using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ParseModXIV.Classes;

namespace ParseModXIV.Monitors
{
    class TimelineMonitor : EventMonitor
    {
        private readonly Regex partyJoinRegex = new Regex(@"^(\w+\s\w+)\s+joins the party", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly Regex partyDisbandRegex = new Regex(@"the party disbands", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly Regex partyLeaveRegex = new Regex(@"(?<whoLeft>you|\w+\s\w+)\s+leaves? the party", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public TimelineMonitor() : base("Timeline")
        {
            Filter = ((UInt16)EventType.Notice | (UInt16) EventSubject.You | (UInt16) EventDirection.On);
        }

        protected override void HandleEvent(Event e)
        {
            var matches = partyJoinRegex.Match(e.RawLine);
            if(matches.Success)
            {
                var whoJoined = matches.Groups[1].Captures[0];
                EventParser.Instance.DoPartyChanged(this, new PartyEventArgs(PartyEventArgs.PartyEventType.Join, Convert.ToString(whoJoined)));
            }
            else if(partyDisbandRegex.Match(e.RawLine).Success)
            {
                EventParser.Instance.DoPartyChanged(this, new PartyEventArgs(PartyEventArgs.PartyEventType.Disband, String.Empty));
            }
            else
            {
                var leftParty = partyLeaveRegex.Match(e.RawLine);
                if (leftParty.Success)
                {
                    EventParser.Instance.DoPartyChanged(this, new PartyEventArgs(PartyEventArgs.PartyEventType.Leave, Convert.ToString(leftParty.Groups["whoLeft"].Value)));
                }
            }

        }
    }
}
