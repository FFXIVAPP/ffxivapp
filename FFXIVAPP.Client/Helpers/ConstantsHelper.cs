// FFXIVAPP.Client
// ConstantsHelper.cs
// 
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
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
