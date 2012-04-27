// Project: LogModXIV
// File: MainStatusView.xaml.cs
// 
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System.Windows;
using System.Windows.Input;
using AppModXIV.Classes;
using LogModXIV.Classes;

namespace LogModXIV.View
{
    /// <summary>
    /// Interaction logic for MainStatusView.xaml
    /// </summary>
    public partial class MainStatusView
    {
        public static MainStatusView View;

        public MainStatusView()
        {
            InitializeComponent();
            // Insert code required on object creation below this point.
            View = this;
        }

        private void Chatter_KeyUp(object sender, KeyEventArgs e)
        {
            var tmsg = gui_Chatter.Text.Replace(" ", "");
            if ((e.Key == Key.Enter || e.Key == Key.Return) && tmsg.Length > 0)
            {
                ChatterTrans();
            }
        }

        private void ChatterTrans()
        {
            var tmpTranString = GoogleTranslate.TranslateText(gui_Chatter.Text, "en", GoogleTranslate.Offsets[Gui_ManualTranslate.Text].ToString(), false, false);
            gui_Chatter.Clear();
            gui_Chatter.Text = tmpTranString;
            Clipboard.SetText(tmpTranString);
            if (!Settings.Default.Gui_SendToGame)
            {
                return;
            }
            Clipboard.SetText(gui_ChatToSend.Text.Trim() + " " + tmpTranString);
            KeyHelper.KeyPress(Keys.Escape);
            KeyHelper.KeyPress(Keys.Space);
            KeyHelper.Paste();
            KeyHelper.KeyPress(Keys.Return);
        }

        private void gui_ChatToSend_GotFocus(object sender, RoutedEventArgs e)
        {
            gui_ChatToSend.SelectAll();
        }

        private void gui_Chatter_GotFocus(object sender, RoutedEventArgs e)
        {
            gui_Chatter.SelectAll();
        }
    }
}