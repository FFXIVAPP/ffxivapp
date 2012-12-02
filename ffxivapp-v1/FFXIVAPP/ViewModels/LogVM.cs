// FFXIVAPP
// LogVM.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using FFXIVAPP.Classes;
using FFXIVAPP.Classes.Helpers;
using FFXIVAPP.Classes.RegExs;
using FFXIVAPP.Controls;
using FFXIVAPP.Properties;
using FFXIVAPP.Views;

namespace FFXIVAPP.ViewModels
{
    public class LogVM
    {
        private static int _count;
        public static ListBox[] ChatScan = new ListBox[0];
        public static readonly ArrayList TabNames = new ArrayList();
        public static readonly ArrayList RegExs = new ArrayList();
        private static FlowDoc _rtb;
        public static readonly Dictionary<string, FlowDocument> DFlowDoc = new Dictionary<string, FlowDocument>();
        public static readonly Dictionary<string, FlowDocumentReader> DFlowDocReader = new Dictionary<string, FlowDocumentReader>();
        public ICommand ManualTranslateCommand { get; private set; }
        public ICommand ShowAddTabCommand { get; private set; }
        public ICommand CancelAddTabCommand { get; private set; }
        public ICommand AddTabCommand { get; private set; }
        public ICommand DeleteTabCommand { get; private set; }
        public ICommand CheckTabsCommand { get; private set; }

        public LogVM()
        {
            ManualTranslateCommand = new DelegateCommand<string>(ManualTranslate);
            ShowAddTabCommand = new DelegateCommand(ShowAddTab);
            CancelAddTabCommand = new DelegateCommand(CancelAddTab);
            AddTabCommand = new DelegateCommand(AddTab);
            DeleteTabCommand = new DelegateCommand(DeleteTab);
            CheckTabsCommand = new DelegateCommand(CheckTabs);
        }

        #region GUI Functions

        /// <summary>
        /// </summary>
        /// <param name="t"> </param>
        private static void ManualTranslate(string t)
        {
            t = t.Trim();
            var o = GoogleTranslate.Offsets[Settings.Default.ManualTranslate].ToString();
            if (t.Length > 0)
            {
                var tmpTranString = GoogleTranslate.TranslateText(t, "en", o, false);
                LogV.View.Chatter.Text = tmpTranString;
                if (Settings.Default.SendToGame)
                {
                    var CM = LogV.View.CM.Text.Trim();
                    var mReg = Shared.TranslateCOMS.Match(CM);
                    if (!mReg.Success)
                    {
                        string title, message;
                        switch (MainWindow.Lang)
                        {
                            case "ja":
                                title = "/cmで無効！";
                                message = "使用してください：/s, /p, /t 名 姓, /l, /sh";
                                break;
                            case "de":
                                title = "Ungültige /cm!";
                                message = "Bitte Verwenden : /s, /p, /t vorname nachname, /l, /sh";
                                break;
                            case "fr":
                                title = "Blancs /cm!";
                                message = "S'il Vous Plaît Utiliser : /s, /p, /t prénom nom, /l, /sh";
                                break;
                            default:
                                title = "Invalid /cm!";
                                message = "Please Use : /s, /p, /t first last, /l, /sh";
                                break;
                        }
                        NotifyHelper.ShowBalloonMessage(3000, title, message);
                        return;
                    }
                    t = string.Format("{0} {1}", CM, tmpTranString);
                    var asc = Encoding.GetEncoding("utf-16");
                    KeyHelper.SendNotify(asc.GetBytes(t));
                }
            }
        }

        /// <summary>
        /// </summary>
        private static void ShowAddTab()
        {
            LogV.View.OptionsTab.Visibility = Visibility.Visible;
            LogV.View.TabControl.SelectedIndex = 0;
        }

        /// <summary>
        /// </summary>
        private static void CancelAddTab()
        {
            LogV.View.OptionsTab.Visibility = Visibility.Collapsed;
            LogV.View.Options.TName.Text = "";
            //LogV.View.Options.TRegEx.Text = "";
            LogV.View.Options.Codes.UnselectAll();
            LogV.View.TabControl.SelectedIndex = 2;
        }

        /// <summary>
        /// </summary>
        private static void AddTab()
        {
            var regex = LogV.View.Options.TRegEx.Text;
            var name = LogV.View.Options.TName.Text;
            var code = "";
            if (LogV.View.Options.Codes.SelectedItems.Count < 1)
            {
                code = "";
            }
            else
            {
                code = LogV.View.Options.Codes.SelectedItems.Cast<object>().Aggregate("", (current, item) => current + (item.ToString().Split(',')[0] + ",")).Replace("[", "");
                code = code.Substring(0, code.Length - 1);
            }
            if (name == "" || code == "" || regex == "")
            {
                string title, message;
                switch (MainWindow.Lang)
                {
                    case "ja":
                        title = "情報が欠落して！";
                        message = "タブ名は、コードを選択します, RegEx：入力してください";
                        break;
                    case "de":
                        title = "Fehlende Information!";
                        message = "Bitte Eingeben: Tab Name, Wählen Codes, RegEx";
                        break;
                    case "fr":
                        title = "Renseignements Manquants!";
                        message = "S'il Vous Plaît Entrer: Nom D'Onglet, Choisir Des Codes, RegEx";
                        break;
                    default:
                        title = "Missing Information!";
                        message = "Please Enter : Tab Name, Choose Codes, RegEx";
                        break;
                }
                NotifyHelper.ShowBalloonMessage(3000, title, message);
                return;
            }
            AddTabPageName(name, code.Split(','), regex);
            LogV.View.OptionsTab.Visibility = Visibility.Collapsed;
            LogV.View.Options.TName.Text = "";
            //LogV.View.Options.TRegEx.Text = "";
            LogV.View.Options.Codes.UnselectAll();
            LogV.View.TabControl.SelectedIndex = 2;
        }

