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
            Dictionary.Add("app_PLACEHOLDER", "*PH*");
            Dictionary.Add("app_AllowPluginCommandsHeader", "Allow Plugin Commands");
            Dictionary.Add("app_AttachProcessButtonText", "Attach Selected Process");
            Dictionary.Add("app_ChangeThemeHeader", "Change Theme");
            Dictionary.Add("app_CharacterInformationHeader", "Character Information");
            Dictionary.Add("app_CharacterSettingsTabHeader", "Character Settings");
            Dictionary.Add("app_CodeHeader", "Code");
            Dictionary.Add("app_CodeLabel", "Code:");
            Dictionary.Add("app_ColorHeader", "Color");
            Dictionary.Add("app_ColorLabel", "Color:");
            Dictionary.Add("app_ColorSettingsTabHeader", "Color Settings");
            Dictionary.Add("app_ComingSoonText", "Coming Soon!");
            Dictionary.Add("app_CopyrightLabel", "Copyright:");
            Dictionary.Add("app_CurrentLabel", "Current:");
            Dictionary.Add("app_DefaultSettingsButtonText", "Default Settings");
            Dictionary.Add("app_DeleteMessage", "Delete");
            Dictionary.Add("app_DescriptionHeader", "Description");
            Dictionary.Add("app_DescriptionLabel", "Description:");
            Dictionary.Add("app_EnableNLogHeader", "Enable Logging With NLog");
            Dictionary.Add("app_FirstNameLabel", "First Name:");
            Dictionary.Add("app_GameLanguageLabel", "Game Language:");
            Dictionary.Add("app_ImportLodestoneIDButtonText", "Import Lodestone ID");
            Dictionary.Add("app_InformationMessage", "Information!");
            Dictionary.Add("app_LastNameLabel", "Last Name:");
            Dictionary.Add("app_LatestLabel", "Latest:");
            Dictionary.Add("app_LodestoneIDLabel", "Lodestone ID");
            Dictionary.Add("app_MainToolTip", "Main");
            Dictionary.Add("app_MainSettingsTabHeader", "Main Settings");
            Dictionary.Add("app_NoButtonText", "No");
            Dictionary.Add("app_PluginsToolTip", "Plugins");
            Dictionary.Add("app_PluginSettingsTabHeader", "Plugin Settings");
            Dictionary.Add("app_PluginWarningText", "This will let any loaded plugin send commands to your game. Enable this if you trust them.");
            Dictionary.Add("app_ProcessIDHeader", "Current Process ID");
            Dictionary.Add("app_RefreshProcessButtonText", "Refresh Process List");
            Dictionary.Add("app_SaveCharacterButtonText", "Save Character");
            Dictionary.Add("app_SaveHistoryMessage", "Press \"Yes\" to export chat history. Press \"No\" to just exit. Application will exit after exporting.");
            Dictionary.Add("app_SaveLogHeader", "Save Log");
            Dictionary.Add("app_ScreenshotToolTip", "ScreenShot");
            Dictionary.Add("app_ServerLabel", "Server:");
            Dictionary.Add("app_SettingsToolTip", "Settings");
            Dictionary.Add("app_TabSettingsTabHeader", "Tab Settings");
            Dictionary.Add("app_UpdateColorButtonText", "Update Color");
            Dictionary.Add("app_VersionHeader", "Version Information");
            Dictionary.Add("app_VersionLabel", "Version:");
            Dictionary.Add("app_WarningMessage", "Warning!");
            Dictionary.Add("app_YesButtonText", "Yes");
            Dictionary.Add("app_OtherOptionsTabHeader", "Other Options");
            return Dictionary;
        }
    }
}
