// FFXIVAPP
// SettingsVM.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.
// 

using System;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Input;
using FFXIVAPP.Classes;
using FFXIVAPP.Classes.Commands;
using FFXIVAPP.Classes.Helpers;
using FFXIVAPP.Classes.RegExs;
using FFXIVAPP.Controls.Settings;
using HtmlAgilityPack;
using NLog;

namespace FFXIVAPP.ViewModels
{
    public class SettingsVM
    {
        private static string _key = "";
        private static string _colorcode = "";
        public ICommand ChangeThemeCommand { get; private set; }
        public ICommand DefaultSettingsCommand { get; private set; }
        public ICommand ManualUpdateCommand { get; private set; }
        public ICommand GetCICUIDCommand { get; private set; }
        public ICommand SaveCharacterCommand { get; private set; }
        public ICommand ColorSelectionCommand { get; private set; }
        public ICommand UpdateColorCommand { get; private set; }
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public SettingsVM()
        {
            ChangeThemeCommand = new DelegateCommand(ChangeTheme);
            DefaultSettingsCommand = new DelegateCommand(DefaultSettings);
            ManualUpdateCommand = new DelegateCommand(ManualUpdate);
            GetCICUIDCommand = new DelegateCommand(GetCICUID);
            SaveCharacterCommand = new DelegateCommand(SaveCharacter);
            ColorSelectionCommand = new DelegateCommand(ColorSelection);
            UpdateColorCommand = new DelegateCommand(UpdateColor);
        }

        #region GUI Functions

        /// <summary>
        /// </summary>
        private static void ChangeTheme()
        {
            ThemeHelper.ChangeTheme(Settings.Default.Theme);
        }

        /// <summary>
        /// </summary>
        private static void DefaultSettings()
        {
            App.DefaultSettings();
        }

        /// <summary>
        /// </summary>
        private static void ManualUpdate()
        {
            Process.Start("Updater.exe", "FFXIVAPP");
        }

        /// <summary>
        /// </summary>
        private static void GetCICUID()
        {
            SaveCharacter();
            if (Settings.Default.CharacterName.Replace(" ", "").Length < 3 || Settings.Default.ServerName == "")
            {
                string title, message;
                switch (MainWindow.Lang)
                {
                    case "ja":
                        title = "情報が欠落して！";
                        message = "姓、名、サーバー：入力してください";
                        break;
                    case "de":
                        title = "Fehlende Information!";
                        message = "Bitte Eingeben: Vorname, Nachname, Server";
                        break;
                    case "fr":
                        title = "Renseignements Manquants!";
                        message = "S'il Vous Plaît Entrer: Prénom, Nom, Serveur";
                        break;
                    default:
                        title = "Missing Information!";
                        message = "Please Enter : First Name, Last Name, Server";
                        break;
                }
                NotifyHelper.ShowBalloonMessage(3000, title, message);
                return;
            }
            var cicuid = "";
            var uri = "";
            try
            {
                const string url = "http://lodestone.finalfantasyxiv.com/rc/search/search?tgt=77&q=\"{0}\"&cms=&cw={1}";
                var request = (HttpWebRequest) WebRequest.Create(String.Format(url, Settings.Default.CharacterName, Settings.Default.ServerName));
                request.UserAgent = "Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_3; en-US) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.375.70 Safari/533.4";
                request.Headers.Add("Accept-Language", "en;q=0.8");
                var response = (HttpWebResponse) request.GetResponse();
                var s = response.GetResponseStream();
                if (response.StatusCode != HttpStatusCode.OK || s == null)
                {
                }
                else
                {
                    var doc = new HtmlDocument();
                    doc.Load(s);
                    var iconNode = doc.DocumentNode.SelectSingleNode(String.Format("//a[@class='']/@href"));
                    if (iconNode != null)
                    {
                        uri = iconNode.GetAttributeValue("href", "");
                    }
                }
                try
                {
                    cicuid = Shared.Cicuid.Match(uri).Groups["cicuid"].Value;
                }
                catch
                {
                }
                Settings.Default.CICUID = cicuid;
            }
            catch
            {
            }
            finally
            {
                if (String.IsNullOrWhiteSpace(cicuid))
                {
                    string title, message;
                    switch (MainWindow.Lang)
                    {
                        case "ja":
                            title = "エラー！";
                            message = "文字が見つかりませんでした！";
                            break;
                        case "de":
                            title = "Error!";
                            message = "Zeichen Nicht Gefunden!";
                            break;
                        case "fr":
                            title = "Erreur!";
                            message = "Caractère Non Trouvé!";
                            break;
                        default:
                            title = "Error!";
                            message = "Character Not Found!";
                            break;
                    }
                    NotifyHelper.ShowBalloonMessage(3000, title, message);
                }
            }
        }

        /// <summary>
        /// </summary>
        public static void SaveCharacter()
        {
            try
            {
                var first = Character.View.FirstName.Text;
                var last = Character.View.LastName.Text;
                Settings.Default.CharacterName = (first + " " + last).Trim();
                Settings.Default.ServerName = Constants.XServerName[Settings.Default.Server];
            }
            catch
            {
                //MessageBox.Show("Invalid Character Data!", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// </summary>
        private static void ColorSelection()
        {
            if (LogColor.View.Colors.SelectedItems.Count > 0)
            {
                var t = LogColor.View.Colors.SelectedItem.ToString().Split(',');
                _key = t[0].Trim().Replace("[", "");
                _colorcode = Constants.XColor[_key][0];
                LogColor.View.TKey.Text = _key;
                LogColor.View.TValue.Text = _colorcode;
            }
        }

        /// <summary>
        /// </summary>
        private static void UpdateColor()
        {
            _key = LogColor.View.TKey.Text;
            _colorcode = LogColor.View.TValue.Text;
            if (!String.IsNullOrEmpty(_key))
            {
                if (Regex.IsMatch(_colorcode, "^[A-F0-9]{6,6}$"))
                {
                    Constants.XColor[_key][0] = _colorcode;
                    LogColor.View.Colors.Items.Refresh();
                }
                else
                {
                    string title, message;
                    switch (MainWindow.Lang)
                    {
                        case "ja":
                            title = "エラー！";
                            message = "正しいフォーマット： XXXXXX";
                            break;
                        case "de":
                            title = "Error!";
                            message = "Korrekte Format: XXXXXX";
                            break;
                        case "fr":
                            title = "Erreur!";
                            message = "Format Correct: XXXXXX";
                            break;
                        default:
                            title = "Error!";
                            message = "Correct Format: XXXXXX";
                            break;
                    }
                    NotifyHelper.ShowBalloonMessage(3000, title, message);
                }
            }
        }

        #endregion
    }
}