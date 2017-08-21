// FFXIVAPP.Client ~ English.cs
// 
// Copyright © 2007 - 2017 Ryan Wilson - All Rights Reserved
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System.Windows;

namespace FFXIVAPP.Client.Localization
{
    internal abstract class English
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
            Dictionary.Add("app_UILanguageChangeWarningGeneral", "Do you want to change the GameLanguage setting as well to match this applications UILanguage? If you cancel you will manually have to change GameLanguage in Settings later.");
            Dictionary.Add("app_UILanguageChangeWarningChinese", " When changing to or from Chinese an application restart is also required.");
            Dictionary.Add("app_UILanguageChangeWarningKorean", " When changing to or from Korean an application restart is also required.");
            Dictionary.Add("app_UILanguageChangeWarningNoGameLanguage", "The selected UILanguage does not have a supported GameLanguage. Please choose your game language in Settings.");
            Dictionary.Add("app_UIScaleHeader", "UI.Scale");
            Dictionary.Add("app_HomePluginLabel", "Home Plugin");
            Dictionary.Add("app_ProcessSelectedInfo", "*Only use this if you restarted the game or are dual-boxing.");
            Dictionary.Add("app_PALSettingsTabHeader", "Performance & Logging");
            Dictionary.Add("app_DefNetInterfaceLabel", "Default Network Interface (Packet Reading)");
            Dictionary.Add("app_EnableNetReadingLabel", "Enable Network Reading");
            Dictionary.Add("app_BTNResNetWorker", "Reset Network Worker");
            Dictionary.Add("app_DefAudioDeviceLabel", "Default Audio Device");
            Dictionary.Add("app_MemScanSpeedLabel", "Memory Scanning Speed (Milliseconds)");
            Dictionary.Add("app_ActorMSSLabel", "Actors (Anything Targetable)");
            Dictionary.Add("app_ChatLogMSSLabel", "ChatLog");
            Dictionary.Add("app_PartyInfMSSLabel", "Party Info");
            Dictionary.Add("app_PlayerInfMSSLabel", "Player Info (YOU)");
            Dictionary.Add("app_TargEnmMSSLabel", "Targets &amp; Enmity");
            Dictionary.Add("app_InvMSSLabel", "Inventory");
            Dictionary.Add("app_NetworkUseWinPCapLabel", "Use WinPCap For Network Reading");
            Dictionary.Add("app_UseLocalMemoryJSONDataCacheHeader", "Cache Memory JSON Data Locally");
            Dictionary.Add("app_RefreshMemoryWorkersButtonText", "Refresh Memory Workers");
            return Dictionary;
        }
    }
}
