// FFXIVAPP.Client
// Player.Stats.DamageOverTime.cs
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

using FFXIVAPP.Client.Helpers.Parse;
using FFXIVAPP.Client.Models.Parse.Stats;
using FFXIVAPP.Client.Properties;

namespace FFXIVAPP.Client.Models.Parse.StatGroups
{
    public partial class Player
    {
        /// <summary>
        /// </summary>
        /// <param name="line"></param>
        public void SetDamageOverTime(Line line)
        {
            if (Name == Settings.Default.CharacterName)
            {
                //LineHistory.Add(new LineHistory(line));
            }

            if ((LimitBreaks.IsLimit(line.Action)) && Constants.Parse.PluginSettings.IgnoreLimitBreaks)
            {
                return;
            }

            var currentDamage = line.Crit ? line.Amount > 0 ? ParseHelper.GetOriginalAmount(line.Amount, (decimal) .5) : 0 : line.Amount;
            if (currentDamage > 0)
            {
                ParseHelper.LastAmountByAction.EnsurePlayerAction(line.Source, line.Action, currentDamage);
            }

            var abilityGroup = GetGroup("DamageOverTimeByAction");
            StatGroup subAbilityGroup;
            if (!abilityGroup.TryGetGroup(line.Action, out subAbilityGroup))
            {
                subAbilityGroup = new StatGroup(line.Action);
                subAbilityGroup.Stats.AddStats(DamageOverTimeStatList(null));
                abilityGroup.AddGroup(subAbilityGroup);
            }
            var monsterGroup = GetGroup("DamageOverTimeToMonsters");
            StatGroup subMonsterGroup;
            if (!monsterGroup.TryGetGroup(line.Target, out subMonsterGroup))
            {
                subMonsterGroup = new StatGroup(line.Target);
                subMonsterGroup.Stats.AddStats(DamageOverTimeStatList(null));
                monsterGroup.AddGroup(subMonsterGroup);
            }
            var monsters = subMonsterGroup.GetGroup("DamageOverTimeToMonstersByAction");
            StatGroup subMonsterAbilityGroup;
            if (!monsters.TryGetGroup(line.Action, out subMonsterAbilityGroup))
            {
                subMonsterAbilityGroup = new StatGroup(line.Action);
                subMonsterAbilityGroup.Stats.AddStats(DamageOverTimeStatList(subMonsterGroup, true));
                monsters.AddGroup(subMonsterAbilityGroup);
            }
            Stats.IncrementStat("TotalDamageOverTimeActionsUsed");
            subAbilityGroup.Stats.IncrementStat("TotalDamageOverTimeActionsUsed");
            subMonsterGroup.Stats.IncrementStat("TotalDamageOverTimeActionsUsed");
            subMonsterAbilityGroup.Stats.IncrementStat("TotalDamageOverTimeActionsUsed");
            Stats.IncrementStat("TotalOverallDamageOverTime", line.Amount);
            subAbilityGroup.Stats.IncrementStat("TotalOverallDamageOverTime", line.Amount);
            subMonsterGroup.Stats.IncrementStat("TotalOverallDamageOverTime", line.Amount);
            subMonsterAbilityGroup.Stats.IncrementStat("TotalOverallDamageOverTime", line.Amount);
            if (line.Crit)
            {
                Stats.IncrementStat("DamageOverTimeCritHit");
                subAbilityGroup.Stats.IncrementStat("DamageOverTimeCritHit");
                subMonsterGroup.Stats.IncrementStat("DamageOverTimeCritHit");
                subMonsterAbilityGroup.Stats.IncrementStat("DamageOverTimeCritHit");
                Stats.IncrementStat("CriticalDamageOverTime", line.Amount);
                subAbilityGroup.Stats.IncrementStat("CriticalDamageOverTime", line.Amount);
                subMonsterGroup.Stats.IncrementStat("CriticalDamageOverTime", line.Amount);
                subMonsterAbilityGroup.Stats.IncrementStat("CriticalDamageOverTime", line.Amount);
            }
            else
            {
                Stats.IncrementStat("DamageOverTimeRegHit");
                subAbilityGroup.Stats.IncrementStat("DamageOverTimeRegHit");
                subMonsterGroup.Stats.IncrementStat("DamageOverTimeRegHit");
                subMonsterAbilityGroup.Stats.IncrementStat("DamageOverTimeRegHit");
                Stats.IncrementStat("RegularDamageOverTime", line.Amount);
                subAbilityGroup.Stats.IncrementStat("RegularDamageOverTime", line.Amount);
                subMonsterGroup.Stats.IncrementStat("RegularDamageOverTime", line.Amount);
                subMonsterAbilityGroup.Stats.IncrementStat("RegularDamageOverTime", line.Amount);
            }
        }
    }
}
