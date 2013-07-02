// FFXIVAPP.Plugin.Parse
// ParseChartsViewModel.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using FFXIVAPP.Plugin.Parse.Models;
using FFXIVAPP.Plugin.Parse.Models.Stats;
using OxyPlot.Series;

#endregion

namespace FFXIVAPP.Plugin.Parse.ViewModels
{
    internal sealed class ParseChartsViewModel : INotifyPropertyChanged
    {
        #region Property Bindings

        private static ParseChartsViewModel _instance;
        private PrivatePlotModel _damageModel;
        private PrivatePlotModel _damageTakenModel;
        private PrivatePlotModel _dpsModel;

        public static ParseChartsViewModel Instance
        {
            get { return _instance ?? (_instance = new ParseChartsViewModel()); }
        }

        public PrivatePlotModel DPSModel
        {
            get { return _dpsModel ?? (_dpsModel = new PrivatePlotModel()); }
            set
            {
                _dpsModel = value;
                RaisePropertyChanged();
            }
        }

        public PrivatePlotModel DamageModel
        {
            get { return _damageModel ?? (_damageModel = new PrivatePlotModel()); }
            set
            {
                _damageModel = value;
                RaisePropertyChanged();
            }
        }

        public PrivatePlotModel DamageTakenModel
        {
            get { return _damageTakenModel ?? (_damageTakenModel = new PrivatePlotModel()); }
            set
            {
                _damageTakenModel = value;
                RaisePropertyChanged();
            }
        }

        #endregion
                    
        #region Declarations

        private readonly Timer _updateTimer = new Timer(1000);

        #endregion

        private ParseChartsViewModel()
        {
            // setup graphs
            DPSModel = new PrivatePlotModel("DPS");
            DamageModel = new PrivatePlotModel("Damage");
            DamageTakenModel = new PrivatePlotModel("Damage Taken");
            _updateTimer.Elapsed += UpdateTimerOnElapsed;
            _updateTimer.Start();
        }

        readonly Dictionary<string,Dictionary<int,StatGroup>> _players = new Dictionary<string, Dictionary<int, StatGroup>>();

        private void UpdateTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            var partyMembers = ParseControl.Instance.Timeline.Party;
            for (var i = 0; i < partyMembers.Count; i++)
            {
                if (!_players.ContainsKey(partyMembers[i].Name))
                {
                    _players.Add(partyMembers[i].Name, new Dictionary<int, StatGroup>
                    {
                        {
                            _players.Count, partyMembers[i]
                        }
                    });
                    DamageModel.Model.Series.Add(new LineSeries(partyMembers[i].Name));
                }
                DamageModel.AddDataPoint(i, (double) partyMembers[i].Stats.GetStatValue("TotalOverallDamage"));
            }
            //DamageModel.AddDataPoint((double)partyOverall.Stats.GetStatValue("TotalOverallDamage"));
            //DamageTakenModel.AddDataPoint((double)partyOverall.Stats.GetStatValue("TotalOverallDamageTaken"));
        }

        #region Loading Functions

        #endregion

        #region Utility Functions

        #endregion

        #region Command Bindings

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
