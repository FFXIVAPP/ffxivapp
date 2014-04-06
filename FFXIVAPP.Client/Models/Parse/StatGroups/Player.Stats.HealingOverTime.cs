// FFXIVAPP.Client
// Player.Stats.HealingOverTime.cs
// 
// Copyright © 2007 - 2014 Ryan Wilson - All Rights Reserved
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions are met: 
// 
//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright 
//    notice, this list of conditions and the following disclaimer in the 
//    documentation and/or other materials provided with the distribution. 
//  * Neither the name of SyndicatedLife nor the names of its contributors may 
//    be used to endorse or promote products derived from this software 
//    without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE. 

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
