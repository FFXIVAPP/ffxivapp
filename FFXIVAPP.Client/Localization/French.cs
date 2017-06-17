﻿// FFXIVAPP.Client ~ French.cs
// 
// Copyright © 2007 - 2016 Ryan Wilson - All Rights Reserved
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
    public abstract class French
    {
        private static readonly ResourceDictionary Dictionary = new ResourceDictionary();

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public static ResourceDictionary Context()
        {
            Dictionary.Clear();
            Dictionary.Add("app_", "*PH*");
            Dictionary.Add("app_AllowPluginCommandsHeader", "Autoriser les commandes de plugin");
            Dictionary.Add("app_AttachProcessButtonText", "Lier le processus sélectionné");
            Dictionary.Add("app_ChangeThemeHeader", "Changer le thème");
            Dictionary.Add("app_CharacterInformationHeader", "Informations du personnage");
            Dictionary.Add("app_CharacterSettingsTabHeader", "Paramètres du personnage");
            Dictionary.Add("app_CodeHeader", "Code");
            Dictionary.Add("app_CodeLabel", "Code:");
            Dictionary.Add("app_ColorHeader", "Couleur");
            Dictionary.Add("app_ColorLabel", "Couleur:");
            Dictionary.Add("app_ColorSettingsTabHeader", "Paramètres des couleurs");
            Dictionary.Add("app_ComingSoonText", "A venir!");
            Dictionary.Add("app_CopyrightLabel", "Droits d'auteur");
            Dictionary.Add("app_CurrentLabel", "Actuelle:");
            Dictionary.Add("app_DefaultSettingsButtonText", "Paramètres par défaut");
            Dictionary.Add("app_DeleteMessage", "Supprimer");
            Dictionary.Add("app_DescriptionHeader", "Description");
            Dictionary.Add("app_DescriptionLabel", "Description:");
            Dictionary.Add("app_EnableNLogHeader", "Activer le logging avec NLog");
            Dictionary.Add("app_CharacterNameLabel", "Character Name:");
            Dictionary.Add("app_FirstNameLabel", "Prénom:");
            Dictionary.Add("app_GameLanguageLabel", "Langage du jeu:");
            Dictionary.Add("app_ImportLodestoneIDButtonText", "Importer l'ID Lodestone");
            Dictionary.Add("app_InformationMessage", "Information!");
            Dictionary.Add("app_LastNameLabel", "Nom:");
            Dictionary.Add("app_LatestLabel", "Dernière:");
            Dictionary.Add("app_LodestoneIDLabel", "ID Lodestone");
            Dictionary.Add("app_MainToolTip", "Général");
            Dictionary.Add("app_MainSettingsTabHeader", "Paramètres généraux");
            Dictionary.Add("app_CancelButtonText", "Annuler");
            Dictionary.Add("app_PluginsToolTip", "Plugins");
            Dictionary.Add("app_PluginSettingsTabHeader", "Paramètres de plugin");
            Dictionary.Add("app_PluginWarningText", "Ceci permettra à tous les plugins chargés d'envoyer des commandes au jeu. Activer-le si vous y faites confiance.");
            Dictionary.Add("app_ProcessIDHeader", "ID du processus actuel");
            Dictionary.Add("app_RefreshProcessButtonText", "Rafraîchir la liste des processus");
            Dictionary.Add("app_SaveCharacterButtonText", "Sauvegarder le personnage");
            Dictionary.Add("app_SaveAndClearHistoryToolTip", "Exporter l'historique du chat");
            Dictionary.Add("app_SaveLogHeader", "Sauvegarder le log");
            Dictionary.Add("app_ScreenShotToolTip", "Capture d'écran");
            Dictionary.Add("app_ServerLabel", "Serveur:");
            Dictionary.Add("app_SettingsToolTip", "Paramètres");
            Dictionary.Add("app_TabSettingsTabHeader", "Paramètres des onglets");
            Dictionary.Add("app_UpdateColorButtonText", "Mettre à jour la couleur");
            Dictionary.Add("app_VersionInformationHeader", "Information des versions");
            Dictionary.Add("app_VersionLabel", "Version:");
            Dictionary.Add("app_WarningMessage", "Avertissement!");
            Dictionary.Add("app_YesButtonText", "Oui");
            Dictionary.Add("app_OtherOptionsTabHeader", "Autres options");
            Dictionary.Add("app_AboutToolTip", "À propos");
            Dictionary.Add("app_ManualUpdateButtonText", "Mise à jour manuelle");
            Dictionary.Add("app_TranslationsHeader", "Traductions");
            Dictionary.Add("app_DonationsContributionsHeader", "Dons et Contributions");
            Dictionary.Add("app_SpecialThanksHeader", "Remerciements spéciaux");
            Dictionary.Add("app_DownloadNoticeHeader", "Mise à jour possible!");
            Dictionary.Add("app_DownloadNoticeMessage", "Télécharger?");
            Dictionary.Add("app_IntegrationWarningText", "Activer cette option signifie qu'aucune information identifiable à titre personnel (IG ou IRL) n'est envoyée au serveur. Vous autoriserez donc la collecte de données relatives au jeu seulement.\n\nLes informations traitées sont les morts de monstres, loot, ainsi que l'emplacement des monstres, pnj et points de récolte.\n\nCeci est complètement optionnel et peut être activé ou désactivé à tout moment.");
            Dictionary.Add("app_EnableHelpLabelsHeader", "Activer Aide étiquettes");
            Dictionary.Add("app_OKButtonText", "OK");
            Dictionary.Add("app_TopMostHeader", "Toujours au premier plan");
            Dictionary.Add("app_OfficialPluginsTabHeader", "Plugins officiels");
            Dictionary.Add("app_ThirdPartyPluginsTabHeader", "Third Party Plugins");
            Dictionary.Add("app_IntegrationSettingsTabHeader", "Paramètres d'intégration");
            Dictionary.Add("app_NoPluginsLineOneTextBlock", "Vous avez peut-être récemment activé ou désactivé tous les plugins; or simplement n'avez rien chargé du tout.");
            Dictionary.Add("app_NoPluginsLineTwoTextBlock", "Confirmez vos paramètres et s'ils sont chargés, sélectionnez un icône de plugin sur le tab menu.");
            Dictionary.Add("app_AlwaysReadUpdatesMessage", "Toujours lire l'historique de la mise à jour pour tous les changements.");
            Dictionary.Add("app_UpdateNotesHeader", "Notes de mise-à-jour");
            Dictionary.Add("app_ChangesOnRestartMessage", "Les changements seront pris en compte après avoir redémarré l'application.");
            Dictionary.Add("app_AvailablePluginsTabHeader", "Plugins disponibles");
            Dictionary.Add("app_PluginSourcesTabHeader", "Plugin Sources");
            Dictionary.Add("app_SourceLabel", "Source:");
            Dictionary.Add("app_EnabledHeader", "Activé");
            Dictionary.Add("app_VersionHeader", "Version");
            Dictionary.Add("app_StatusHeader", "Etat");
            Dictionary.Add("app_FilesHeader", "Fichiers");
            Dictionary.Add("app_SourceURIHeader", "SourceURI");
            Dictionary.Add("app_AddUpdateSourceButtonText", "Ajouter ou mettre à jour la source");
            Dictionary.Add("app_RefreshPluginsButtonText", "Rafraichir les plugins");
            Dictionary.Add("app_UnInstallButtonText", "Désinstaller");
            Dictionary.Add("app_InstallButtonText", "Installer");
            Dictionary.Add("app_AddOrUpdateSourceButtonText", "Ajouter ou mettre à jour la source");
            Dictionary.Add("app_NameHeader", "Nom");
            Dictionary.Add("app_UpdateToolTip", "Mises à jour");
            Dictionary.Add("app_pluginUpdateTitle", "Mise à jour du plugins!");
            Dictionary.Add("app_pluginUpdateMessageText", "Il semble que certains plugins aient des mises à jour disponibles. Pour vous assurer de leur compatibilité, veuillez les mettre à jour à votre meilleure convenance via le tab \"Mises à jour\"");
            Dictionary.Add("app_CurrentVersionHeader", "Actuelle");
            Dictionary.Add("app_LatestVersionHeader", "Dernière");
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
            return Dictionary;
        }
    }
}
