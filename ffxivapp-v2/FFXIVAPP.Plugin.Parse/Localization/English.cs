// FFXIVAPP.Plugin.Parse
// English.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System.Windows;

#endregion

namespace FFXIVAPP.Plugin.Parse.Localization
{
    public abstract class English
    {
        private static readonly ResourceDictionary Dictionary = new ResourceDictionary();

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public static ResourceDictionary Context()
        {
            Dictionary.Clear();
            Dictionary.Add("parse_PLACEHOLDER", "*PH*");
            Dictionary.Add("parse_NameHeader", "Name");
            Dictionary.Add("parse_TotalOverallDamageHeader", "Total");
            Dictionary.Add("parse_RegularDamageHeader", "Reg");
            Dictionary.Add("parse_CriticalDamageHeader", "Crit");
            Dictionary.Add("parse_TotalDamageActionsUsedHeader", "Used");
            Dictionary.Add("parse_DPSHeader", "DPS");
            Dictionary.Add("parse_DamageRegHitHeader", "Hit");
            Dictionary.Add("parse_DamageRegMissHeader", "Miss");
            Dictionary.Add("parse_DamageRegAccuracyHeader", "Acc");
            Dictionary.Add("parse_DamageRegLowHeader", "Low");
            Dictionary.Add("parse_DamageRegHighHeader", "High");
            Dictionary.Add("parse_DamageRegAverageHeader", "Avg");
            Dictionary.Add("parse_DamageRegModHeader", "Reg Mod");
            Dictionary.Add("parse_DamageRegModAverageHeader", "Reg Mod Avg");
            Dictionary.Add("parse_DamageCritHitHeader", "C.Hit");
            Dictionary.Add("parse_DamageCritPercentHeader", "Crit %");
            Dictionary.Add("parse_DamageCritLowHeader", "C.Low");
            Dictionary.Add("parse_DamageCritHighHeader", "C.High");
            Dictionary.Add("parse_DamageCritAverageHeader", "C.Avg");
            Dictionary.Add("parse_DamageCritModHeader", "Crit Mod");
            Dictionary.Add("parse_DamageCritModAverageHeader", "Crit Mod Avg");
            Dictionary.Add("parse_DamageCounterHeader", "Counter");
            Dictionary.Add("parse_DamageCounterPercentHeader", "Counter %");
            Dictionary.Add("parse_DamageCounterModHeader", "C. Mod");
            Dictionary.Add("parse_DamageCounterModAverageHeader", "C. Mod Avg");
            Dictionary.Add("parse_DamageBlockHeader", "Block");
            Dictionary.Add("parse_DamageBlockPercentHeader", "Block %");
            Dictionary.Add("parse_DamageBlockModHeader", "B. Mod");
            Dictionary.Add("parse_DamageBlockModAverageHeader", "B. Mod Avg");
            Dictionary.Add("parse_DamageParryHeader", "Parry");
            Dictionary.Add("parse_DamageParryPercentHeader", "Parry %");
            Dictionary.Add("parse_DamageParryModHeader", "P. Mod");
            Dictionary.Add("parse_DamageParryModAverageHeader", "P. Mod Avg");
            Dictionary.Add("parse_DamageResistHeader", "Resist");
            Dictionary.Add("parse_DamageResistPercentHeader", "Resist %");
            Dictionary.Add("parse_DamageResistModHeader", "R. Mod");
            Dictionary.Add("parse_DamageResistModAverageHeader", "R. Mod Avg");
            Dictionary.Add("parse_DamageEvadeHeader", "Evade");
            Dictionary.Add("parse_DamageEvadePercentHeader", "Evade %");
            Dictionary.Add("parse_DamageEvadeModHeader", "E. Mod");
            Dictionary.Add("parse_DamageEvadeModAverageHeader", "E. Mod Avg");
            Dictionary.Add("parse_PercentOfTotalOverallDamageHeader", "% of Total");
            Dictionary.Add("parse_PercentOfRegularDamageHeader", "% of Reg");
            Dictionary.Add("parse_PercentOfCriticalDamageHeader", "% of Crit");
            Dictionary.Add("parse_TotalOverallHealingHeader", "Total");
            Dictionary.Add("parse_RegularHealingHeader", "Reg");
            Dictionary.Add("parse_CriticalHealingHeader", "Crit");
            Dictionary.Add("parse_TotalHealingActionsUsedHeader", "Used");
            Dictionary.Add("parse_HPSHeader", "HPS");
            Dictionary.Add("parse_HealingRegHitHeader", "Hit");
            Dictionary.Add("parse_HealingRegLowHeader", "Low");
            Dictionary.Add("parse_HealingRegHighHeader", "High");
            Dictionary.Add("parse_HealingRegAverageHeader", "Avg");
            Dictionary.Add("parse_HealingRegModHeader", "Reg Mod");
            Dictionary.Add("parse_HealingRegModAverageHeader", "Reg Mod Avg");
            Dictionary.Add("parse_HealingCritHitHeader", "C.Hit");
            Dictionary.Add("parse_HealingCritPercentHeader", "Crit %");
            Dictionary.Add("parse_HealingCritLowHeader", "C.Low");
            Dictionary.Add("parse_HealingCritHighHeader", "C.High");
            Dictionary.Add("parse_HealingCritAverageHeader", "C.Avg");
            Dictionary.Add("parse_HealingCritModHeader", "Crit Mod");
            Dictionary.Add("parse_HealingCritModAverageHeader", "Crit Mod Avg");
            Dictionary.Add("parse_PercentOfTotalOverallHealingHeader", "% of Total");
            Dictionary.Add("parse_PercentOfRegularHealingHeader", "% of Reg");
            Dictionary.Add("parse_PercentOfCriticalHealingHeader", "% of Crit");
            Dictionary.Add("parse_TotalOverallDamageTakenHeader", "Total");
            Dictionary.Add("parse_RegularDamageTakenHeader", "Reg");
            Dictionary.Add("parse_CriticalDamageTakenHeader", "Crit");
            Dictionary.Add("parse_TotalDamageTakenActionsUsedHeader", "Used");
            Dictionary.Add("parse_DTPSHeader", "DTPS");
            Dictionary.Add("parse_DamageTakenRegHitHeader", "Hit");
            Dictionary.Add("parse_DamageTakenRegMissHeader", "Miss");
            Dictionary.Add("parse_DamageTakenRegAccuracyHeader", "Acc");
            Dictionary.Add("parse_DamageTakenRegLowHeader", "Low");
            Dictionary.Add("parse_DamageTakenRegHighHeader", "High");
            Dictionary.Add("parse_DamageTakenRegAverageHeader", "Avg");
            Dictionary.Add("parse_DamageTakenRegModHeader", "Reg Mod");
            Dictionary.Add("parse_DamageTakenRegModAverageHeader", "Reg Mod Avg");
            Dictionary.Add("parse_DamageTakenCritHitHeader", "C.Hit");
            Dictionary.Add("parse_DamageTakenCritPercentHeader", "Crit %");
            Dictionary.Add("parse_DamageTakenCritLowHeader", "C.Low");
            Dictionary.Add("parse_DamageTakenCritHighHeader", "C.High");
            Dictionary.Add("parse_DamageTakenCritAverageHeader", "C.Avg");
            Dictionary.Add("parse_DamageTakenCritModHeader", "Crit Mod");
            Dictionary.Add("parse_DamageTakenCritModAverageHeader", "Crit Mod Avg");
            Dictionary.Add("parse_DamageTakenCounterHeader", "Counter");
            Dictionary.Add("parse_DamageTakenCounterPercentHeader", "Counter %");
            Dictionary.Add("parse_DamageTakenCounterModHeader", "C. Mod");
            Dictionary.Add("parse_DamageTakenCounterModAverageHeader", "C. Mod Avg");
            Dictionary.Add("parse_DamageTakenBlockHeader", "Block");
            Dictionary.Add("parse_DamageTakenBlockPercentHeader", "Block %");
            Dictionary.Add("parse_DamageTakenBlockModHeader", "B. Mod");
            Dictionary.Add("parse_DamageTakenBlockModAverageHeader", "B. Mod Avg");
            Dictionary.Add("parse_DamageTakenParryHeader", "Parry");
            Dictionary.Add("parse_DamageTakenParryPercentHeader", "Parry %");
            Dictionary.Add("parse_DamageTakenParryModHeader", "P. Mod");
            Dictionary.Add("parse_DamageTakenParryModAverageHeader", "P. Mod Avg");
            Dictionary.Add("parse_DamageTakenResistHeader", "Resist");
            Dictionary.Add("parse_DamageTakenResistPercentHeader", "Resist %");
            Dictionary.Add("parse_DamageTakenResistModHeader", "R. Mod");
            Dictionary.Add("parse_DamageTakenResistModAverageHeader", "R. Mod Avg");
            Dictionary.Add("parse_DamageTakenEvadeHeader", "Evade");
            Dictionary.Add("parse_DamageTakenEvadePercentHeader", "Evade %");
            Dictionary.Add("parse_DamageTakenEvadeModHeader", "E. Mod");
            Dictionary.Add("parse_DamageTakenEvadeModAverageHeader", "E. Mod Avg");
            Dictionary.Add("parse_PercentOfTotalOverallDamageTakenHeader", "% of Total");
            Dictionary.Add("parse_PercentOfRegularDamageTakenHeader", "% of Reg");
            Dictionary.Add("parse_PercentOfCriticalDamageTakenHeader", "% of Crit");
            Dictionary.Add("parse_OverallDamageLabel", "Damage:");
            Dictionary.Add("parse_OverallAccuracyLabel", "Accuracy");
            Dictionary.Add("parse_OverallCriticalPercentLabel", "Critical %:");
            Dictionary.Add("parse_BasicTabHeader", "Basic");
            Dictionary.Add("parse_ActionLogTabHeader", "Action Log");
            Dictionary.Add("parse_PartyDamageTabHeader", "Party Damage");
            Dictionary.Add("parse_PartyHealingTabHeader", "Party Healing");
            Dictionary.Add("parse_PartyDamageTakenTabHeader", "Party Damage Taken");
            Dictionary.Add("parse_MonsterDamageTakenTabHeader", "Monster Damage Taken");
            Dictionary.Add("parse_ResetStatsToolTip", "Reset Stats");
            Dictionary.Add("parse_ShowActionLogHeader", "Show Action Log");
            Dictionary.Add("parse_ShowPartyDamageHeader", "Show Party Damage");
            Dictionary.Add("parse_ShowPartyHealingHeader", "Show Party Healing");
            Dictionary.Add("parse_ShowPartyDamageTakenHeader", "Show Party Damage Taken");
            Dictionary.Add("parse_ShowMonsterDamageTakenHeader", "Show Monster Damage Taken");
            Dictionary.Add("parse_UploadParseHeader", "Upload Parse");
            Dictionary.Add("parse_ExportXMLHeader", "Export XML");
            Dictionary.Add("parse_PlayerDamageByActionText", "Damage By Action By Selected Player");
            Dictionary.Add("parse_PlayerDamageToMonstersText", "Damage To Monsters By Selected Player");
            Dictionary.Add("parse_PlayerDamageToMonstersByActionText", "Damage To Monsters By Action By Selected Player");
            Dictionary.Add("parse_PlayerHealingByActionText", "Healing By Action By Selected Player");
            Dictionary.Add("parse_PlayerHealingToPlayersText", "Healing To Players By Selected Player");
            Dictionary.Add("parse_PlayerHealingToPlayersByActionText", "Healing To Players By Action By Selected Player");
            Dictionary.Add("parse_PlayerDamageTakenByMonstersText", "Damage Taken From Monsters By Selected Player");
            Dictionary.Add("parse_PlayerDamageTakenByMonstersByActionText", "Damage Taken From Monsters By Action By Selected Monster By Selected Player");
            Dictionary.Add("parse_MonsterDamageTakenByActionText", "Damage Taken By Action By Selected Monster");
            Dictionary.Add("parse_MonsterDropsByMonsterText", "Drops By Selected Monster");
            Dictionary.Add("parse_TotalOverallDropsHeader", "Drops");
            Dictionary.Add("parse_TotalKilledHeader", "Killed");
            Dictionary.Add("parse_AverageHPHeader", "Avg HP");
            Dictionary.Add("parse_TotalDropsHeader", "Total");
            Dictionary.Add("parse_DropPercentHeader", "Drop %");
            Dictionary.Add("parse_ResetStatsMessage", "Do you wish to reset and remove all current stats?");
            return Dictionary;
        }
    }
}
