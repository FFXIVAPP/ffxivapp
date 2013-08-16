// FFXIVAPP.Plugin.Parse
// MainViewModel.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using FFXIVAPP.Common.Events;
using FFXIVAPP.Common.Models;
using FFXIVAPP.Common.RegularExpressions;
using FFXIVAPP.Common.ViewModelBase;
using FFXIVAPP.Plugin.Parse.Models;
using FFXIVAPP.Plugin.Parse.Models.Events;
using FFXIVAPP.Plugin.Parse.Properties;
using FFXIVAPP.Plugin.Parse.Views;
using Microsoft.Win32;
using Newtonsoft.Json;

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

        #endregion

        #region Declarations

        public ICommand ProcessSampleCommand { get; private set; }
        public ICommand ResetStatsCommand { get; private set; }
        public ICommand Convert2JsonCommand { get; private set; }

        #endregion

        public MainViewModel()
        {
            ProcessSampleCommand = new DelegateCommand(ProcessSample);
            ResetStatsCommand = new DelegateCommand(ResetStats);
            Convert2JsonCommand = new DelegateCommand(Convert2Json);
        }

        #region Loading Functions

        #endregion

        #region Utility Functions

        #endregion

        #region Command Bindings

        private static void ProcessSample()
        {
            var openFileDialog = new OpenFileDialog
            {
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "Logs",
                Multiselect = false,
                Filter = "XML Files (*.xml)|*.xml"
            };
            openFileDialog.FileOk += delegate
            {
                var count = 0;
                var sampleXml = XDocument.Load(openFileDialog.FileName);
                var items = new Dictionary<int, string[]>();
                foreach (var xElement in sampleXml.Descendants()
                    .Elements("Entry"))
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
                Func<bool> dFunc = delegate
                {
                    foreach (var item in items)
                    {
                        var code = item.Value[0];
                        var line = item.Value[1];
                        line = line.Replace("", "⇒");
                        line = line.Replace("[01010101]", "");
                        line = line.Replace("[CF010101]", "");
                        var timeStampColor = Settings.Default.TimeStampColor.ToString();
                        var timeStamp = DateTime.Now.ToString("[HH:mm:ss] ");
                        timeStamp = String.IsNullOrWhiteSpace(item.Value[2]) ? timeStamp : item.Value[2].Trim() + " ";
                        var color = (Constants.Colors.ContainsKey(code)) ? Constants.Colors[code][0] : "FFFFFF";
                        if (Constants.Abilities.Contains(code) && Regex.IsMatch(line, @".+(((cast|use)s?|(lance|utilise)z?)\s|の「)", SharedRegEx.DefaultOptions))
                        {
                            Common.Constants.FD.AppendFlow(timeStamp, "", line, new[]
                            {
                                timeStampColor, "#" + color
                            }, MainView.View.AbilityChatFD._FDR);
                        }
                        EventParser.Instance.ParseAndPublish(Convert.ToUInt32(code, 16), line, false);
                    }
                    return true;
                };
                dFunc.BeginInvoke(null, null);
            };
            openFileDialog.ShowDialog();
        }

        /// <summary>
        /// </summary>
        private static void ResetStats()
        {
            var popupContent = new PopupContent();
            popupContent.PluginName = Plugin.PName;
            popupContent.Title = PluginViewModel.Instance.Locale["app_WarningMessage"];
            popupContent.Message = PluginViewModel.Instance.Locale["parse_ResetStatsMessage"];
            popupContent.CanSayNo = true;
            bool popupDisplayed;
            Plugin.PHost.PopupMessage(Plugin.PName, out popupDisplayed, popupContent);
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

        private static void Convert2Json()
        {
            #region Generate Overall-Player-Monster

            dynamic overallStats = new Dictionary<string, object>();
            dynamic playerStats = new Dictionary<string, object>();
            dynamic monsterStats = new Dictionary<string, object>();
            overallStats.Add("Stats", ParseControl.Instance.Timeline.Overall.Stats.ToDictionary(s => s.Name, s => s.Value));
            var partyTimeline = ParseControl.Instance.Timeline.Party;
            var playerNames = partyTimeline.Select(p => p.Name)
                .ToList();
            foreach (var playerName in playerNames)
            {
                var player = partyTimeline.GetGroup(playerName);
                playerStats.Add(playerName, new Dictionary<string, object>
                {
                    {
                        "Stats", new Dictionary<string, object>()
                    },
                    {
                        "Abilities", new Dictionary<string, object>()
                    },
                    {
                        "Monsters", new Dictionary<string, object>()
                    },
                    {
                        "Healing", new Dictionary<string, object>()
                    },
                    {
                        "Players", new Dictionary<string, object>()
                    },
                    {
                        "Damage", new Dictionary<string, object>()
                    }
                });
                playerStats[playerName]["Stats"] = player.Stats.ToDictionary(s => s.Name, s => s.Value);
                var playerAbilities = player.GetGroup("Abilities");
                foreach (var playerAbility in playerAbilities)
                {
                    playerStats[playerName]["Abilities"].Add(playerAbility.Name, playerAbility.Stats.ToDictionary(s => s.Name, s => s.Value));
                }
                var playerMonsters = player.GetGroup("Monsters");
                foreach (var playerMonster in playerMonsters)
                {
                    playerStats[playerName]["Monsters"].Add(playerMonster.Name, new Dictionary<string, object>
                    {
                        {
                            "Stats", playerMonster.Stats.ToDictionary(s => s.Name, s => s.Value)
                        },
                        {
                            "Abilities", playerMonster.GetGroup("Abilities")
                                .ToDictionary(a => a.Name, a => a.Stats.ToDictionary(s => s.Name, s => s.Value))
                        }
                    });
                }
                var playerHealings = player.GetGroup("Healing");
                foreach (var playerHealing in playerHealings)
                {
                    playerStats[playerName]["Healing"].Add(playerHealing.Name, playerHealing.Stats.ToDictionary(s => s.Name, s => s.Value));
                }
                var playerPlayers = player.GetGroup("Players");
                foreach (var playerPlayer in playerPlayers)
                {
                    playerStats[playerName]["Players"].Add(playerPlayer.Name, new Dictionary<string, object>
                    {
                        {
                            "Stats", playerPlayer.Stats.ToDictionary(s => s.Name, s => s.Value)
                        },
                        {
                            "Abilities", playerPlayer.GetGroup("Abilities")
                                .ToDictionary(a => a.Name, a => a.Stats.ToDictionary(s => s.Name, s => s.Value))
                        }
                    });
                }
                var playerDamages = player.GetGroup("Damage");
                foreach (var playerDamage in playerDamages)
                {
                    playerStats[playerName]["Damage"].Add(playerDamage.Name, new Dictionary<string, object>
                    {
                        {
                            "Stats", playerDamage.Stats.ToDictionary(s => s.Name, s => s.Value)
                        },
                        {
                            "Abilities", playerDamage.GetGroup("Abilities")
                                .ToDictionary(a => a.Name, a => a.Stats.ToDictionary(s => s.Name, s => s.Value))
                        }
                    });
                }
            }
            var monsterTimeline = ParseControl.Instance.Timeline.Monster;
            var monsterNames = monsterTimeline.Select(p => p.Name)
                .ToList();
            foreach (var monsterName in monsterNames)
            {
                var monster = monsterTimeline.GetGroup(monsterName);
                monsterStats.Add(monsterName, new Dictionary<string, object>
                {
                    {
                        "Stats", new Dictionary<string, object>()
                    },
                    {
                        "Abilities", new Dictionary<string, object>()
                    },
                    {
                        "Drops", new Dictionary<string, object>()
                    }
                });
                monsterStats[monsterName]["Stats"] = monster.Stats.ToDictionary(s => s.Name, s => s.Value);
                var monsterAbilities = monster.GetGroup("Abilities");
                foreach (var monsterAbility in monsterAbilities)
                {
                    monsterStats[monsterName]["Abilities"].Add(monsterAbility.Name, monsterAbility.Stats.ToDictionary(s => s.Name, s => s.Value));
                }
                var monsterDrops = monster.GetGroup("Drops");
                foreach (var monsterDrop in monsterDrops)
                {
                    monsterStats[monsterName]["Drops"].Add(monsterDrop.Name, monsterDrop.Stats.ToDictionary(s => s.Name, s => s.Value));
                }
            }
            dynamic results = new Dictionary<string, object>
            {
                {
                    "Overall", overallStats
                },
                {
                    "Player", playerStats
                },
                {
                    "Monster", monsterStats
                }
            };

            #endregion

            Clipboard.SetText(JsonConvert.SerializeObject(results));
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
