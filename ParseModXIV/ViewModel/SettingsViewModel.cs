// ParseModXIV
// SettingsViewModel.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using AppModXIV.Commands;
using MahApps.Metro;
using ParseModXIV.Classes;
using ParseModXIV.View;

namespace ParseModXIV.ViewModel
{
    internal class SettingsViewModel : INotifyPropertyChanged
    {
        private static HttpWebRequest _httpWReq;
        private static HttpWebResponse _httpWResp;
        private static Encoding _resEncoding;
        private DelegateCommand _command;

        #region " COMMAND FUNCTIONS "

        public ICommand ChangeThemeCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(ChangeTheme);

                return _command;
            }
        }

        public ICommand DefaultSettingsCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(DefaultSettings);

                return _command;
            }
        }

        public ICommand SetProcessCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(SetProcess);

                return _command;
            }
        }

        public ICommand RefreshListCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(RefreshList);

                return _command;
            }
        }

        public ICommand GetCICUIDCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(GetCICUID);

                return _command;
            }
        }

        #endregion

        #region " GUI FUNCTIONS "

        private static void ChangeTheme()
        {
            try
            {
                var split = Settings.Default.Theme.Split('|');
                var accent = split[0];
                var theme = split[1];
                switch (theme)
                {
                    case "Dark":
                        ThemeManager.ChangeTheme(MainView.View, ThemeHelper.DefaultAccents.First(a => a.Name == accent), Theme.Dark);
                        break;
                    case "Light":
                        ThemeManager.ChangeTheme(MainView.View, ThemeHelper.DefaultAccents.First(a => a.Name == accent), Theme.Light);
                        break;
                }
            }
            catch
            {
            }
        }

        private static void DefaultSettings()
        {
            Settings.Default.Reset();
            Settings.Default.Reload();
            try
            {
                var p = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Assembly.GetExecutingAssembly().GetName().Name);
                Directory.Delete(p, true);
            }
            catch
            {
            }
        }

        private static void SetProcess()
        {
            ParseMod.Instance.StopLogging();
            ParseMod.SetPid();
            ParseMod.Instance.StartLogging();
        }

        private static void RefreshList()
        {
            SettingsView.View.gui_PIDSelect.Items.Clear();
            ParseMod.Instance.StopLogging();
            ParseMod.ResetPid();
            ParseMod.Instance.StartLogging();
        }

        private void GetCICUID()
        {
            Character();
            if (Settings.Default.CharacterName.Replace(" ", "") == "" || Settings.Default.ServerName == "")
            {
                return;
            }
            try
            {
                const string url = "http://lodestone.finalfantasyxiv.com/rc/search/search?tgt=77&q=\"{0}\"&cms=&cw={1}";
                _httpWReq = (HttpWebRequest) WebRequest.Create(String.Format(url, Settings.Default.CharacterName, Settings.Default.ServerName));
                _httpWReq.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:10.0) Gecko/20100101 Firefox/10.0";
                _httpWReq.Headers.Add("Accept-Language", "en;q=0.8");
                _httpWResp = (HttpWebResponse) _httpWReq.GetResponse();
                _resEncoding = Encoding.GetEncoding(_httpWResp.CharacterSet);
                var sr = new StreamReader(_httpWResp.GetResponseStream(), _resEncoding);
                var textResponse = sr.ReadToEnd();
                sr.Close();
                sr.Dispose();
                _httpWResp.Close();
                var source = textResponse;
                source = source.Substring(source.IndexOf("/rc/character/top", StringComparison.Ordinal));
                source = source.Substring(0, source.IndexOf("\"", StringComparison.Ordinal));
                var cicuid = "";
                try
                {
                    cicuid = RegExps.Cicuid.Match(source).Groups["cicuid"].Value;
                }
                catch
                {
                }
                Settings.Default.CICUID = cicuid;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public static void Character()
        {
            try
            {
                Settings.Default.CharacterName = (SettingsView.View.gui_FirstName.Text + " " + SettingsView.View.gui_LastName.Text).Trim();
                Settings.Default.ServerName = ParseMod.ServerName[Settings.Default.Server];
            }
            catch
            {
                //MessageBox.Show("Invalid Character Data!", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #endregion
    }
}