// FFXIVAPP.Plugin.Log
// French.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System.Windows;

#endregion

namespace FFXIVAPP.Plugin.Log.Localization
{
    public abstract class French
    {
        private static readonly ResourceDictionary Dictionary = new ResourceDictionary();

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public static ResourceDictionary Context()
        {
            Dictionary.Clear();
            Dictionary.Add("log_PLACEHOLDER", "*PH*");
            Dictionary.Add("log_AddTabButtonText", "Ajouter onglet");
            Dictionary.Add("log_AllTabHeader", "Tout");
            Dictionary.Add("log_DebugTabHeader", "Debug");
            Dictionary.Add("log_DebugOptionsHeader", "Options de Debug");
            Dictionary.Add("log_EnableDebugHeader", "Activer Debug");
            Dictionary.Add("log_EnableTranslateHeader", "Activer Traduction");
            Dictionary.Add("log_RegExLabel", "RegEx:");
            Dictionary.Add("log_UseRomanizationHeader", "Envoyer Romanisation");
            Dictionary.Add("log_SendToEchoHeader", "Envoyer au Echo");
            Dictionary.Add("log_SendToGameHeader", "Envoyer au jeu");
            Dictionary.Add("log_ShowASCIIDebugHeader", "Afficher Debug ASCII");
            Dictionary.Add("log_TabNameLabel", "Nom de l'onglet:");
            Dictionary.Add("log_TranslateLSHeader", "LS");
            Dictionary.Add("log_TranslateFCHeader", "FC");
            Dictionary.Add("log_TranslatePartyHeader", "Equipe");
            Dictionary.Add("log_TranslateableChatTabHeader", "Chat traduisible");
            Dictionary.Add("log_TranslatedTabHeader", "Traduit");
            Dictionary.Add("log_TranslateJPOnlyHeader", "Traduire exclusivement le JP");
            Dictionary.Add("log_TranslateSettingsTabHeader", "Paramètres de traduction");
            Dictionary.Add("log_TranslateToHeader", "Traduire en");
            Dictionary.Add("log_TranslateSayHeader", "Dire");
            Dictionary.Add("log_TranslateShoutHeader", "Crier");
            Dictionary.Add("log_TranslateYellHeader", "Yell");
            Dictionary.Add("log_TranslateTellHeader", "Murmurer");
            return Dictionary;
        }
    }
}
