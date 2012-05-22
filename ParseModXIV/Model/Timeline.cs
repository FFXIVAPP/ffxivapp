// ParseModXIV
// Timeline.cs
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
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

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
        public FightList Fights { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public StatGroup Overall { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public StatGroup Ability { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public StatGroup Healing { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public StatGroup Damage { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public StatGroup Monster { get; internal set; }

        public event EventHandler<TimelineEventArgs> OnTimelineEvent;

        /// <summary>
        /// 
        /// </summary>
        public Timeline()
        {
            FightingRightNow = false;
            Fights = new FightList();
            Overall = new StatGroup("Overall");
            Ability = new StatGroup("Ability") { IncludeSelf = false };
            Healing = new StatGroup("Healing") { IncludeSelf = false };
            Damage = new StatGroup("Damage") { IncludeSelf = false };
            Monster = new StatGroup("Monster") { IncludeSelf = false };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mobName"></param>
        /// <returns></returns>
        public StatGroupMonster GetSetMob(string mobName)
        {
            StatGroup g;
            if (!Monster.TryGetGroup(mobName, out g))
            {
                Logger.Debug("StatEvent : Adding new stat group for mob : {0}", mobName);
                g = new StatGroupMonster(mobName);
                Monster.AddGroup(g);
            }
            return (StatGroupMonster) g;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerName"></param>
        /// <param name="overall"></param>
        /// <returns></returns>
        public StatGroupAbility GetSetAbility(string playerName, Stat<Decimal> overall)
        {
            StatGroup g;
            if (!Ability.TryGetGroup(playerName, out g))
            {
                g = new StatGroupAbility(playerName, overall);
                Ability.AddGroup(g);
            }
            return (StatGroupAbility)g;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerName"></param>
        /// <param name="overall"></param>
        /// <returns></returns>
        public StatGroupHealing GetSetHealing(string playerName, Stat<Decimal> overall)
        {
            StatGroup g;
            if (!Healing.TryGetGroup(playerName, out g))
            {
                g = new StatGroupHealing(playerName, overall);
                Healing.AddGroup(g);
            }
            return (StatGroupHealing)g;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerName"></param>
        /// <param name="overall"></param>
        /// <returns></returns>
        public StatGroupDamage GetSetDamage(string playerName, Stat<Decimal> overall)
        {
            StatGroup g;
            if (!Damage.TryGetGroup(playerName, out g))
            {
                g = new StatGroupDamage(playerName, overall);
                Damage.AddGroup(g);
            }
            return (StatGroupDamage)g;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="args"></param>
        public void PublishTimelineEvent(TimelineEventType t, params object[] args)
        {
            Logger.Debug("TimelineEvent : {0} {1}", t, (args != null && args.Any()) ? args[0] : "(no args)");
            var mobName = args.First() as String;
            switch (t)
            {
                case TimelineEventType.PartyJoin:
                    if ((string) args[0] == "You")
                    {
                        args[0] = Settings.Default.CharacterName;
                    }
                    break;
                case TimelineEventType.PartyDisband:
                    break;
                case TimelineEventType.PartyLeave:
                    var whoLeft = args.Any() ? args.First() as String : String.Empty;
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
                        GetSetMob(mobName).AddKillStats(dFight);
                    }
                    else
                    {
                        var outOfOrderFight = new Fight(mobName);
                        Fights.Add(outOfOrderFight);
                        GetSetMob(mobName).AddKillStats(outOfOrderFight);
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
            ParseMod.Instance.Timeline.Overall.Clear();
            ParseMod.Instance.Timeline.Ability.Clear();
            ParseMod.Instance.Timeline.Healing.Clear();
            ParseMod.Instance.Timeline.Damage.Clear();
            ParseMod.Instance.Timeline.Monster.Clear();
            ParseMod.Instance.Timeline.Fights.Clear();
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