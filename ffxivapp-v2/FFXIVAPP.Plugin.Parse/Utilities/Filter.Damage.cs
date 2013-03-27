// FFXIVAPP.Plugin.Parse
// Filter.Damage.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Text.RegularExpressions;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.Plugin.Parse.Enums;
using FFXIVAPP.Plugin.Parse.Models;
using FFXIVAPP.Plugin.Parse.Models.Events;
using FFXIVAPP.Plugin.Parse.RegularExpressions;

#endregion

namespace FFXIVAPP.Plugin.Parse.Utilities
{
    public static partial class Filter
    {
        private static void ProcessDamage(Event e, Expressions exp)
        {
            var line = new Line();
            Match damage;
            if (e.Direction == EventDirection.From)
            {
                damage = exp.pDamage;
                switch (e.Subject)
                {
                    case EventSubject.You:
                        if (damage.Success)
                        {
                            line.Source = String.IsNullOrWhiteSpace(Common.Constants.CharacterName) ? "You" : Common.Constants.CharacterName;
                        }
                        break;
                    case EventSubject.Party:
                        if (damage.Success)
                        {
                            line.Source = _lastAttacker;
                        }
                        break;
                    case EventSubject.Other:
                        if (damage.Success)
                        {
                            line.Source = _lastAttacker;
                        }
                        break;
                    case EventSubject.Engaged:
                        break;
                    case EventSubject.UnEngaged:
                        break;
                }
                if (damage.Success)
                {
                    Logging.Log(NLog.LogManager.GetCurrentClassLogger(), String.Format("Damage Line -> {0}", exp.Cleaned));
                    line.Hit = true;
                    switch (damage.Groups["source"].Success)
                    {
                        case true:
                            line.Action = "Attack";
                            break;
                        case false:
                            line.Action = String.IsNullOrWhiteSpace(_lastAction) ? "Attack" : _lastAction;
                            break;
                    }
                    line.Amount = Convert.ToDecimal(damage.Groups["amount"].Value);
                    line.Crit = damage.Groups["crit"].Success;
                    line.Target = StringHelper.TitleCase(Convert.ToString(damage.Groups["target"].Value));
                    ParseControl.Instance.Timeline.PublishTimelineEvent(TimelineEventType.MobFighting, line.Target);
                    ParseControl.Instance.Timeline.GetSetMob(line.Target)
                                .SetPlayerStat(line);
                    ParseControl.Instance.Timeline.GetSetPlayer(line.Source)
                                .SetAbilityStat(line);
                }
                _lastAction = "";
            }
        }
    }
}