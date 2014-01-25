// FFXIVAPP.Client
// Player.Stats.Healing.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Linq;
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
        public void SetHealing(Line line)
        {
            if (Name == Settings.Default.CharacterName)
            {
                //LineHistory.Add(new LineHistory(line));
            }

            Last20HealingActions.Add(new LineHistory(line));
            if (Last20HealingActions.Count > 20)
            {
                Last20HealingActions.RemoveAt(0);
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
            Stats.IncrementStat("TotalHealingActionsUsed");
            subAbilityGroup.Stats.IncrementStat("TotalHealingActionsUsed");
            subPlayerGroup.Stats.IncrementStat("TotalHealingActionsUsed");
            subPlayerAbilityGroup.Stats.IncrementStat("TotalHealingActionsUsed");
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

            #region Handle Mitigaged (With Initial Healing)

            if (MagicBarrierHelper.Adloquium.Any(action => String.Equals(line.Action, action, Constants.InvariantComparer)))
            {
                line.Amount = originalAmount;
                SetupHealingMitigated(line, "adloquium");
            }
            if (MagicBarrierHelper.Succor.Any(action => String.Equals(line.Action, action, Constants.InvariantComparer)))
            {
                line.Amount = originalAmount;
                SetupHealingMitigated(line, "succor");
            }

            #endregion

            #region OverHealing Handler

            if (unusedAmount <= 0m)
            {
                return;
            }

            line.Amount = unusedAmount;
            SetupHealingOverHealing(line, HealingType.Normal);

            #endregion
        }
    }
}
