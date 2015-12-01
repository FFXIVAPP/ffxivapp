// FFXIVAPP.Client
// FFXIVAPP & Related Plugins/Modules
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using FFXIVAPP.Common.Core.Constant;

namespace FFXIVAPP.Client.Helpers
{
    public static class ConstantsHelper
    {
        public static void UpdatePluginConstants()
        {
            AppContextHelper.Instance.RaiseNewConstants(new ConstantsEntity
            {
                AutoTranslate = Constants.AutoTranslate,
                CharacterName = Constants.CharacterName,
                ChatCodes = Constants.ChatCodes,
                Actions = Constants.Actions,
                ChatCodesXml = Constants.ChatCodesXml,
                Colors = Constants.Colors,
                CultureInfo = Constants.CultureInfo,
                EnableHelpLabels = Constants.EnableHelpLabels,
                GameLanguage = Constants.GameLanguage,
                ServerName = Constants.ServerName,
                Theme = Constants.Theme,
                UIScale = Constants.UIScale,
                EnableNLog = Constants.EnableNLog,
                EnableNetworkReading = Constants.EnableNetworkReading
            });
        }

        #region Action Helpers

        //public static string GetActionNameByID(int key)
        //{
        //    var skillKey = key.ToString(CultureInfo.InvariantCulture);
        //    try
        //    {
        //        if (Constants.Actions.ContainsKey(skillKey))
        //        {
        //            switch (Settings.Default.GameLanguage)
        //            {
        //                case "French":
        //                    return Constants.Actions[skillKey].FR;
        //                case "Japanese":
        //                    return Constants.Actions[skillKey].JA;
        //                case "German":
        //                    return Constants.Actions[skillKey].DE;
        //                case "Chinese":
        //                    return Constants.Actions[skillKey].ZH;
        //                default:
        //                    return Constants.Actions[skillKey].EN;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return "???";
        //}

        //public static ActionInfo GetActionInfoByName(string name)
        //{
        //    try
        //    {
        //        foreach (var actionInfo in Constants.Actions)
        //        {
        //            var info = actionInfo.Value;
        //            switch (Settings.Default.GameLanguage)
        //            {
        //                case "French":
        //                    if (String.Equals(info.FR, name, Constants.InvariantComparer))
        //                    {
        //                        return info;
        //                    }
        //                    break;
        //                case "Japanese":
        //                    if (String.Equals(info.JA, name, Constants.InvariantComparer))
        //                    {
        //                        return info;
        //                    }
        //                    break;
        //                case "German":
        //                    if (String.Equals(info.DE, name, Constants.InvariantComparer))
        //                    {
        //                        return info;
        //                    }
        //                    break;
        //                case "Chinese":
        //                    if (String.Equals(info.EN, name, Constants.InvariantComparer))
        //                    {
        //                        return info;
        //                    }
        //                    break;
        //                default:
        //                    if (String.Equals(info.EN, name, Constants.InvariantComparer))
        //                    {
        //                        return info;
        //                    }
        //                    break;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return null;
        //}

        #endregion
    }
}
