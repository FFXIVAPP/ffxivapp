// FFXIVAPP.Plugin.Log
// English.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

using System.Windows;

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
            Dictionary.Add("log_", "PLACEHOLDER");
            Dictionary.Add("log_addtab", "Add Tab");
            Dictionary.Add("log_alllog", "All");
            Dictionary.Add("log_debuglog", "Debug");
            Dictionary.Add("log_debugoptions", "Debug Options");
            Dictionary.Add("log_enabledebug", "Enable Debug");
            Dictionary.Add("log_enabletranslate", "Enable Translate");
            Dictionary.Add("log_regexlabel", "RegEx:");
            Dictionary.Add("log_sendromanization", "Send Romanization");
            Dictionary.Add("log_sendtoecho", "Send To Echo");
            Dictionary.Add("log_sendtogame", "Send To Game");
            Dictionary.Add("log_showasciidebug", "Show ASCII Debug");
            Dictionary.Add("log_tabnamelabel", "Tab Name:");
            Dictionary.Add("log_tls", "LS");
            Dictionary.Add("log_tparty", "Party");
            Dictionary.Add("log_translateablechat", "Translateable Chat");
            Dictionary.Add("log_translatedlog", "Translated");
            Dictionary.Add("log_translatejponly", "Translate JP Only");
            Dictionary.Add("log_translatesettings", "Translate Settings");
            Dictionary.Add("log_translateto", "Translate To");
            Dictionary.Add("log_tsay", "Say");
            Dictionary.Add("log_tshout", "Shout");
            Dictionary.Add("log_ttell", "Tell");
            return Dictionary;
        }
    }
}