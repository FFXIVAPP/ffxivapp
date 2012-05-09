// ParseModXIV
// StatGroupToXml.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace ParseModXIV.Classes
{
    public static class StatGroupToXml
    {
        private static readonly XmlWriterSettings XmlSettings = new XmlWriterSettings();

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
        public static void ExportParty()
        {
            XmlSettings.Indent = true;
            XmlSettings.IndentChars = "	";
            var filename = MainWindow.View.Lpath + DateTime.Now.ToString("dd.MM.yyyy.HH.mm.ss") + "_Party_Stats.xml";
            using (var writer = XmlWriter.Create(filename, XmlSettings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("party");
                using (var aEnum = ParseMod.Instance.Timeline.Party.GetEnumerator())
                {
                    while (aEnum.MoveNext()) //name of player
                    {
                        writer.WriteStartElement("player");
                        writer.WriteAttributeString("name", aEnum.Current.Name);
                        writer.WriteAttributeString("server", Settings.Default.Server);
                        foreach (var mstat in aEnum.Current.Children.Where(mstat => mstat.Children.Any()))
                        {
                            #region " EXPORT PLAYER ABILITIES "

                            if (mstat.Name.ToLower() == "abilities")
                            {
                                writer.WriteStartElement(mstat.Name.ToLower());
                                foreach (var astat in mstat.Children) //name of action in subsection
                                {
                                    writer.WriteStartElement("action");
                                    writer.WriteAttributeString("name", astat.Name);
                                    writer.WriteElementString("total", CleanString(astat.Stats.GetStatValue("Total").ToString(CultureInfo.InvariantCulture)));
                                    writer.WriteElementString("reg", CleanString(astat.Stats.GetStatValue("Reg").ToString(CultureInfo.InvariantCulture)));
                                    writer.WriteElementString("crit", CleanString(astat.Stats.GetStatValue("Crit").ToString(CultureInfo.InvariantCulture)));
                                    writer.WriteElementString("perofreg", CleanString(astat.Stats.GetStatValue("% of Reg").ToString(CultureInfo.InvariantCulture)));
                                    writer.WriteElementString("perofcrit", CleanString(astat.Stats.GetStatValue("% of Crit").ToString(CultureInfo.InvariantCulture)));
                                    writer.WriteElementString("hit", CleanString(astat.Stats.GetStatValue("Hit").ToString(CultureInfo.InvariantCulture)));
                                    writer.WriteElementString("miss", CleanString(astat.Stats.GetStatValue("Miss").ToString(CultureInfo.InvariantCulture)));
                                    writer.WriteElementString("chit", CleanString(astat.Stats.GetStatValue("C Hit").ToString(CultureInfo.InvariantCulture)));
                                    writer.WriteElementString("accuracy", CleanString(astat.Stats.GetStatValue("Acc").ToString(CultureInfo.InvariantCulture)));
                                    writer.WriteElementString("low", CleanString(astat.Stats.GetStatValue("Low").ToString(CultureInfo.InvariantCulture)));
                                    writer.WriteElementString("high", CleanString(astat.Stats.GetStatValue("High").ToString(CultureInfo.InvariantCulture)));
                                    writer.WriteElementString("clow", CleanString(astat.Stats.GetStatValue("C Low").ToString(CultureInfo.InvariantCulture)));
                                    writer.WriteElementString("chigh", CleanString(astat.Stats.GetStatValue("C High").ToString(CultureInfo.InvariantCulture)));
                                    writer.WriteElementString("avgdamage", CleanString(astat.Stats.GetStatValue("Avg").ToString(CultureInfo.InvariantCulture)));
                                    writer.WriteElementString("avgcdamage", CleanString(astat.Stats.GetStatValue("C Avg").ToString(CultureInfo.InvariantCulture)));
                                    writer.WriteEndElement();
                                }
                                writer.WriteEndElement();
                            }

                            #endregion

                            #region " EXPORT PLAYER DAMAGE TAKEN "

                            if (mstat.Name.ToLower() == "damage")
                            {
                                writer.WriteStartElement(mstat.Name.ToLower());
                                foreach (var astat in mstat.Children) //name of action in subsection
                                {
                                    writer.WriteStartElement("action");
                                    writer.WriteAttributeString("name", astat.Name);
                                    writer.WriteElementString("total", CleanString(astat.Stats.GetStatValue("Total").ToString(CultureInfo.InvariantCulture)));
                                    writer.WriteElementString("reg", CleanString(astat.Stats.GetStatValue("Reg").ToString(CultureInfo.InvariantCulture)));
                                    writer.WriteElementString("crit", CleanString(astat.Stats.GetStatValue("Crit").ToString(CultureInfo.InvariantCulture)));
                                    writer.WriteElementString("perofreg", CleanString(astat.Stats.GetStatValue("% of Reg").ToString(CultureInfo.InvariantCulture)));
                                    writer.WriteElementString("perofcrit", CleanString(astat.Stats.GetStatValue("% of Crit").ToString(CultureInfo.InvariantCulture)));
                                    writer.WriteElementString("hit", CleanString(astat.Stats.GetStatValue("Hit").ToString(CultureInfo.InvariantCulture)));
                                    writer.WriteElementString("chit", CleanString(astat.Stats.GetStatValue("C Hit").ToString(CultureInfo.InvariantCulture)));
                                    writer.WriteElementString("low", CleanString(astat.Stats.GetStatValue("Low").ToString(CultureInfo.InvariantCulture)));
                                    writer.WriteElementString("high", CleanString(astat.Stats.GetStatValue("High").ToString(CultureInfo.InvariantCulture)));
                                    writer.WriteElementString("clow", CleanString(astat.Stats.GetStatValue("C Low").ToString(CultureInfo.InvariantCulture)));
                                    writer.WriteElementString("chigh", CleanString(astat.Stats.GetStatValue("C High").ToString(CultureInfo.InvariantCulture)));
                                    writer.WriteElementString("avgdamage", CleanString(astat.Stats.GetStatValue("Avg").ToString(CultureInfo.InvariantCulture)));
                                    writer.WriteElementString("avgcdamage", CleanString(astat.Stats.GetStatValue("C Avg").ToString(CultureInfo.InvariantCulture)));
                                    writer.WriteEndElement();
                                }
                                writer.WriteEndElement();
                            }

                            #endregion

                            #region " EXPORT PLAYER HEALING "

                            if (mstat.Name.ToLower() == "healing")
                            {
                                writer.WriteStartElement(mstat.Name.ToLower());
                                foreach (var astat in mstat.Children) //name of action in subsection
                                {
                                    writer.WriteStartElement("action");
                                    writer.WriteAttributeString("name", astat.Name);
                                    writer.WriteElementString("total", CleanString(astat.Stats.GetStatValue("Total").ToString(CultureInfo.InvariantCulture)));
                                    writer.WriteElementString("perofheal", CleanString(astat.Stats.GetStatValue("% of Heal").ToString(CultureInfo.InvariantCulture)));
                                    writer.WriteElementString("low", CleanString(astat.Stats.GetStatValue("Low").ToString(CultureInfo.InvariantCulture)));
                                    writer.WriteElementString("high", CleanString(astat.Stats.GetStatValue("High").ToString(CultureInfo.InvariantCulture)));
                                    writer.WriteElementString("avghealing", CleanString(astat.Stats.GetStatValue("Avg").ToString(CultureInfo.InvariantCulture)));
                                    writer.WriteEndElement();
                                }
                                writer.WriteEndElement();
                            }

                            #endregion
                        }
                        writer.WriteEndElement();
                    }
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void ExportMonsterStats()
        {
            XmlSettings.Indent = true;
            XmlSettings.IndentChars = "	";
            var filename = MainWindow.View.Lpath + DateTime.Now.ToString("dd.MM.yyyy.HH.mm.ss") + "_Monster_Stats.xml";
            using (var writer = XmlWriter.Create(filename, XmlSettings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("monster");
                using (var aEnum = ParseMod.Instance.Timeline.Monster.GetEnumerator())
                {
                    while (aEnum.MoveNext()) //name of monster
                    {
                        #region " BASIC MOB STATS "

                        writer.WriteStartElement("monster");
                        writer.WriteAttributeString("name", aEnum.Current.Name);
                        writer.WriteAttributeString("server", Settings.Default.Server);
                        writer.WriteStartElement("basic");
                        writer.WriteElementString("totaldrops", CleanString(aEnum.Current.Stats.GetStatValue("Total Drops").ToString(CultureInfo.InvariantCulture)));
                        writer.WriteElementString("killed", CleanString(aEnum.Current.Stats.GetStatValue("Killed").ToString(CultureInfo.InvariantCulture)));
                        writer.WriteElementString("total", CleanString(aEnum.Current.Stats.GetStatValue("Total").ToString(CultureInfo.InvariantCulture)));
                        writer.WriteElementString("reg", CleanString(aEnum.Current.Stats.GetStatValue("Reg").ToString(CultureInfo.InvariantCulture)));
                        writer.WriteElementString("avghp", CleanString(aEnum.Current.Stats.GetStatValue("Avg HP").ToString(CultureInfo.InvariantCulture)));
                        writer.WriteElementString("crit", CleanString(aEnum.Current.Stats.GetStatValue("Crit").ToString(CultureInfo.InvariantCulture)));
                        writer.WriteElementString("hit", CleanString(aEnum.Current.Stats.GetStatValue("Hit").ToString(CultureInfo.InvariantCulture)));
                        writer.WriteElementString("chit", CleanString(aEnum.Current.Stats.GetStatValue("C Hit").ToString(CultureInfo.InvariantCulture)));
                        writer.WriteElementString("low", CleanString(aEnum.Current.Stats.GetStatValue("Low").ToString(CultureInfo.InvariantCulture)));
                        writer.WriteElementString("high", CleanString(aEnum.Current.Stats.GetStatValue("High").ToString(CultureInfo.InvariantCulture)));
                        writer.WriteElementString("clow", CleanString(aEnum.Current.Stats.GetStatValue("C Low").ToString(CultureInfo.InvariantCulture)));
                        writer.WriteElementString("chigh", CleanString(aEnum.Current.Stats.GetStatValue("C High").ToString(CultureInfo.InvariantCulture)));
                        writer.WriteElementString("avgdamage", CleanString(aEnum.Current.Stats.GetStatValue("Avg").ToString(CultureInfo.InvariantCulture)));
                        writer.WriteElementString("avgcdamage", CleanString(aEnum.Current.Stats.GetStatValue("C Avg").ToString(CultureInfo.InvariantCulture)));
                        writer.WriteEndElement();

                        #endregion

                        #region " EXPORT MONSTER DAMAGE BY ABILITY AND DROPS"

                        foreach (var mstat in aEnum.Current.Children.Where(mstat => mstat.Children.Any()))
                        {
                            switch (mstat.Name.ToLower())
                            {
                                case "m abilities":
                                    writer.WriteStartElement("abilities");
                                    foreach (var astat in mstat.Children) //name of action in subsection
                                    {
                                        writer.WriteStartElement("action");
                                        writer.WriteAttributeString("name", astat.Name);
                                        writer.WriteElementString("total", CleanString(astat.Stats.GetStatValue("Total").ToString(CultureInfo.InvariantCulture)));
                                        writer.WriteElementString("used", CleanString(astat.Stats.GetStatValue("Used").ToString(CultureInfo.InvariantCulture)));
                                        writer.WriteElementString("reg", CleanString(astat.Stats.GetStatValue("Reg").ToString(CultureInfo.InvariantCulture)));
                                        writer.WriteElementString("crit", CleanString(astat.Stats.GetStatValue("Crit").ToString(CultureInfo.InvariantCulture)));
                                        writer.WriteElementString("hit", CleanString(astat.Stats.GetStatValue("Hit").ToString(CultureInfo.InvariantCulture)));
                                        writer.WriteElementString("chit", CleanString(astat.Stats.GetStatValue("C Hit").ToString(CultureInfo.InvariantCulture)));
                                        writer.WriteElementString("low", CleanString(astat.Stats.GetStatValue("Low").ToString(CultureInfo.InvariantCulture)));
                                        writer.WriteElementString("high", CleanString(astat.Stats.GetStatValue("High").ToString(CultureInfo.InvariantCulture)));
                                        writer.WriteElementString("clow", CleanString(astat.Stats.GetStatValue("C Low").ToString(CultureInfo.InvariantCulture)));
                                        writer.WriteElementString("chigh", CleanString(astat.Stats.GetStatValue("C High").ToString(CultureInfo.InvariantCulture)));
                                        writer.WriteElementString("avgdamage", CleanString(astat.Stats.GetStatValue("Avg").ToString(CultureInfo.InvariantCulture)));
                                        writer.WriteElementString("avgcdamage", CleanString(astat.Stats.GetStatValue("C Avg").ToString(CultureInfo.InvariantCulture)));
                                        writer.WriteElementString("resist", CleanString(astat.Stats.GetStatValue("Resist").ToString(CultureInfo.InvariantCulture)));
                                        writer.WriteElementString("resistper", CleanString(astat.Stats.GetStatValue("Resist %").ToString(CultureInfo.InvariantCulture)));
                                        writer.WriteElementString("evade", CleanString(astat.Stats.GetStatValue("Evade").ToString(CultureInfo.InvariantCulture)));
                                        writer.WriteElementString("evadeper", CleanString(astat.Stats.GetStatValue("Evade %").ToString(CultureInfo.InvariantCulture)));
                                        writer.WriteEndElement();
                                    }
                                    writer.WriteEndElement();
                                    break;
                                case "drops":
                                    writer.WriteStartElement(mstat.Name.ToLower());
                                    foreach (var astat in mstat.Children) //name of drop with percent and total
                                    {
                                        writer.WriteStartElement("drop");
                                        writer.WriteAttributeString("name", astat.Name);
                                        writer.WriteElementString("total", CleanString(astat.Stats.GetStatValue("Total").ToString(CultureInfo.InvariantCulture)));
                                        writer.WriteElementString("dropper", CleanString(astat.Stats.GetStatValue("Drop %").ToString(CultureInfo.InvariantCulture)));
                                        writer.WriteEndElement();
                                    }
                                    writer.WriteEndElement();
                                    break;
                            }
                        }
                        writer.WriteEndElement();

                        #endregion
                    }
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void ExportBattleLog()
        {
            XmlSettings.Indent = true;
            XmlSettings.IndentChars = "	";
            var filename = MainWindow.View.Lpath + DateTime.Now.ToString("dd.MM.yyyy.HH.mm.ss") + "_Battle_RawLog.xml";
            using (var writer = XmlWriter.Create(filename, XmlSettings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("battle");
                foreach (var t in MainWindow.BattleLog)
                {
                    writer.WriteStartElement("log");
                    writer.WriteElementString("server", Settings.Default.Server);
                    writer.WriteElementString("crit", t[0]);
                    writer.WriteElementString("counter", t[1]);
                    writer.WriteElementString("by", t[2]);
                    writer.WriteElementString("to", t[3]);
                    writer.WriteElementString("ability", t[4]);
                    writer.WriteElementString("direction", t[5]);
                    writer.WriteElementString("amount", t[6]);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void ExportHealingLog()
        {
            XmlSettings.Indent = true;
            XmlSettings.IndentChars = "	";
            var filename = MainWindow.View.Lpath + DateTime.Now.ToString("dd.MM.yyyy.HH.mm.ss") + "_Healing_RawLog.xml";
            using (var writer = XmlWriter.Create(filename, XmlSettings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("healing");
                foreach (var t in MainWindow.HealingLog)
                {
                    writer.WriteStartElement("log");
                    writer.WriteElementString("server", Settings.Default.Server);
                    writer.WriteElementString("by", t[0]);
                    writer.WriteElementString("to", t[1]);
                    writer.WriteElementString("ability", t[2]);
                    writer.WriteElementString("amount", t[3]);
                    writer.WriteElementString("type", t[4]);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }
    }
}