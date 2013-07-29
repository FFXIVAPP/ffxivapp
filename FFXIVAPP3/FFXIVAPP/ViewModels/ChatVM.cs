// FFXIVAPP
// ChatVM.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System.Windows.Input;
using FFXIVAPP.Classes;
using FFXIVAPP.Classes.Commands;
using FFXIVAPP.Views;

namespace FFXIVAPP.ViewModels
{
    public class ChatVM
    {
        public ICommand ToggleConnectionCommand { get; private set; }

        public ChatVM()
        {
            ToggleConnectionCommand = new DelegateCommand(ToggleConnection);
        }

        #region GUI Functions

        /// <summary>
        /// </summary>
        private static void ToggleConnection()
        {
            if (Settings.Default.SiteName == "" || Settings.Default.APIKey == "")
            {
                MainWindowVM.SwitchView("settings");
                SettingsV.View.TabControl.SelectedIndex = 1;
                string title, message;
                switch (MainWindow.Lang)
                {
                    case "ja":
                        title = "情報が欠落して！";
                        message = "入力してください：サイト名/ APIキー。";
                        break;
                    case "de":
                        title = "Fehlende Information!";
                        message = "Geben Sie Bitte: Site Benutzername / API-Key.";
                        break;
                    case "fr":
                        title = "Renseignements Manquants!";
                        message = "S'il Vous Plaît Entrer: Nom D'Utilisateur Du Site / API Key.";
                        break;
                    default:
                        title = "Missing Information!";
                        message = "Please Enter : Site Username/API Key.";
                        break;
                }
                NotifyHelper.ShowBalloonMessage(3000, title, message);
                return;
            }
            if (FFXIV.Instance.SIO.Socket == null)
            {
                FFXIV.Instance.SIO.Create();
            }
            else
            {
                FFXIV.Instance.SIO.Destroy();
                FFXIV.Instance.SIO.Socket = null;
                ChatV.View.StatusMessage.Content = "Waiting...";
            }
        }

        #endregion
    }
}