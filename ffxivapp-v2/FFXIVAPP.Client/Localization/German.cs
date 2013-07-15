// FFXIVAPP.Client
// German.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System.Windows;

#endregion

namespace FFXIVAPP.Client.Localization
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
            Dictionary.Add("app_PLACEHOLDER", "*PH*");
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
            Dictionary.Add("app_FirstNameLabel", "Virname:");
            Dictionary.Add("app_GameLanguageLabel", "Spielsprache");
            Dictionary.Add("app_ImportLodestoneIDButtonText", "Lodestone ID Importieren");
            Dictionary.Add("app_InformationMessage", "Informationen!");
            Dictionary.Add("app_LastNameLabel", "Nachname:");
            Dictionary.Add("app_LatestLabel", "Neuestes:");
            Dictionary.Add("app_LodestoneIDLabel", "Lodestone ID");
            Dictionary.Add("app_MainToolTip", "Startseite");
            Dictionary.Add("app_MainSettingsTabHeader", "Startseitellungen");
            Dictionary.Add("app_NoButtonText", "Nien");
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
            Dictionary.Add("app_VersionHeader", "Versionsinformationen");
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
            return Dictionary;
        }
    }
}
