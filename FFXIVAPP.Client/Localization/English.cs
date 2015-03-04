// FFXIVAPP.Client
// English.cs
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

using System.Windows;

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
            Dictionary.Add("app_", "*PH*");
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
            Dictionary.Add("app_CharacterNameLabel", "Character Name:");
            Dictionary.Add("app_FirstNameLabel", "First Name:");
            Dictionary.Add("app_GameLanguageLabel", "Game Language:");
            Dictionary.Add("app_ImportLodestoneIDButtonText", "Import Lodestone ID");
            Dictionary.Add("app_InformationMessage", "Information!");
            Dictionary.Add("app_LastNameLabel", "Last Name:");
            Dictionary.Add("app_LatestLabel", "Latest:");
            Dictionary.Add("app_LodestoneIDLabel", "Lodestone ID");
            Dictionary.Add("app_MainToolTip", "Main");
            Dictionary.Add("app_MainSettingsTabHeader", "Main Settings");
            Dictionary.Add("app_CancelButtonText", "Cancel");
            Dictionary.Add("app_PluginsToolTip", "Plugins");
            Dictionary.Add("app_PluginSettingsTabHeader", "Plugin Settings");
            Dictionary.Add("app_PluginWarningText", "This will let any loaded plugin send commands to your game. Enable this if you trust them.");
            Dictionary.Add("app_ProcessIDHeader", "Current Process ID");
            Dictionary.Add("app_RefreshProcessButtonText", "Refresh Process List");
            Dictionary.Add("app_SaveCharacterButtonText", "Save Character");
            Dictionary.Add("app_SaveAndClearHistoryToolTip", "Save & Clear Chat History");
            Dictionary.Add("app_SaveLogHeader", "Save Log");
            Dictionary.Add("app_ScreenShotToolTip", "ScreenShot");
            Dictionary.Add("app_ServerLabel", "Server:");
            Dictionary.Add("app_SettingsToolTip", "Settings");
            Dictionary.Add("app_TabSettingsTabHeader", "Tab Settings");
            Dictionary.Add("app_UpdateColorButtonText", "Update Color");
            Dictionary.Add("app_VersionInformationHeader", "Version Information");
            Dictionary.Add("app_VersionLabel", "Version:");
            Dictionary.Add("app_WarningMessage", "Warning!");
            Dictionary.Add("app_YesButtonText", "Yes");
            Dictionary.Add("app_OtherOptionsTabHeader", "Other Options");
            Dictionary.Add("app_AboutToolTip", "About");
            Dictionary.Add("app_ManualUpdateButtonText", "Manual Update");
            Dictionary.Add("app_TranslationsHeader", "Translations");
            Dictionary.Add("app_DonationsContributionsHeader", "Donations & Contributions");
            Dictionary.Add("app_SpecialThanksHeader", "Special Thanks");
            Dictionary.Add("app_DownloadNoticeHeader", "Update Available!");
            Dictionary.Add("app_DownloadNoticeMessage", "Download?");
            Dictionary.Add("app_IntegrationWarningText", "Enabling this option means no personally identifable information (game or real life) is sent to the server.  You would be authorization the collection of game related data only.\n\nThe information processed is monster deaths, loot, monster spawn locations, npc and gathering locations.\n\nThis is completely optional and can be turned on or off at any time.");
            Dictionary.Add("app_EnableHelpLabelsHeader", "Enable Help Labels");
            Dictionary.Add("app_OKButtonText", "OK");
            Dictionary.Add("app_TopMostHeader", "Always On Top");
            Dictionary.Add("app_OfficialPluginsTabHeader", "Official Plugins");
            Dictionary.Add("app_ThirdPartyPluginsTabHeader", "Third Party Plugins");
            Dictionary.Add("app_IntegrationSettingsTabHeader", "Integration Settings");
            Dictionary.Add("app_NoPluginsLineOneTextBlock", "You might have recently turned on or off all plugins; or just have nothing loaded at all.");
            Dictionary.Add("app_NoPluginsLineTwoTextBlock", "Confirm your settings and if loaded choose a plugin icon from the tab menu.");
            Dictionary.Add("app_AlwaysReadUpdatesMessage", "Always read the update history for all changes.");
            Dictionary.Add("app_UpdateNotesHeader", "Update Notes");
            Dictionary.Add("app_ChangesOnRestartMessage", "Changes will take place after restarting the application.");
            Dictionary.Add("app_AvailablePluginsTabHeader", "Available Plugins");
            Dictionary.Add("app_PluginSourcesTabHeader", "Plugin Sources");
            Dictionary.Add("app_SourceLabel", "Source:");
            Dictionary.Add("app_EnabledHeader", "Enabled");
            Dictionary.Add("app_VersionHeader", "Version");
            Dictionary.Add("app_StatusHeader", "Status");
            Dictionary.Add("app_FilesHeader", "Files");
            Dictionary.Add("app_SourceURIHeader", "SourceURI");
            Dictionary.Add("app_AddUpdateSourceButtonText", "Add Or Update Source");
            Dictionary.Add("app_RefreshPluginsButtonText", "Refresh Plugins");
            Dictionary.Add("app_UnInstallButtonText", "Un-Install");
            Dictionary.Add("app_InstallButtonText", "Install");
            Dictionary.Add("app_AddOrUpdateSourceButtonText", "Add Or Update Source");
            Dictionary.Add("app_NameHeader", "Name");
            Dictionary.Add("app_UpdateToolTip", "Update");
            Dictionary.Add("app_pluginUpdateTitle", "Plugin Updates!");
            Dictionary.Add("app_pluginUpdateMessageText", "It appears some plugins have updates available. To ensure compatibility please update at your earliest convenience via the \"Update\" tab.");
            Dictionary.Add("app_CurrentVersionHeader", "Current");
            Dictionary.Add("app_LatestVersionHeader", "Latest");
            return Dictionary;
        }
    }
}
