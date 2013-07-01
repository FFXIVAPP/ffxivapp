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

        Dictionary<string,StatGroup> Players = new Dictionary<string, StatGroup>();

        private void UpdateTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            var partyOverall = ParseControl.Instance.Timeline.Overall;
            DPSModel.AddDataPoint((double) (partyOverall.Stats.GetStatValue("TotalOverallDamage") / ParseControl.Instance.Timeline.Party.Count));
            var partyMembers = ParseControl.Instance.Timeline.Party;
            foreach (var partyMember in partyMembers)
            {
                if (!Players.ContainsKey(partyMember.Name))
                {
                    Players.Add(partyMember.Name, partyMember);
                    DamageModel.Model.Series.Add(new LineSeries(partyMember.Name));
                }
                
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
