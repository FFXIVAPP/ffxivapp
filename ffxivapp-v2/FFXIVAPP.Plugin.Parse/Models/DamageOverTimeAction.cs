// FFXIVAPP.Plugin.Parse
// DamageOverTimeAction.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Collections.Generic;
using System.Timers;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Plugin.Parse.Helpers;

#endregion

namespace FFXIVAPP.Plugin.Parse.Models
{
    public class DamageOverTimeAction : IDisposable
    {
        #region Auto Properties

        private Line Line { get; set; }
        private decimal OriginalAmount { get; set; }
        private int ActionPotency { get; set; }
        private int DamageOverTimePotency { get; set; }
        private int DefaultDuration { get; set; }
        private int TotalTicks { get; set; }
        private int CurrentTick { get; set; }
        private decimal TickDamage { get; set; }

        #endregion

        #region Declarations

        private readonly Timer _timer = new Timer(3000);

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="line"></param>
        public DamageOverTimeAction(Line line)
        {
            Line = line;
            OriginalAmount = Line.Amount;
            List<int> actionData;
            if (!ParseHelper.DamageOverTimeActions.TryGetValue(Line.Action, out actionData))
            {
                return;
            }
            ActionPotency = actionData[0];
            DamageOverTimePotency = actionData[1];
            DefaultDuration = actionData[2];
            TotalTicks = (int) Math.Ceiling(DefaultDuration / 3.0);
            TickDamage = (OriginalAmount / ActionPotency) * DamageOverTimePotency;
            _timer = new Timer(3000);
            _timer.Elapsed += TimerOnElapsed;
            _timer.Start();
        }

        /// <summary>
        /// </summary>
        public void Dispose()
        {
            _timer.Stop();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="elapsedEventArgs"></param>
        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (CurrentTick < TotalTicks)
            {
                _timer.Stop();
                if (TickDamage >= 300 && Line.Action.Contains("Thunder"))
                {
                    return;
                }
                Line.Amount = TickDamage;
                DispatcherHelper.Invoke(delegate
                {
                    ParseControl.Instance.Timeline.GetSetPlayer(Line.Source)
                                .SetDamageOverTime(Line);
                    ParseControl.Instance.Timeline.GetSetMob(Line.Target)
                                .SetPlayerDamageOverTime(Line);
                });
            }
            ++CurrentTick;
        }
    }
}