        /// <summary>
        /// </summary>
        /// <param name="nameOfTab"> </param>
        /// <param name="splitOfChatCodes"> </param>
        /// <param name="regex"> </param>
        public static void AddTabPageName(string nameOfTab, IEnumerable<string> splitOfChatCodes, string regex)
        {
            nameOfTab = Regex.Replace(nameOfTab, "[^a-zA-Z]", "");
            if (splitOfChatCodes.Count() > 0)
            {
                switch (Constants.XTab.ContainsKey(nameOfTab))
                {
                    case true:
                        string title, message;
                        switch (MainWindow.Lang)
                        {
                            case "ja":
                                title = "エラー！";
                                message = "名が一意ではありません！";
                                break;
                            case "de":
                                title = "Error!";
                                message = "Einzigartige Name Nicht!";
                                break;
                            case "fr":
                                title = "Erreur!";
                                message = "Nommez N'Est Pas Unique!";
                                break;
                            default:
                                title = "Error!";
                                message = "Name Not Unique!";
                                break;
                        }
                        NotifyHelper.ShowBalloonMessage(3000, title, message);
                        break;
                    case false:
                        var chatString = splitOfChatCodes.Aggregate("", (current, t) => current + (t + ","));
                        chatString = chatString.Substring(0, chatString.Length - 1);
                        Constants.XTab.Add(nameOfTab, new[] {chatString, regex});
                        _rtb = new FlowDoc();
                        DFlowDoc.Add(nameOfTab + "_FD", _rtb._FD);
                        DFlowDocReader.Add(nameOfTab + "_FDR", _rtb._FDR);
                        var newTab = new TabItem();
                        newTab.Name = nameOfTab + "_TabItem";
                        newTab.Header = nameOfTab;
                        newTab.ToolTip = String.Format("Codes : {0} :: RegEx : {1}", chatString, regex);
                        newTab.Content = _rtb;
                        LogV.View.TabControl.Items.Add(newTab);
                        TabNames.Add(nameOfTab);
                        RegExs.Add(regex);
                        var newListBox = new ListBox();
                        Array.Resize(ref ChatScan, (ChatScan).GetUpperBound(0) + 2);
                        ChatScan[_count] = newListBox;
                        foreach (var b in splitOfChatCodes)
                        {
                            ChatScan[_count].Items.Add(b);
                        }
                        var tempFlow = DFlowDoc[nameOfTab + "_FD"];
                        if (Settings.Default.LogFont != null)
                        {
                            var f = Settings.Default.LogFont;
                            tempFlow.FontFamily = new FontFamily(f.Name);
                            tempFlow.FontWeight = f.Bold ? FontWeights.Bold : FontWeights.Regular;
                            tempFlow.FontStyle = f.Italic ? FontStyles.Italic : FontStyles.Normal;
                            tempFlow.FontSize = f.Size;
                            f.Dispose();
                        }
                        tempFlow.Background = Brushes.Black;
                        tempFlow.Foreground = Brushes.White;
                        _count += 1;
                        break;
                }
            }
        }

        /// <summary>
        /// </summary>
        private static void DeleteTab()
        {
            if (LogV.View.TabControl.SelectedIndex < 4)
            {
                return;
            }
            var index = LogV.View.TabControl.SelectedIndex;
            var lookup = index - 4;
            var tname = TabNames[lookup].ToString();
            for (var i2 = lookup; i2 < ChatScan.GetUpperBound(0); i2++)
            {
                ChatScan[i2] = ChatScan[i2 + 1];
            }
            Array.Resize(ref ChatScan, ChatScan.GetUpperBound(0));
            Constants.XTab.Remove(tname);
            DFlowDoc.Remove(tname + "_FD");
            DFlowDocReader.Remove(tname + "_FDR");
            TabNames.RemoveAt(lookup);
            RegExs.RemoveAt(lookup);
            LogV.View.TabControl.Items.RemoveAt(index);
            LogV.View.TabControl.SelectedIndex = (TabNames.Count == 0) ? 2 : LogV.View.TabControl.Items.Count - 1;
            _count -= 1;
        }

        /// <summary>
        /// </summary>
        private static void CheckTabs()
        {
            var arrays = "";
            foreach (var tn in TabNames)
            {
                arrays += String.Format("TabName : {0}\n", tn);
            }
            foreach (var re in RegExs)
            {
                arrays += String.Format("RegEx : {0}\n", re);
            }
            foreach (var cs in ChatScan)
            {
                var list = cs.Items.Cast<object>().Aggregate("", (current, t) => current + (t.ToString() + ","));
                arrays += String.Format("ChatScan : {0}\n", list.Substring(0, list.Length - 1));
            }
            foreach (var fd in DFlowDoc)
            {
                arrays += String.Format("DFlowDoc : {0}\n", fd.Key);
            }
            foreach (var fdr in DFlowDocReader)
            {
                arrays += String.Format("DFlowDocReader : {0}\n", fdr.Key);
            }
            if (!String.IsNullOrWhiteSpace(arrays.Trim()))
            {
                Clipboard.SetText(arrays);
            }
        }

        #endregion
    }
}