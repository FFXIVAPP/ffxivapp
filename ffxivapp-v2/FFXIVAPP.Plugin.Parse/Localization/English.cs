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
            Dictionary.Add("parse_", "PLACEHOLDER");
            Dictionary.Add("parse_abilitiesheader", "Abilities");
            Dictionary.Add("parse_abilitychattab", "Ability Chat");
            Dictionary.Add("parse_accheader", "Acc");
            Dictionary.Add("parse_accuracylabel", "Accuracy:");
            Dictionary.Add("parse_avgheader", "Avg");
            Dictionary.Add("parse_avghpheader", "Avg HP");
            Dictionary.Add("parse_basictab", "Basic");
            Dictionary.Add("parse_blockheader", "Block");
            Dictionary.Add("parse_blockperheader", "Block %");
            Dictionary.Add("parse_cavgheader", "C Avg");
            Dictionary.Add("parse_chighheader", "C High");
            Dictionary.Add("parse_chitheader", "C Hit");
            Dictionary.Add("parse_clowheader", "C Low");
            Dictionary.Add("parse_counterheader", "Counter");
            Dictionary.Add("parse_counterperheader", "Counter %");
            Dictionary.Add("parse_critheader", "Crit");
            Dictionary.Add("parse_criticalpercentlabel", "Critical %:");
            Dictionary.Add("parse_critperheader", "Crit %");
            Dictionary.Add("parse_damageheader", "Damage");
            Dictionary.Add("parse_damagetab", "Damage");
            Dictionary.Add("parse_debugheader", "Debug");
            Dictionary.Add("parse_debugtab", "Debug");
            Dictionary.Add("parse_dpsheader", "DPS");
            Dictionary.Add("parse_dropperheader", "Drop %");
            Dictionary.Add("parse_dropsheader", "Drops");
            Dictionary.Add("parse_dtavgheader", "DT Avg");
            Dictionary.Add("parse_dtcavgheader", "DT C Avg");
            Dictionary.Add("parse_dtchighheader", "DT C High");
            Dictionary.Add("parse_dtchitheader", "DT C Hit");
            Dictionary.Add("parse_dtclowheader", "DT C Low");
            Dictionary.Add("parse_dtcritheader", "DT Crit");
            Dictionary.Add("parse_dthighheader", "DT High");
            Dictionary.Add("parse_dthitheader", "DT Hit");
            Dictionary.Add("parse_dtlowheader", "DT Low");
            Dictionary.Add("parse_dtpsheader", "DTPS");
            Dictionary.Add("parse_dtregheader", "DT Reg");
            Dictionary.Add("parse_dttotalheader", "DT Total");
            Dictionary.Add("parse_evadeheader", "Evade");
            Dictionary.Add("parse_evadeperheader", "Evade %");
            Dictionary.Add("parse_exportxml", "Export XML");
            Dictionary.Add("parse_healavgheader", "Heal Avg");
            Dictionary.Add("parse_healhighheader", "Heal High");
            Dictionary.Add("parse_healingheader", "Healing");
            Dictionary.Add("parse_healingtab", "Healing");
            Dictionary.Add("parse_heallowheader", "Heal Low");
            Dictionary.Add("parse_highheader", "High");
            Dictionary.Add("parse_hitheader", "Hit");
            Dictionary.Add("parse_hpsheader", "HPS");
            Dictionary.Add("parse_htotalheader", "H Total");
            Dictionary.Add("parse_killedheader", "Killed");
            Dictionary.Add("parse_lowheader", "Low");
            Dictionary.Add("parse_missheader", "Miss");
            Dictionary.Add("parse_monsterdamagetakenbyabilityheader", "Monster Damage Taken (By Ability)");
            Dictionary.Add("parse_monsterdropsheader", "Monster Drops (By Selected Monster)");
            Dictionary.Add("parse_monsterheader", "Monster");
            Dictionary.Add("parse_monstersheader", "Monsters");
            Dictionary.Add("parse_monstertab", "Monster");
            Dictionary.Add("parse_nameheader", "Name");
            Dictionary.Add("parse_otheroptions", "Other Options");
            Dictionary.Add("parse_overalldamagelabel", "Overall:");
            Dictionary.Add("parse_overallheader", "Overall");
            Dictionary.Add("parse_parryheader", "Parry");
            Dictionary.Add("parse_parryperheader", "Parry %");
            Dictionary.Add("parse_partyheader", "Party");
            Dictionary.Add("parse_partytab", "Party");
            Dictionary.Add("parse_perofcritheader", "% of Crit");
            Dictionary.Add("parse_perofdmgheader", "% of Dmg");
            Dictionary.Add("parse_perofdtcdmgheader", "% of DT C Dmg");
            Dictionary.Add("parse_perofdtheader", "% of DT");
            Dictionary.Add("parse_perofhealheader", "% of Heal");
            Dictionary.Add("parse_perofregheader", "% of Reg");
            Dictionary.Add("parse_playerabilityheader", "Player Abilities (By Selected Player)");
            Dictionary.Add("parse_playerabilityonmonsterbyplayerheader", "Player Abilities (On Selected Monster By Selected Player)");
            Dictionary.Add("parse_playerdamagetakenbyabilityheader", "Player Damage Taken (By Selected Monster To Selected Player)");
            Dictionary.Add("parse_playerdamagetakenheader", "Player Damage Taken (On Selected Player By Ability)");
            Dictionary.Add("parse_playerhealingbyplayeronotherheader", "Player Healing (On Selected Other By Selected Player)");
            Dictionary.Add("parse_playerhealingheader", "Player Healing (By Selected Player)");
            Dictionary.Add("parse_playeronmonsterheader", "Player Abilities (On Monsters)");
            Dictionary.Add("parse_playeronotherheader", "Player Healing (On Others)");
            Dictionary.Add("parse_playersheader", "Players");
            Dictionary.Add("parse_regheader", "Reg");
            Dictionary.Add("parse_resetstats", "Reset");
            Dictionary.Add("parse_resetstatspopupmessage", "Do you wish to reset and remove all current stats?");
            Dictionary.Add("parse_resistheader", "Resist");
            Dictionary.Add("parse_resistperheader", "Resist %");
            Dictionary.Add("parse_showabilitychattab", "Show Ability Chat");
            Dictionary.Add("parse_showdamagetab", "Show Damage");
            Dictionary.Add("parse_showdebugtab", "Show Debug");
            Dictionary.Add("parse_showhealingtab", "Show Healing");
            Dictionary.Add("parse_showmonstertab", "Show Monster");
            Dictionary.Add("parse_showpartytab", "Show Party");
            Dictionary.Add("parse_timelineheader", "Timeline");
            Dictionary.Add("parse_totalheader", "Total");
            Dictionary.Add("parse_uploadparse", "Upload Parse");
            Dictionary.Add("parse_usedheader", "Used");
            Dictionary.Add("parse_valueheader", "Value");
            return Dictionary;
        }
    }
}
