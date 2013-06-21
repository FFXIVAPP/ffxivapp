// FFXIVAPP.Plugin.Log
// Japanese.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System.Windows;

#endregion

namespace FFXIVAPP.Plugin.Log.Localization
{
    public abstract class Japanese
    {
        private static readonly ResourceDictionary Dictionary = new ResourceDictionary();

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public static ResourceDictionary Context()
        {
            Dictionary.Clear();
            Dictionary.Add("log_PLACEHOLDER", "*PH*");
            Dictionary.Add("log_AddTabButtonText", "加える");
            Dictionary.Add("log_AllTabHeader", "全て");
            Dictionary.Add("log_DebugTabHeader", "デバッグ");
            Dictionary.Add("log_DebugOptionsHeader", "デバッグオプション");
            Dictionary.Add("log_EnableDebugHeader", "デバッグ情報を表示");
            Dictionary.Add("log_EnableTranslateHeader", "チャットを翻訳する");
            Dictionary.Add("log_RegExLabel", "RegEx");
            Dictionary.Add("log_UseRomanizationHeader", "ローマ字");
            Dictionary.Add("log_SendToEchoHeader", "/echoを送信");
            Dictionary.Add("log_SendToGameHeader", "/cmを送信");
            Dictionary.Add("log_ShowASCIIDebugHeader", "ASCIIコードを表示 (デバッグ用)");
            Dictionary.Add("log_TabNameLabel", "タブ名");
            Dictionary.Add("log_TranslateLSHeader", "翻訳するLS");
            Dictionary.Add("log_TranslatePartyHeader", "翻訳するParty");
            Dictionary.Add("log_TranslateableChatTabHeader", "翻訳可能なチャット");
            Dictionary.Add("log_TranslatedTabHeader", "翻訳");
            Dictionary.Add("log_TranslateJPOnlyHeader", "日本語のみ翻訳する");
            Dictionary.Add("log_TranslateSettingsTabHeader", "翻訳オプション");
            Dictionary.Add("log_TranslateToHeader", "翻訳先言語");
            Dictionary.Add("log_TranslateSayHeader", "翻訳するSay");
            Dictionary.Add("log_TranslateShoutHeader", "翻訳するShout");
            Dictionary.Add("log_TranslateTellHeader", "翻訳するTell");
            return Dictionary;
        }
    }
}
