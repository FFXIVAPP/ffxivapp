// FFXIVAPP.Client
// English.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System.Windows;

#endregion

namespace FFXIVAPP.Client.Localization
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
            Dictionary.Add("app_", "PLACEHOLDER");
            Dictionary.Add("app_allowplugincommands", "Allow Plugin Commands");
            Dictionary.Add("app_attachprocess", "Attach Selected Process");
            Dictionary.Add("app_changetheme", "Change Theme");
            Dictionary.Add("app_characterinformation", "Character Information");
            Dictionary.Add("app_charactersettings", "Character Settings");
            Dictionary.Add("app_code", "Code");
            Dictionary.Add("app_codelabel", "Code:");
            Dictionary.Add("app_color", "Color");
            Dictionary.Add("app_colorlabel", "Color:");
            Dictionary.Add("app_colorsettings", "Color Settings");
            Dictionary.Add("app_comingsoon", "Coming Soon!");
            Dictionary.Add("app_copyrightlabel", "Copyright:");
            Dictionary.Add("app_currentversionlabel", "Current:");
            Dictionary.Add("app_defaultsettings", "Default Settings");
            Dictionary.Add("app_deletepopupmessage", "Delete");
            Dictionary.Add("app_description", "Description");
            Dictionary.Add("app_descriptionlabel", "Description:");
            Dictionary.Add("app_enablenlog", "Enable Logging With NLog (Requires Restart)");
            Dictionary.Add("app_firstnamelabel", "First Name:");
            Dictionary.Add("app_gamelanguagelabel", "Game Language:");
            Dictionary.Add("app_importlodestoneid", "Import Lodestone ID");
            Dictionary.Add("app_informationpopuptitle", "Information!");
            Dictionary.Add("app_lastnamelabel", "Last Name:");
            Dictionary.Add("app_latestversionlabel", "Latest:");
            Dictionary.Add("app_licenses", "Licences");
            Dictionary.Add("app_loaded", "Loaded");
            Dictionary.Add("app_lodestoneidlabel", "Lodestone ID:");
            Dictionary.Add("app_main", "Main");
            Dictionary.Add("app_mainsettings", "Main Settings");
            Dictionary.Add("app_nobutton", "No");
            Dictionary.Add("app_plugins", "Plugins");
            Dictionary.Add("app_pluginsettings", "Plugin Settings");
            Dictionary.Add("app_pluginwarning", "This will let any loaded plugin send commands to your game. Enable this if you trust them.");
            Dictionary.Add("app_processidnum", "Current Process ID");
            Dictionary.Add("app_refresh", "Refresh");
            Dictionary.Add("app_refreshprocess", "Refresh Process List");
            Dictionary.Add("app_savecharacter", "Save Character");
            Dictionary.Add("app_savehistorypopupmessage", "Press \"Yes\" to export chat history. Press \"No\" to just exit. Application will exit after exporting.");
            Dictionary.Add("app_savelog", "Save Log");
            Dictionary.Add("app_screenshot", "ScreenShot");
            Dictionary.Add("app_serverlabel", "Server:");
            Dictionary.Add("app_settings", "Settings");
            Dictionary.Add("app_tabsettings", "Tab Settings");
            Dictionary.Add("app_updatecolor", "Update Color");
            Dictionary.Add("app_versioninformation", "Version Information");
            Dictionary.Add("app_versionlabel", "Version:");
            Dictionary.Add("app_warningpopuptitle", "Warning!");
            Dictionary.Add("app_yesbutton", "Yes");
            return Dictionary;
        }
    }
}
