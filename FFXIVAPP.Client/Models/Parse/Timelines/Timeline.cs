// FFXIVAPP.Client
// Timeline.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using FFXIVAPP.Client.Enums.Parse;
using FFXIVAPP.Client.Models.Parse.Fights;
using FFXIVAPP.Client.Models.Parse.LinkedStats;
using FFXIVAPP.Client.Models.Parse.StatGroups;
using FFXIVAPP.Client.Models.Parse.Stats;
using FFXIVAPP.Client.SettingsProviders.Parse;
using FFXIVAPP.Common.Utilities;
using NLog;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Models.Parse.Timelines
{
    [DoNotObfuscate]
    public sealed class Timeline : INotifyPropertyChanged
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Property Bindings

        private ParseControl _controller;
        private bool _deathFound;
        private bool _fightingRightNow;
        private FightList _fights;
        private string _lastEngaged;
        private string _lastKilled;
        private StatGroup _monster;
        private StatGroup _overall;
        private StatGroup _party;

        public bool FightingRightNow
        {
            get { return _fightingRightNow; }
            set
            {
                _fightingRightNow = value;
                RaisePropertyChanged();
            }
        }

        public bool DeathFound
        {
            get { return _deathFound; }
            private set
            {
                _deathFound = value;
                RaisePropertyChanged();
            }
        }

        public string LastKilled
        {
            get { return _lastKilled; }
            set
            {
                _lastKilled = value;
                RaisePropertyChanged();
            }
        }

        public string LastEngaged
        {
            get { return _lastEngaged; }
            set
            {
                _lastEngaged = value;
                RaisePropertyChanged();
            }
        }

        public FightList Fights
        {
            get { return _fights; }
            internal set
            {
                _fights = value;
                RaisePropertyChanged();
            }
        }

        private ParseControl Controller
        {
            get { return _controller; }
            set
            {
                _controller = value;
                RaisePropertyChanged();
            }
        }

        public StatGroup Overall
        {
            get { return _overall; }
            internal set
            {
                _overall = value;
                RaisePropertyChanged();
            }
        }

        public StatGroup Party
        {
            get { return _party; }
            internal set
            {
                _party = value;
                RaisePropertyChanged();
            }
        }

        public StatGroup Monster
        {
            get { return _monster; }
            internal set
            {
                _monster = value;
                RaisePropertyChanged();
            }
        }

        #region Player Curables Accessors and Setters

        private Dictionary<string, int> _playerCurables;

        private Dictionary<string, int> PlayerCurables
        {
            get { return _playerCurables; }
            set
            {
                _playerCurables = value;
                RaisePropertyChanged();
            }
        }

        public void TrySetPlayerCurable(string key, int value)
        {
            lock (PlayerCurables)
            {
                if (PlayerCurables.ContainsKey(key))
                {
                    PlayerCurables[key] = value;
                }
                else
                {
                    PlayerCurables.Add(key, value);
                }
            }
        }

        public int TryGetPlayerCurable(string key)
        {
            lock (PlayerCurables)
            {
                return PlayerCurables.ContainsKey(key) ? PlayerCurables[key] : 0;
            }
        }

        public void ClearPlayerCurables()
        {
            lock (PlayerCurables)
            {
                PlayerCurables.Clear();
            }
        }

        public Dictionary<string, int> GetPlayerCurables()
        {
            lock (PlayerCurables)
            {
                return PlayerCurables.ToDictionary(playerCurable => playerCurable.Key, playerCurable => playerCurable.Value);
            }
        }

        #endregion

        #endregion

        #region Implementation of TimelineChangedEvent

        public event EventHandler<TimelineChangedEvent> TimelineChangedEvent = delegate { };

        private void RaiseTimelineChangedEvent(object sender, TimelineChangedEvent e)
        {
            TimelineChangedEvent(sender, e);
        }

        #endregion

        #region Declarations

        public readonly Timer FightingTimer = new Timer(2500);
        public readonly Timer StoreHistoryTimer = new Timer(5000);

        #endregion

        /// <summary>
        /// </summary>
        public Timeline(ParseControl parseControl)
        {
            Controller = parseControl;
            FightingRightNow = false;
            DeathFound = false;
            Fights = new FightList();
            // setup party
            Overall = new StatGroup("Overall");
            Party = new StatGroup("Party")
            {
                IncludeSelf = false
            };
            Monster = new StatGroup("Monster")
            {
                IncludeSelf = false
            };
            PlayerCurables = new Dictionary<string, int>();
            SetStoreHistoryInterval();
            StoreHistoryTimer.Elapsed += StoreHistoryTimerOnElapsed;
            FightingTimer.Elapsed += FightingTimerOnElapsed;
            InitStats();
        }

        private void FightingTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            FightingRightNow = false;
            FightingTimer.Stop();
        }

        private void SetStoreHistoryInterval()
        {
            try
            {
                double interval;
                double.TryParse(Settings.Default.StoreHistoryInterval, out interval);
                StoreHistoryTimer.Interval = interval;
            }
            catch (Exception ex)
            {
            }
        }

        private void StoreHistoryTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            SetStoreHistoryInterval();
            if (Settings.Default.EnableStoreHistoryReset)
            {
                if (!FightingRightNow && !Controller.IsHistoryBased)
                {
                    Controller.Reset();
                }
            }
            StoreHistoryTimer.Stop();
        }

        /// <summary>
        /// </summary>
        /// <param name="monsterName"> </param>
        /// <returns> </returns>
        public Monster GetSetMonster(string monsterName)
        {
            if (!Monster.HasGroup(monsterName))
            {
                Logging.Log(Logger, String.Format("StatEvent : Adding new stat group for monster : {0}", monsterName));
                Monster.AddGroup(new Monster(monsterName, Controller));
            }
            var monster = (Monster) Monster.GetGroup(monsterName);
            return monster;
        }

        /// <summary>
        /// </summary>
        /// <param name="playerName"> </param>
        /// <returns> </returns>
        public Player GetSetPlayer(string playerName)
        {
            if (!Party.HasGroup(playerName))
            {
                Logging.Log(Logger, String.Format("StatEvent : Adding new stat group for player : {0}", playerName));
                Party.AddGroup(new Player(playerName, Controller));
            }
            var player = (Player) Party.GetGroup(playerName);
            return player;
        }

        /// <summary>
        /// </summary>
        /// <param name="eventType"> </param>
        /// <param name="eventArgs"> </param>
        public void PublishTimelineEvent(TimelineEventType eventType, params object[] eventArgs)
        {
            var args = (eventArgs != null && eventArgs.Any()) ? eventArgs[0] : "(no args)";
            Logging.Log(Logger, String.Format("TimelineEvent : {0} {1}", eventType, args));
            if (eventArgs != null)
            {
                var monsterName = eventArgs.First() as String;
                switch (eventType)
                {
                    case TimelineEventType.PartyJoin:
                    case TimelineEventType.PartyDisband:
                    case TimelineEventType.PartyLeave:
                        break;
                    case TimelineEventType.PartyMonsterFighting:
                    case TimelineEventType.AllianceMonsterFighting:
                    case TimelineEventType.OtherMonsterFighting:
                        DeathFound = false;
                        if (monsterName != null && (monsterName.ToLower()
                                                               .Contains("target") || monsterName == ""))
                        {
                            break;
                        }
                        Fight fighting;
                        if (!Fights.TryGet(monsterName, out fighting))
                        {
                            fighting = new Fight(monsterName);
                            Fights.Add(fighting);
                        }
                        Controller.Timeline.LastEngaged = monsterName;
                        break;
                    case TimelineEventType.PartyMonsterKilled:
                    case TimelineEventType.AllianceMonsterKilled:
                    case TimelineEventType.OtherMonsterKilled:
                        DeathFound = true;
                        if (monsterName != null && (monsterName.ToLower()
                                                               .Contains("target") || monsterName == ""))
                        {
                            break;
                        }
                        Fight killed;
                        if (!Fights.TryGet(monsterName, out killed))
                        {
                            killed = new Fight(monsterName);
                            Fights.Add(killed);
                        }
                        switch (eventType)
                        {
                            case TimelineEventType.PartyMonsterKilled:
                                GetSetMonster(monsterName)
                                    .SetKill(killed);
                                break;
                            case TimelineEventType.AllianceMonsterKilled:
                                GetSetMonster(monsterName)
                                    .SetKill(killed);
                                break;
                            case TimelineEventType.OtherMonsterKilled:
                                GetSetMonster(monsterName)
                                    .SetKill(killed);
                                break;
                        }
                        Controller.Timeline.LastKilled = monsterName;
                        break;
                }
            }
            RaiseTimelineChangedEvent(this, new TimelineChangedEvent(eventType, eventArgs));
        }

        private void InitStats()
        {
            Overall.Stats.Clear();
            var overallStats = OverallStats()
                .Select(s => s.Value)
                .ToList();
            Overall.Stats.AddStats(overallStats);
        }

        private Dictionary<string, Stat<decimal>> OverallStats()
        {
            var stats = new Dictionary<string, Stat<decimal>>();

            #region Player

            #region Combined

            stats.Add("CombinedTotalOverallDamage", new TotalStat("CombinedTotalOverallDamage"));
            stats.Add("CombinedDPS", new PerSecondAverageStat("CombinedDPS", stats["CombinedTotalOverallDamage"], Controller.IsHistoryBased));
            stats.Add("CombinedStaticPlayerDPS", new TotalStat("CombinedStaticPlayerDPS"));
            stats.Add("CombinedRegularDamage", new TotalStat("CombinedRegularDamage"));
            stats.Add("CombinedCriticalDamage", new TotalStat("CombinedCriticalDamage"));

            stats.Add("CombinedTotalOverallHealing", new TotalStat("CombinedTotalOverallHealing"));
            stats.Add("CombinedHPS", new PerSecondAverageStat("CombinedHPS", stats["CombinedTotalOverallHealing"], Controller.IsHistoryBased));
            stats.Add("CombinedStaticPlayerHPS", new TotalStat("CombinedStaticPlayerHPS"));
            stats.Add("CombinedRegularHealing", new TotalStat("CombinedRegularHealing"));
            stats.Add("CombinedCriticalHealing", new TotalStat("CombinedCriticalHealing"));

            stats.Add("CombinedTotalOverallDamageTaken", new TotalStat("CombinedTotalOverallDamageTaken"));
            stats.Add("CombinedDTPS", new PerSecondAverageStat("CombinedDTPS", stats["CombinedTotalOverallDamageTaken"], Controller.IsHistoryBased));
            stats.Add("CombinedStaticPlayerDTPS", new TotalStat("CombinedStaticPlayerDTPS"));
            stats.Add("CombinedRegularDamageTaken", new TotalStat("CombinedRegularDamageTaken"));
            stats.Add("CombinedCriticalDamageTaken", new TotalStat("CombinedCriticalDamageTaken"));

            #endregion

            // damage
            stats.Add("TotalOverallDamage", new TotalStat("TotalOverallDamage"));
            stats.Add("DPS", new PerSecondAverageStat("DPS", stats["TotalOverallDamage"], Controller.IsHistoryBased));
            stats.Add("StaticPlayerDPS", new TotalStat("StaticPlayerDPS"));
            stats.Add("RegularDamage", new TotalStat("RegularDamage"));
            stats.Add("CriticalDamage", new TotalStat("CriticalDamage"));

            stats.Add("TotalOverallDamageOverTime", new TotalStat("TotalOverallDamageOverTime"));
            stats.Add("DOTPS", new PerSecondAverageStat("DOTPS", stats["TotalOverallDamageOverTime"], Controller.IsHistoryBased));
            stats.Add("StaticPlayerDOTPS", new TotalStat("StaticPlayerDOTPS"));
            stats.Add("RegularDamageOverTime", new TotalStat("RegularDamageOverTime"));
            stats.Add("CriticalDamageOverTime", new TotalStat("CriticalDamageOverTime"));

            // healing
            stats.Add("TotalOverallHealing", new TotalStat("TotalOverallHealing"));
            stats.Add("HPS", new PerSecondAverageStat("HPS", stats["TotalOverallHealing"], Controller.IsHistoryBased));
            stats.Add("StaticPlayerHPS", new TotalStat("StaticPlayerHPS"));
            stats.Add("RegularHealing", new TotalStat("RegularHealing"));
            stats.Add("CriticalHealing", new TotalStat("CriticalHealing"));

            stats.Add("TotalOverallHealingOverHealing", new TotalStat("TotalOverallHealingOverHealing"));
            stats.Add("HOHPS", new PerSecondAverageStat("HOHPS", stats["TotalOverallHealingOverHealing"], Controller.IsHistoryBased));
            stats.Add("StaticPlayerHOHPS", new TotalStat("StaticPlayerHOHPS"));
            stats.Add("RegularHealingOverHealing", new TotalStat("RegularHealingOverHealing"));
            stats.Add("CriticalHealingOverHealing", new TotalStat("CriticalHealingOverHealing"));

            stats.Add("TotalOverallHealingOverTime", new TotalStat("TotalOverallHealingOverTime"));
            stats.Add("HOTPS", new PerSecondAverageStat("HOTPS", stats["TotalOverallHealingOverTime"], Controller.IsHistoryBased));
            stats.Add("StaticPlayerHOTPS", new TotalStat("StaticPlayerHOTPS"));
            stats.Add("RegularHealingOverTime", new TotalStat("RegularHealingOverTime"));
            stats.Add("CriticalHealingOverTime", new TotalStat("CriticalHealingOverTime"));

            stats.Add("TotalOverallHealingMitigated", new TotalStat("TotalOverallHealingMitigated"));
            stats.Add("HMPS", new PerSecondAverageStat("HMPS", stats["TotalOverallHealingMitigated"], Controller.IsHistoryBased));
            stats.Add("StaticPlayerHMPS", new TotalStat("StaticPlayerHMPS"));
            stats.Add("RegularHealingMitigated", new TotalStat("RegularHealingMitigated"));
            stats.Add("CriticalHealingMitigated", new TotalStat("CriticalHealingMitigated"));

            // damage taken
            stats.Add("TotalOverallDamageTaken", new TotalStat("TotalOverallDamageTaken"));
            stats.Add("DTPS", new PerSecondAverageStat("DTPS", stats["TotalOverallDamageTaken"], Controller.IsHistoryBased));
            stats.Add("StaticPlayerDTPS", new TotalStat("StaticPlayerDTPS"));
            stats.Add("RegularDamageTaken", new TotalStat("RegularDamageTaken"));
            stats.Add("CriticalDamageTaken", new TotalStat("CriticalDamageTaken"));

            stats.Add("TotalOverallDamageTakenOverTime", new TotalStat("TotalOverallDamageTakenOverTime"));
            stats.Add("DTOTPS", new PerSecondAverageStat("DTOTPS", stats["TotalOverallDamageTakenOverTime"], Controller.IsHistoryBased));
            stats.Add("StaticPlayerDTOTPS", new TotalStat("StaticPlayerDTOTPS"));
            stats.Add("RegularDamageTakenOverTime", new TotalStat("RegularDamageTakenOverTime"));
            stats.Add("CriticalDamageTakenOverTime", new TotalStat("CriticalDamageTakenOverTime"));

            // other
            stats.Add("TotalOverallActiveTime", new TotalStat("TotalOverallActiveTime"));
            stats.Add("TotalOverallTP", new TotalStat("TotalOverallTP"));
            stats.Add("TotalOverallMP", new TotalStat("TotalOverallMP"));

            #endregion

            #region Monster

            #region Combined

            stats.Add("CombinedTotalOverallDamageMonster", new TotalStat("CombinedTotalOverallDamageMonster"));
            stats.Add("CombinedDPSMonster", new PerSecondAverageStat("CombinedDPSMonster", stats["CombinedTotalOverallDamageMonster"], Controller.IsHistoryBased));
            stats.Add("CombinedStaticMonsterDPSMonster", new TotalStat("CombinedStaticMonsterDPSMonster"));
            stats.Add("CombinedRegularDamageMonster", new TotalStat("CombinedRegularDamageMonster"));
            stats.Add("CombinedCriticalDamageMonster", new TotalStat("CombinedCriticalDamageMonster"));

            stats.Add("CombinedTotalOverallHealingMonster", new TotalStat("CombinedTotalOverallHealingMonster"));
            stats.Add("CombinedHPSMonster", new PerSecondAverageStat("CombinedHPSMonster", stats["CombinedTotalOverallHealingMonster"], Controller.IsHistoryBased));
            stats.Add("CombinedStaticMonsterHPSMonster", new TotalStat("CombinedStaticMonsterHPSMonster"));
            stats.Add("CombinedRegularHealingMonster", new TotalStat("CombinedRegularHealingMonster"));
            stats.Add("CombinedCriticalHealingMonster", new TotalStat("CombinedCriticalHealingMonster"));

            stats.Add("CombinedTotalOverallDamageTakenMonster", new TotalStat("CombinedTotalOverallDamageTakenMonster"));
            stats.Add("CombinedDTPSMonster", new PerSecondAverageStat("CombinedDTPSMonster", stats["CombinedTotalOverallDamageTakenMonster"], Controller.IsHistoryBased));
            stats.Add("CombinedStaticMonsterDTPSMonster", new TotalStat("CombinedStaticMonsterDTPSMonster"));
            stats.Add("CombinedRegularDamageTakenMonster", new TotalStat("CombinedRegularDamageTakenMonster"));
            stats.Add("CombinedCriticalDamageTakenMonster", new TotalStat("CombinedCriticalDamageTakenMonster"));

            #endregion

            // damage
            stats.Add("TotalOverallDamageMonster", new TotalStat("TotalOverallDamageMonster"));
            stats.Add("DPSMonster", new PerSecondAverageStat("DPSMonster", stats["TotalOverallDamageMonster"], Controller.IsHistoryBased));
            stats.Add("StaticMonsterDPSMonster", new TotalStat("StaticMonsterDPSMonster"));
            stats.Add("RegularDamageMonster", new TotalStat("RegularDamageMonster"));
            stats.Add("CriticalDamageMonster", new TotalStat("CriticalDamageMonster"));

            stats.Add("TotalOverallDamageOverTimeMonster", new TotalStat("TotalOverallDamageOverTimeMonster"));
            stats.Add("DOTPSMonster", new PerSecondAverageStat("DOTPSMonster", stats["TotalOverallDamageOverTimeMonster"], Controller.IsHistoryBased));
            stats.Add("StaticMonsterDOTPSMonster", new TotalStat("StaticMonsterDOTPSMonster"));
            stats.Add("RegularDamageOverTimeMonster", new TotalStat("RegularDamageOverTimeMonster"));
            stats.Add("CriticalDamageOverTimeMonster", new TotalStat("CriticalDamageOverTimeMonster"));

            // healing
            stats.Add("TotalOverallHealingMonster", new TotalStat("TotalOverallHealingMonster"));
            stats.Add("HPSMonster", new PerSecondAverageStat("HPSMonster", stats["TotalOverallHealingMonster"], Controller.IsHistoryBased));
            stats.Add("StaticMonsterHPSMonster", new TotalStat("StaticMonsterHPSMonster"));
            stats.Add("RegularHealingMonster", new TotalStat("RegularHealingMonster"));
            stats.Add("CriticalHealingMonster", new TotalStat("CriticalHealingMonster"));

            stats.Add("TotalOverallHealingOverHealingMonster", new TotalStat("TotalOverallHealingOverHealingMonster"));
            stats.Add("HOHPSMonster", new PerSecondAverageStat("HOHPSMonster", stats["TotalOverallHealingOverHealingMonster"], Controller.IsHistoryBased));
            stats.Add("StaticMonsterHOHPSMonster", new TotalStat("StaticMonsterHOHPSMonster"));
            stats.Add("RegularHealingOverHealingMonster", new TotalStat("RegularHealingOverHealingMonster"));
            stats.Add("CriticalHealingOverHealingMonster", new TotalStat("CriticalHealingOverHealingMonster"));

            stats.Add("TotalOverallHealingOverTimeMonster", new TotalStat("TotalOverallHealingOverTimeMonster"));
            stats.Add("HOTPSMonster", new PerSecondAverageStat("HOTPSMonster", stats["TotalOverallHealingOverTimeMonster"], Controller.IsHistoryBased));
            stats.Add("StaticMonsterHOTPSMonster", new TotalStat("StaticMonsterHOTPSMonster"));
            stats.Add("RegularHealingOverTimeMonster", new TotalStat("RegularHealingOverTimeMonster"));
            stats.Add("CriticalHealingOverTimeMonster", new TotalStat("CriticalHealingOverTimeMonster"));

            stats.Add("TotalOverallHealingMitigatedMonster", new TotalStat("TotalOverallHealingMitigatedMonster"));
            stats.Add("HMPSMonster", new PerSecondAverageStat("HMPSMonster", stats["TotalOverallHealingMitigatedMonster"], Controller.IsHistoryBased));
            stats.Add("StaticMonsterHMPSMonster", new TotalStat("StaticMonsterHMPSMonster"));
            stats.Add("RegularHealingMitigatedMonster", new TotalStat("RegularHealingMitigatedMonster"));
            stats.Add("CriticalHealingMitigatedMonster", new TotalStat("CriticalHealingMitigatedMonster"));

            // damage taken
            stats.Add("TotalOverallDamageTakenMonster", new TotalStat("TotalOverallDamageTakenMonster"));
            stats.Add("DTPSMonster", new PerSecondAverageStat("DTPSMonster", stats["TotalOverallDamageTakenMonster"], Controller.IsHistoryBased));
            stats.Add("StaticMonsterDTPSMonster", new TotalStat("StaticMonsterDTPSMonster"));
            stats.Add("RegularDamageTakenMonster", new TotalStat("RegularDamageTakenMonster"));
            stats.Add("CriticalDamageTakenMonster", new TotalStat("CriticalDamageTakenMonster"));

            stats.Add("TotalOverallDamageTakenOverTimeMonster", new TotalStat("TotalOverallDamageTakenOverTimeMonster"));
            stats.Add("DTOTPSMonster", new PerSecondAverageStat("DTOTPSMonster", stats["TotalOverallDamageTakenOverTimeMonster"], Controller.IsHistoryBased));
            stats.Add("StaticMonsterDTOTPSMonster", new TotalStat("StaticMonsterDTOTPSMonster"));
            stats.Add("RegularDamageTakenOverTimeMonster", new TotalStat("RegularDamageTakenOverTimeMonster"));
            stats.Add("CriticalDamageTakenOverTimeMonster", new TotalStat("CriticalDamageTakenOverTimeMonster"));

            #endregion

            return stats;
        }

        /// <summary>
        /// </summary>
        public void Clear()
        {
            Fights.Clear();
            Overall.Clear();
            Party.Clear();
            Monster.Clear();
            InitStats();
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
