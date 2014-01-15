// FFXIVAPP.Client
// ConstantsHelper.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Globalization;
using FFXIVAPP.Client.Models;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Common.Core.Constant;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Helpers
{
    [DoNotObfuscate]
    public static class ConstantsHelper
    {
        public static void UpdatePluginConstants()
        {
            AppContextHelper.Instance.RaiseNewConstants(new ConstantsEntity
            {
                AutoTranslate = Constants.AutoTranslate,
                CharacterName = Constants.CharacterName,
                ChatCodes = Constants.ChatCodes,
                ChatCodesXml = Constants.ChatCodesXml,
                Colors = Constants.Colors,
                CultureInfo = Constants.CultureInfo,
                EnableHelpLabels = Constants.EnableHelpLabels,
                GameLanguage = Constants.GameLanguage,
                ServerName = Constants.ServerName
            });
        }

        #region Action Helpers

        public static string GetActionNameByID(int key)
        {
            var skillKey = key.ToString(CultureInfo.InvariantCulture);
            try
            {
                if (Constants.Actions.ContainsKey(skillKey))
                {
                    switch (Settings.Default.GameLanguage)
                    {
                        case "English":
                            return Constants.Actions[skillKey].EN;
                        case "French":
                            return Constants.Actions[skillKey].FR;
                        case "Japanese":
                            return Constants.Actions[skillKey].JA;
                        case "German":
                            return Constants.Actions[skillKey].DE;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return "???";
        }

        public static ActionInfo GetActionInfoByName(string name)
        {
            try
            {
                foreach (var actionInfo in Constants.Actions)
                {
                    var info = actionInfo.Value;
                    switch (Settings.Default.GameLanguage)
                    {
                        case "English":
                            if (String.Equals(info.EN, name, Constants.InvariantComparer))
                            {
                                return info;
                            }
                            break;
                        case "French":
                            if (String.Equals(info.FR, name, Constants.InvariantComparer))
                            {
                                return info;
                            }
                            break;
                        case "Japanese":
                            if (String.Equals(info.JA, name, Constants.InvariantComparer))
                            {
                                return info;
                            }
                            break;
                        case "German":
                            if (String.Equals(info.DE, name, Constants.InvariantComparer))
                            {
                                return info;
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        #endregion
    }
}
