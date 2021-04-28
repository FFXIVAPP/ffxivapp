// --------------------------------------------------------------------------------------------------------------------
// <copyright file="German.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   German.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.Localization {
    using System.Windows;

    internal abstract class German {
        private static readonly ResourceDictionary Dictionary = new ResourceDictionary();

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public static ResourceDictionary Context() {
            Dictionary.Clear();
            Dictionary.Add("app_", "*PH*");
            Dictionary.Add("app_AllowPluginCommandsHeader", "Plugin Kommandos zulassen");
            Dictionary.Add("app_AttachProcessButtonText", "Ausgewählten Prozess anfügen");
            Dictionary.Add("app_ChangeThemeHeader", "Farbschema ändern");
            Dictionary.Add("app_CharacterInformationHeader", "Character Informationen");
            Dictionary.Add("app_CharacterSettingsTabHeader", "Charaktereinstellungen");
            Dictionary.Add("app_CodeHeader", "Code");
            Dictionary.Add("app_CodeLabel", "Code:");
            Dictionary.Add("app_ColorHeader", "Farbe");
            Dictionary.Add("app_ColorLabel", "Farbe:");
            Dictionary.Add("app_ColorSettingsTabHeader", "Farbeinstellungen");
            Dictionary.Add("app_ComingSoonText", "Coming Soon!");
            Dictionary.Add("app_CopyrightLabel", "Copyright:");
            Dictionary.Add("app_CurrentLabel", "gegenwärtig:");
            Dictionary.Add("app_DefaultSettingsButtonText", "Standardeinstellungen");
            Dictionary.Add("app_DeleteMessage", "Löschen");
            Dictionary.Add("app_DescriptionHeader", "Beschreibung");
            Dictionary.Add("app_DescriptionLabel", "Beschreibung:");
            Dictionary.Add("app_EnableNLogHeader", "Protokoll mit Nlog zulassen");
            Dictionary.Add("app_CharacterNameLabel", "Character Name:");
            Dictionary.Add("app_FirstNameLabel", "Virname:");
            Dictionary.Add("app_GameLanguageLabel", "Spielsprache");
            Dictionary.Add("app_ImportLodestoneIDButtonText", "Lodestone ID Importieren");
            Dictionary.Add("app_InformationMessage", "Informationen!");
            Dictionary.Add("app_LastNameLabel", "Nachname:");
            Dictionary.Add("app_LatestLabel", "Neuestes:");
            Dictionary.Add("app_LodestoneIDLabel", "Lodestone ID");
            Dictionary.Add("app_MainToolTip", "Startseite");
            Dictionary.Add("app_MainSettingsTabHeader", "Startseitellungen");
            Dictionary.Add("app_CancelButtonText", "Cancel");
            Dictionary.Add("app_PluginsToolTip", "Plugins");
            Dictionary.Add("app_PluginSettingsTabHeader", "Plugin einstellungen");
            Dictionary.Add("app_PluginWarningText", "Dies lässte jedes Plugin Befehle an das spiel schicken. Aktiviere diese Option wenn du den Plugins vertraust.");
            Dictionary.Add("app_ProcessIDHeader", "Aktuelle Prozess ID");
            Dictionary.Add("app_RefreshProcessButtonText", "Prozessliste neu laden");
            Dictionary.Add("app_SaveCharacterButtonText", "Character Speichern");
            Dictionary.Add("app_SaveAndClearHistoryToolTip", "Wählen um Chatprotokoll zu exportieren.");
            Dictionary.Add("app_SaveLogHeader", "Protokoll Speichern");
            Dictionary.Add("app_ScreenShotToolTip", "Screenshot");
            Dictionary.Add("app_ServerLabel", "Server:");
            Dictionary.Add("app_SettingsToolTip", "Einstellungen");
            Dictionary.Add("app_TabSettingsTabHeader", "Tab Einstellungen");
            Dictionary.Add("app_UpdateColorButtonText", "Farbe updaten");
            Dictionary.Add("app_VersionInformationHeader", "Versionsinformationen");
            Dictionary.Add("app_VersionLabel", "Version:");
            Dictionary.Add("app_WarningMessage", "Warnung!");
            Dictionary.Add("app_YesButtonText", "Ja");
            Dictionary.Add("app_OtherOptionsTabHeader", "Weitere Optionen");
            Dictionary.Add("app_AboutToolTip", "About");
            Dictionary.Add("app_ManualUpdateButtonText", "Manuelles Update");
            Dictionary.Add("app_TranslationsHeader", "Übersetzungen");
            Dictionary.Add("app_DonationsContributionsHeader", "Spenden & Beiträge");
            Dictionary.Add("app_SpecialThanksHeader", "Special Thanks");
            Dictionary.Add("app_DownloadNoticeHeader", "Update Verfügbar!");
            Dictionary.Add("app_DownloadNoticeMessage", "Herunterladen?");
            Dictionary.Add("app_IntegrationWarningText", "Enabling this option means no personally identifable information (game or real life) is sent to the server.  You would be authorization the collection of game related data only.\n\nThe information processed is monster deaths, loot, monster spawn locations, npc and gathering locations.\n\nThis is completely optional and can be turned on or off at any time.");
            Dictionary.Add("app_EnableHelpLabelsHeader", "Aktivieren Hilfe Labels");
            Dictionary.Add("app_OKButtonText", "OK");
            Dictionary.Add("app_TopMostHeader", "Always On Top");
            Dictionary.Add("app_OfficialPluginsTabHeader", "Official Plugins");
            Dictionary.Add("app_ThirdPartyPluginsTabHeader", "Third Party Plugins");
            Dictionary.Add("app_IntegrationSettingsTabHeader", "Integration Settings");
            Dictionary.Add("app_NoPluginsLineOneTextBlock", "You might have recently turned on or off all plugins; or just have nothing loaded at all.");
            Dictionary.Add("app_NoPluginsLineTwoTextBlock", "Confirm your settings and if loaded choose a plugin icon from the tab menu.");
            Dictionary.Add("app_AlwaysReadUpdatesMessage", "Immer für alle Änderungen lesen Sie die Update-Geschichte.");
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