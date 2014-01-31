// FFXIVAPP.Client
// Player.Handlers.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FFXIVAPP.Client.Delegates;
using FFXIVAPP.Client.Enums.Parse;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Helpers.Parse;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Common.Core.Memory;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.Utilities;

namespace FFXIVAPP.Client.Models.Parse.StatGroups
{
    public partial class Player
    {
        #region Damage Over Time

        /// <summary>
        /// </summary>
        /// <param name="statusEntriesMonsters"></param>
        private void ProcessDamageOverTime(IEnumerable<StatusEntry> statusEntriesMonsters)
        {
            foreach (var statusEntry in statusEntriesMonsters)
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
                    var amount = NPCEntry.Level / ((60 - NPCEntry.Level) * .025m);
                    var key = statusKey;
                    XOverTimeAction actionData = null;
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
                    var bio = Regex.IsMatch(key, @"(バイオ|bactérie|bio)", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                    var thunder = Regex.IsMatch(key, @"(サンダ|foudre|blitz|thunder)", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                    var lastDamageAmountByActions = ParseHelper.LastAmountByAction.GetPlayer(Name)
                                                                   .ToList();
                    var resolvedPotency = 80;
                    var thunderDuration = 24;
                    var originalThunderDamage = 0m;
                    foreach (var lastDamageAmountByAction in lastDamageAmountByActions)
                    {
                        if (thunder)
                        {
                            var found = false;
                            var thunderActions = DamageOverTimeHelper.ThunderActions;
                            var action = lastDamageAmountByAction;
                            if (thunderActions["III"].Any(thunderAction => String.Equals(action.Key, thunderAction, Constants.InvariantComparer)))
                            {
                                found = true;
                                thunderDuration = DamageOverTimeHelper.PlayerActions["thunder iii"].Duration;
                                originalThunderDamage = action.Value;
                                amount = (action.Value / DamageOverTimeHelper.PlayerActions["thunder iii"].ActionPotency) * 30;
                            }
                            if (thunderActions["II"].Any(thunderAction => String.Equals(action.Key, thunderAction, Constants.InvariantComparer)))
                            {
                                found = true;
                                thunderDuration = DamageOverTimeHelper.PlayerActions["thunder ii"].Duration;
                                originalThunderDamage = action.Value;
                                amount = (action.Value / DamageOverTimeHelper.PlayerActions["thunder ii"].ActionPotency) * 30;
                            }
                            if (thunderActions["I"].Any(thunderAction => String.Equals(action.Key, thunderAction, Constants.InvariantComparer)))
                            {
                                found = true;
                                thunderDuration = DamageOverTimeHelper.PlayerActions["thunder"].Duration;
                                originalThunderDamage = action.Value;
                                amount = action.Value;
                            }
                            if (found)
                            {
                                
                                break;
                            }
                        }
                        if (bio)
                        {
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
                    resolvedPotency = zeroFoundInList ? resolvedPotency : bio ? resolvedPotency : actionData.ActionPotency;
                    var tickDamage = Math.Ceiling(((amount / resolvedPotency) * actionData.ActionOverTimePotency) / 3);
                    if (actionData.HasNoInitialResult && !zeroFoundInList)
                    {
                        var nonZeroActions = lastDamageAmountByActions.Where(d => !d.Key.Contains("•"));
                        var keyValuePairs = nonZeroActions as IList<KeyValuePair<string, decimal>> ?? nonZeroActions.ToList();
                        var damage = 0m;
                        switch (bio)
                        {
                            case true:
                                damage = Math.Ceiling(((amount / resolvedPotency) * actionData.ActionOverTimePotency) / 3);
                                break;
                            case false:
                                if (keyValuePairs.Any())
                                {
                                    amount = keyValuePairs.Sum(action => action.Value);
                                    amount = amount / keyValuePairs.Count();
                                }
                                damage = Math.Ceiling(((amount / resolvedPotency) * actionData.ActionOverTimePotency) / 3);
                                break;
                        }
                        tickDamage = damage > 0 ? damage : tickDamage;
                    }
                    if (originalThunderDamage > 300 && thunder)
                    {
                        tickDamage = Math.Ceiling(originalThunderDamage / (thunderDuration + 3));
                    }
                    var line = new Line
                    {
                        Action = statusKey,
                        Amount = tickDamage,
                        EventDirection = EventDirection.Unknown,
                        EventType = EventType.Damage,
                        EventSubject = EventSubject.Unknown,
                        Source = Name,
                        Target = statusEntry.TargetName
                    };
                    Controller.Timeline.FightingRightNow = true;
                    Controller.Timeline.FightingTimer.Stop();
                    Controller.Timeline.StoreHistoryTimer.Stop();
                    DispatcherHelper.Invoke(delegate
                    {
                        line.Hit = true;
                        var currentCritPercent = (double) Stats.GetStatValue("DamageCritPercent");
                        if (new Random().NextDouble() * 3 < currentCritPercent)
                        {
                            line.Crit = true;
                            line.Amount = line.Amount * 1.5m;
                        }
                        Controller.Timeline.GetSetPlayer(line.Source)
                                  .SetDamageOverTime(line);
                        Controller.Timeline.GetSetMonster(line.Target)
                                  .SetDamageTakenOverTime(line);
                    });
                }
                catch (Exception ex)
                {
                }
            }
            Controller.Timeline.FightingTimer.Start();
            Controller.Timeline.StoreHistoryTimer.Start();
        }

        #endregion

        #region Healing Over Time

        /// <summary>
        /// </summary>
        /// <param name="statusEntriesPlayers"></param>
        private void ProcessHealingOverTime(IEnumerable<StatusEntry> statusEntriesPlayers)
        {
            foreach (var statusEntry in statusEntriesPlayers)
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
                    var amount = NPCEntry.Level / ((60 - NPCEntry.Level) * .025m);
                    var key = statusKey;
                    XOverTimeAction actionData = null;
                    foreach (var healingOverTimeAction in HealingOverTimeHelper.PlayerActions.ToList()
                                                                               .Where(d => String.Equals(d.Key, key, Constants.InvariantComparer)))
                    {
                        actionData = healingOverTimeAction.Value;
                    }
                    if (actionData == null)
                    {
                        continue;
                    }
                    var zeroFoundInList = false;
                    var regen = Regex.IsMatch(key, @"(リジェネ|récup|regen|whispering|murmure|erhebendes|光の囁き)", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                    var healingHistoryList = ParseHelper.LastAmountByAction.GetPlayer(Name)
                                                        .ToList();
                    var resolvedPotency = 350;
                    foreach (var healingAction in healingHistoryList)
                    {
                        if (regen)
                        {
                            var found = false;
                            var cureActions = HealingOverTimeHelper.CureActions;
                            var medicaActions = HealingOverTimeHelper.MedicaActions;
                            var action = healingAction;
                            if (cureActions["III"].Any(cureAction => String.Equals(action.Key, cureAction, Constants.InvariantComparer)))
                            {
                                found = zeroFoundInList = true;
                                resolvedPotency = 550;
                            }
                            if (cureActions["II"].Any(cureAction => String.Equals(action.Key, cureAction, Constants.InvariantComparer)))
                            {
                                found = zeroFoundInList = true;
                                resolvedPotency = 650;
                            }
                            if (cureActions["I"].Any(cureAction => String.Equals(action.Key, cureAction, Constants.InvariantComparer)))
                            {
                                found = zeroFoundInList = true;
                                resolvedPotency = 400;
                            }
                            if (medicaActions["II"].Any(medicaAction => String.Equals(action.Key, medicaAction, Constants.InvariantComparer)))
                            {
                                found = zeroFoundInList = true;
                                resolvedPotency = 200;
                            }
                            if (medicaActions["I"].Any(medicaAction => String.Equals(action.Key, medicaAction, Constants.InvariantComparer)))
                            {
                                found = zeroFoundInList = true;
                                resolvedPotency = 300;
                            }
                            if (found)
                            {
                                if (action.Value > 0)
                                {
                                    amount = action.Value;
                                }
                                break;
                            }
                        }
                        if (String.Equals(healingAction.Key, key, Constants.InvariantComparer))
                        {
                            amount = healingAction.Value;
                            break;
                        }
                    }
                    statusKey = String.Format("{0} [•]", statusKey);
                    if (amount == 0)
                    {
                        amount = 75;
                    }
                    resolvedPotency = zeroFoundInList ? resolvedPotency : regen ? resolvedPotency : actionData.ActionPotency;
                    var tickHealing = Math.Ceiling(((amount / resolvedPotency) * actionData.ActionOverTimePotency) / 3);
                    if (actionData.HasNoInitialResult && !zeroFoundInList)
                    {
                        var nonZeroActions = healingHistoryList.Where(d => !d.Key.Contains("•"));
                        var keyValuePairs = nonZeroActions as IList<KeyValuePair<string, decimal>> ?? nonZeroActions.ToList();
                        var healing = 0m;
                        switch (regen)
                        {
                            case true:
                                healing = Math.Ceiling(((amount / resolvedPotency) * actionData.ActionOverTimePotency) / 3);
                                break;
                            case false:
                                if (keyValuePairs.Any())
                                {
                                    amount = keyValuePairs.Sum(action => action.Value);
                                    amount = amount / keyValuePairs.Count();
                                }
                                healing = Math.Ceiling(((amount / resolvedPotency) * actionData.ActionOverTimePotency) / 3);
                                break;
                        }
                        tickHealing = healing > 0 ? healing : tickHealing;
                    }
                    var line = new Line
                    {
                        Action = statusKey,
                        Amount = tickHealing,
                        EventDirection = EventDirection.Unknown,
                        EventType = EventType.Cure,
                        EventSubject = EventSubject.Unknown,
                        Source = Name
                    };
                    try
                    {
                        var players = Controller.Timeline.Party.ToList();
                        var entry = statusEntry;
                        foreach (var player in players.Where(player => player.Name.Contains(entry.TargetName)))
                        {
                            line.Target = player.Name;
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    if (String.IsNullOrWhiteSpace(line.Target))
                    {
                        line.Target = String.Format("[???] {0}", statusEntry.TargetName);
                    }
                    Controller.Timeline.FightingRightNow = true;
                    Controller.Timeline.FightingTimer.Stop();
                    switch (Settings.Default.StoreHistoryEvent)
                    {
                        case "Any":
                            Controller.Timeline.StoreHistoryTimer.Stop();
                            break;
                    }
                    DispatcherHelper.Invoke(delegate
                    {
                        line.Hit = true;
                        // resolve player hp each tick to ensure they are not at max
                        try
                        {
                            var players = PCWorkerDelegate.GetNPCEntities();
                            if (!players.Any())
                            {
                                return;
                            }
                            foreach (var actorEntity in players)
                            {
                                var playerName = actorEntity.Name;
                                Controller.Timeline.TrySetPlayerCurable(playerName, actorEntity.HPMax - actorEntity.HPCurrent);
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        var currentCritPercent = (double) Stats.GetStatValue("HealingCritPercent");
                        if (new Random().NextDouble() * 3 < currentCritPercent)
                        {
                            line.Crit = true;
                            line.Amount = line.Amount * 1.5m;
                        }
                        Controller.Timeline.GetSetPlayer(line.Source)
                                  .SetHealingOverTime(line);
                    });
                }
                catch (Exception ex)
                {
                }
            }
            Controller.Timeline.FightingTimer.Start();
            switch (Settings.Default.StoreHistoryEvent)
            {
                case "Any":
                    Controller.Timeline.StoreHistoryTimer.Start();
                    break;
            }
        }

        #endregion

        #region Buff Tracker

        private void ProcessBuffs(IEnumerable<StatusEntry> statusEntriesPlayers)
        {
            foreach (var statusEntry in statusEntriesPlayers)
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
                    var line = new Line
                    {
                        Action = statusKey,
                        Amount = 0,
                        EventDirection = EventDirection.Unknown,
                        EventType = EventType.Unknown,
                        EventSubject = EventSubject.Unknown,
                        Source = Name,
                    };
                    try
                    {
                        var players = Controller.Timeline.Party.ToList();
                        var entry = statusEntry;
                        foreach (var player in players.Where(player => player.Name.Contains(entry.TargetName)))
                        {
                            line.Target = player.Name;
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    if (String.IsNullOrWhiteSpace(line.Target))
                    {
                        line.Target = String.Format("[???] {0}", statusEntry.TargetName);
                    }
                    DispatcherHelper.Invoke(() => Controller.Timeline.GetSetPlayer(line.Source)
                                                            .SetBuff(line));
                }
                catch (Exception ex)
                {
                }
            }
        }

        #endregion
    }
}
