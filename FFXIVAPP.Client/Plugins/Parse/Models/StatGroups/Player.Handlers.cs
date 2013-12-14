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
                    decimal amount = NPCEntry.Level * 3;
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
                    foreach (var lastDamageAmountByAction in LastDamageAmountByAction.ToList())
                    {
                        if (Regex.IsMatch(key, @"(サンダ|foudre|blitz|thunder)", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase))
                        {
                            amount = 75;
                            var thunderActions = DamageOverTimeHelper.ThunderActions;
                            var action = lastDamageAmountByAction;
                            if (thunderActions["III"].Any(thunderAction => String.Equals(action.Key, thunderAction, Constants.InvariantComparer)))
                            {
                                amount = (lastDamageAmountByAction.Value / DamageOverTimeHelper.PlayerActions["thunder iii"].ActionPotency) * 30;
                            }
                            if (thunderActions["II"].Any(thunderAction => String.Equals(action.Key, thunderAction, Constants.InvariantComparer)))
                            {
                                amount = (lastDamageAmountByAction.Value / DamageOverTimeHelper.PlayerActions["thunder ii"].ActionPotency) * 30;
                            }
                            if (thunderActions["I"].Any(thunderAction => String.Equals(action.Key, thunderAction, Constants.InvariantComparer)))
                            {
                                amount = lastDamageAmountByAction.Value;
                            }
                        }
                        else if (String.Equals(lastDamageAmountByAction.Key, key, Constants.InvariantComparer))
                        {
                            amount = lastDamageAmountByAction.Value;
                        }
                    }
                    statusKey = String.Format("{0} [•]", statusKey);
                    if (amount == 0)
                    {
                        amount = 75;
                    }
                    var tickDamage = Math.Ceiling(((amount / actionData.ActionPotency) * actionData.DamageOverTimePotency) / 3);
                    if (actionData.ZeroBaseDamageDOT)
                    {
                        var nonZeroActions = LastDamageAmountByAction.ToList()
                                                                     .Where(d => !d.Key.Contains("•"))
                                                                     .Where(lastDamageAmountByAction => lastDamageAmountByAction.Value > 0);
                        var keyValuePairs = nonZeroActions as IList<KeyValuePair<string, decimal>> ?? nonZeroActions.ToList();
                        amount = keyValuePairs.Sum(action => action.Value);
                        amount = amount / keyValuePairs.Count();
                        var damage = Math.Ceiling(((amount / actionData.ActionPotency) * actionData.DamageOverTimePotency) / 3);
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
