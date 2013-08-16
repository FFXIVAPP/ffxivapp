// FFXIVAPP.Plugin.Log
// German.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.Windows;

#endregion

namespace FFXIVAPP.Plugin.Log.Localization
{
    public abstract class German
    {
        private static readonly ResourceDictionary Dictionary = new ResourceDictionary();

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public static ResourceDictionary Context()
        {
            Dictionary.Clear();
            Dictionary.Add("log_PLACEHOLDER", "*PH*");
            Dictionary.Add("log_AddTabButtonText", "Tab hinzufügen");
            Dictionary.Add("log_AllTabHeader", "Alle");
            Dictionary.Add("log_DebugTabHeader", "Debug");
            Dictionary.Add("log_DebugOptionsHeader", "Debug Einstellungen");
            Dictionary.Add("log_EnableDebugHeader", "Debug Aktivieren");
            Dictionary.Add("log_EnableTranslateHeader", "Übersetzen Aktivieren");
            Dictionary.Add("log_RegExLabel", "RegEx:");
            Dictionary.Add("log_UseRomanizationHeader", "Romanisierung benutzen");
            Dictionary.Add("log_SendToEchoHeader", "An Echo senden");
            Dictionary.Add("log_SendToGameHeader", "Ans Spiel senden");
            Dictionary.Add("log_ShowASCIIDebugHeader", "Ascii Debug zeigen");
            Dictionary.Add("log_TabNameLabel", "Tab Name");
            Dictionary.Add("log_TranslateLSHeader", "LS übersetzen");
            Dictionary.Add("log_TranslateFCHeader", "FC übersetzen");
            Dictionary.Add("log_TranslatePartyHeader", "Party übersetzen");
            Dictionary.Add("log_TranslateableChatTabHeader", "Chat übersetzen");
            Dictionary.Add("log_TranslatedTabHeader", "übersetzen");
            Dictionary.Add("log_TranslateJPOnlyHeader", "Nur JP übersetzen");
            Dictionary.Add("log_TranslateSettingsTabHeader", "übersetzen einstellungen");
            Dictionary.Add("log_TranslateToHeader", "übersetzen");
            Dictionary.Add("log_TranslateSayHeader", "Say übersetzen");
            Dictionary.Add("log_TranslateShoutHeader", "Shout übersetzen");
            Dictionary.Add("log_TranslateTellHeader", "Tell übersetzen");
            Dictionary.Add("log_TranslateYellHeader", "Yell übersetzen");
            return Dictionary;
        }
    }
}
