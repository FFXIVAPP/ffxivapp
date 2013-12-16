// FFXIVAPP.Client
// Player.Handlers.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Plugins.Parse.Enums;
using FFXIVAPP.Client.Plugins.Parse.Helpers;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Common.Core.Memory;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.Utilities;
using NLog;

namespace FFXIVAPP.Client.Plugins.Parse.Models.StatGroups
{
    public partial class Player
    {
        /// <summary>
        /// </summary>
        /// <param name="statusEntriesMonster"></param>
        /// <param name="isYou"></param>
        private void ProcessDamageOverTime(IEnumerable<StatusEntry> statusEntriesMonster, bool isYou)
        {
            foreach (var statusEntry in statusEntriesMonster)
            {
                try
                {
                    var statusInfo = StatusEffectHelper.StatusInfo(statusEntry.StatusID);
                    var statusKey = "";
                    switch (Settings.Default.GameLanguage)
                    {
                        case "English":
                            statusKey = statusInfo.Name.English;
                            break;
                        case "French":
                            statusKey = statusInfo.Name.French;
                            break;
                        case "German":
                            statusKey = statusInfo.Name.German;
                            break;
                        case "Japanese":
                            statusKey = statusInfo.Name.Japanese;
                            break;
                    }
                    if (String.IsNullOrWhiteSpace(statusKey))
                    {
                        continue;
                    }
                    var amount = NPCEntry.Level * 3.5m;
                    var key = statusKey;
                    DamageOverTimeAction actionData = null;
                    foreach (var damageOverTimeAction in DamageOverTimeHelper.PlayerActions.ToList()
                                                                             .Where(d => String.Equals(d.Key, key, Constants.InvariantComparer)))
                    {
                        actionData = damageOverTimeAction.Value;
                    }
                    if (actionData == null)
                    {
                        continue;
                    }
                    var zeroFoundInList = false;
                    var bio = false;
                    foreach (var lastDamageAmountByAction in LastDamageAmountByAction.ToList())
                    {
                        if (Regex.IsMatch(key, @"(サンダ|foudre|blitz|thunder)", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase))
                        {
                            var found = false;
                            var thunderActions = DamageOverTimeHelper.ThunderActions;
                            var action = lastDamageAmountByAction;
                            if (thunderActions["III"].Any(thunderAction => String.Equals(action.Key, thunderAction, Constants.InvariantComparer)))
                            {
                                found = true;
                                amount = (action.Value / DamageOverTimeHelper.PlayerActions["thunder iii"].ActionPotency) * 30;
                            }
                            if (thunderActions["II"].Any(thunderAction => String.Equals(action.Key, thunderAction, Constants.InvariantComparer)))
                            {
                                found = true;
                                amount = (action.Value / DamageOverTimeHelper.PlayerActions["thunder ii"].ActionPotency) * 30;
                            }
                            if (thunderActions["I"].Any(thunderAction => String.Equals(action.Key, thunderAction, Constants.InvariantComparer)))
                            {
                                found = true;
                                amount = action.Value;
                            }
                            if (found)
                            {
                                break;
                            }
                        }
                        if (Regex.IsMatch(key, @"(バイオ|bactérie|bio)", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase))
                        {
                            bio = true;
                            var found = false;
                            var ruinActions = DamageOverTimeHelper.RuinActions;
                            var action = lastDamageAmountByAction;
                            if (ruinActions["II"].Any(ruinAction => String.Equals(action.Key, ruinAction, Constants.InvariantComparer)))
                            {
                                found = zeroFoundInList = true;
                                amount = action.Value;
                            }
                            if (ruinActions["I"].Any(ruinAction => String.Equals(action.Key, ruinAction, Constants.InvariantComparer)))
                            {
                                found = zeroFoundInList = true;
                                amount = action.Value;
                            }
                            if (found)
                            {
                                break;
                            }
                        }
                        if (String.Equals(lastDamageAmountByAction.Key, key, Constants.InvariantComparer))
                        {
                            amount = lastDamageAmountByAction.Value;
                            break;
                        }
                    }
                    statusKey = String.Format("{0} [•]", statusKey);
                    if (amount == 0)
                    {
                        amount = 75;
                    }
                    var tickDamage = Math.Ceiling(((amount / actionData.ActionPotency) * actionData.DamageOverTimePotency) / 3);
                    if (actionData.ZeroBaseDamageDOT && !zeroFoundInList)
                    {
                        var nonZeroActions = LastDamageAmountByAction.ToList()
                                                                     .Where(d => !d.Key.Contains("•"));
                        var keyValuePairs = nonZeroActions as IList<KeyValuePair<string, decimal>> ?? nonZeroActions.ToList();
                        amount = keyValuePairs.Sum(action => action.Value);
                        amount = amount / keyValuePairs.Count();
                        var damage = 0m;
                        switch (bio)
                        {
                            case true:
                                damage = Math.Ceiling(((amount / actionData.ActionPotency) * actionData.DamageOverTimePotency) * (NPCEntry.Level / 50m));
                                break;
                            case false:
                                damage = Math.Ceiling(((amount / actionData.ActionPotency) * actionData.DamageOverTimePotency) / 3);
                                break;
                        }
                        tickDamage = damage > 0 ? damage : tickDamage;
                    }
                    if (amount > 300)
                    {
                        tickDamage = Math.Ceiling(tickDamage / ((decimal) actionData.Duration / 3));
                    }
                    var line = new Line
                    {
                        Action = statusKey,
                        Source = Name,
                        Target = statusEntry.TargetName,
                        Amount = tickDamage,
                        EventDirection = EventDirection.Engaged,
                        EventSubject = isYou ? EventSubject.You : EventSubject.Party,
                        EventType = EventType.Damage
                    };
                    DispatcherHelper.Invoke(delegate
                    {
                        line.Hit = true;
                        ParseControl.Instance.Timeline.GetSetPlayer(line.Source)
                                    .SetDamage(line, true);
                        ParseControl.Instance.Timeline.GetSetMob(line.Target)
                                    .SetDamageTaken(line, true);
                    });
                }
                catch (Exception ex)
                {
                    Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
                }
            }
            StatusUpdateTimerProcessing = false;
        }
    }
}
