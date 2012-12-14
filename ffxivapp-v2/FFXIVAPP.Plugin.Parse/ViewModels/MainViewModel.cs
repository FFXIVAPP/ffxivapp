// FFXIVAPP.Plugin.Parse
// MainViewModel.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Xml.Linq;
using FFXIVAPP.Common.Events;
using FFXIVAPP.Common.Models;
using FFXIVAPP.Common.ViewModelBase;
using FFXIVAPP.Plugin.Parse.Models;
using FFXIVAPP.Plugin.Parse.Models.Events;
using FFXIVAPP.Plugin.Parse.Properties;
using FFXIVAPP.Plugin.Parse.Views;

#endregion

namespace FFXIVAPP.Plugin.Parse.ViewModels
{
    internal sealed class MainViewModel : INotifyPropertyChanged
    {
        #region Property Bindings

        private static MainViewModel _instance;

        public static MainViewModel Instance
        {
            get { return _instance ?? (_instance = new MainViewModel()); }
        }

        public static bool SampleOK
        {
            get { return File.Exists(Constants.BaseDirectory + "sample.xml"); }
        }

        #endregion

        #region Declarations

        public ICommand ProcessSampleCommand { get; private set; }
        public ICommand ResetStatsCommand { get; private set; }

        #endregion

        public MainViewModel()
        {
            ProcessSampleCommand = new DelegateCommand(ProcessSample);
            ResetStatsCommand = new DelegateCommand(ResetStats);
        }

        #region Loading Functions

        #endregion

        #region Utility Functions

        #endregion

        #region Command Bindings

        private static void ProcessSample()
        {
            var count = 0;
            var sampleXml = XDocument.Load(Constants.BaseDirectory + "sample.xml");
            var items = new Dictionary<int, string[]>();
            foreach (var xElement in sampleXml.Descendants().Elements("Entry"))
            {
                var xKey = (string) xElement.Attribute("Key");
                var xLine = (string) xElement.Element("Line");
                var xTimeStamp = (string) xElement.Element("TimeStamp");
                if (String.IsNullOrWhiteSpace(xKey) || String.IsNullOrWhiteSpace(xLine))
                {
                    continue;
                }
                items.Add(count, new[]
                {
                    xKey, xLine, xTimeStamp
                });
                ++count;
            }
            count = 0;
            foreach (var d in items.Select(item => (Func<bool>) delegate
            {
                var code = item.Value[0];
                var line = item.Value[1];
                var timeStampColor = Settings.Default.TimeStampColor.ToString();
                var timeStamp = DateTime.Now.ToString("[HH:mm:ss] ");
                timeStamp = String.IsNullOrWhiteSpace(item.Value[2]) ? timeStamp : item.Value[2].Trim() + " ";
                var color = (Common.Constants.Colors.ContainsKey(code)) ? Common.Constants.Colors[code][0] : "FFFFFF";
                if (line.Contains("readies") || line.Contains("prépare") || line.Contains("をしようとしている。"))
                {
                    if (Constants.Abilities.Contains(code))
                    {
                        Common.Constants.FD.AppendFlow(timeStamp, line, new[]
                        {
                            timeStampColor, "#" + color
                        }, MainView.View.AbilityChatFD._FDR);
                    }
                }
                Func<bool> funcParse = delegate
                {
                    EventParser.Instance.ParseAndPublish(Convert.ToUInt16(code, 16), line);
                    return true;
                };
                funcParse.BeginInvoke(null, null);
                ++count;
                return true;
            }))
            {
                d.BeginInvoke(null, null);
            }
        }

        /// <summary>
        /// </summary>
        private static void ResetStats()
        {
            var popupContent = new PopupContent();
            popupContent.PluginName = Plugin.PName;
            popupContent.Title = PluginViewModel.Instance.Locale["app_warningpopuptitle"];
            popupContent.Message = PluginViewModel.Instance.Locale["parse_resetstatspopupmessage"];
            popupContent.CanSayNo = true;
            bool popupDisplayed;
            Plugin.PHost.PopupMessage(out popupDisplayed, popupContent);
            if (!popupDisplayed)
            {
                return;
            }
            EventHandler<PopupResultEvent> resultChanged = null;
            resultChanged = delegate(object sender, PopupResultEvent e)
            {
                switch (e.NewValue.ToString())
                {
                    case "Yes":
                        MainView.View.AbilityChatFD._FD.Blocks.Clear();
                        ParseControl.Instance.StatMonitor.Clear();
                        break;
                    case "No":
                        break;
                }
                PluginViewModel.Instance.PopupResultChanged -= resultChanged;
            };
            PluginViewModel.Instance.PopupResultChanged += resultChanged;
        }

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion
    }
}
