// FFXIVAPP.Client
// French.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.Windows;

#endregion

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
            Dictionary.Add("app_PLACEHOLDER", "*PH*");
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
            Dictionary.Add("app_FirstNameLabel", "Prénom:");
            Dictionary.Add("app_GameLanguageLabel", "Langage du jeu:");
            Dictionary.Add("app_ImportLodestoneIDButtonText", "Importer l'ID Lodestone");
            Dictionary.Add("app_InformationMessage", "Information!");
            Dictionary.Add("app_LastNameLabel", "Nom:");
            Dictionary.Add("app_LatestLabel", "Dernière:");
            Dictionary.Add("app_LodestoneIDLabel", "ID Lodestone");
            Dictionary.Add("app_MainToolTip", "Général");
            Dictionary.Add("app_MainSettingsTabHeader", "Paramètres généraux");
            Dictionary.Add("app_NoButtonText", "Non");
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
            Dictionary.Add("app_VersionHeader", "Information des versions");
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
            Dictionary.Add("app_IntegrationWarningText", "Enabling this option means no personally identifable information (game or real life) is sent to the server.  You would be authorization the collection of game related data only.\n\nThe information processed is monster deaths, loot, monster spawn locations, npc and gathering locations.\n\nThis is completely optional and can be turned on or off at any time.");
            Dictionary.Add("app_EnableHelpLabelsHeader", "Activer Aide étiquettes");
            Dictionary.Add("app_OKButtonText", "OK");
            Dictionary.Add("app_TopMostHeader", "Always On Top");
            return Dictionary;
        }
    }
}
