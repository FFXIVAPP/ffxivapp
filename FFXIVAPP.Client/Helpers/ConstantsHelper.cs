// FFXIVAPP.Client
// ConstantsHelper.cs
// 
// © 2013 Ryan Wilson

using FFXIVAPP.Common.Core.Constant;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Helpers
{
    [DoNotObfuscate]
    public static class ConstantsHelper
    {
        public static void UpdatePluginConstants()
        {
            PluginHost.Instance.RaiseNewConstantsEntity(new ConstantsEntity
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
    }
}
