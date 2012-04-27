// Project: ParseModXIV
// File: MainMenuViewModel.cs
// 
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using AppModXIV.Commands;
using ParseModXIV.Classes;
using ParseModXIV.View;
using ParseModXIV.Windows;

namespace ParseModXIV.ViewModel
{
    internal class MainMenuViewModel : INotifyPropertyChanged
    {
        private DelegateCommand _command;

        #region " COMMAND FUNCTIONS "

        public ICommand ShowSettingsCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(ShowSettings);

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

        public ICommand ExitCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(Exit);

                return _command;
            }
        }

        public ICommand UploadDataCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(UploadData);

                return _command;
            }
        }

        public ICommand ExportXmlCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(ExportXml);

                return _command;
            }
        }

        public ICommand SaveLogCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(SaveLog);

                return _command;
            }
        }

        public ICommand TopMostCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(TopMost);

                return _command;
            }
        }

        public ICommand OptionsCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(Options);

                return _command;
            }
        }

        public ICommand AboutCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(About);

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

        public ICommand ToggleMobAbilityCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(ToggleMobAbility);

                return _command;
            }
        }

        public ICommand TogglePartyStatsCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(TogglePartyStats);

                return _command;
            }
        }

        public ICommand ToggleHealingStatsCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(ToggleHealingStats);

                return _command;
            }
        }

        public ICommand ToggleDamageStatsCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(ToggleDamageStats);

                return _command;
            }
        }

        public ICommand ToggleMonsterStatsCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(ToggleMonsterStats);

                return _command;
            }
        }

        public ICommand ToggleDebugCommand
        {
            get
            {
                _command = null;
                _command = new DelegateCommand(ToggleDebug);

                return _command;
            }
        }

        #endregion

        #region " GUI FUNCTIONS "

        private static void ShowSettings()
        {
            var dialog = new SettingsDialog();
            if (dialog.ShowDialog() == true)
            {
                Settings.Default.Save();
            }
            else
            {
                Settings.Default.Reload();
            }
        }

        private static void DefaultSettings()
        {
            Settings.Default.Reset();
            Settings.Default.Reload();
        }

        private static void Exit()
        {
            Application.Current.Shutdown();
        }

        private static void UploadData()
        {
            Settings.Default.Gui_UploadData = !Settings.Default.Gui_UploadData;
        }

        private static void SaveLog()
        {
            Settings.Default.Gui_SaveLog = !Settings.Default.Gui_SaveLog;
        }

        private static void ExportXml()
        {
            Settings.Default.Gui_ExportXML = !Settings.Default.Gui_ExportXML;
        }

        private static void TopMost()
        {
            Settings.Default.TopMost = !Settings.Default.TopMost;
        }

        private static void Options()
        {
        }

        private static void About()
        {
            MessageBox.Show("Created by Ryan Wilson.\nCopyright (c) 2010-2012, Ryan Wilson. All rights reserved.", "About!", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private static void SetProcess()
        {
            ParseMod.Instance.StopLogging();
            ParseMod.SetPid();
            ParseMod.Instance.StartLogging();
        }

        private static void RefreshList()
        {
            MainMenuView.View.gui_PIDSelect.Items.Clear();
            ParseMod.Instance.StopLogging();
            ParseMod.ResetPid();
            ParseMod.Instance.StartLogging();
        }

        private static void ToggleMobAbility()
        {
            Settings.Default.Gui_MobAbility = !Settings.Default.Gui_MobAbility;
            if (Settings.Default.Gui_MobAbility)
            {
                Settings.Default.Gui_MobAbilityVisibility = Visibility.Visible;
            }
            else
            {
                Settings.Default.Gui_MobAbilityVisibility = Visibility.Collapsed;
                if (MainTabControlView.View.gui_TabControl.SelectedIndex == 1)
                {
                    MainTabControlView.View.gui_TabControl.SelectedIndex = 0;
                }
            }
        }

        private static void TogglePartyStats()
        {
            Settings.Default.Gui_PartyStats = !Settings.Default.Gui_PartyStats;
            if (Settings.Default.Gui_PartyStats)
            {
                Settings.Default.Gui_PartyStatsVisibility = Visibility.Visible;
            }
            else
            {
                Settings.Default.Gui_PartyStatsVisibility = Visibility.Collapsed;
                if (MainTabControlView.View.gui_TabControl.SelectedIndex == 2)
                {
                    MainTabControlView.View.gui_TabControl.SelectedIndex = 0;
                }
            }
        }

        private static void ToggleHealingStats()
        {
            Settings.Default.Gui_HealingStats = !Settings.Default.Gui_HealingStats;
            if (Settings.Default.Gui_HealingStats)
            {
                Settings.Default.Gui_HealingStatsVisibility = Visibility.Visible;
            }
            else
            {
                Settings.Default.Gui_HealingStatsVisibility = Visibility.Collapsed;
                if (MainTabControlView.View.gui_TabControl.SelectedIndex == 3)
                {
                    MainTabControlView.View.gui_TabControl.SelectedIndex = 0;
                }
            }
        }

        private static void ToggleDamageStats()
        {
            Settings.Default.Gui_DamageStats = !Settings.Default.Gui_DamageStats;
            if (Settings.Default.Gui_DamageStats)
            {
                Settings.Default.Gui_DamageStatsVisibility = Visibility.Visible;
            }
            else
            {
                Settings.Default.Gui_DamageStatsVisibility = Visibility.Collapsed;
                if (MainTabControlView.View.gui_TabControl.SelectedIndex == 4)
                {
                    MainTabControlView.View.gui_TabControl.SelectedIndex = 0;
                }
            }
        }

        private static void ToggleMonsterStats()
        {
            Settings.Default.Gui_MonsterStats = !Settings.Default.Gui_MonsterStats;
            if (Settings.Default.Gui_MonsterStats)
            {
                Settings.Default.Gui_MonsterStatsVisibility = Visibility.Visible;
            }
            else
            {
                Settings.Default.Gui_MonsterStatsVisibility = Visibility.Collapsed;
                if (MainTabControlView.View.gui_TabControl.SelectedIndex == 5)
                {
                    MainTabControlView.View.gui_TabControl.SelectedIndex = 0;
                }
            }
        }

        private static void ToggleDebug()
        {
            Settings.Default.Gui_Debug = !Settings.Default.Gui_Debug;
            if (Settings.Default.Gui_Debug)
            {
                Settings.Default.Gui_DebugVisibility = Visibility.Visible;
            }
            else
            {
                Settings.Default.Gui_DebugVisibility = Visibility.Collapsed;
                if (MainTabControlView.View.gui_TabControl.SelectedIndex == 6)
                {
                    MainTabControlView.View.gui_TabControl.SelectedIndex = 0;
                }
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