// FFXIVAPP.Client
// Player.Stats.Healing.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Linq;
using System.Text.RegularExpressions;
using FFXIVAPP.Client.Plugins.Parse.Enums;
using FFXIVAPP.Client.Plugins.Parse.Helpers;
using FFXIVAPP.Client.Plugins.Parse.Models.Stats;
using FFXIVAPP.Client.Properties;

namespace FFXIVAPP.Client.Plugins.Parse.Models.StatGroups
{
    public partial class Player
    {
        /// <summary>
        /// </summary>
        /// <param name="line"> </param>
        /// <param name="healingType"></param>
        /// <param name="preProcessed"></param>
        public void SetHealing(Line line, HealingType healingType, bool preProcessed = false)
        {
            if (Name == Settings.Default.CharacterName && !Controller.IsHistoryBased)
            {
                //LineHistory.Add(new LineHistory(line));
            }

            IsActiveTimer.Start();

            var currentHealing = line.Crit ? line.Amount > 0 ? ParseHelper.GetOriginalAmount(line.Amount, (decimal) .5) : 0 : line.Amount;
            if (currentHealing > 0)
            {
                ParseHelper.LastAmountByAction.EnsurePlayerAction(line.Source, line.Action, currentHealing);
            }

            var unusedAmount = 0m;
            var originalAmount = line.Amount;
            if (!preProcessed)
            {
                // get curable of target
                try
                {
                    var cleanedName = Regex.Replace(line.Target, @"\[[\w]+\]", "")
                                           .Trim();
                    var curable = ParseControl.Instance.Timeline.TryGetPlayerCurable(cleanedName);
                    if (line.Amount > curable)
                    {
                        unusedAmount = line.Amount - curable;
                        line.Amount = curable;
                    }
                }
                catch (Exception ex)
                {
                }
            }

            var abilityGroup = GetGroup("HealingByAction");
            StatGroup subAbilityGroup;
            if (!abilityGroup.TryGetGroup(line.Action, out subAbilityGroup))
            {
                subAbilityGroup = new StatGroup(line.Action);
                subAbilityGroup.Stats.AddStats(HealingStatList(null));
                abilityGroup.AddGroup(subAbilityGroup);
            }
            var playerGroup = GetGroup("HealingToPlayers");
            StatGroup subPlayerGroup;
            if (!playerGroup.TryGetGroup(line.Target, out subPlayerGroup))
            {
                subPlayerGroup = new StatGroup(line.Target);
                subPlayerGroup.Stats.AddStats(HealingStatList(null));
                playerGroup.AddGroup(subPlayerGroup);
            }
            var abilities = subPlayerGroup.GetGroup("HealingToPlayersByAction");
            StatGroup subPlayerAbilityGroup;
            if (!abilities.TryGetGroup(line.Action, out subPlayerAbilityGroup))
            {
                subPlayerAbilityGroup = new StatGroup(line.Action);
                subPlayerAbilityGroup.Stats.AddStats(HealingStatList(subPlayerGroup, true));
                abilities.AddGroup(subPlayerAbilityGroup);
            }

            switch (preProcessed)
            {
                case true:

                    #region Was Pre-Processed

                    switch (healingType)
                    {
                        case HealingType.Normal:
                            break;
                        case HealingType.OverHealing:
                        case HealingType.HealingOverTime:
                        case HealingType.HealingOverTimeOverHealing:
                            if (line.Crit)
                            {
                                subAbilityGroup.Stats.IncrementStat("HealingCritHit");
                                subAbilityGroup.Stats.IncrementStat("CriticalHealing", line.Amount);
                                subPlayerGroup.Stats.IncrementStat("HealingCritHit");
                                subPlayerGroup.Stats.IncrementStat("CriticalHealing", line.Amount);
                                subPlayerAbilityGroup.Stats.IncrementStat("HealingCritHit");
                                subPlayerAbilityGroup.Stats.IncrementStat("CriticalHealing", line.Amount);
                                if (line.Modifier != 0)
                                {
                                    var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                                    var modStat = "HealingCritMod";
                                    subAbilityGroup.Stats.IncrementStat(modStat, mod);
                                    subPlayerGroup.Stats.IncrementStat(modStat, mod);
                                    subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
                                }
                            }
                            else
                            {
                                subAbilityGroup.Stats.IncrementStat("HealingRegHit");
                                subAbilityGroup.Stats.IncrementStat("RegularHealing", line.Amount);
                                subPlayerGroup.Stats.IncrementStat("HealingRegHit");
                                subPlayerGroup.Stats.IncrementStat("RegularHealing", line.Amount);
                                subPlayerAbilityGroup.Stats.IncrementStat("HealingRegHit");
                                subPlayerAbilityGroup.Stats.IncrementStat("RegularHealing", line.Amount);
                                if (line.Modifier != 0)
                                {
                                    var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                                    var modStat = "HealingRegMod";
                                    subAbilityGroup.Stats.IncrementStat(modStat, mod);
                                    subPlayerGroup.Stats.IncrementStat(modStat, mod);
                                    subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
                                }
                            }
                            break;
                        case HealingType.Mitigated:
                            Stats.IncrementStat("TotalHealingActionsUsed");
                            subAbilityGroup.Stats.IncrementStat("TotalHealingActionsUsed");
                            subPlayerGroup.Stats.IncrementStat("TotalHealingActionsUsed");
                            subPlayerAbilityGroup.Stats.IncrementStat("TotalHealingActionsUsed");

                            Stats.IncrementStat("TotalOverallHealing", line.Amount);
                            subAbilityGroup.Stats.IncrementStat("TotalOverallHealing", line.Amount);
                            subPlayerGroup.Stats.IncrementStat("TotalOverallHealing", line.Amount);
                            subPlayerAbilityGroup.Stats.IncrementStat("TotalOverallHealing", line.Amount);

                            Stats.IncrementStat("TotalOverallMitigatedHealing", line.Amount);
                            subAbilityGroup.Stats.IncrementStat("TotalOverallMitigatedHealing", line.Amount);
                            subPlayerGroup.Stats.IncrementStat("TotalOverallMitigatedHealing", line.Amount);
                            subPlayerAbilityGroup.Stats.IncrementStat("TotalOverallMitigatedHealing", line.Amount);

                            if (line.Crit)
                            {
                                Stats.IncrementStat("HealingCritHit");
                                Stats.IncrementStat("CriticalHealing", line.Amount);
                                subAbilityGroup.Stats.IncrementStat("HealingCritHit");
                                subAbilityGroup.Stats.IncrementStat("CriticalHealing", line.Amount);
                                subPlayerGroup.Stats.IncrementStat("HealingCritHit");
                                subPlayerGroup.Stats.IncrementStat("CriticalHealing", line.Amount);
                                subPlayerAbilityGroup.Stats.IncrementStat("HealingCritHit");
                                subPlayerAbilityGroup.Stats.IncrementStat("CriticalHealing", line.Amount);
                                if (line.Modifier != 0)
                                {
                                    var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                                    var modStat = "HealingCritMod";
                                    Stats.IncrementStat(modStat, mod);
                                    subAbilityGroup.Stats.IncrementStat(modStat, mod);
                                    subPlayerGroup.Stats.IncrementStat(modStat, mod);
                                    subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
                                }
                            }
                            else
                            {
                                Stats.IncrementStat("HealingRegHit");
                                Stats.IncrementStat("RegularHealing", line.Amount);
                                subAbilityGroup.Stats.IncrementStat("HealingRegHit");
                                subAbilityGroup.Stats.IncrementStat("RegularHealing", line.Amount);
                                subPlayerGroup.Stats.IncrementStat("HealingRegHit");
                                subPlayerGroup.Stats.IncrementStat("RegularHealing", line.Amount);
                                subPlayerAbilityGroup.Stats.IncrementStat("HealingRegHit");
                                subPlayerAbilityGroup.Stats.IncrementStat("RegularHealing", line.Amount);
                                if (line.Modifier != 0)
                                {
                                    var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                                    var modStat = "HealingRegMod";
                                    Stats.IncrementStat(modStat, mod);
                                    subAbilityGroup.Stats.IncrementStat(modStat, mod);
                                    subPlayerGroup.Stats.IncrementStat(modStat, mod);
                                    subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
                                }
                            }
                            break;
                    }

                    #endregion

                    break;
                case false:

                    #region Not Pre-Processed

                    if (healingType == HealingType.Normal)
                    {
                        Stats.IncrementStat("TotalHealingActionsUsed");
                        subAbilityGroup.Stats.IncrementStat("TotalHealingActionsUsed");
                        subPlayerGroup.Stats.IncrementStat("TotalHealingActionsUsed");
                        subPlayerAbilityGroup.Stats.IncrementStat("TotalHealingActionsUsed");
                    }

                    Stats.IncrementStat("TotalOverallHealing", line.Amount);
                    subAbilityGroup.Stats.IncrementStat("TotalOverallHealing", line.Amount);
                    subPlayerGroup.Stats.IncrementStat("TotalOverallHealing", line.Amount);
                    subPlayerAbilityGroup.Stats.IncrementStat("TotalOverallHealing", line.Amount);
                    if (line.Crit)
                    {
                        Stats.IncrementStat("HealingCritHit");
                        Stats.IncrementStat("CriticalHealing", line.Amount);
                        subAbilityGroup.Stats.IncrementStat("HealingCritHit");
                        subAbilityGroup.Stats.IncrementStat("CriticalHealing", line.Amount);
                        subPlayerGroup.Stats.IncrementStat("HealingCritHit");
                        subPlayerGroup.Stats.IncrementStat("CriticalHealing", line.Amount);
                        subPlayerAbilityGroup.Stats.IncrementStat("HealingCritHit");
                        subPlayerAbilityGroup.Stats.IncrementStat("CriticalHealing", line.Amount);
                        if (line.Modifier != 0)
                        {
                            var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                            var modStat = "HealingCritMod";
                            Stats.IncrementStat(modStat, mod);
                            subAbilityGroup.Stats.IncrementStat(modStat, mod);
                            subPlayerGroup.Stats.IncrementStat(modStat, mod);
                            subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
                        }
                    }
                    else
                    {
                        Stats.IncrementStat("HealingRegHit");
                        Stats.IncrementStat("RegularHealing", line.Amount);
                        subAbilityGroup.Stats.IncrementStat("HealingRegHit");
                        subAbilityGroup.Stats.IncrementStat("RegularHealing", line.Amount);
                        subPlayerGroup.Stats.IncrementStat("HealingRegHit");
                        subPlayerGroup.Stats.IncrementStat("RegularHealing", line.Amount);
                        subPlayerAbilityGroup.Stats.IncrementStat("HealingRegHit");
                        subPlayerAbilityGroup.Stats.IncrementStat("RegularHealing", line.Amount);
                        if (line.Modifier != 0)
                        {
                            var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                            var modStat = "HealingRegMod";
                            Stats.IncrementStat(modStat, mod);
                            subAbilityGroup.Stats.IncrementStat(modStat, mod);
                            subPlayerGroup.Stats.IncrementStat(modStat, mod);
                            subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
                        }
                    }

                    #endregion

                    #region Handle Mitigaged (With Initial Healing)

                    var mitigated = false;
                    var mitigagedSkill = "";
                    if (MagicBarrierHelper.Adloquium.Any(action => String.Equals(line.Action, action, Constants.InvariantComparer)))
                    {
                        mitigated = true;
                        line.Amount = originalAmount;
                        mitigagedSkill = "adloquium";
                    }
                    if (MagicBarrierHelper.Succor.Any(action => String.Equals(line.Action, action, Constants.InvariantComparer)))
                    {
                        mitigated = true;
                        line.Amount = originalAmount;
                        mitigagedSkill = "succor";
                    }
                    if (mitigated)
                    {
                        SetHealingMitigated(line, mitigagedSkill);
                    }

                    #endregion

                    break;
            }

            #region Full Handling

            #endregion

            #region OverHealing Handler

            if (unusedAmount > 0m)
            {
                line.Amount = healingType == HealingType.HealingOverTime ? originalAmount : unusedAmount;
                if (healingType == HealingType.HealingOverTime)
                {
                    Stats.IncrementStat("TotalOverallHealingOverTimeOverHealing", line.Amount);
                    subAbilityGroup.Stats.IncrementStat("TotalOverallHealingOverTimeOverHealing", line.Amount);
                    subPlayerGroup.Stats.IncrementStat("TotalOverallHealingOverTimeOverHealing", line.Amount);
                    subPlayerAbilityGroup.Stats.IncrementStat("TotalOverallHealingOverTimeOverHealing", line.Amount);
                }
                else
                {
                    Stats.IncrementStat("TotalOverallOverHealing", line.Amount);
                    subAbilityGroup.Stats.IncrementStat("TotalOverallOverHealing", line.Amount);
                    subPlayerGroup.Stats.IncrementStat("TotalOverallOverHealing", line.Amount);
                    subPlayerAbilityGroup.Stats.IncrementStat("TotalOverallOverHealing", line.Amount);
                }
                SetHealingOver(line, healingType);
            }

            #endregion
        }

        public void SetHealingMitigated(Line line, string type = "")
        {
            var cleanedAction = Regex.Replace(line.Action, @" \[.+\]", "");
            line.Action = String.Format("{0} [☯]", cleanedAction);
            switch (type)
            {
                case "adloquium":
                    if (line.Crit)
                    {
                        line.Amount = line.Amount * 2;
                    }
                    break;
                case "succor":
                    break;
                default:
                    break;
            }
            SetHealing(line, HealingType.Mitigated, true);
        }

        private void SetHealingOver(Line line, HealingType healingType)
        {
            var cleanedAction = Regex.Replace(line.Action, @" \[.+\]", "");
            switch (healingType)
            {
                case HealingType.Normal:
                    line.Action = String.Format("{0} [∞]", cleanedAction);
                    SetHealing(line, HealingType.OverHealing, true);
                    break;
                case HealingType.HealingOverTime:
                    line.Action = String.Format("{0} [•][∞]", cleanedAction);
                    SetHealing(line, HealingType.HealingOverTimeOverHealing, true);
                    break;
            }
        }
    }
}
