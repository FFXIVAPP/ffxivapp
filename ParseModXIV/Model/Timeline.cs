// Project: ParseModXIV
// File: Timeline.cs
// 
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.ComponentModel;
using System.Linq;
using NLog;
using ParseModXIV.Classes;
using ParseModXIV.Stats;

namespace ParseModXIV.Model
{
    public sealed class Timeline : INotifyPropertyChanged
    {
        private Boolean _fightingRightNow;
        private Boolean _playerInParty;
        private Logger Logger { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Boolean FightingRightNow
        {
            get { return _fightingRightNow; }
            private set
            {
                _fightingRightNow = value;
                DoPropertyChanged("FightingRightNow");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Boolean PlayerInParty
        {
            //get { return _playerInParty;  }
            get { return true; }
            set
            {
                //_playerInParty = value;
                _playerInParty = true;
                DoPropertyChanged("PlayerInParty");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public StatGroup OverallStats { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public StatGroup PlayerStats { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public StatGroup PartyStats { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public StatGroup InactivePartyStats { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public StatGroup MobStats { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public FightList Fights { get; private set; }

        public event EventHandler<TimelineEventArgs> OnTimelineEvent;

        /// <summary>
        /// 
        /// </summary>
        public Timeline()
        {
            Logger = LogManager.GetCurrentClassLogger();
            FightingRightNow = false;
            PlayerInParty = false;
            Fights = new FightList();
            OverallStats = new StatGroup("Overall");
            PlayerStats = new StatGroup(Settings.Default.CharacterName);
            Settings.Default.PropertyChanged += (src, e) =>
                                                    {
                                                        if (e.PropertyName != "CharacterName")
                                                        {
                                                            return;
                                                        }
                                                        var oldName = PlayerStats.Name;
                                                        PlayerStats.Name = Settings.Default.CharacterName;
                                                        StatGroup playerPartyGroup;
                                                        if (PartyStats.TryGetGroup(oldName, out playerPartyGroup))
                                                        {
                                                            playerPartyGroup.Name = Settings.Default.CharacterName;
                                                        }
                                                    };
            PartyStats = new StatGroup("Party") { IncludeSelf = false };
            InactivePartyStats = new StatGroup("Inactive Party Members") { IncludeSelf = false };
            MobStats = new StatGroup("Mobs") { IncludeSelf = false };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mobName"></param>
        /// <returns></returns>
        public StatGroupMob GetOrAddStatsForMob(string mobName)
        {
            StatGroup g;
            if (!MobStats.TryGetGroup(mobName, out g))
            {
                //logger.Trace("Adding new stat group for mob : {0}", mobName);
                g = new StatGroupMob(mobName);
                MobStats.AddGroup(g);
            }
            return (StatGroupMob) g;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="args"></param>
        public void PublishTimelineEvent(TimelineEventType t, params object[] args)
        {
            //logger.Trace("TimelineEvent : {0} {1}", t, (args != null && args.Any()) ? args[0] : "(no args)");
            var mobName = args.First() as String;
            switch (t)
            {
                case TimelineEventType.PartyJoin:
                    if ((string) args[0] == "You")
                    {
                        args[0] = Settings.Default.CharacterName;
                    }
                    if (!PlayerInParty)
                    {
                        PlayerInParty = true;
                    }
                    break;
                case TimelineEventType.PartyDisband:
                    PlayerInParty = false;
                    break;
                case TimelineEventType.PartyLeave:
                    var whoLeft = args.Any() ? args.First() as String : String.Empty;
                    if (!String.IsNullOrWhiteSpace(whoLeft) && whoLeft == Settings.Default.CharacterName)
                    {
                        PlayerInParty = false;
                    }
                    break;
                case TimelineEventType.MobFighting:
                    if (mobName != null && (mobName.ToLower().Contains("target") || mobName == ""))
                    {
                        break;
                    }
                    Fight eFight;
                    if (!Fights.TryGetLastOrCurrent(mobName, out eFight))
                    {
                        var outOfOrderFight = new Fight(mobName);
                        Fights.Add(outOfOrderFight);
                    }
                    FightingRightNow = true;
                    break;
                case TimelineEventType.MobKilled:
                    Fight dFight;
                    if (Fights.TryGetLastOrCurrent(mobName, out dFight))
                    {
                        GetOrAddStatsForMob(mobName).AddKillStats(dFight);
                    }
                    else
                    {
                        var outOfOrderFight = new Fight(mobName);
                        Fights.Add(outOfOrderFight);
                        GetOrAddStatsForMob(mobName).AddKillStats(outOfOrderFight);
                    }
                    FightingRightNow = false;
                    break;
            }
            var onTimelineEvent = OnTimelineEvent;
            if (onTimelineEvent != null)
            {
                onTimelineEvent(this, new TimelineEventArgs(t, args));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Clear()
        {
            ParseMod.Instance.Timeline.Fights.Clear();
            ParseMod.Instance.Timeline.InactivePartyStats.Clear();
            ParseMod.Instance.Timeline.MobStats.Clear();
            ParseMod.Instance.Timeline.OverallStats.Clear();
            ParseMod.Instance.Timeline.PartyStats.Clear();
            ParseMod.Instance.Timeline.PlayerStats.Clear();
        }

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void DoPropertyChanged(String name)
        {
            var propChanged = PropertyChanged;
            if (propChanged != null)
            {
                propChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion
    }
}