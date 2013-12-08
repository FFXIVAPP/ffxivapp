// FFXIVAPP.Client
// ParseControl.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Plugins.Parse.Models.Timelines;
using FFXIVAPP.Client.Plugins.Parse.Monitors;
using FFXIVAPP.Common.Core.Parse;
using Newtonsoft.Json;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Plugins.Parse.Models
{
    [DoNotObfuscate]
    public class ParseControl : IParsingControl, INotifyPropertyChanged
    {
        #region Auto Properties

        public DateTime StartTime { get; set; }
        public bool IsHistoryBased { get; set; }
        public bool FirstActionFound { get; set; }

        #endregion

        #region Declarations

        private ParseEntity LastParseEntity { get; set; }

        private Timer ParseEntityTimer = new Timer(100);

        #endregion

        public ParseControl(bool isHistoryBased = false)
        {
            IsHistoryBased = isHistoryBased;
            Timeline = new Timeline(this);
            TimelineMonitor = new TimelineMonitor(this);
            StatMonitor = new StatMonitor(this);
            StartTime = DateTime.Now;
            if (isHistoryBased)
            {
                return;
            }
            ParseEntityTimer.Elapsed += ParseEntityTimerOnElapsed;
            ParseEntityTimer.Start();
        }

        private bool ParseEntityTimerProcessing { get; set; }

        private void ParseEntityTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (ParseEntityTimerProcessing)
            {
                return;
            }
            ParseEntityTimerProcessing = true;
            var parseEntity = new ParseEntity
            {
                Players = new List<PlayerEntity>()
            };
            foreach (var playerEntity in Timeline.Party.Select(player => new PlayerEntity
            {
                Name = player.Name,
                DPS = (decimal) player.GetStatValue("DPS"),
                HPS = (decimal) player.GetStatValue("HPS"),
                DTPS = (decimal) player.GetStatValue("DTPS"),
                TotalOverallDamage = (decimal) player.GetStatValue("TotalOverallDamage"),
                TotalOverallHealing = (decimal) player.GetStatValue("TotalOverallHealing"),
                TotalOverallDamageTaken = (decimal) player.GetStatValue("TotalOverallDamageTaken"),
                PercentOfTotalOverallDamage = (decimal) player.GetStatValue("PercentOfTotalOverallDamage"),
                PercentOfTotalOverallHealing = (decimal) player.GetStatValue("PercentOfTotalOverallHealing"),
                PercentOfTotalOverallDamageTaken = (decimal) player.GetStatValue("PercentOfTotalOverallDamageTaken")
            }))
            {
                parseEntity.Players.Add(playerEntity);
            }
            parseEntity.DPS = (decimal) Timeline.Overall.GetStatValue("DPS");
            parseEntity.HPS = (decimal) Timeline.Overall.GetStatValue("HPS");
            parseEntity.DTPS = (decimal) Timeline.Overall.GetStatValue("DTPS");
            parseEntity.TotalOverallDamage = (decimal) Timeline.Overall.GetStatValue("TotalOverallDamage");
            parseEntity.TotalOverallHealing = (decimal) Timeline.Overall.GetStatValue("TotalOverallHealing");
            parseEntity.TotalOverallDamageTaken = (decimal) Timeline.Overall.GetStatValue("TotalOverallDamageTaken");
            var notify = false;
            if (LastParseEntity == null)
            {
                LastParseEntity = parseEntity;
                notify = true;
            }
            else
            {
                var hash1 = JsonConvert.SerializeObject(LastParseEntity)
                                       .GetHashCode();
                var hash2 = JsonConvert.SerializeObject(parseEntity)
                                       .GetHashCode();
                if (!hash1.Equals(hash2))
                {
                    LastParseEntity = parseEntity;
                    notify = true;
                }
            }
            if (notify)
            {
                AppContextHelper.Instance.RaiseNewParseEntity(parseEntity);
            }
            ParseEntityTimerProcessing = false;
        }

        #region Implementation of IParsingControl

        private static ParseControl _instance;
        private StatMonitor _statMonitor;
        private Timeline _timeline;
        private TimelineMonitor _timelineMonitor;

        public static ParseControl Instance
        {
            get { return _instance ?? (_instance = new ParseControl()); }
            set { _instance = value; }
        }

        IParsingControl IParsingControl.Instance
        {
            get { return Instance; }
        }

        public Timeline Timeline
        {
            get { return _timeline ?? (_timeline = new Timeline(this)); }
            set
            {
                _timeline = value;
                RaisePropertyChanged();
            }
        }

        public StatMonitor StatMonitor
        {
            get { return _statMonitor ?? (_statMonitor = new StatMonitor(this)); }
            set
            {
                _statMonitor = value;
                RaisePropertyChanged();
            }
        }

        public TimelineMonitor TimelineMonitor
        {
            get { return _timelineMonitor ?? (_timelineMonitor = new TimelineMonitor(this)); }
            set
            {
                _timelineMonitor = value;
                RaisePropertyChanged();
            }
        }

        public void Initialize()
        {
        }

        public void Reset()
        {
            ParseEntityTimer.Stop();
            ParseEntityTimer.Elapsed -= ParseEntityTimerOnElapsed;
            if (!FirstActionFound)
            {
                FirstActionFound = true;
                StartTime = DateTime.Now;
            }
            else
            {
                FirstActionFound = false;
            }
            StatMonitor.Clear();
            Timeline.Clear();
            TimelineMonitor.Clear();
            var parseEntity = new ParseEntity
            {
                Players = new List<PlayerEntity>()
            };
            AppContextHelper.Instance.RaiseNewParseEntity(parseEntity);
            if (IsHistoryBased)
            {
                return;
            }
            ParseEntityTimer.Elapsed += ParseEntityTimerOnElapsed;
            ParseEntityTimer.Start();
        }

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion
    }
}
