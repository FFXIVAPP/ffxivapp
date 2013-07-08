// FFXIVAPP.Plugin.Parse
// Timeline.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.Plugin.Parse.Enums;
using FFXIVAPP.Plugin.Parse.Models.Fights;
using FFXIVAPP.Plugin.Parse.Models.StatGroups;
using FFXIVAPP.Plugin.Parse.Models.Stats;
using NLog;

#endregion

namespace FFXIVAPP.Plugin.Parse.Models.Timelines
{
    public sealed class Timeline : INotifyPropertyChanged
    {
        #region Property Bindings

        private bool _fightingRightNow;

        public bool FightingRightNow
        {
            get { return _fightingRightNow; }
            private set
            {
                _fightingRightNow = value;
                RaisePropertyChanged();
            }
        }

        public List Fights { get; private set; }

        public StatGroup Overall { get; internal set; }

        public StatGroup Party { get; internal set; }

        public StatGroup Monster { get; internal set; }

        #endregion

        #region Events

        public event EventHandler<TimelineChangedEvent> TimelineChangedEvent = delegate { };

        private void RaiseTimelineChangedEvent(object sender, TimelineChangedEvent e)
        {
            TimelineChangedEvent(sender, e);
        }

        #endregion

        #region Declarations

        #endregion

        /// <summary>
        /// </summary>
        public Timeline()
        {
            FightingRightNow = false;
            Fights = new List();
            Overall = new StatGroup("Overall");
            Party = new StatGroup("Party")
            {
                IncludeSelf = false
            };
            Monster = new StatGroup("Monster")
            {
                IncludeSelf = false
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="mobName"> </param>
        /// <returns> </returns>
        public Monster GetSetMob(string mobName)
        {
            StatGroup statGroup;
            if (!Monster.TryGetGroup(mobName, out statGroup))
            {
                Logging.Log(LogManager.GetCurrentClassLogger(), String.Format("StatEvent : Adding new stat group for mob : {0}", mobName));
                statGroup = new Monster(mobName);
                Monster.AddGroup(statGroup);
            }
            return (Monster) statGroup;
        }

        /// <summary>
        /// </summary>
        /// <param name="playerName"> </param>
        /// <returns> </returns>
        public Player GetSetPlayer(string playerName)
        {
            StatGroup statGroup;
            if (!Party.TryGetGroup(playerName, out statGroup))
            {
                statGroup = new Player(playerName);
                Party.AddGroup(statGroup);
            }
            return (Player) statGroup;
        }

        /// <summary>
        /// </summary>
        /// <param name="eventType"> </param>
        /// <param name="eventArgs"> </param>
        public void PublishTimelineEvent(TimelineEventType eventType, params object[] eventArgs)
        {
            var args = (eventArgs != null && eventArgs.Any()) ? eventArgs[0] : "(no args)";
            Logging.Log(LogManager.GetCurrentClassLogger(), String.Format("TimelineEvent : {0} {1}", eventType, args));
            if (eventArgs != null)
            {
                var mobName = eventArgs.First() as String;
                switch (eventType)
                {
                    case TimelineEventType.PartyJoin:
                        if ((string) eventArgs[0] == "You")
                        {
                            eventArgs[0] = Common.Constants.CharacterName;
                        }
                        break;
                    case TimelineEventType.PartyDisband:
                        break;
                    case TimelineEventType.PartyLeave:
                        var whoLeft = eventArgs.Any() ? eventArgs.First() as string : String.Empty;
                        break;
                    case TimelineEventType.MobFighting:
                        if (mobName != null && (mobName.ToLower()
                                                       .Contains("target") || mobName == ""))
                        {
                            break;
                        }
                        Fight fighting;
                        if (!Fights.TryGetLastOrCurrent(mobName, out fighting))
                        {
                            var outOfOrderFight = new Fight(mobName);
                            Fights.Add(outOfOrderFight);
                        }
                        FightingRightNow = true;
                        break;
                    case TimelineEventType.MobKilled:
                        Fight killed;
                        if (Fights.TryGetLastOrCurrent(mobName, out killed))
                        {
                            GetSetMob(mobName)
                                .SetKill(killed);
                        }
                        else
                        {
                            var outOfOrderFight = new Fight(mobName);
                            Fights.Add(outOfOrderFight);
                            GetSetMob(mobName)
                                .SetKill(outOfOrderFight);
                        }
                        FightingRightNow = false;
                        break;
                }
            }
            RaiseTimelineChangedEvent(this, new TimelineChangedEvent(eventType, eventArgs));
        }

        /// <summary>
        /// </summary>
        public static void Clear()
        {
            ParseControl.Instance.Timeline.Fights.Clear();
            ParseControl.Instance.Timeline.Overall.Clear();
            ParseControl.Instance.Timeline.Party.Clear();
            ParseControl.Instance.Timeline.Monster.Clear();
        }

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion
    }
}
