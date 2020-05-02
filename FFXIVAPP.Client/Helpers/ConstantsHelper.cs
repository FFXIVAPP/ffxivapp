// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstantsHelper.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2020 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   ConstantsHelper.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.Helpers {
    using FFXIVAPP.Common.Core.Constant;

    internal static class ConstantsHelper {
        public static void UpdatePluginConstants() {
            AppContextHelper.Instance.RaiseConstantsUpdated(
                new ConstantsEntity {
                    AutoTranslate = Constants.AutoTranslate,
                    CharacterName = Constants.CharacterName,
                    ChatCodes = Constants.ChatCodes,
                    ChatCodesXML = Constants.ChatCodesXML,
                    Colors = Constants.Colors,
                    CultureInfo = Constants.CultureInfo,
                    EnableHelpLabels = Constants.EnableHelpLabels,
                    GameLanguage = Constants.GameLanguage,
                    ServerName = Constants.ServerName,
                    Theme = Constants.Theme,
                    UIScale = Constants.UIScale,
                    EnableNLog = Constants.EnableNLog,
                    EnableNetworkReading = Constants.EnableNetworkReading,
                });
        }
    }
}