// Project: ParseModXIV
// File: StatGroupToChat.cs
// 
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Text.RegularExpressions;
using ParseModXIV.View;

namespace ParseModXIV.Classes
{
    public static class StatGroupToChat
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string CleanString(string input)
        {
            return Double.Parse(input).ToString("F2").Replace("-1", "0");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statName"></param>
        /// <returns></returns>
        public static string ExportPartyStats(string statName)
        {
            var toClipboard = "";
            var playerName = "";
            if (statName == "party")
            {
                statName = "Abilities";
                playerName = MainTabControlView.View.gui_CAbilityName.Text;
            }
            if (statName == "healing")
            {
                statName = "Healing";
                playerName = MainTabControlView.View.gui_CHealingName.Text;
            }
            if (statName == "damage")
            {
                statName = "Damage";
                playerName = MainTabControlView.View.gui_CDamageName.Text;
            }
            if (Regex.IsMatch(statName, "^Abilities$|^Healing$|^Damage$"))
            {
                if (playerName != "")
                {
                    using (var aEnum = ParseMod.Instance.Timeline.PartyStats.GetEnumerator())
                    {
                        while (aEnum.MoveNext()) //name of player
                        {
                            if (aEnum.Current.Name != playerName)
                            {
                                continue;
                            }
                            toClipboard += playerName + "@ ";
                            switch (statName)
                            {
                                case "Abilities":
                                    toClipboard += "Total: " + CleanString(aEnum.Current.GetStatValue("Total").ToString()) + " ~ ";
                                    toClipboard += "DPS: " + CleanString(aEnum.Current.GetStatValue("DPS").ToString()) + " ~ ";
                                    toClipboard += "Reg: " + CleanString(aEnum.Current.GetStatValue("Reg").ToString()) + " ~ ";
                                    toClipboard += "% of Dmg: " + CleanString(aEnum.Current.GetStatValue("% of Dmg").ToString()) + " ~ ";
                                    toClipboard += "Low: " + CleanString(aEnum.Current.GetStatValue("Low").ToString()) + " ~ ";
                                    toClipboard += "High: " + CleanString(aEnum.Current.GetStatValue("High").ToString()) + " ~ ";
                                    toClipboard += "Hit: " + CleanString(aEnum.Current.GetStatValue("Hit").ToString()) + " ~ ";
                                    toClipboard += "Miss: " + CleanString(aEnum.Current.GetStatValue("Miss").ToString()) + " ~ ";
                                    toClipboard += "Acc: " + CleanString(aEnum.Current.GetStatValue("Acc").ToString()) + " ~ ";
                                    toClipboard += "Avg: " + CleanString(aEnum.Current.GetStatValue("Avg").ToString()) + " ~ ";
                                    toClipboard += "Crit: " + CleanString(aEnum.Current.GetStatValue("Crit").ToString()) + " ~ ";
                                    toClipboard += "% of Crit: " + CleanString(aEnum.Current.GetStatValue("% of Crit").ToString()) + " ~ ";
                                    toClipboard += "C Hit: " + CleanString(aEnum.Current.GetStatValue("C Hit").ToString()) + " ~ ";
                                    toClipboard += "C Low: " + CleanString(aEnum.Current.GetStatValue("C Low").ToString()) + " ~ ";
                                    toClipboard += "C High: " + CleanString(aEnum.Current.GetStatValue("C High").ToString()) + " ~ ";
                                    toClipboard += "C Avg: " + CleanString(aEnum.Current.GetStatValue("C Avg").ToString()) + " ~ ";
                                    toClipboard += "Crit %: " + CleanString(aEnum.Current.GetStatValue("Crit %").ToString());
                                    break;
                                case "Healing":
                                    toClipboard += "H Total: " + CleanString(aEnum.Current.GetStatValue("H Total").ToString()) + " ~ ";
                                    toClipboard += "HPS: " + CleanString(aEnum.Current.GetStatValue("HPS").ToString()) + " ~ ";
                                    toClipboard += "% of Heal: " + CleanString(aEnum.Current.GetStatValue("% of Heal").ToString()) + " ~ ";
                                    toClipboard += "Heal Low: " + CleanString(aEnum.Current.GetStatValue("Heal Low").ToString()) + " ~ ";
                                    toClipboard += "Heal High: " + CleanString(aEnum.Current.GetStatValue("Heal High").ToString()) + " ~ ";
                                    toClipboard += "Heal Avg: " + CleanString(aEnum.Current.GetStatValue("Heal Avg").ToString());
                                    break;
                                case "Damage":
                                    toClipboard += "DT Total: " + CleanString(aEnum.Current.GetStatValue("DT Total").ToString()) + " ~ ";
                                    toClipboard += "DTPS: " + CleanString(aEnum.Current.GetStatValue("DTPS").ToString()) + " ~ ";
                                    toClipboard += "DT Reg: " + CleanString(aEnum.Current.GetStatValue("DT Reg").ToString()) + " ~ ";
                                    toClipboard += "% of DT Dmg: " + CleanString(aEnum.Current.GetStatValue("% of DT Dmg").ToString()) + " ~ ";
                                    toClipboard += "DT Low: " + CleanString(aEnum.Current.GetStatValue("DT Low").ToString()) + " ~ ";
                                    toClipboard += "DT High: " + CleanString(aEnum.Current.GetStatValue("DT High").ToString()) + " ~ ";
                                    toClipboard += "DT Hit: " + CleanString(aEnum.Current.GetStatValue("DT Hit").ToString()) + " ~ ";
                                    toClipboard += "DT Avg: " + CleanString(aEnum.Current.GetStatValue("DT Avg").ToString()) + " ~ ";
                                    toClipboard += "DT Crit: " + CleanString(aEnum.Current.GetStatValue("DT Crit").ToString()) + " ~ ";
                                    toClipboard += "% of DT C Dmg: " + CleanString(aEnum.Current.GetStatValue("% of DT C Dmg").ToString()) + " ~ ";
                                    toClipboard += "DT C Hit: " + CleanString(aEnum.Current.GetStatValue("DT C Hit").ToString()) + " ~ ";
                                    toClipboard += "DT C Low: " + CleanString(aEnum.Current.GetStatValue("DT C Low").ToString()) + " ~ ";
                                    toClipboard += "DT C High: " + CleanString(aEnum.Current.GetStatValue("DT C High").ToString()) + " ~ ";
                                    toClipboard += "DT C Avg: " + CleanString(aEnum.Current.GetStatValue("DT C Avg").ToString());
                                    break;
                            }
                        }
                    }
                }
            }
            return toClipboard;
        }
    }
}