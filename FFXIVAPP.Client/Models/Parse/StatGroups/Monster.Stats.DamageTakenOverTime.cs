// FFXIVAPP.Client
// Monster.Stats.DamageTakenOverTime.cs
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

using FFXIVAPP.Client.Models.Parse.Stats;

namespace FFXIVAPP.Client.Models.Parse.StatGroups
{
    public partial class Monster
    {
        /// <summary>
        /// </summary>
        /// <param name="line"></param>
        public void SetDamageTakenOverTime(Line line)
        {
            if ((LimitBreaks.IsLimit(line.Action)) && Constants.Parse.PluginSettings.IgnoreLimitBreaks)
            {
                return;
            }

            //LineHistory.Add(new LineHistory(line));

            var abilityGroup = GetGroup("DamageTakenOverTimeByAction");
            StatGroup subAbilityGroup;
            if (!abilityGroup.TryGetGroup(line.Action, out subAbilityGroup))
            {
                subAbilityGroup = new StatGroup(line.Action);
                subAbilityGroup.Stats.AddStats(DamageTakenOverTimeStatList(null));
                abilityGroup.AddGroup(subAbilityGroup);
            }
            var damageGroup = GetGroup("DamageTakenOverTimeByPlayers");
            StatGroup subPlayerGroup;
            if (!damageGroup.TryGetGroup(line.Source, out subPlayerGroup))
            {
                subPlayerGroup = new StatGroup(line.Source);
                subPlayerGroup.Stats.AddStats(DamageTakenOverTimeStatList(null));
                damageGroup.AddGroup(subPlayerGroup);
            }
            var abilities = subPlayerGroup.GetGroup("DamageTakenOverTimeByPlayersByAction");
            StatGroup subPlayerAbilityGroup;
            if (!abilities.TryGetGroup(line.Action, out subPlayerAbilityGroup))
            {
                subPlayerAbilityGroup = new StatGroup(line.Action);
                subPlayerAbilityGroup.Stats.AddStats(DamageTakenOverTimeStatList(subPlayerGroup, true));
                abilities.AddGroup(subPlayerAbilityGroup);
            }
            Stats.IncrementStat("TotalDamageTakenOverTimeActionsUsed");
            subAbilityGroup.Stats.IncrementStat("TotalDamageTakenOverTimeActionsUsed");
            subPlayerGroup.Stats.IncrementStat("TotalDamageTakenOverTimeActionsUsed");
            subPlayerAbilityGroup.Stats.IncrementStat("TotalDamageTakenOverTimeActionsUsed");
            Stats.IncrementStat("TotalOverallDamageTakenOverTime", line.Amount);
            subAbilityGroup.Stats.IncrementStat("TotalOverallDamageTakenOverTime", line.Amount);
            subPlayerGroup.Stats.IncrementStat("TotalOverallDamageTakenOverTime", line.Amount);
            subPlayerAbilityGroup.Stats.IncrementStat("TotalOverallDamageTakenOverTime", line.Amount);
            if (line.Crit)
            {
                Stats.IncrementStat("DamageTakenOverTimeCritHit");
                subAbilityGroup.Stats.IncrementStat("DamageTakenOverTimeCritHit");
                subPlayerGroup.Stats.IncrementStat("DamageTakenOverTimeCritHit");
                subPlayerAbilityGroup.Stats.IncrementStat("DamageTakenOverTimeCritHit");
                Stats.IncrementStat("CriticalDamageTakenOverTime", line.Amount);
                subAbilityGroup.Stats.IncrementStat("CriticalDamageTakenOverTime", line.Amount);
                subPlayerGroup.Stats.IncrementStat("CriticalDamageTakenOverTime", line.Amount);
                subPlayerAbilityGroup.Stats.IncrementStat("CriticalDamageTakenOverTime", line.Amount);
            }
            else
            {
                Stats.IncrementStat("DamageTakenOverTimeRegHit");
                subAbilityGroup.Stats.IncrementStat("DamageTakenOverTimeRegHit");
                subPlayerGroup.Stats.IncrementStat("DamageTakenOverTimeRegHit");
                subPlayerAbilityGroup.Stats.IncrementStat("DamageTakenOverTimeRegHit");
                Stats.IncrementStat("RegularDamageTakenOverTime", line.Amount);
                subAbilityGroup.Stats.IncrementStat("RegularDamageTakenOverTime", line.Amount);
                subPlayerGroup.Stats.IncrementStat("RegularDamageTakenOverTime", line.Amount);
                subPlayerAbilityGroup.Stats.IncrementStat("RegularDamageTakenOverTime", line.Amount);
            }
        }
    }
}
