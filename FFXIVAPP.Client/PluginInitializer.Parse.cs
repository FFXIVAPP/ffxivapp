// FFXIVAPP.Client
// PluginInitializer.Parse.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using System.Xml.Linq;
using FFXIVAPP.Client.Plugins.Parse.Views;
using FFXIVAPP.Client.RegularExpressions;
using FFXIVAPP.Client.SettingsProviders.Parse;
using FFXIVAPP.Common.Controls;
using FFXIVAPP.Common.RegularExpressions;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client
{
    internal static partial class PluginInitializer
    {
        [DoNotObfuscate]
        public static class Parse
        {
            /// <summary>
            /// </summary>
            public static void LoadSettings()
            {
                if (Constants.Parse.XSettings != null)
                {
                    foreach (var xElement in Constants.Parse.XSettings.Descendants()
                                                      .Elements("Setting"))
                    {
                        var xKey = (string) xElement.Attribute("Key");
                        var xValue = (string) xElement.Element("Value");
                        if (String.IsNullOrWhiteSpace(xKey) || String.IsNullOrWhiteSpace(xValue))
                        {
                            continue;
                        }
                        Settings.SetValue(xKey, xValue);
                        if (!Constants.Parse.Settings.Contains(xKey))
                        {
                            Constants.Parse.Settings.Add(xKey);
                        }
                    }
                }
            }

            /// <summary>
            /// </summary>
            public static void LoadPlayerRegEx()
            {
                if (Constants.Parse.XRegEx == null)
                {
                    return;
                }
                foreach (var xElement in Constants.Parse.XRegEx.Descendants()
                                                  .Elements("Player"))
                {
                    var xKey = (string) xElement.Attribute("Key");
                    var xLanguage = (string) xElement.Attribute("Language");
                    var xValue = (string) xElement.Element("Value");
                    if (String.IsNullOrWhiteSpace(xKey) || String.IsNullOrWhiteSpace(xValue))
                    {
                        continue;
                    }
                    if (!Common.Constants.IsValidRegex(xValue))
                    {
                        continue;
                    }
                    var regex = new Regex(xValue, SharedRegEx.DefaultOptions);
                    switch (xLanguage)
                    {
                        case "EN":

                            #region Handle English Regular Expressions

                            switch (xKey)
                            {
                                case "Damage":
                                    PlayerRegEx.DamageEn = regex;
                                    break;
                                case "DamageAuto":
                                    PlayerRegEx.DamageAutoEn = regex;
                                    break;
                                case "Failed":
                                    PlayerRegEx.FailedEn = regex;
                                    break;
                                case "FailedAuto":
                                    PlayerRegEx.FailedAutoEn = regex;
                                    break;
                                case "Actions":
                                    PlayerRegEx.ActionsEn = regex;
                                    break;
                                case "Items":
                                    PlayerRegEx.ItemsEn = regex;
                                    break;
                                case "Cure":
                                    PlayerRegEx.CureEn = regex;
                                    break;
                                case "BeneficialGain":
                                    PlayerRegEx.BeneficialGainEn = regex;
                                    break;
                                case "BeneficialLose":
                                    PlayerRegEx.BeneficialLoseEn = regex;
                                    break;
                                case "DetrimentalGain":
                                    PlayerRegEx.DetrimentalGainEn = regex;
                                    break;
                                case "DetrimentalLose":
                                    PlayerRegEx.DetrimentalLoseEn = regex;
                                    break;
                            }

                            #endregion

                            break;
                        case "FR":

                            #region Handle French Regular Expressions

                            switch (xKey)
                            {
                                case "Damage":
                                    PlayerRegEx.DamageFr = regex;
                                    break;
                                case "DamageAuto":
                                    PlayerRegEx.DamageAutoFr = regex;
                                    break;
                                case "Failed":
                                    PlayerRegEx.FailedFr = regex;
                                    break;
                                case "FailedAuto":
                                    PlayerRegEx.FailedAutoFr = regex;
                                    break;
                                case "Actions":
                                    PlayerRegEx.ActionsFr = regex;
                                    break;
                                case "Items":
                                    PlayerRegEx.ItemsFr = regex;
                                    break;
                                case "Cure":
                                    PlayerRegEx.CureFr = regex;
                                    break;
                                case "BeneficialGain":
                                    PlayerRegEx.BeneficialGainFr = regex;
                                    break;
                                case "BeneficialLose":
                                    PlayerRegEx.BeneficialLoseFr = regex;
                                    break;
                                case "DetrimentalGain":
                                    PlayerRegEx.DetrimentalGainFr = regex;
                                    break;
                                case "DetrimentalLose":
                                    PlayerRegEx.DetrimentalLoseFr = regex;
                                    break;
                            }

                            #endregion

                            break;
                        case "JA":

                            #region Handle Japanese Regular Expressions

                            switch (xKey)
                            {
                                case "Damage":
                                    PlayerRegEx.DamageJa = regex;
                                    break;
                                case "DamageAuto":
                                    PlayerRegEx.DamageAutoJa = regex;
                                    break;
                                case "Failed":
                                    PlayerRegEx.FailedJa = regex;
                                    break;
                                case "FailedAuto":
                                    PlayerRegEx.FailedAutoJa = regex;
                                    break;
                                case "Actions":
                                    PlayerRegEx.ActionsJa = regex;
                                    break;
                                case "Items":
                                    PlayerRegEx.ItemsJa = regex;
                                    break;
                                case "Cure":
                                    PlayerRegEx.CureJa = regex;
                                    break;
                                case "BeneficialGain":
                                    PlayerRegEx.BeneficialGainJa = regex;
                                    break;
                                case "BeneficialLose":
                                    PlayerRegEx.BeneficialLoseJa = regex;
                                    break;
                                case "DetrimentalGain":
                                    PlayerRegEx.DetrimentalGainJa = regex;
                                    break;
                                case "DetrimentalLose":
                                    PlayerRegEx.DetrimentalLoseJa = regex;
                                    break;
                            }

                            #endregion

                            break;
                        case "DE":

                            #region Handle German Regular Expressions

                            switch (xKey)
                            {
                                case "Damage":
                                    PlayerRegEx.DamageDe = regex;
                                    break;
                                case "DamageAuto":
                                    PlayerRegEx.DamageAutoDe = regex;
                                    break;
                                case "Failed":
                                    PlayerRegEx.FailedDe = regex;
                                    break;
                                case "FailedAuto":
                                    PlayerRegEx.FailedAutoDe = regex;
                                    break;
                                case "Actions":
                                    PlayerRegEx.ActionsDe = regex;
                                    break;
                                case "Items":
                                    PlayerRegEx.ItemsDe = regex;
                                    break;
                                case "Cure":
                                    PlayerRegEx.CureDe = regex;
                                    break;
                                case "BeneficialGain":
                                    PlayerRegEx.BeneficialGainDe = regex;
                                    break;
                                case "BeneficialLose":
                                    PlayerRegEx.BeneficialLoseDe = regex;
                                    break;
                                case "DetrimentalGain":
                                    PlayerRegEx.DetrimentalGainDe = regex;
                                    break;
                                case "DetrimentalLose":
                                    PlayerRegEx.DetrimentalLoseDe = regex;
                                    break;
                            }

                            #endregion

                            break;
                    }
                }
            }

            /// <summary>
            /// </summary>
            public static void LoadMonsterRegEx()
            {
                if (Constants.Parse.XRegEx == null)
                {
                    return;
                }
                foreach (var xElement in Constants.Parse.XRegEx.Descendants()
                                                  .Elements("Monster"))
                {
                    var xKey = (string) xElement.Attribute("Key");
                    var xLanguage = (string) xElement.Attribute("Language");
                    var xValue = (string) xElement.Element("Value");
                    if (String.IsNullOrWhiteSpace(xKey) || String.IsNullOrWhiteSpace(xValue))
                    {
                        continue;
                    }
                    if (!Common.Constants.IsValidRegex(xValue))
                    {
                        continue;
                    }
                    var regex = new Regex(xValue, SharedRegEx.DefaultOptions);
                    switch (xLanguage)
                    {
                        case "EN":

                            #region Handle English Regular Expressions

                            switch (xKey)
                            {
                                case "Damage":
                                    MonsterRegEx.DamageEn = regex;
                                    break;
                                case "DamageAuto":
                                    MonsterRegEx.DamageAutoEn = regex;
                                    break;
                                case "Failed":
                                    MonsterRegEx.FailedEn = regex;
                                    break;
                                case "FailedAuto":
                                    MonsterRegEx.FailedAutoEn = regex;
                                    break;
                                case "Actions":
                                    MonsterRegEx.ActionsEn = regex;
                                    break;
                                case "Items":
                                    MonsterRegEx.ItemsEn = regex;
                                    break;
                                case "Cure":
                                    MonsterRegEx.CureEn = regex;
                                    break;
                                case "BeneficialGain":
                                    MonsterRegEx.BeneficialGainEn = regex;
                                    break;
                                case "BeneficialLose":
                                    MonsterRegEx.BeneficialLoseEn = regex;
                                    break;
                                case "DetrimentalGain":
                                    MonsterRegEx.DetrimentalGainEn = regex;
                                    break;
                                case "DetrimentalLose":
                                    MonsterRegEx.DetrimentalLoseEn = regex;
                                    break;
                            }

                            #endregion

                            break;
                        case "FR":

                            #region Handle French Regular Expressions

                            switch (xKey)
                            {
                                case "Damage":
                                    MonsterRegEx.DamageFr = regex;
                                    break;
                                case "DamageAuto":
                                    MonsterRegEx.DamageAutoFr = regex;
                                    break;
                                case "Failed":
                                    MonsterRegEx.FailedFr = regex;
                                    break;
                                case "FailedAuto":
                                    MonsterRegEx.FailedAutoFr = regex;
                                    break;
                                case "Actions":
                                    MonsterRegEx.ActionsFr = regex;
                                    break;
                                case "Items":
                                    MonsterRegEx.ItemsFr = regex;
                                    break;
                                case "Cure":
                                    MonsterRegEx.CureFr = regex;
                                    break;
                                case "BeneficialGain":
                                    MonsterRegEx.BeneficialGainFr = regex;
                                    break;
                                case "BeneficialLose":
                                    MonsterRegEx.BeneficialLoseFr = regex;
                                    break;
                                case "DetrimentalGain":
                                    MonsterRegEx.DetrimentalGainFr = regex;
                                    break;
                                case "DetrimentalLose":
                                    MonsterRegEx.DetrimentalLoseFr = regex;
                                    break;
                            }

                            #endregion

                            break;
                        case "JA":

                            #region Handle Japanese Regular Expressions

                            switch (xKey)
                            {
                                case "Damage":
                                    MonsterRegEx.DamageJa = regex;
                                    break;
                                case "DamageAuto":
                                    MonsterRegEx.DamageAutoJa = regex;
                                    break;
                                case "Failed":
                                    MonsterRegEx.FailedJa = regex;
                                    break;
                                case "FailedAuto":
                                    MonsterRegEx.FailedAutoJa = regex;
                                    break;
                                case "Actions":
                                    MonsterRegEx.ActionsJa = regex;
                                    break;
                                case "Items":
                                    MonsterRegEx.ItemsJa = regex;
                                    break;
                                case "Cure":
                                    MonsterRegEx.CureJa = regex;
                                    break;
                                case "BeneficialGain":
                                    MonsterRegEx.BeneficialGainJa = regex;
                                    break;
                                case "BeneficialLose":
                                    MonsterRegEx.BeneficialLoseJa = regex;
                                    break;
                                case "DetrimentalGain":
                                    MonsterRegEx.DetrimentalGainJa = regex;
                                    break;
                                case "DetrimentalLose":
                                    MonsterRegEx.DetrimentalLoseJa = regex;
                                    break;
                            }

                            #endregion

                            break;
                        case "DE":

                            #region Handle German Regular Expressions

                            switch (xKey)
                            {
                                case "Damage":
                                    MonsterRegEx.DamageDe = regex;
                                    break;
                                case "DamageAuto":
                                    MonsterRegEx.DamageAutoDe = regex;
                                    break;
                                case "Failed":
                                    MonsterRegEx.FailedDe = regex;
                                    break;
                                case "FailedAuto":
                                    MonsterRegEx.FailedAutoDe = regex;
                                    break;
                                case "Actions":
                                    MonsterRegEx.ActionsDe = regex;
                                    break;
                                case "Items":
                                    MonsterRegEx.ItemsDe = regex;
                                    break;
                                case "Cure":
                                    MonsterRegEx.CureDe = regex;
                                    break;
                                case "BeneficialGain":
                                    MonsterRegEx.BeneficialGainDe = regex;
                                    break;
                                case "BeneficialLose":
                                    MonsterRegEx.BeneficialLoseDe = regex;
                                    break;
                                case "DetrimentalGain":
                                    MonsterRegEx.DetrimentalGainDe = regex;
                                    break;
                                case "DetrimentalLose":
                                    MonsterRegEx.DetrimentalLoseDe = regex;
                                    break;
                            }

                            #endregion

                            break;
                    }
                }
            }

            /// <summary>
            /// </summary>
            public static void ApplyTheming()
            {
                SetupFont(ref MainView.View.AbilityChatFD);
                SetupColor(ref MainView.View.AbilityChatFD);
            }

            private static void SetupFont(ref xFlowDocument flowDoc)
            {
                var font = Properties.Settings.Default.ChatFont;
                flowDoc._FD.FontFamily = new FontFamily(font.Name);
                flowDoc._FD.FontWeight = font.Bold ? FontWeights.Bold : FontWeights.Regular;
                flowDoc._FD.FontStyle = font.Italic ? FontStyles.Italic : FontStyles.Normal;
                flowDoc._FD.FontSize = font.Size;
            }

            private static void SetupColor(ref xFlowDocument flowDoc)
            {
                flowDoc._FD.Background = new SolidColorBrush(Properties.Settings.Default.ChatBackgroundColor);
            }
        }
    }
}
