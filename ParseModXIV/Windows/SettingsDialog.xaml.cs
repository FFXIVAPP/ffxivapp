// ParseModXIV
// SettingsDialog.xaml.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using ParseModXIV.Classes;

namespace ParseModXIV.Windows
{
    /// <summary>
    /// Interaction logic for SettingsDialog.xaml
    /// </summary>
    public partial class SettingsDialog
    {
        private static HttpWebRequest _httpWReq;
        private static HttpWebResponse _httpWResp;
        private static Encoding _resEncoding;

        /// <summary>
        /// 
        /// </summary>
        public SettingsDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gui_OK_Click(object sender, RoutedEventArgs e)
        {
            UpdateCharacter();
            DialogResult = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gui_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gui_GetCICUID_Click(object sender, RoutedEventArgs e)
        {
            UpdateCharacter();
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

        /// <summary>
        /// 
        /// </summary>
        private void UpdateCharacter()
        {
            try
            {
                Settings.Default.CharacterName = gui_FirstName.Text + " " + gui_LastName.Text;
                Settings.Default.ServerName = ParseMod.ServerName[Settings.Default.Server];
            }
            catch
            {
                MessageBox.Show("Invalid Character Data!", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}