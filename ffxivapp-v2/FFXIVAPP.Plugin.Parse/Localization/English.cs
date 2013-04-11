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
            Dictionary.Add("parse_DamageCritHitHeader", "C.Hit");
            Dictionary.Add("parse_DamageCritPercentHeader", "Crit %");
            Dictionary.Add("parse_DamageCritLowHeader", "C.Low");
            Dictionary.Add("parse_DamageCritHighHeader", "C.High");
            Dictionary.Add("parse_DamageCritAverageHeader", "C.Avg");
            Dictionary.Add("parse_DamageCounterHeader", "Counter");
            Dictionary.Add("parse_DamageCounterPercentHeader", "Counter %");
            Dictionary.Add("parse_DamageCounterBonusHeader", "C. Mod");
            Dictionary.Add("parse_DamageCounterBonusAverageHeader", "C. Mod Avg");
            Dictionary.Add("parse_DamageBlockHeader", "Block");
            Dictionary.Add("parse_DamageBlockPercentHeader", "Block %");
            Dictionary.Add("parse_DamageBlockBonusHeader", "B. Mod");
            Dictionary.Add("parse_DamageBlockBonusAverageHeader", "B. Mod Avg");
            Dictionary.Add("parse_DamageParryHeader", "Parry");
            Dictionary.Add("parse_DamageParryPercentHeader", "Parry %");
            Dictionary.Add("parse_DamageParryBonusHeader", "P. Mod");
            Dictionary.Add("parse_DamageParryBonusAverageHeader", "P. Mod Avg");
            Dictionary.Add("parse_DamageResistHeader", "Resist");
            Dictionary.Add("parse_DamageResistPercentHeader", "Resist %");
            Dictionary.Add("parse_DamageResistBonusHeader", "R. Mod");
            Dictionary.Add("parse_DamageResistBonusAverageHeader", "R. Mod Avg");
            Dictionary.Add("parse_DamageEvadeHeader", "Evade");
            Dictionary.Add("parse_DamageEvadePercentHeader", "Evade %");
            Dictionary.Add("parse_DamageEvadeBonusHeader", "E. Mod");
            Dictionary.Add("parse_DamageEvadeBonusAverageHeader", "E. Mod Avg");
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
            Dictionary.Add("parse_HealingCritHitHeader", "C.Hit");
            Dictionary.Add("parse_HealingCritPercentHeader", "Crit %");
            Dictionary.Add("parse_HealingCritLowHeader", "C.Low");
            Dictionary.Add("parse_HealingCritHighHeader", "C.High");
            Dictionary.Add("parse_HealingCritAverageHeader", "C.Avg");
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
            Dictionary.Add("parse_DamageTakenCritHitHeader", "C.Hit");
            Dictionary.Add("parse_DamageTakenCritPercentHeader", "Crit %");
            Dictionary.Add("parse_DamageTakenCritLowHeader", "C.Low");
            Dictionary.Add("parse_DamageTakenCritHighHeader", "C.High");
            Dictionary.Add("parse_DamageTakenCritAverageHeader", "C.Avg");
            Dictionary.Add("parse_DamageTakenCounterHeader", "Counter");
            Dictionary.Add("parse_DamageTakenCounterPercentHeader", "Counter %");
            Dictionary.Add("parse_DamageTakenCounterReductionHeader", "C. Mod");
            Dictionary.Add("parse_DamageTakenCounterReductionAverageHeader", "C. Mod Avg");
            Dictionary.Add("parse_DamageTakenBlockHeader", "Block");
            Dictionary.Add("parse_DamageTakenBlockPercentHeader", "Block %");
            Dictionary.Add("parse_DamageTakenBlockReductionHeader", "B. Mod");
            Dictionary.Add("parse_DamageTakenBlockReductionAverageHeader", "B. Mod Avg");
            Dictionary.Add("parse_DamageTakenParryHeader", "Parry");
            Dictionary.Add("parse_DamageTakenParryPercentHeader", "Parry %");
            Dictionary.Add("parse_DamageTakenParryReductionHeader", "P. Mod");
            Dictionary.Add("parse_DamageTakenParryReductionAverageHeader", "P. Mod Avg");
            Dictionary.Add("parse_DamageTakenResistHeader", "Resist");
            Dictionary.Add("parse_DamageTakenResistPercentHeader", "Resist %");
            Dictionary.Add("parse_DamageTakenResistReductionHeader", "R. Mod");
            Dictionary.Add("parse_DamageTakenResistReductionAverageHeader", "R. Mod Avg");
            Dictionary.Add("parse_DamageTakenEvadeHeader", "Evade");
            Dictionary.Add("parse_DamageTakenEvadePercentHeader", "Evade %");
            Dictionary.Add("parse_DamageTakenEvadeReductionHeader", "E. Mod");
            Dictionary.Add("parse_DamageTakenEvadeReductionAverageHeader", "E. Mod Avg");
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

            return Dictionary;
        }
    }
}
