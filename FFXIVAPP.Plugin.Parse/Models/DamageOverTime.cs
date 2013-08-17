// FFXIVAPP.Plugin.Parse
// DamageOverTime.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Linq;
using System.Timers;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Plugin.Parse.Helpers;

#endregion

namespace FFXIVAPP.Plugin.Parse.Models
{
    public class DamageOverTime
    {
        public class Monster : IDisposable
        {
            public void Dispose()
            {
            }
        }

        public class Player : IDisposable
        {
            #region Auto Properties

            public Line Line { get; set; }
            private decimal OriginalAmount { get; set; }
            private int ActionPotency { get; set; }
            private int DamageOverTimePotency { get; set; }
            private int Duration { get; set; }
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
            public Player(Line line, out bool isValid)
            {
                Line = line;
                Timer = new Timer(3000);
                Timer.Elapsed += TimerOnElapsed;
                if (line.Amount > 0)
                {
                    OriginalAmount = Line.Crit ? ParseHelper.GetOriginalDamage(OriginalAmount, 50) : Line.Amount;
                }
                if (DamageOverTimeHelper.ZeroBaseDamageDOT.Any(action => Line.Action.ToLower()
                                                                             .Contains(action)))
                {
                    OriginalAmount = ParseControl.Instance.Timeline.GetSetPlayer(line.Source)
                                                 .LastDamageAmount;
                }
                var actionData = DamageOverTimeHelper.PlayerActions()[Line.Action.ToLower()];
                ActionPotency = actionData.ActionPotency;
                DamageOverTimePotency = actionData.DamageOverTimePotency;
                Duration = actionData.Duration;
                TotalTicks = (int) Math.Ceiling(Duration / 3.0);
                TickDamage = (OriginalAmount / ActionPotency) * DamageOverTimePotency;
                if (TickDamage >= 300 && DamageOverTimeHelper.Thunders.Any(action => Line.Action.ToLower()
                                                                                         .Contains(action)))
                {
                    isValid = false;
                    return;
                }
                Timer.Start();
                isValid = true;
            }

            /// <summary>
            /// </summary>
            public void Dispose()
            {
                Timer.Elapsed -= TimerOnElapsed;
                Timer.Stop();
            }

            /// <summary>
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="elapsedEventArgs"></param>
            private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
            {
                if (CurrentTick < TotalTicks && ParseControl.Instance.Timeline.FightingRightNow)
                {
                    Line.Amount = Math.Ceiling(TickDamage);
                    DispatcherHelper.Invoke(delegate
                    {
                        ParseControl.Instance.Timeline.GetSetPlayer(Line.Source)
                                    .SetDamageOverTime(Line);
                        ParseControl.Instance.Timeline.GetSetMob(Line.Target)
                                    .SetDamageOverTimeFromPlayer(Line);
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
}
