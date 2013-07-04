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
using FFXIVAPP.Plugin.Parse.Models.StatGroups;

#endregion

namespace FFXIVAPP.Plugin.Parse.Models
{
    public class DamageOverTimeAction
    {
        #region Declarations

        private Line Line { get; set; }
        private decimal OriginalAmount { get; set; }
        private int DefaultDuration { get; set; }
        private Timer Timer { get; set; }
        private DateTime EventStartTime { get; set; }

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
            DefaultDuration = actionData[2];
            Timer = new Timer(DefaultDuration * 1000);
            Timer.Elapsed += TimerOnElapsed;
            EventStartTime = DateTime.Now;
            Timer.Start();
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            HandleDamage(false);
        }

        /// <summary>
        /// </summary>
        /// <param name="useActual"></param>
        public void HandleDamage(bool useActual = true)
        {
            Timer.Stop();
            var actualDuration = Convert.ToDecimal(DateTime.Now.Subtract(EventStartTime)
                                                           .TotalSeconds);
            Line.Amount = ParseHelper.GetDamageOverTime(Line, useActual ? actualDuration : 0);
            DispatcherHelper.Invoke(() => Player.SetDamageOverTime(Line));
        }
    }
}
