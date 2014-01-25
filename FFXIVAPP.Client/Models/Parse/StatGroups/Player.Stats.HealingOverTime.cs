// FFXIVAPP.Client
// Player.Stats.HealingOverTime.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Text.RegularExpressions;
using FFXIVAPP.Client.Enums.Parse;
using FFXIVAPP.Client.Helpers.Parse;
using FFXIVAPP.Client.Models.Parse.Stats;
using FFXIVAPP.Client.Properties;

namespace FFXIVAPP.Client.Models.Parse.StatGroups
{
    public partial class Player
    {
        /// <summary>
        /// </summary>
        /// <param name="line"> </param>
        public void SetHealingOverTime(Line line)
        {
            if (Name == Settings.Default.CharacterName)
            {
                //LineHistory.Add(new LineHistory(line));
            }

            var currentHealing = line.Crit ? line.Amount > 0 ? ParseHelper.GetOriginalAmount(line.Amount, (decimal) .5) : 0 : line.Amount;
            if (currentHealing > 0)
            {
                ParseHelper.LastAmountByAction.EnsurePlayerAction(line.Source, line.Action, currentHealing);
            }

            var unusedAmount = 0m;
            var originalAmount = line.Amount;
            // get curable of target
            try
            {
                var cleanedName = Regex.Replace(line.Target, @"\[[\w]+\]", "")
                                       .Trim();
                var curable = Controller.Timeline.TryGetPlayerCurable(cleanedName);
                if (line.Amount > curable)
                {
                    unusedAmount = line.Amount - curable;
                    line.Amount = curable;
                }
            }
            catch (Exception ex)
            {
            }

            var abilityGroup = GetGroup("HealingOverTimeByAction");
            StatGroup subAbilityGroup;
            if (!abilityGroup.TryGetGroup(line.Action, out subAbilityGroup))
            {
                subAbilityGroup = new StatGroup(line.Action);
                subAbilityGroup.Stats.AddStats(HealingOverTimeStatList(null));
                abilityGroup.AddGroup(subAbilityGroup);
            }
            var playerGroup = GetGroup("HealingOverTimeToPlayers");
            StatGroup subPlayerGroup;
            if (!playerGroup.TryGetGroup(line.Target, out subPlayerGroup))
            {
                subPlayerGroup = new StatGroup(line.Target);
                subPlayerGroup.Stats.AddStats(HealingOverTimeStatList(null));
                playerGroup.AddGroup(subPlayerGroup);
            }
            var abilities = subPlayerGroup.GetGroup("HealingOverTimeToPlayersByAction");
            StatGroup subPlayerAbilityGroup;
            if (!abilities.TryGetGroup(line.Action, out subPlayerAbilityGroup))
            {
                subPlayerAbilityGroup = new StatGroup(line.Action);
                subPlayerAbilityGroup.Stats.AddStats(HealingOverTimeStatList(subPlayerGroup, true));
                abilities.AddGroup(subPlayerAbilityGroup);
            }
            Stats.IncrementStat("TotalHealingOverTimeActionsUsed");
            subAbilityGroup.Stats.IncrementStat("TotalHealingOverTimeActionsUsed");
            subPlayerGroup.Stats.IncrementStat("TotalHealingOverTimeActionsUsed");
            subPlayerAbilityGroup.Stats.IncrementStat("TotalHealingOverTimeActionsUsed");
            Stats.IncrementStat("TotalOverallHealingOverTime", line.Amount);
            subAbilityGroup.Stats.IncrementStat("TotalOverallHealingOverTime", line.Amount);
            subPlayerGroup.Stats.IncrementStat("TotalOverallHealingOverTime", line.Amount);
            subPlayerAbilityGroup.Stats.IncrementStat("TotalOverallHealingOverTime", line.Amount);
            if (line.Crit)
            {
                Stats.IncrementStat("HealingOverTimeCritHit");
                Stats.IncrementStat("CriticalHealingOverTime", line.Amount);
                subAbilityGroup.Stats.IncrementStat("HealingOverTimeCritHit");
                subAbilityGroup.Stats.IncrementStat("CriticalHealingOverTime", line.Amount);
                subPlayerGroup.Stats.IncrementStat("HealingOverTimeCritHit");
                subPlayerGroup.Stats.IncrementStat("CriticalHealingOverTime", line.Amount);
                subPlayerAbilityGroup.Stats.IncrementStat("HealingOverTimeCritHit");
                subPlayerAbilityGroup.Stats.IncrementStat("CriticalHealingOverTime", line.Amount);
                if (line.Modifier != 0)
                {
                    var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                    var modStat = "HealingOverTimeCritMod";
                    Stats.IncrementStat(modStat, mod);
                    subAbilityGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
                }
            }
            else
            {
                Stats.IncrementStat("HealingOverTimeRegHit");
                Stats.IncrementStat("RegularHealingOverTime", line.Amount);
                subAbilityGroup.Stats.IncrementStat("HealingOverTimeRegHit");
                subAbilityGroup.Stats.IncrementStat("RegularHealingOverTime", line.Amount);
                subPlayerGroup.Stats.IncrementStat("HealingOverTimeRegHit");
                subPlayerGroup.Stats.IncrementStat("RegularHealingOverTime", line.Amount);
                subPlayerAbilityGroup.Stats.IncrementStat("HealingOverTimeRegHit");
                subPlayerAbilityGroup.Stats.IncrementStat("RegularHealingOverTime", line.Amount);
                if (line.Modifier != 0)
                {
                    var mod = ParseHelper.GetBonusAmount(line.Amount, line.Modifier);
                    var modStat = "HealingOverTimeRegMod";
                    Stats.IncrementStat(modStat, mod);
                    subAbilityGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerGroup.Stats.IncrementStat(modStat, mod);
                    subPlayerAbilityGroup.Stats.IncrementStat(modStat, mod);
                }
            }

            #region OverHealing Handler

            if (unusedAmount <= 0m)
            {
                return;
            }

            line.Amount = originalAmount;
            SetupHealingOverHealing(line, HealingType.HealingOverTime);

            #endregion
        }
    }
}
