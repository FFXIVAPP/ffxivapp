// FFXIVAPP.Plugin.Log
// English.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System.Windows;

#endregion

namespace FFXIVAPP.Plugin.Log.Localization
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
            Dictionary.Add("log_PLACEHOLDER", "*PH*");
            Dictionary.Add("log_AddTabButtonText", "Add Tab");
            Dictionary.Add("log_AllTabHeader", "All");
            Dictionary.Add("log_DebugTabHeader", "Debug");
            Dictionary.Add("log_DebugOptionsHeader", "Debug Options");
            Dictionary.Add("log_EnableDebugHeader", "Enable Debug");
            Dictionary.Add("log_EnableTranslateHeader", "Enable Translate");
            Dictionary.Add("log_RegExLabel", "RegEx:");
            Dictionary.Add("log_UseRomanizationHeader", "Use Romanization");
            Dictionary.Add("log_SendToEchoHeader", "Send To Echo");
            Dictionary.Add("log_SendToGameHeader", "Send To Game");
            Dictionary.Add("log_ShowASCIIDebugHeader", "Show ASCII Debug");
            Dictionary.Add("log_TabNameLabel", "Tab Name:");
            Dictionary.Add("log_TranslateLSHeader", "LS");
            Dictionary.Add("log_TranslateFCHeader", "FC");
            Dictionary.Add("log_TranslatePartyHeader", "Party");
            Dictionary.Add("log_TranslateableChatTabHeader", "Translateable Chat");
            Dictionary.Add("log_TranslatedTabHeader", "Translated");
            Dictionary.Add("log_TranslateJPOnlyHeader", "Translate JP Only");
            Dictionary.Add("log_TranslateSettingsTabHeader", "Translate Settings");
            Dictionary.Add("log_TranslateToHeader", "Translate To");
            Dictionary.Add("log_TranslateSayHeader", "Say");
            Dictionary.Add("log_TranslateShoutHeader", "Shout");
            Dictionary.Add("log_TranslateYellHeader", "Yell");
            Dictionary.Add("log_TranslateTellHeader", "Tell");
            return Dictionary;
        }
    }
}
