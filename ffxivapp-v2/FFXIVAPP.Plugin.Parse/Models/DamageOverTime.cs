// FFXIVAPP.Plugin.Parse
// DamageOverTime.cs
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
    public class DamageOverTime : IDisposable
    {
        #region Auto Properties

        private Line Line { get; set; }
        private decimal OriginalAmount { get; set; }
        private int ActionPotency { get; set; }
        private int DamageOverTimePotency { get; set; }
        private int DefaultDuration { get; set; }
        private int TotalTicks { get; set; }
        private int CurrentTick { get; set; }
        public decimal TickDamage { get; set; }

        #endregion

        #region Declarations

        private Timer Timer { get; set; }

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="line"></param>
        /// <param name="isValid"></param>
        public DamageOverTime(Line line, out bool isValid)
        {
            Line = line;
            OriginalAmount = Line.Amount;
            List<int> actionData;
            if (!ParseHelper.DamageOverTimeActions.TryGetValue(Line.Action, out actionData))
            {
                isValid = false;
                return;
            }
            ActionPotency = actionData[0];
            DamageOverTimePotency = actionData[1];
            DefaultDuration = actionData[2];
            TotalTicks = (int) Math.Ceiling(DefaultDuration / 3.0);
            TickDamage = (OriginalAmount / ActionPotency) * DamageOverTimePotency;
            if (TickDamage >= 300 && Line.Action.Contains("Thunder"))
            {
                isValid = false;
                return;
            }
            Timer = new Timer(3000);
            Timer.Elapsed += TimerOnElapsed;
            Timer.Start();
            isValid = true;
        }

        /// <summary>
        /// </summary>
        public void Dispose()
        {
            Timer.Stop();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="elapsedEventArgs"></param>
        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (CurrentTick < TotalTicks)
            {
                Line.Amount = Math.Ceiling(TickDamage);
                DispatcherHelper.Invoke(delegate
                {
                    ParseControl.Instance.Timeline.GetSetPlayer(Line.Source)
                                .SetDamageOverTime(Line);
                    ParseControl.Instance.Timeline.GetSetMob(Line.Target)
                                .SetPlayerDamageOverTime(Line);
                });
            }
            else
            {
                Dispose();
            }
            ++CurrentTick;
        }
    }
}
